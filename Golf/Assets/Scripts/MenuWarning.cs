using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuWarning : MonoBehaviour
{

    private static GameObject menuWarning;
    private static GameObject pauseMenu;
    private static GameObject winCanvas;

    private void Awake()
    {
        menuWarning = GameObject.Find("Menu Warning");
        pauseMenu = GameObject.Find("Pause Screen");
        winCanvas = GameObject.Find("LevelFinishedCanvas");
        menuWarning.SetActive(false);
        print("menu warning " + menuWarning);
        print("menu [pause " + pauseMenu);
        print("level finish canvas " + winCanvas);
    }

    public static void warningOpen()
    {
        if (GameMode.isAnySpeedrun() || GameMode.current == GameMode.TYPE.FREEPLAY)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            
            winCanvas.SetActive(false);
            pauseMenu.SetActive(false);
            menuWarning.SetActive(true);
        }

    }
}
