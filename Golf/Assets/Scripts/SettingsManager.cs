using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public Canvas soundMenuCanvas;
    private Ball ball;
    
    private void Start()
    {
        soundMenuCanvas.enabled = false;
        ball = GameObject.FindObjectOfType<Ball>();
    }
    private void Update()
    {

        if (ball.isTeleportReady)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                soundMenuCanvas.enabled = !soundMenuCanvas.enabled;
                if (soundMenuCanvas.enabled)
                {
                    FindObjectOfType<PauseManager>().UpdatePauseMenu();
                }
            }
            return;
        }

        if (ball != null && ball.isTraveling)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                soundMenuCanvas.enabled = !soundMenuCanvas.enabled;
                ball.isBallLocked = soundMenuCanvas.enabled;
                if (soundMenuCanvas.enabled)
                {
                    FindObjectOfType<PauseManager>().UpdatePauseMenu();
                }
            }
            return;
        }
        if (ball != null && ball.GetComponent<Ball>().enabled != false && !ball.isActiveAndEnabled)
        {
            soundMenuCanvas.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && ball.isActiveAndEnabled)
        {
            soundMenuCanvas.enabled = !soundMenuCanvas.enabled;
            if (ball != null)
            {
                ball.isBallLocked = soundMenuCanvas.enabled;
                if (soundMenuCanvas.enabled)
                {
                    FindObjectOfType<PauseManager>().UpdatePauseMenu();
                }
            }   
            
        }
    }
    public void backButton()
    {
        soundMenuCanvas.enabled = !soundMenuCanvas.enabled;
        if (ball != null)
        {
            ball.isBallLocked = soundMenuCanvas.enabled;
        }
    }
}
