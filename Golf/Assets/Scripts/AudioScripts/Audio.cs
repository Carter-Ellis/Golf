using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class Audio : MonoBehaviour
{

    public enum TYPE
    {
        MASTER,
        MUSIC,
        SFX,
        AMBIENCE,
        MAX
    }

    private static Audio instance = null;

    private static float[] volumes = new float[(int)TYPE.MAX];
    private static Bus[] buses = new Bus[(int)TYPE.MAX];
    private static EventInstance[] events = new EventInstance[(int)TYPE.MAX];
    private static EventReference[] currentRef = new EventReference[(int)TYPE.MAX];

    private static string[] busPaths =
    {
        "bus:/",
        "bus:/Music",
        "bus:/SFX",
        "bus:/Ambience",
    };
    private static bool isBusSet = false;

    private static FMODEvents fmodEvents;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        fmodEvents = FindObjectOfType<FMODEvents>();

        clearVariables();
        setBuses();

        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

    }

    private static void clearVariables()
    {

        isBusSet = false;
        for (int i = 0; i < (int)TYPE.MAX; i++)
        {
            events[i] = default;
            currentRef[i] = default;
        }

    }

    private static void setBuses()
    {

        if (isBusSet) { return; }
        isBusSet = true;

        for (int i = 0; i < (int)TYPE.MAX; i++)
        {
            buses[i] = RuntimeManager.GetBus(busPaths[i]);
        }

    }

    public static void playMainMusic()
    {
        play(TYPE.MUSIC, fmodEvents.mainMusic);
    }

    public static void playGameMusic()
    {
        EventReference sound = fmodEvents.music;
        if (Map.current == Map.TYPE.BEACH)
        {
            sound = fmodEvents.beachMusic;
        }
        play(TYPE.MUSIC, sound);
    }

    public static void playShopMusic()
    {
        play(TYPE.MUSIC, fmodEvents.shopMusic);
    }

    public static void playAmbienceMusic()
    {
        EventReference sound = fmodEvents.ambience;
        if (Map.current == Map.TYPE.BEACH)
        {
            sound = fmodEvents.beachAmbience;
        }
        play(TYPE.AMBIENCE, sound);
    }

    public static float volume(TYPE type, float value = -1)
    {
        if (value < 0)
        {
            return volumes[(int)type];
        }
        setBuses();
        volumes[(int)type] = Mathf.Clamp01(value);
        buses[(int)type].setVolume(volumes[(int)type]);
        return value;
    }

    private static void play(TYPE type, EventReference eventRef, Vector3 pos = default)
    {
        if (type == TYPE.SFX)
        {
            RuntimeManager.PlayOneShot(eventRef, pos);
            return;
        }
        if (currentRef[(int)type].Guid == eventRef.Guid)
        {
            return; //Don't restart music/ambience
        }
        stop(type);

        EventInstance eventInst = RuntimeManager.CreateInstance(eventRef);
        eventInst.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
        eventInst.start();
        events[(int)type] = eventInst;
        currentRef[(int)type] = eventRef;
    }

    public static void playSFX(EventReference eventRef, Vector3 pos)
    {
        play(TYPE.SFX, eventRef, pos);
    }

    private static void stop(TYPE type)
    {
        EventInstance eventInst = events[(int)type];
        if (!eventInst.isValid()) { return; }

        eventInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInst.release();
        events[(int)type] = default;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu")
        {
            playMainMusic();
        }
        else
        {
            playGameMusic();
        }

        playAmbienceMusic();

    }

}
