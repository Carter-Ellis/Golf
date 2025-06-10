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

    [Header("Scoreboard")]
    [SerializeField] private Sprite camp18On;
    [SerializeField] private Sprite camp18Off;
    [SerializeField] private Sprite campSpeedOn;
    [SerializeField] private Sprite campSpeedOff;
    [SerializeField] private Sprite campHardOn;
    [SerializeField] private Sprite campHardOff;
    
    [SerializeField] private Sprite classic18On;
    [SerializeField] private Sprite classic18Off;
    [SerializeField] private Sprite classicSpeedOn;
    [SerializeField] private Sprite classicSpeedOff;
    [SerializeField] private Sprite classicHardOn;
    [SerializeField] private Sprite classicHardOff;

    [SerializeField] private UnityEngine.UI.Button camp18Button;
    [SerializeField] private UnityEngine.UI.Button campSpeedButton;
    [SerializeField] private UnityEngine.UI.Button campHardButton;
    [SerializeField] private UnityEngine.UI.Button classic18Button;
    [SerializeField] private UnityEngine.UI.Button classicSpeedButton;
    [SerializeField] private UnityEngine.UI.Button classicHardButton;

    private Image camp18Image;
    private Image campSpeedImage;
    private Image campHardImage;
    private Image classic18Image;
    private Image classicSpeedImage;
    private Image classicHardImage;

    private void Awake()
    {
        if (graphicsButton != null)
        {
            graphicsImage = graphicsButton.GetComponent<Image>();
            audioImage = audioButton.GetComponent<Image>();
        }

        if (camp18Button != null)
        {
            camp18Image = camp18Button.GetComponent<Image>();
            campSpeedImage = campSpeedButton.GetComponent<Image>();
            campHardImage = campHardButton.GetComponent<Image>();
            classic18Image = classic18Button.GetComponent<Image>();
            classicSpeedImage = classicSpeedButton.GetComponent<Image>();
            classicHardImage = classicHardButton.GetComponent<Image>();
        }
        
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

    public void camp18()
    {
        camp18Button.enabled = false;
        campSpeedButton.enabled = true;
        campHardButton.enabled = true;
        classic18Button.enabled = true;
        classicSpeedButton.enabled = true;
        classicHardButton.enabled = true;

        camp18Image.sprite = camp18On;
        campSpeedImage.sprite = campSpeedOff;
        campHardImage.sprite = campHardOff;
        classic18Image.sprite = classic18Off;
        classicSpeedImage.sprite = classicSpeedOff;
        classicHardImage.sprite = classicHardOff;
    }

    public void campSpeed()
    {
        camp18Button.enabled = true;
        campSpeedButton.enabled = false;
        campHardButton.enabled = true;
        classic18Button.enabled = true;
        classicSpeedButton.enabled = true;
        classicHardButton.enabled = true;

        camp18Image.sprite = camp18Off;
        campSpeedImage.sprite = campSpeedOn;
        campHardImage.sprite = campHardOff;
        classic18Image.sprite = classic18Off;
        classicSpeedImage.sprite = classicSpeedOff;
        classicHardImage.sprite = classicHardOff;
    }

    public void campHard()
    {
        camp18Button.enabled = true;
        campSpeedButton.enabled = true;
        campHardButton.enabled = false;
        classic18Button.enabled = true;
        classicSpeedButton.enabled = true;
        classicHardButton.enabled = true;

        camp18Image.sprite = camp18Off;
        campSpeedImage.sprite = campSpeedOff;
        campHardImage.sprite = campHardOn;
        classic18Image.sprite = classic18Off;
        classicSpeedImage.sprite = classicSpeedOff;
        classicHardImage.sprite = classicHardOff;
    }

    public void classic18()
    {
        camp18Button.enabled = true;
        campSpeedButton.enabled = true;
        campHardButton.enabled = true;
        classic18Button.enabled = false;
        classicSpeedButton.enabled = true;
        classicHardButton.enabled = true;

        camp18Image.sprite = camp18Off;
        campSpeedImage.sprite = campSpeedOff;
        campHardImage.sprite = campHardOff;
        classic18Image.sprite = classic18On;
        classicSpeedImage.sprite = classicSpeedOff;
        classicHardImage.sprite = classicHardOff;
    }

    public void classicSpeed()
    {
        camp18Button.enabled = true;
        campSpeedButton.enabled = true;
        campHardButton.enabled = true;
        classic18Button.enabled = true;
        classicSpeedButton.enabled = false;
        classicHardButton.enabled = true;

        camp18Image.sprite = camp18Off;
        campSpeedImage.sprite = campSpeedOff;
        campHardImage.sprite = campHardOff;
        classic18Image.sprite = classic18Off;
        classicSpeedImage.sprite = classicSpeedOn;
        classicHardImage.sprite = classicHardOff;
    }

    public void classicHard()
    {
        camp18Button.enabled = true;
        campSpeedButton.enabled = true;
        campHardButton.enabled = true;
        classic18Button.enabled = true;
        classicSpeedButton.enabled = true;
        classicHardButton.enabled = false;

        camp18Image.sprite = camp18Off;
        campSpeedImage.sprite = campSpeedOff;
        campHardImage.sprite = campHardOff;
        classic18Image.sprite = classic18Off;
        classicSpeedImage.sprite = classicSpeedOff;
        classicHardImage.sprite = classicHardOn;
    }
}
