using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Networking.Transport.Relay;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class MultiConnectionHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RoomID;
    [SerializeField] private GameObject startScene;
    [SerializeField] private GameObject GameLobby;
    [SerializeField] private GameObject HOST_status;
    [SerializeField] private GameObject CLIENT_status;
    [SerializeField] private TMP_InputField Code;
    [SerializeField] private GameObject NetworkHandler;

    private readonly string KEY_START_GAME = "StartGame_RelayCode";
    private readonly string LOBBY_STATUS = "Lobby_Status";
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private string PlayerId;

    private float lobbyUpdateTimer;
    private float lobbyTTLTimer;

    /*
    private async void Start()
    {
        //Initial Unity Services
        await UnityServices.InitializeAsync();
        //Notify me when someone signed in
        AuthenticationService.Instance.SignedIn += () =>
        {
            PlayerId = AuthenticationService.Instance.PlayerId;
            ID.text = PlayerId;
            Debug.Log($"Signed in {PlayerId}");
        };
        //Sign in anonymoustly
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    */

    public async void Authenticate(string playerName)
    {
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () =>
        {
            PlayerId = playerName;
            Debug.Log($"Signed in {PlayerId}");
        };

        //Sign in anonymoustly
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        if (playerName == "HOST")
        {
            CreateLobby();
        }
        else if (playerName == "CLIENT")
        {
            JoinLobby(Code);
        }
    }
    #region Unity Relay
    public async Task<string> CreateRelay()
    {
        try
        {
            //Scenario: 1 HOST, 1 CLIENT
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
            //Create joincode
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            //Set up network manager base on allocation
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
                );

            NetworkManager.Singleton.StartHost();
            return joinCode;
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"Error: {ex.Message}");
            return null;
        }
    }

    public async void JoinRelay(string relayCode)
    {
        try
        {
            Debug.Log($"join room {relayCode}");
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
                );

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException ex)
        {
            Debug.Log($"Error: {ex.Message}");
        }
    }
    #endregion

    #region Unity Lobby
    public async void CreateLobby()
    {
        try
        {
            //Default settings
            string lobbyName = "SomeLobby";
            int maxPlayers = 2;

            //Get relay code
            string relayCode = await CreateRelay();

            //Create lobby options
            var hostOptions = new CreateLobbyOptions
            {
                Player = GetPlayer("HOST"),
                Data = new Dictionary<string, DataObject>
                {
                    {KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Public, relayCode) },
                    {LOBBY_STATUS, new DataObject(DataObject.VisibilityOptions.Public, "true") }
                }
            };

            //Create lobby
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, hostOptions);
            hostLobby = lobby;
            joinedLobby = hostLobby;

            //Display lobby
            DisplayLobby();

        }
        catch (LobbyServiceException ex)
        {
            Debug.Log($"Lobby Error {ex.Message}");
        }
    }

    public async void JoinLobby(TMP_InputField Code)
    {
        try
        {
            //Get lobby code from input
            string lobbyCode = Code.text;

            //Create lobby option
            var clientOption = new JoinLobbyByCodeOptions 
            {
                Player = GetPlayer("CLIENT")
            };
            //Join lobby by lobby code
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, clientOption);
            joinedLobby = lobby;

            //Join relay by lobby.data
            JoinRelay(joinedLobby.Data[KEY_START_GAME].Value);

            //Display lobby
            DisplayLobby();
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
        }
    }


    private Player GetPlayer(string role)
    {

        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, role) }
                    }
        };
    }

    private void DisplayLobby()
    {
        RoomID.text = $"CODE: {joinedLobby.LobbyCode}";
        startScene.SetActive(false);
        GameLobby.SetActive(true);
    }

    private void PlayerStatusUpdate()
    {
        foreach (Player player in joinedLobby.Players)
        {
            var playerName = player.Data["PlayerName"].Value;

            if (playerName == "HOST" && HOST_status.activeInHierarchy == false)
            {
                HOST_status.SetActive(true);
            }
            if (playerName == "CLIENT" && CLIENT_status.activeInHierarchy == false)
            {
                CLIENT_status.SetActive(true);
            }
        }
    }

    private async void HandleLobbyTTL()
    {
        if (hostLobby != null)
        {
            lobbyTTLTimer -= Time.deltaTime;
            if (lobbyTTLTimer < 0f)
            {
                float lobbyTTLTimerMax = 30f;
                lobbyTTLTimer = lobbyTTLTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }    
    }

    private async void HandleLobbyUpdate()
    {
        if (joinedLobby != null)
        {
            var lobbyStatus = joinedLobby.Data[LOBBY_STATUS].Value;

            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
                PlayerStatusUpdate();
            }
            if (lobbyStatus == "false")
            {
                NetworkHandler.SetActive(false);
                GameLobby.SetActive(false);
            }
        }    
    }

    #endregion

    public async void StartGame()
    {
        var lobbyUpdateOptions = new UpdateLobbyOptions
        {
            Data = new Dictionary<string, DataObject>
            {
                {LOBBY_STATUS, new DataObject(DataObject.VisibilityOptions.Public, "false") }
            }
        };

        Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, lobbyUpdateOptions);
        joinedLobby = lobby;
    }
    private void Update()
    {
        HandleLobbyTTL();
        HandleLobbyUpdate();
    }
}
