using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuApple : MonoBehaviour
{
    public Sprite eatenApple;
    UnityEngine.UI.Button button;
    private Image image;
    private ButtonAudio audioButton;
    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<UnityEngine.UI.Button>();
        audioButton = GetComponent<ButtonAudio>();
    }

    public void EatApple()
    {
        image.sprite = eatenApple;
        button.enabled = false;
        audioButton.enabled = false;
        Achievement.Give(Achievement.TYPE.APPLE_A_DAY);
    }

}
