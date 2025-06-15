using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public Canvas soundMenuCanvas;
    private Ball ball;
    private GameObject soundMenu;
    private PauseManager pauseManager;
    private PopupController popupController;

    private void Start()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        soundMenu = soundMenuCanvas.gameObject;
        pauseManager = GetComponent<PauseManager>();
        popupController = FindObjectOfType<PopupController>();

        if (pauseManager != null)
        {
            pauseManager.initialize(soundMenu);
        }
        if (soundMenu != null)
        {
            soundMenu.SetActive(false);
        }
    }
    private void Update()
    {
        
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            return;
        }

        if (popupController != null && popupController.popup.activeSelf)
        {
            return;
        }

        if (ball.isTeleportReady)
        {
            if (PlayerInput.isDown(PlayerInput.Axis.Cancel))
            {
                soundMenu.SetActive(!soundMenu.activeSelf);
                if (soundMenu.activeSelf)
                {
                    pauseManager.UpdatePauseMenu();
                }
            }
            return;
        }

        if (ball != null && ball.isTraveling)
        {
            if (PlayerInput.isDown(PlayerInput.Axis.Cancel))
            {
                soundMenu.SetActive(!soundMenu.activeSelf);
                ball.isBallLocked = soundMenu.activeSelf;
                if (soundMenu.activeSelf)
                {
                    pauseManager.UpdatePauseMenu();
                }
            }
            return;
        }
        if (ball != null && ball.GetComponent<Ball>().enabled != false && !ball.isActiveAndEnabled)
        {
            soundMenu.SetActive(false);
        }
        if (PlayerInput.isDown(PlayerInput.Axis.Cancel) && ball.isActiveAndEnabled && GameObject.Find("Menu Warning") == null)
        {
            soundMenu.SetActive(!soundMenu.activeSelf);
            if (ball != null)
            {
                ball.isBallLocked = soundMenu.activeSelf;
                if (soundMenu.activeSelf)
                {
                    ball.hasClickedBall = false;
                    ball.cursor.GetComponent<SpriteRenderer>().enabled = false;
                    ball.ClearDots();
                    pauseManager.UpdatePauseMenu();
                }
            }   
            
        }
    }
    public void backButton()
    {
        soundMenu.SetActive(!soundMenu.activeSelf);
        if (ball != null)
        {
            ball.isBallLocked = soundMenu.activeSelf;
        }
    }
}
