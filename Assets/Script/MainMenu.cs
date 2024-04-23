using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject startScene;
    //TODO chose play or exit game
    public void Play ()
    {
        //TODO play when hit New game
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        startScene.SetActive(false);
    }

    public void Exit ()
    {
        //TODO quit game when hit Exit
        Application.Quit();
    }

}
