using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Canvas soundMenuCanvas;
    public Canvas mainMenu;
    public bool isActive = true;
    public GameObject userInterface;
    private GameObject mainCursor;
    private bool inShop = false;
    [SerializeField] private GameObject selectedMapTxt;
    [SerializeField] private GameObject mapTitleTxt;

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

    public static Map.TYPE selectedMap = Map.TYPE.CAMPAIGN;

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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Play(int level)
    {
        SceneManager.LoadSceneAsync(Map.name(selectedMap) + " " + level);
    }

    public void GoTo(State state)
    {
        if (currentState == state)
        {
            return;
        }

        if ((currentState == State.SHOP || currentState == State.COSMETICS) && (state == State.SHOP || state == State.COSMETICS))
        {
            inShop = true;
        }
        else
        {
            inShop = false;
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
                if (!inShop)
                {
                    userInterface.SetActive(true);
                    Audio.playShopMusic();

                }
                break;
            case State.COSMETICS:
                if (!inShop)
                {
                    userInterface.SetActive(true);
                    Audio.playShopMusic();

                }
                
                break;
            case State.MODE_SELECT:
                var hardcoreButton = GameObject.Find("Hardcore").GetComponent<UnityEngine.UI.Button>();
                Image lockImage = hardcoreButton.transform.GetChild(0).GetComponent<Image>();
                bool hardcoreAllowed = Map.get(selectedMap).isHardcoreUnlocked;
                hardcoreButton.interactable = hardcoreAllowed;
                lockImage.enabled = !hardcoreAllowed;

                mapTitleTxt.SetActive(true);
                selectedMapTxt.SetActive(true);
                selectedMapTxt.GetComponent<TextMeshProUGUI>().text = Map.name(selectedMap);

                break;
            case State.LEVEL_SELECT:
                var coinTxt = GameObject.Find("TotalCoinsTxt")?.GetComponent<TextMeshProUGUI>();
                if (coinTxt != null)
                {
                    int coins = Map.get(selectedMap).coinsUnlocked;
                    coinTxt.text = coins + "/" + 3 * 18;
                }
                mapTitleTxt.SetActive(true);
                selectedMapTxt.SetActive(true);
                selectedMapTxt.GetComponent<TextMeshProUGUI>().text = Map.name(selectedMap);
                break;
            case State.SPEEDRUN_SELECT:
                mapTitleTxt.SetActive(true);
                selectedMapTxt.SetActive(true);
                selectedMapTxt.GetComponent<TextMeshProUGUI>().text = Map.name(selectedMap);
                break;
            case State.MAP_SELECT:
                var beachButton = GameObject.Find("Beach").GetComponent<UnityEngine.UI.Button>();
                Image beachLockImage = beachButton.transform.GetChild(0).GetComponent<Image>();
                bool beachUnlocked = Map.isBeachUnlocked;
                beachButton.interactable = beachUnlocked;
                beachLockImage.enabled = !beachUnlocked;
                break;
        }
    }

    private void OnExit(State state)
    {
        mapTitleTxt.SetActive(false);
        selectedMapTxt.SetActive(false);

        if (inShop)
        {
            return;
        }
        switch (state)
        {
            case State.SHOP:
                Audio.playMainMusic();
                userInterface.SetActive(false);
                break;
            case State.COSMETICS:
                Audio.playMainMusic();
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

    public static void SetMap(int map)
    {
        if (map < 0 || map >= (int)Map.TYPE.MAX)
        {
            return;
        }

        selectedMap = (Map.TYPE)map;

    }

    public static Map.TYPE GetMap()
    {
        return selectedMap;
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

    public State GetState()
    {
        return currentState;
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
