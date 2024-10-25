using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Canvas soundMenuCanvas;
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
        soundMenuCanvas.enabled = !soundMenuCanvas.enabled;
        if (ball != null)
        {
            ball.isBallLocked = soundMenuCanvas.enabled;
        }
    }

}
