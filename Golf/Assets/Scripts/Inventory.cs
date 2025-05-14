using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using Unity.VisualScripting;

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

    public Dictionary<int, int> upgradeLevels = new Dictionary<int, int>();

    public Dictionary<ABILITIES, int> maxChargesByType = new Dictionary<ABILITIES, int>()
    {
        { ABILITIES.FREEZE, 1 },
        { ABILITIES.WIND, 1 },
        { ABILITIES.TELEPORT, 1 },
        { ABILITIES.BURST, 1 }
    };

    public Dictionary<Hat.TYPE, bool> unlockedHats = new Dictionary<Hat.TYPE, bool>();


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

    [Header("Upgrades")]
    [SerializeField] private GameObject upgrades;

    [Header("Cosmetics")]
    [SerializeField] GameObject hatPos;
    public Sprite hat;
    public string hatName;
    public Color hatColor;
    public Hat.TYPE hatType;

    private Vector2[] hatPositions = new Vector2[]
    {
        new Vector2(-.09369f,.28132f),
        new Vector2(0,.375f),
        new Vector2(0,.406f),
        new Vector2(0,.34385f),
        new Vector2(0,.406f),
        new Vector2(0,.468f),
        new Vector2(0,.312f),
        new Vector2(0,.55f),
        new Vector2(0,.437f),
        new Vector2(0,.406f),
        new Vector2(.09379f,.375f),
        new Vector2(0,.5f),
        new Vector2(.09372f,.34379f),
        new Vector2(0,.406f),
        new Vector2(0,.406f),
        new Vector2(0,.375f),
        new Vector2(.064f,.375f),
    };

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
        PopulateShop();
        PopulateCharges();
        DisplayHat();
        SavePlayer();
    }

    private void DisplayHat()
    {
        if (hatPos == null) { return; }
        
        SpriteRenderer sr = hatPos.GetComponent<SpriteRenderer>();
        if (hatType == Hat.TYPE.NONE) { return; }
        sr.transform.localPosition = new Vector3(hatPositions[(int)hatType - 1].x, hatPositions[(int)hatType - 1].y, -.01f);
        sr.sprite = hat;
        sr.color = hatColor;
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

    private void CheckHats()
    {
        for (int i = 1; i < (int)Hat.TYPE.MAX_HATS; i++)
        {
            print(unlockedHats[(Hat.TYPE)i]);
        }
        for (int i = 1; i < (int)Hat.TYPE.MAX_HATS; i++)
        {
            unlockedHats[(Hat.TYPE)i] = false;
        }

    }

    private void PopulateCharges()
    {
        Ability.maxChargesByType = maxChargesByType;
    }

    private void PopulateShop()
    {
        if (upgrades == null || upgrades.transform.childCount < 1) { return; }

        foreach (Transform child in upgrades.transform)
        {
            UpgradeButton upgrade = child.GetComponent<UpgradeButton>();
            int index = upgrade.transform.GetSiblingIndex();
            if (!upgradeLevels.ContainsKey(index)) continue;
            upgrade.upgradeLevel = upgradeLevels[upgrade.transform.GetSiblingIndex()];
            int count = 0;
            foreach (Image image in upgrade.progressSquares)
            {
                if (count < upgrade.upgradeLevel)
                {
                    image.sprite = upgrade.purchasedSquare;
                    count++;
                }
                
            }
            

        }
    }

    private void CheckPopup()
    {
        if (popupController == null || SceneManager.GetActiveScene().name == "Main Menu")
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
        if (data.upgradeLevels != null)
        {
            upgradeLevels = data.upgradeLevels;
        }

        if (data.maxChargesList != null && data.maxChargesList.Count > 0)
        {
            maxChargesByType = data.maxChargesList.ToDictionary(a => a.ability, a => a.charges);
        }
        else
        {
            Debug.LogWarning("maxChargesList was null or empty — initializing defaults.");
            maxChargesByType = new Dictionary<ABILITIES, int>()
            {
                { ABILITIES.FREEZE, 1 },
                { ABILITIES.WIND, 1 },
                { ABILITIES.TELEPORT, 1 },
                { ABILITIES.BURST, 1 }
            };
        }

        unlockedHats = data.unlockedHats;

        data.RestoreHatSprite();
        hat = data.hat;
        hatColor = data.hatColor.ToColor();
        hatType = data.hatType;
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
            selectedAbilityTxt.text = unlockedAbilities[indexOfAbility].getCharges(ball) + "/" + unlockedAbilities[indexOfAbility].getMaxCharges(ball) + " " + unlockedAbilities[indexOfAbility].name;
            selectedAbilityTxt.color = unlockedAbilities[indexOfAbility].color;
            //abilityChargesTxt.text = unlockedAbilities[indexOfAbility].getCharges(ball) + "/" + unlockedAbilities[indexOfAbility].getMaxCharges(ball);
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
    }

}
