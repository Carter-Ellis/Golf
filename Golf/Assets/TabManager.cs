using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private Sprite audioOn;
    [SerializeField] private Sprite audioOff;
    [SerializeField] private Sprite graphicsOn;
    [SerializeField] private Sprite graphicsOff;
    [SerializeField] private UnityEngine.UI.Button audioButton;
    [SerializeField] private UnityEngine.UI.Button graphicsButton;
    private Image graphicsImage;
    private Image audioImage;
    private void Start()
    {
        graphicsImage = graphicsButton.GetComponent<Image>();
        audioImage = audioButton.GetComponent<Image>();
    }

    public void AudioTabToggle()
    {
        if (audioButton.enabled) 
        {
            audioButton.enabled = false;
            graphicsButton.enabled = true;
            audioImage.sprite = audioOn;
            graphicsImage.sprite = graphicsOff;
            
        }
        else
        {
            audioButton.enabled = true;
            graphicsButton.enabled = false;
            audioImage.sprite = audioOff;
            graphicsImage.sprite = graphicsOn;
        }
    }

    public void BackSettings()
    {
        audioButton.enabled = false;
        graphicsButton.enabled = true;
        audioImage.sprite = audioOn;
        graphicsImage.sprite = graphicsOff;
    }

}
