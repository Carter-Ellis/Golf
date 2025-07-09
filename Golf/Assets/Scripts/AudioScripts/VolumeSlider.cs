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
                volumeSlider.value = Audio.volume(Audio.TYPE.MASTER);
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = Audio.volume(Audio.TYPE.MUSIC);
                break;
            case VolumeType.SFX:
                volumeSlider.value = Audio.volume(Audio.TYPE.SFX);
                break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = Audio.volume(Audio.TYPE.AMBIENCE);    
                break;
            default:
                Debug.Log("Volume Type not supported: " + volumeType);
                break;
        }

        isInitializing = false;
        
    }

    public void OnSliderValueChange()
    {
        if (isInitializing) { return; }

        Audio.playSFX(FMODEvents.instance.tick, transform.position);
        
        
        switch (volumeType)
        {
            case VolumeType.MASTER:
                Audio.volume(Audio.TYPE.MASTER, volumeSlider.value);
                inv.masterVol = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                Audio.volume(Audio.TYPE.MUSIC, volumeSlider.value);
                inv.musicVol = volumeSlider.value;
                break;
            case VolumeType.SFX:
                Audio.volume(Audio.TYPE.SFX, volumeSlider.value);
                inv.SFXVol = volumeSlider.value;
                break;
            case VolumeType.AMBIENCE:
                Audio.volume(Audio.TYPE.AMBIENCE, volumeSlider.value);
                inv.ambienceVol = volumeSlider.value;
                break;
            default:
                Debug.Log("Volume Type not supported: " + volumeType);
                break;
        }
        inv.SavePlayer();
    }

}
