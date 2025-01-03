using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX,
        AMBIENCE,
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    public Slider volumeSlider;
    private Inventory inv;
    public bool isInitializing;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
        inv = FindObjectOfType<Inventory>();
        if (inv == null)
        {
            Debug.LogWarning("Inventory object not found!");
        }
    }

    private void Update()
    {
        switch(volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.instance.masterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioManager.instance.musicVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.instance.SFXVolume;
                break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = AudioManager.instance.ambienceVolume;    
                break;
            default:
                Debug.Log("Volume Type not supported: " + volumeType);
                break;
        }

        isInitializing = false;
        
    }

    public void OnSliderValueChange()
    {
        if (!isInitializing)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.tick, transform.position);
        }
        
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = volumeSlider.value;
                inv.masterVol = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                AudioManager.instance.musicVolume = volumeSlider.value;
                inv.musicVol = volumeSlider.value;
                break;
            case VolumeType.SFX:
                AudioManager.instance.SFXVolume = volumeSlider.value;
                inv.SFXVol = volumeSlider.value;
                break;
            case VolumeType.AMBIENCE:
                AudioManager.instance.ambienceVolume = volumeSlider.value;
                inv.ambienceVol = volumeSlider.value;
                break;
            default:
                Debug.Log("Volume Type not supported: " + volumeType);
                break;
        }
        inv.SavePlayer();
    }

}
