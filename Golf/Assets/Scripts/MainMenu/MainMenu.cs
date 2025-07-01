using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    public Canvas soundMenuCanvas;
    public Canvas mainMenu;
    public bool isActive = true;
    public GameObject userInterface;
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
        CREDITS,
        MAX
    };

    [SerializeField]
    private GameObject[] stateScreens = new GameObject[(int)State.MAX];
    private State currentState = State.MAIN;

    private Map.TYPE currentMap = Map.TYPE.CAMPAIGN;

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
                    GoTo(i);
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
        SceneManager.LoadSceneAsync(Map.name(currentMap) + " " + level);
    }

    public void GoTo(State state)
    {
        if (currentState == state)
        {
            return;
        }

        OnExit(currentState);

        stateScreens[(int)currentState].SetActive(false);
        stateScreens[(int)state].SetActive(true);
        currentState = state;

        OnEnter(state);

    }

    private void OnEnter(State state)
    {
        switch (state)
        {
            case State.SHOP:
                userInterface.SetActive(true);
                AudioManager.instance.PlayShopMusic();
                break;
        }
    }

    private void OnExit(State state)
    {
        switch (state)
        {
            case State.SHOP:
                AudioManager.instance.StartMainMusic();
                userInterface.SetActive(false);
                break;
        }
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
        if (mode < 0 || mode >= (int)GameMode.TYPE.MAX)
        {
            return;
        }

        GameMode.current = (GameMode.TYPE)mode;

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

    public void SetState(State state)
    {
        currentState = state;
    }

    public void ExitRun(int state)
    {

        if (GameMode.current == GameMode.TYPE.HOLE18 || GameMode.current == GameMode.TYPE.HARDCORE)
        {
            GoTo((State)state);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

}
