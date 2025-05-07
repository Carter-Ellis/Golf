using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    Ball ball;
    PopupController popupController;
    public List<Ability> unlockedAbilities = new List<Ability>();
    public int indexOfAbility = 0;

    public int currentLevel = 1;
    public int coins;
    public int maxAbilities = 3;
    public int abilityCount = 0;

    public float masterVol = 1f;
    public float musicVol = 1f;
    public float SFXVol = 1f;
    public float ambienceVol = 1f;

    public float zoom = 5f;

    public Dictionary<int, List<int>> coinsCollected = new Dictionary<int, List<int>>();
    public Dictionary<int, bool> levelPopups = new Dictionary<int, bool>();
    public List<int> heightLevels = new List<int>() { 0, 1, 2, 3 };
    public int currentHeight = 0;

    [Header("Coins")]
    public GameObject coin1;
    public GameObject coin2;
    public GameObject coin3;
    public int redCoinCount;

    [Header("Coins Menu")]
    public GameObject coin1Menu;
    public GameObject coin2Menu;
    public GameObject coin3Menu;

    [Header("Coin Sprites")]
    public Sprite coinCollectedSprite;
    public Sprite coinSprite;

    [Header("TextDisplay")]
    [SerializeField] public TextMeshProUGUI selectedAbilityTxt;
    [SerializeField] public TextMeshProUGUI abilityChargesTxt;
    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private TextMeshProUGUI strokeTxt;

    public Inventory()
    {
        unlockedAbilities = new List<Ability>();
    }
    private void Start()
    {
        popupController = FindObjectOfType<PopupController>();
        ball = GetComponent<Ball>();

        LoadPlayer();
        LoadZoom();
        ChangeCoinSprites();
        CheckPopup();
        clearPopups();
        
        
    }
    private void ChangeCoinSprites()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (coinsCollected != null && coinsCollected.ContainsKey(currentLevel))
        {
            if (coinsCollected[currentLevel].Contains(1) && coin1 != null && coin1Menu != null)
            {
                SetCoinState(coin1, coin1Menu);
            }
            if (coinsCollected[currentLevel].Contains(2) && coin2 != null && coin2Menu != null)
            {
                SetCoinState(coin2, coin2Menu);
            }
            if (coinsCollected[currentLevel].Contains(3) && coin3 != null && coin3Menu != null)
            {
                SetCoinState(coin3, coin3Menu);
            }
        }
    }

    private void CheckPopup()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            return;
        }
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (popupController.popup != null)
        {
            ball.enabled = false;
        }
        
        if (levelPopups != null && levelPopups.ContainsKey(currentLevel))
        {
            if (levelPopups[currentLevel] == true)
            {
                popupController.disablePopup();
                ball.enabled = true;
            }
            else
            {
                levelPopups[currentLevel] = true;
            }
        }
        else if (levelPopups == null)
        {
            levelPopups = new Dictionary<int, bool>();
            levelPopups[currentLevel] = true;
        }
        else
        {
            levelPopups[currentLevel] = true;
        }
        SavePlayer();
    }

    private void SetCoinState(GameObject coin, GameObject coinMenu)
    {
        coin.GetComponent<Animator>().enabled = false;
        coin.GetComponent<SpriteRenderer>().sprite = coinCollectedSprite;
        coinMenu.GetComponent<Image>().sprite = coinSprite;
        
    }

    private void Update()
    {
        AbilityManager();
        DisplayAbility();
        if (coinTxt != null)
        {
            coinTxt.text = "" + coins;
        }
        if (ball == null)
        {
            return;
        }
        if (strokeTxt != null)
        {
            strokeTxt.text = "Stroke " + ball.strokes;
        }

    }

    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        int nextLevelIndex = currentIndex + 1;
        if (nextLevelIndex < sceneCount)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
        
    }
    public void LoadZoom()
    {
        float zoomData = SaveSystem.LoadZoom();
        zoom = zoomData;
    }
    public void ReloadLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
        LoadZoom();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        ChangeCoinSprites();
    }
    public void LoadPlayer()
    {
        //ErasePlayerData();
        print("Player Loaded");
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            Debug.Log("No data found when loaded.");
            return;
        }

        coins = data.coins;
        currentLevel = data.currentLevel;

        foreach (VolumeSlider slider in FindObjectsOfType<VolumeSlider>())
        {
            slider.isInitializing = true;
        }
        

        masterVol = data.masterVol;
        musicVol = data.musicVol;
        SFXVol = data.SFXVol;
        ambienceVol = data.ambienceVol;

        AudioManager.instance.masterVolume = masterVol;
        AudioManager.instance.musicVolume = musicVol;
        AudioManager.instance.SFXVolume = SFXVol;
        AudioManager.instance.ambienceVolume = ambienceVol;

        coinsCollected = data.coinsCollected;

        levelPopups = data.levelPopups;

    }

    public void ErasePlayerData()
    {
        SaveSystem.ErasePlayerData();
        SaveSystem.EraseZoomData();
        PlayerPrefs.DeleteAll();
    }

    private void AbilityManager()
    {

        if (unlockedAbilities == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].reset(ball);
            indexOfAbility = (indexOfAbility + 1) % unlockedAbilities.Count;
        }
        if (Input.GetKeyDown(KeyCode.Q) && unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].reset(ball);
            indexOfAbility = (unlockedAbilities.Count - 1 + indexOfAbility) % unlockedAbilities.Count;
        }

        if (Input.GetKeyDown(KeyCode.Space) && unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].onUse(ball);
        }
        if (unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].onFrame(ball);
        }
    }

    private void DisplayAbility()
    {
        if (unlockedAbilities == null)
        {
            return;
        }
        if (unlockedAbilities.Count > 0)
        {
            selectedAbilityTxt.text = "Ability: " + unlockedAbilities[indexOfAbility].name;
            selectedAbilityTxt.color = unlockedAbilities[indexOfAbility].color;
            //abilityChargesTxt.text = unlockedAbilities[indexOfAbility].chargeName + ": " + unlockedAbilities[indexOfAbility].getCharges(ball) + " of " + unlockedAbilities[indexOfAbility].getMaxCharges(ball);
            //abilityChargesTxt.color = unlockedAbilities[indexOfAbility].color;
        }
    }

    public void AddAbility(Ability ability)
    {
        
        if (ability == null)
        {
            return;
        }

        foreach (Ability abil in unlockedAbilities)
        {
            if (abil.type == ability.type)
            {
                return;
            }
        }
        unlockedAbilities.Add(ability);
        indexOfAbility = unlockedAbilities.Count - 1;
        abilityCount++;
        
        ability.onPickup(ball);

    }

    public void RechargeAbility(ABILITIES type)
    {
        if (unlockedAbilities == null)
        {
            return;
        }
        foreach (Ability ability in unlockedAbilities)
        {
            if (ability.type == type)
            {
                ability.onRecharge(ball);
                break;
            }
        }
    }

    private void clearPopups()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (levelName != "Main Menu") { return; }

        foreach (var levelNumber in levelPopups.Keys.ToList())
        {
            levelPopups[levelNumber] = false;
        }
        SavePlayer();
    }

}
