using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //TODO press esc to pause game, return to resume, exit to go to main menu
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject startScene;

    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }
    void Pause ()
    {
        //TODO pause gameplay
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Resume ()
    {
        //TODO resume gameplay
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Exit ()
    {
        //TODO return to start scene 
        Time.timeScale = 1f;
        startScene.SetActive(true);
    }
}
