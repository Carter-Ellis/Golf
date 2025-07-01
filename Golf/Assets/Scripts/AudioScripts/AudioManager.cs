using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    private EventInstance mainMusicInstance;
    private EventInstance shopMusicInstance;

    private EventReference currentAmbience;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
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

    private void InitializeMainMusic(EventReference musicEventReference)
    {
        mainMusicInstance = CreateInstance(musicEventReference);
        mainMusicInstance.start();
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {

        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);


        // Ensure eventInstances is not null
        if (eventInstances == null)
            eventInstances = new List<EventInstance>();

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu")
        {
            instance.StartMainMusic();
        }
        else
        {
            instance.StartGameMusic();
        }

        StartAmbience();

    }

    public void StartMainMusic()
    {
        StopMusic();
        InitializeMainMusic(FMODEvents.instance.mainMusic);
    }

    public void StartGameMusic()
    {
        if (musicEventInstance.isValid()) return;

        StopMusic();
        if (Map.current == Map.TYPE.BEACH)
        {
            musicEventInstance = CreateInstance(FMODEvents.instance.beachMusic);
            musicEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
            musicEventInstance.start();
        }
        else
        {
            musicEventInstance = CreateInstance(FMODEvents.instance.music);
            musicEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
            musicEventInstance.start();
        }
        
    }

    public void StopMusic()
    {
        if (mainMusicInstance.isValid())
        {
            mainMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mainMusicInstance.release();
            mainMusicInstance = default;
        }
        if (musicEventInstance.isValid())
        {
            musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicEventInstance.release();
            musicEventInstance = default;
        }
        if (shopMusicInstance.isValid())
        {
            shopMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            shopMusicInstance.release();
            shopMusicInstance = default;
        }
    }


    public void PlayShopMusic()
    {
        StopMusic();

        shopMusicInstance = CreateInstance(FMODEvents.instance.shopMusic);
        shopMusicInstance.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
        shopMusicInstance.start();
    }

    public void StartAmbience()
    {
        EventReference eventRef;

        if (Map.current == Map.TYPE.BEACH)
        {
            eventRef = FMODEvents.instance.beachAmbience;
        }
        else
        {
            eventRef = FMODEvents.instance.ambience;
        }

        if (eventRef.Guid == currentAmbience.Guid)
        {
            return; //Don't start ambience if same
        }
        StopAmbience();
        currentAmbience = eventRef;

        ambienceEventInstance = CreateInstance(eventRef);
        ambienceEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
        ambienceEventInstance.start();
    }


    public void StopAmbience()
    {
        if (ambienceEventInstance.isValid())
        {
            ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambienceEventInstance.release();
            ambienceEventInstance = default;
        }
        currentAmbience = default;
    }

}
