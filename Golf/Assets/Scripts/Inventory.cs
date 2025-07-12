using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Inventory : MonoBehaviour
{
    Ball ball;
    PopupController popupController;
    GhostRecorder ghostRecorder;

    public List<Ability> unlockedAbilities = new List<Ability>();
    public static int indexOfAbility = 0;

    public int currentLevel = 1;
    public int coins;
    public int totalCoins;
    public int maxAbilities = 3;
    public int abilityCount = 0;

    public float masterVol = 1f;
    public float musicVol = 1f;
    public float SFXVol = 1f;
    public float ambienceVol = 1f;

    public float zoom = 5f;

    public Dictionary<int, bool> levelPopups = new Dictionary<int, bool>();
    public Dictionary<int, int> upgradeLevels = new Dictionary<int, int>();
    public Dictionary<int, bool> levelsCompleted = new Dictionary<int, bool>();

    public Dictionary<ABILITIES, int> maxChargesByType = new Dictionary<ABILITIES, int>()
    {
        { ABILITIES.FREEZE, 1 },
        { ABILITIES.WIND, 1 },
        { ABILITIES.TELEPORT, 1 },
        { ABILITIES.BURST, 1 }
    };

    public Dictionary<Hat.TYPE, bool> unlockedHats = new Dictionary<Hat.TYPE, bool>();

    public Dictionary<int, int> campaignHighScore = new Dictionary<int, int>();
    public Dictionary<int, int> campaignCurrScore = new Dictionary<int, int>();

    public Dictionary<int, float> campSpeedHighScore = new Dictionary<int, float>();
    public Dictionary<int, float> campSpeedCurrScore = new Dictionary<int, float>();

    public Dictionary<int, bool> campSpeedGoalsBeat = new Dictionary<int, bool>();

    public Dictionary<int, int> campHardHighScore = new Dictionary<int, int>();
    public Dictionary<int, int> campHardCurrScore = new Dictionary<int, int>();

    public Dictionary<int, int> classicHighScore = new Dictionary<int, int>();
    public Dictionary<int, int> classicCurrScore = new Dictionary<int, int>();

    public Dictionary<int, float> classicSpeedHighScore = new Dictionary<int, float>();
    public Dictionary<int, float> classicSpeedCurrScore = new Dictionary<int, float>();

    public Dictionary<int, int> classicHardHighScore = new Dictionary<int, int>();
    public Dictionary<int, int> classicHardCurrScore = new Dictionary<int, int>();

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
    private Image coinImage;

    [Header("TextDisplay")]
    [SerializeField] public TextMeshProUGUI selectedAbilityTxt;
    [SerializeField] public TextMeshProUGUI abilityChargesTxt;
    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private TextMeshProUGUI strokeTxt;

    [Header("AbilityImages")]
    [SerializeField] private Image abilityImage;
    [SerializeField] private Sprite[] abilitySprites;

    [Header("Upgrades")]
    [SerializeField] private GameObject upgrades;
    public float teleportRange = 3f;

    [Header("Cosmetics")]
    [SerializeField] GameObject hatPos;
    public Sprite hat;
    public string hatName;
    public Color hatColor;
    public Hat.TYPE hatType;
    public Color ballColor;
    public bool isColorUnlocked;

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

    [Header("Speedrun")]
    public float timer = 0f;
    [SerializeField] private TextMeshProUGUI timerTxt;
    private TextMeshProUGUI bestTimeTxt;

    public List<int> tempCollectedCoins = new List<int>();


    [Header("Achievement Variables")]
    public int numBounces;
    public int numResets;

    [HideInInspector]
    public bool[] achievements = new bool[(int)Achievement.TYPE.MAX];

    private Map.TYPE currentMap;
    private int holeNum;

    private GameObject abilityInterface;

    private void Awake()
    {
        ball = GetComponent<Ball>();
        LoadPlayer();
        popupController = FindObjectOfType<PopupController>();
        DisplayReset();
        DisplayCoins();
    }

    private void Start()
    {

        currentMap = Map.current;
        holeNum = Map.hole;
        abilityInterface = GameObject.Find("AbilityInterface");
        LoadZoom();
        ChangeCoinSprites();
        CheckPopup();
        clearPopups();
        PopulateShop();
        PopulateCharges();
        DisplayCosmetics();
        SetGoal();
        DisplayBestTime();

        equipAbility(indexOfAbility);

        ghostRecorder = gameObject.GetComponent<GhostRecorder>();

        GameObject time = GameObject.Find("Timer");
        if (time != null)
        {
            timerTxt = time.GetComponent<TextMeshProUGUI>();
            if (!GameMode.isAnySpeedrun())
            {
                timerTxt.enabled = false;
            }
            else
            {
                timerTxt.enabled = true;
            }
        }

        if (unlockedAbilities == null || currentMap == Map.TYPE.CLASSIC || unlockedAbilities.Count < 2)
        {
            abilityInterface?.SetActive(false);
        }

        SavePlayer();

    }

    private void DisplayBestTime()
    {
        GameObject bestTimeObj = GameObject.Find("Best Time");

        if (!GameMode.isAnySpeedrun()) { 
            
            bestTimeObj?.SetActive(false); 
            return; 
        }

        if (bestTimeObj != null)
        {
            bestTimeTxt = bestTimeObj.GetComponent<TextMeshProUGUI>();
        }
        MapData currentMap = Map.getCurrent();
        int level = Map.hole;
        GameMode.TYPE mode = GameMode.current;
        List<GhostFrame> ghostFrames = currentMap.getGhostFrames(mode, level);
        if (bestTimeTxt == null || ghostFrames == null) { return; }


        float timeInSeconds = ghostFrames[ghostFrames.Count - 1].GetTime();
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
        bestTimeTxt.text = "Best: " + timeSpan.ToString(@"mm\:ss\.ff");

    }

    public void ClearAchievements()
    {
        achievements = new bool[(int)Achievement.TYPE.MAX];
    }

    public Ability getCurrentAbility()
    {
        if (unlockedAbilities.Count == 0)
        {
            return null;
        }
        return unlockedAbilities[indexOfAbility];
    }

    public void DisplayReset()
    {

        GameObject reset = GameObject.Find("Reset");

        if ((GameMode.current != GameMode.TYPE.FREEPLAY) && !GameMode.isAnySpeedrun()) 
        {

            if (reset != null)
            {
                reset.SetActive(false);
            }

            return; 
        }

        if (reset != null)
        {
            reset.SetActive(true);
        }
    }

    private void SetGoal()
    {
        

        Hole hole = FindObjectOfType<Hole>();
        GameObject timeObject = GameObject.Find("Time To Beat");

        if (timeObject == null)
        {
            return;
        }

        if (!GameMode.isAnySpeedrun()) { timeObject.SetActive(false); return; }

        if (timeObject != null)
        {
            hole.timeToBeatTxt = timeObject.GetComponent<TextMeshProUGUI>();
        }
        if (hole.timeToBeatTxt != null)
        {
            if (!GameMode.isAnySpeedrun())
            {
                hole.timeToBeatTxt.enabled = false;
            }
            else
            {
                hole.timeToBeatTxt.enabled = true;
                if (hole.timeToBeat / 10 >= 1)
                {
                    hole.timeToBeatTxt.text = "Goal: " + hole.timeToBeat.ToString() + ".00";
                }
                else if (hole.timeToBeat != Mathf.Floor(hole.timeToBeat))
                {
                    hole.timeToBeatTxt.text = "Goal: 0" + hole.timeToBeat.ToString() + "0";
                }
                else
                {
                    hole.timeToBeatTxt.text = "Goal: 0" + hole.timeToBeat.ToString() + ".00";
                }
            }


        }
    }

    private void SpeedrunTimer()
    {
        if ((!GameMode.isAnySpeedrun()) || timerTxt == null) { return; }

        System.TimeSpan time = System.TimeSpan.FromSeconds(ghostRecorder.timeElapsed);
        timerTxt.text = time.ToString(@"mm\:ss\.ff");
    }

    private void DisplayCosmetics()
    {
        if (ballColor.a < 1f)
        {
            ballColor = Color.white;
        }
        ball.GetComponent<SpriteRenderer>().color = ballColor;

        if (hatPos == null) { return; }
        
        SpriteRenderer sr = hatPos.GetComponent<SpriteRenderer>();
        if (hatType == Hat.TYPE.NONE) { return; }
        sr.transform.localPosition = new Vector3(hatPositions[(int)hatType - 1].x, hatPositions[(int)hatType - 1].y, -.01f);
        sr.sprite = hat;
        sr.color = hatColor;
    }
    private void ChangeCoinSprites()
    {
        MapData map = Map.get(currentMap);
        if (map.isCoinCollected(holeNum, 1) && coin1 != null && coin1Menu != null)
        {
            SetCoinState(coin1, coin1Menu);
        }
        if (map.isCoinCollected(holeNum, 2) && coin2 != null && coin2Menu != null)
        {
            SetCoinState(coin2, coin2Menu);
        }
        if (map.isCoinCollected(holeNum, 3) && coin3 != null && coin3Menu != null)
        {
            SetCoinState(coin3, coin3Menu);
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
        /*if (popupController.popup != null)
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
        }*/
    }

    private void SetCoinState(GameObject coin, GameObject coinMenu)
    {
        coin.GetComponent<Animator>().enabled = false;
        coin.GetComponent<SpriteRenderer>().sprite = coinCollectedSprite;
        coinMenu.GetComponent<Image>().sprite = coinSprite;
        
    }

    private void DisplayCoins()
    {
        coinImage = GameObject.Find("Coin Image")?.GetComponent<Image>();
        if (GameMode.isAnySpeedrun() || currentMap == Map.TYPE.CLASSIC)
        {
            if (coinImage != null && coinTxt != null)
            {
                coinImage.enabled = false;
                coinTxt.enabled = false;
            }
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.P))
        {
            ClearAchievements();
            SavePlayer();
        }*/

        AbilityManager();
        DisplayAbility();
        SpeedrunTimer();

        //CHANGE TOTAL COINS IF ADD MORE LEVELS!
        if (totalCoins >= 108 && !achievements[(int)Achievement.TYPE.PIXEL_PENNY_PARFECT])
        {
            Achievement.Give(Achievement.TYPE.PIXEL_PENNY_PARFECT);
            SavePlayer();
        }

        if (coinTxt != null)
        {
            coinTxt.text = "" + (coins + tempCollectedCoins.Count).ToString();
            
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
        PlayerData data = SaveSystem.LoadPlayer();

        coins = data != null ? data.coins : 0;
        totalCoins = data != null ? data.totalCoins : 0;
        currentLevel = data != null ? data.currentLevel : 0;

        foreach (VolumeSlider slider in FindObjectsOfType<VolumeSlider>())
        {
            slider.isInitializing = true;
        }


        masterVol = data != null ? data.masterVol : 1;
        musicVol = data != null ? data.musicVol : 1;
        SFXVol = data != null ? data.SFXVol : 1;
        ambienceVol = data != null ? data.ambienceVol : 1;

        Audio.volume(Audio.TYPE.MASTER, masterVol);
        Audio.volume(Audio.TYPE.MUSIC, musicVol);
        Audio.volume(Audio.TYPE.SFX, SFXVol);
        Audio.volume(Audio.TYPE.AMBIENCE, ambienceVol);

        if (data?.levelPopups != null)
        {
            levelPopups = data.levelPopups;
        }
        else
        {
            levelPopups = new Dictionary<int, bool>();
        }
        if (data?.upgradeLevels != null)
        {
            upgradeLevels = data.upgradeLevels;
        }
        else
        {
            upgradeLevels = new Dictionary<int, int>();
        }

        if (data?.campaignHighScore != null)
        {
            campaignHighScore = data.campaignHighScore;
        }
        else
        {
            campaignHighScore = new Dictionary<int, int>();
        }

        if (data?.campaignCurrScore != null)
        {
            campaignCurrScore = data.campaignCurrScore;
        }
        else
        {
            campaignCurrScore = new Dictionary<int, int>();
        }

        if (data?.campSpeedHighScore != null)
        {
            campSpeedHighScore = data.campSpeedHighScore;
        }
        else
        {
            campSpeedHighScore = new Dictionary<int, float>();
        }

        if (data?.campSpeedCurrScore != null)
        {
            campSpeedCurrScore = data.campSpeedCurrScore;
        }
        else
        {
            campSpeedCurrScore = new Dictionary<int, float>();
        }

        if (data?.campHardHighScore != null)
        {

            campHardHighScore = data.campHardHighScore;
        }
        else
        {
            campHardHighScore = new Dictionary<int, int>();
        }

        if (data?.campHardCurrScore != null)
        {
            campHardCurrScore = data.campHardCurrScore;
        }
        else
        {
            campHardCurrScore = new Dictionary<int, int>();
        }

        if (data?.classicSpeedHighScore != null)
        {

            classicSpeedHighScore = data.classicSpeedHighScore;
        }
        else
        {
            classicSpeedHighScore = new Dictionary<int, float>();
        }

        if (data?.classicSpeedCurrScore != null)
        {
            classicSpeedCurrScore = data.classicSpeedCurrScore;
        }
        else
        {
            classicSpeedCurrScore = new Dictionary<int, float>();
        }

        if (data?.classicHighScore != null)
        {
            classicHighScore = data.classicHighScore;
        }
        else
        {
            classicHighScore = new Dictionary<int, int>();
        }

        if (data?.classicCurrScore != null)
        {
            classicCurrScore = data.classicCurrScore;
        }
        else
        {
            classicCurrScore = new Dictionary<int, int>();
        }

        if (data?.classicHardHighScore != null)
        {

            classicHardHighScore = data.classicHardHighScore;
        }
        else
        {
            classicHardHighScore = new Dictionary<int, int>();
        }

        if (data?.classicHardCurrScore != null)
        {
            classicHardCurrScore = data.classicHardCurrScore;
        }
        else
        {
            classicHardCurrScore = new Dictionary<int, int>();
        }

        if (data?.campSpeedGoalsBeat != null)
        {
            campSpeedGoalsBeat = data.campSpeedGoalsBeat;
        }
        else
        {
            campSpeedGoalsBeat = new Dictionary<int, bool>();
        }

        if (data?.maxChargesList != null && data.maxChargesList.Count > 0)
        {
            maxChargesByType = data.maxChargesList.ToDictionary(a => a.ability, a => a.charges);
        }
        else
        {
            maxChargesByType = new Dictionary<ABILITIES, int>()
            {
                { ABILITIES.FREEZE, 1 },
                { ABILITIES.WIND, 1 },
                { ABILITIES.TELEPORT, 1 },
                { ABILITIES.BURST, 1 }
            };
        }

        if (data?.unlockedHats != null)
        {
            unlockedHats = data.unlockedHats;
        }
        else
        {
            unlockedHats = new Dictionary<Hat.TYPE, bool>();
        }

        data?.RestoreHatSprite();
        hat = data?.hat;
        hatColor = data != null ? data.hatColor.ToColor() : Color.white;
        hatType = data != null ? data.hatType : Hat.TYPE.NONE;

        if (data?.teleportRange == 0)
        {
            data.teleportRange = teleportRange;
        }
        teleportRange = data != null ? data.teleportRange : teleportRange;

        unlockedAbilities = new List<Ability>();
        if (data != null)
        {
            if (data.unlockedAbilityTypes == null)
            {
                data.unlockedAbilityTypes = new List<ABILITIES>();
            }
            foreach (var type in data.unlockedAbilityTypes)
            {
                int maxCharges = data.maxChargesList.FirstOrDefault(c => c.ability == type).charges;
                Ability.SetMaxCharges(type, maxCharges);
                Ability ability = Ability.Create(type, Color.white);
                ability.onPickup(ball);
                unlockedAbilities.Add(ability);
            }
        }

        if (data?.levelsCompleted != null)
        {
            levelsCompleted = data.levelsCompleted;
        }
        else
        {
            levelsCompleted = new Dictionary<int, bool>();
        }

        List<MapData> mapData = new List<MapData>();
        if (data?.mapData != null)
        {
            mapData = data.mapData;
        }
        for (int i = 0; i < (int)Map.TYPE.MAX; i++)
        {
            if (i >= mapData.Count) {
                mapData.Add(new MapData((Map.TYPE)i));
            }
            else if (mapData[i] == null)
            {
                mapData[i] = new MapData((Map.TYPE)i);
            }
        }
        Map.setAll(mapData);

        ballColor = data != null ? data.ballColor.ToColor() : Color.white;
        isColorUnlocked = data != null ? data.isColorUnlocked : false;

        if (data?.achievements != null && data.achievements.Length > 0)
        {
            achievements = new bool[(int)Achievement.TYPE.MAX];

            for (int i = 0; i < data.achievements.Length; i++)
            {
                achievements[i] = data.achievements[i];
            }
            
        }
        else
        {
            achievements = new bool[(int)Achievement.TYPE.MAX];
        }

        numResets = data != null ? data.numResets : 0;

    }

    public void ErasePlayerData()
    {
        SaveSystem.ErasePlayerData();
        SaveSystem.EraseZoomData();
        PlayerPrefs.DeleteAll();
    }

    private void AbilityManager()
    {

        if (unlockedAbilities == null || currentMap == Map.TYPE.CLASSIC)
        {
            return;
        }
        
        if (PlayerInput.isDown(PlayerInput.Axis.Fire3) && unlockedAbilities.Count > 0)
        {
            ball.ClearDots();
            unlockedAbilities[indexOfAbility].onUse(ball);
        }
        if (unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].onFrame(ball);
        }


        if (ball.isTeleportReady)
        {
            return;
        }

        if (ball.isBallLocked)
        {
            return;
        }

        if (PlayerInput.isDown(PlayerInput.Axis.SwapUp) && unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].reset(ball);
            equipAbility((indexOfAbility + 1) % unlockedAbilities.Count);
        }
        else if (PlayerInput.isDown(PlayerInput.Axis.SwapDown) && unlockedAbilities.Count > 0)
        {
            unlockedAbilities[indexOfAbility].reset(ball);
            equipAbility((unlockedAbilities.Count - 1 + indexOfAbility) % unlockedAbilities.Count);
        }

    }

    private void equipAbility(int index)
    {
        if (index < 0 || index >= unlockedAbilities.Count)
        {
            index = 0;
        }
        indexOfAbility = index;
    }

    private void DisplayAbility()
    {
        if (unlockedAbilities == null || currentMap == Map.TYPE.CLASSIC)
        {
            return;
        }
        if (unlockedAbilities.Count > 0 && selectedAbilityTxt != null && abilityImage != null)
        {
            selectedAbilityTxt.text = unlockedAbilities[indexOfAbility].getCharges(ball) + "/" + unlockedAbilities[indexOfAbility].getMaxCharges(ball);
            selectedAbilityTxt.color = unlockedAbilities[indexOfAbility].color;
            abilityImage.sprite = abilitySprites[indexOfAbility];
            abilityImage.color = Color.white;
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

        if (unlockedAbilities == null)
        {
            unlockedAbilities = new List<Ability>();
        }

        foreach (Ability abil in unlockedAbilities)
        {
            if (abil.type == ability.type)
            {
                return;
            }
        }
        unlockedAbilities.Add(ability);
        abilityCount++;
        equipAbility(unlockedAbilities.Count - 1);
        ability.onPickup(ball);

        if (unlockedAbilities.Count > 1)
        {
            abilityInterface?.SetActive(true);
        }

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
