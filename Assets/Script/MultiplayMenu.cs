using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MultiplayMenu : MonoBehaviour 
{
    //[SerializeField] private Button startHostButton;
    //[SerializeField] private Button startClientButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private GameObject startScence;
    [SerializeField] private TMP_InputField IpInput;

    private void Hide()
    {
        startScence.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void StartAsHost()
    {
        joinButton.onClick.AddListener(() =>
        {
            Debug.Log($"HOST at {IpInput.text}");
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = IpInput.text;
            NetworkManager.Singleton.StartHost();
            Hide();
        });
    }
    public void StartAsClient()
    {
        joinButton.onClick.AddListener(() =>
        {
            Debug.Log($"Client join room at {IpInput.text}");
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = IpInput.text;
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }
}
