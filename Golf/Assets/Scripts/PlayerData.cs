using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins;
    public int totalCoins;
    public int currentLevel;

    public float zoom = 5f;

    public float masterVol = 1f;
    public float musicVol = 1f;
    public float SFXVol = 1f;
    public float ambienceVol = 1f;

    public Dictionary<int, bool> levelPopups = new Dictionary<int, bool>();
    public Dictionary<int, int> upgradeLevels = new Dictionary<int, int>();
    public Dictionary<Hat.TYPE, bool> unlockedHats = new Dictionary<Hat.TYPE, bool>();
    public Dictionary<int, bool> levelsCompleted = new Dictionary<int, bool>();

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

    public List<AbilityChargeData> maxChargesList = new List<AbilityChargeData>();
    public List<ABILITIES> unlockedAbilityTypes = new List<ABILITIES>();

    [NonSerialized] public Sprite hat;
    public string hatName;
    public Hat.TYPE hatType;

    public SerializableColor hatColor;
    public SerializableColor ballColor;

    public bool isColorUnlocked;

    [System.Serializable]
    public struct AbilityChargeData
    {
        public ABILITIES ability;
        public int charges;
    }

    public float teleportRange;

    public bool[] achievements = new bool[(int)Achievement.TYPE.MAX];

    public int numResets;
    
    public List<MapData> mapData = new List<MapData>();

    public PlayerData(Inventory inv)
    {
        coins = inv.coins;
        totalCoins = inv.totalCoins;
        currentLevel = inv.currentLevel;

        zoom = inv.zoom;
        masterVol = inv.masterVol;
        musicVol = inv.musicVol;
        SFXVol = inv.SFXVol;
        ambienceVol = inv.ambienceVol;

        levelPopups = inv.levelPopups;
        upgradeLevels = inv.upgradeLevels;

        campaignHighScore = inv.campaignHighScore;
        campaignCurrScore = inv.campaignCurrScore;

        campSpeedHighScore = inv.campSpeedHighScore;
        campSpeedCurrScore = inv.campSpeedCurrScore;

        campHardHighScore = inv.campHardHighScore;
        campHardCurrScore = inv.campHardCurrScore;

        classicHighScore = inv.classicHighScore;
        classicCurrScore = inv.classicCurrScore;

        classicSpeedHighScore = inv.classicSpeedHighScore;
        classicSpeedCurrScore = inv.classicSpeedCurrScore;

        classicHardHighScore = inv.classicHardHighScore;
        classicHardCurrScore = inv.classicHardCurrScore;

        maxChargesList = inv.maxChargesByType
            .Select(pair => new AbilityChargeData { ability = pair.Key, charges = pair.Value })
            .ToList();

        unlockedHats = inv.unlockedHats;

        hat = inv.hat;
        hatName = inv.hat != null ? inv.hat.name : null;

        hatColor = new SerializableColor(inv.hatColor);

        hatType = inv.hatType;

        unlockedAbilityTypes = inv.unlockedAbilities.Select(a => a.type).ToList();

        levelsCompleted = inv.levelsCompleted;

        ballColor = new SerializableColor(inv.ballColor);
        isColorUnlocked = inv.isColorUnlocked;

        teleportRange = inv.teleportRange;

        campSpeedGoalsBeat = inv.campSpeedGoalsBeat;

        achievements = inv.achievements;
        numResets = inv.numResets;

        mapData = Map.getAll();

}

    public void RestoreHatSprite()
    {
        if (!string.IsNullOrEmpty(hatName))
        {
            // Use LoadAll to support sub-sprites in a sprite sheet
            Sprite[] allHats = Resources.LoadAll<Sprite>("Hats/Hats");
            hat = allHats.FirstOrDefault(s => s.name == hatName);

        }
    }
}

[System.Serializable]
public struct SerializableColor
{
    public float r, g, b, a;

    public SerializableColor(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }

    public Color ToColor()
    {
        return new Color(r, g, b, a);
    }
}
