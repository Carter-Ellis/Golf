using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float SFXVolume = 1f;
    [Range(0, 1)] public float ambienceVolume = 1f;

    [Header("Shop")]
    public bool isShop;

    public Bus masterBus;
    public Bus musicBus;
    public Bus SFXBus;
    public Bus ambienceBus;

    private List<EventInstance> eventInstances;

    public static AudioManager instance { get; private set; }

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
    }

    private void Start()
    {
        if (!musicEventInstance.isValid())
        {
            InitializeMusic(FMODEvents.instance.music);
        }

        if (!ambienceEventInstance.isValid())
        {
            InitializeAmbience(FMODEvents.instance.ambience);
        }

        musicEventInstance.setParameterByName("IsShop", isShop ? 1 : 0);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        SFXBus.setVolume(SFXVolume);
        ambienceBus.setVolume(ambienceVolume);
    }

    public void SetVolumes(float master, float music, float sfx, float ambience)
    {
        masterVolume = master;
        musicVolume = music;
        SFXVolume = sfx;
        ambienceVolume = ambience;

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

    public void StopAllSFXEvents()
    {
        for (int i = eventInstances.Count - 1; i >= 0; i--)
        {
            EventInstance instance = eventInstances[i];

            if (instance.getDescription(out EventDescription desc) == FMOD.RESULT.OK &&
                desc.getPath(out string path) == FMOD.RESULT.OK &&
                path.Contains("SFX"))
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
                eventInstances.RemoveAt(i);
            }
        }
    }
}
