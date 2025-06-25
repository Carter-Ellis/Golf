using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public Canvas soundMenuCanvas;
    public Canvas mainMenu;
    public bool isActive = true;
    private GameObject mainCursor;

    public enum State
    {
        MAIN,
        OPTIONS,
        SHOP,
        COSMETICS,
        ACHIEVEMENTS,
        RECORDS,
        MODE_SELECT,
        LEVEL_SELECT,
        MAP_SELECT,
        SPEEDRUN_SELECT,
        HOW_TO_PLAY,
        MAX
    };

    [SerializeField]
    private GameObject[] stateScreens = new GameObject[(int)State.MAX];
    private State currentState = State.MAIN;



    private Map.TYPE currentMap = Map.TYPE.CAMPAIGN;

    public enum Mode
    {
        HOLE18,
        FREEPLAY,
        SPEEDRUN,
        CLUBLESS,
        HARDCORE,
        MAX
    }

    private Mode currentMode = Mode.HOLE18;

    private void Start()
    {
        mainCursor = GameObject.FindAnyObjectByType<CursorController>()?.gameObject;
        if (stateScreens != null)
        {
            for (int i = 0; i < stateScreens.Length; i++)
            {
                GameObject obj = stateScreens[i]?.gameObject;
                if (obj != null && obj.activeSelf)
                {
                    currentState = (State)i;
                    break;
                }
            }
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Play(int level)
    {
        Inventory inv = GameObject.FindObjectOfType<Inventory>();
        bool isCamp = (currentMap == Map.TYPE.CAMPAIGN);
        bool isClassic = (currentMap == Map.TYPE.CLASSIC);
        inv.isFreeplayMode = currentMode == Mode.FREEPLAY;
        inv.isWalkMode = currentMode == Mode.CLUBLESS;
        inv.isCampaignMode = isCamp && (currentMode == Mode.HOLE18);
        inv.isClassicMode = isClassic && (currentMode == Mode.HOLE18);
        inv.isCampSpeedMode = isCamp && (currentMode == Mode.SPEEDRUN || currentMode == Mode.CLUBLESS);
        inv.isClassicSpeedMode = isClassic && (currentMode == Mode.SPEEDRUN || currentMode == Mode.CLUBLESS);
        inv.isCampHardMode = isCamp && (currentMode == Mode.HARDCORE);
        inv.isClassicHardMode = isClassic && (currentMode == Mode.HARDCORE);
        inv.SavePlayer();

        SceneManager.LoadSceneAsync(Map.Name(currentMap) + " " + level);

    }

    public void GoTo(State state)
    {
        if (currentState == state)
        {
            return;
        }

        stateScreens[(int)currentState].SetActive(false);
        stateScreens[(int)state].SetActive(true);
        currentState = state;

    }

    public void GoTo(int state)
    {
        if (state < 0 || state >= (int)State.MAX)
        {
            return;
        }

        GoTo((State)state);

    }

    public void SetMode(int mode)
    {
        if (mode < 0 || mode >= (int)Mode.MAX)
        {
            return;
        }

        currentMode = (Mode)mode;

    }

    public Mode GetMode()
    {
        return currentMode;
    }

    public void SetMap(int map)
    {
        if (map < 0 || map >= (int)Map.TYPE.MAX)
        {
            return;
        }

        currentMap = (Map.TYPE)map;

    }

    public Map.TYPE GetMap()
    {
        return currentMap;
    }

    public void DisplayOptions()
    {
        Ball ball = FindObjectOfType<Ball>();
        mainMenu.enabled = !mainMenu.enabled;
        isActive = !isActive;
        GameObject soundMenu = soundMenuCanvas.gameObject;
        soundMenu.SetActive(!soundMenu.activeSelf);
        mainCursor?.SetActive(!mainCursor.activeSelf);
        if (ball != null)
        {
            ball.isBallLocked = soundMenu.activeSelf;
        }
    }

}
