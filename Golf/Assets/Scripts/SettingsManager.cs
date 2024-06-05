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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            soundMenuCanvas.enabled = !soundMenuCanvas.enabled;
            ball.isBallLocked = soundMenuCanvas.enabled;
            
        }
    }
}
