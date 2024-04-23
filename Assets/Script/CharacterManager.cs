using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using System.ComponentModel.Design;

public class CharacterManager : NetworkBehaviour 
{
    [SerializeField] private List<GameObject> Character;
    [SerializeField] private GameObject NetworkPlayer;
    [SerializeField] private Vector3 HostPostion;
    [SerializeField] private Vector3 ClientPostion;

    public void SpawnHost()
    {
        //NetworkManager.AddNetworkPrefab(Character[0]);
        //SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 0, HostPostion);
    }
    public void SpawnClient()
    {
        //SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 1, ClientPostion);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong playerId, int characterId, Vector3 spawnPos)
    {
        GameObject player = Instantiate(Character[characterId], spawnPos, Quaternion.identity);
        NetworkObject networkObject = player.GetComponent<NetworkObject>();
        player.SetActive(true);
        networkObject.SpawnAsPlayerObject(playerId, true);
    }

}
