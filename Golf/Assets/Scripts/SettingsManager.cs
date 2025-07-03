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
                pause();
            }
            return;
        }

        if (ball != null && ball.isTraveling)
        {
            if (PlayerInput.isDown(PlayerInput.Axis.Cancel))
            {
                pause();
            }
            return;
        }
        if (ball != null && ball.GetComponent<Ball>().enabled != false && !ball.isActiveAndEnabled)
        {
            soundMenu.SetActive(false);
        }
        if (PlayerInput.isDown(PlayerInput.Axis.Cancel) && ball.isActiveAndEnabled && GameObject.Find("Menu Warning") == null)
        {
            pause();
        }
    }

    public void pause()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            return;
        }
        soundMenu.SetActive(!soundMenu.activeSelf);

        if (pauseManager == null)
        {
            return;
        }
        if (soundMenu.activeSelf)
        {
            pauseManager.UpdatePauseMenu();
        }
        if (ball != null)
        {
            ball.isBallLocked = soundMenu.activeSelf;
            ball.hasClickedBall = false;
            ball.ClearDots();
            ball.swingPowerSlider?.gameObject.SetActive(false);
            ball.powerTxt?.gameObject.SetActive(false);
            ball.cancelImage?.SetActive(false);
            SpriteRenderer cursorSprite = ball.cursor?.GetComponent<SpriteRenderer>();
            if (cursorSprite != null)
            {
                cursorSprite.enabled = false;
            }
        }
        
    }

    public void backButton()
    {
        
        soundMenu = GameObject.Find("Pause Screen").gameObject;
        soundMenu.SetActive(!soundMenu.activeSelf);
        if (ball != null)
        {
            ball.isBallLocked = soundMenu.activeSelf;
        }
    }
}
