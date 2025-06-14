using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public Canvas soundMenuCanvas;
    public Canvas mainMenu;
    public bool isActive = true;
    private GameObject mainCursor;

    private void Start()
    {
        mainCursor = GameObject.FindAnyObjectByType<CursorController>()?.gameObject;
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisplayOptions()
    {
        Ball ball = FindObjectOfType<Ball>();
        mainMenu.enabled = !mainMenu.enabled;
        isActive = !isActive;
        GameObject soundMenu = soundMenuCanvas.gameObject;
        soundMenu.SetActive(!soundMenu.activeSelf);
        mainCursor?.SetActive(!mainCursor.activeSelf);
        if (ball != null)
        {
            ball.isBallLocked = soundMenu.activeSelf;
        }
    }

}
