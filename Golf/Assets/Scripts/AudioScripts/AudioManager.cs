using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0,1)]
    public float masterVolume = 1f;
    [Range(0, 1)]
    public float musicVolume = 1f;
    [Range(0, 1)]
    public float SFXVolume = 1f;
    [Range(0, 1)]
    public float ambienceVolume = 1f;

    private Bus masterBus;
    private Bus musicBus;
    private Bus SFXBus;
    private Bus ambienceBus;

    private List<EventInstance> eventInstances;

    public static AudioManager instance { get; private set; }

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Audio Manager in the scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");

    }

    private void Start()
    {
        InitializeAmbience(FMODEvents.instance.ambience);
        InitializeMusic(FMODEvents.instance.music);
    }

    private void Update()
    {
        print(SFXVolume);
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        SFXBus.setVolume(SFXVolume);
        ambienceBus.setVolume(ambienceVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        // Stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
