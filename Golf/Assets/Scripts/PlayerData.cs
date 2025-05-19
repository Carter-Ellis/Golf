using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins;
    public int currentLevel;

    public float zoom = 5f;

    public float masterVol = 1f;
    public float musicVol = 1f;
    public float SFXVol = 1f;
    public float ambienceVol = 1f;

    public Dictionary<int, List<int>> coinsCollected = new Dictionary<int, List<int>>();
    public Dictionary<int, bool> levelPopups = new Dictionary<int, bool>();
    public Dictionary<int, int> upgradeLevels = new Dictionary<int, int>();
    public Dictionary<Hat.TYPE, bool> unlockedHats = new Dictionary<Hat.TYPE, bool>();
    public Dictionary<int, bool> levelsCompleted = new Dictionary<int, bool>();

    public List<AbilityChargeData> maxChargesList = new List<AbilityChargeData>();
    public List<ABILITIES> unlockedAbilityTypes = new List<ABILITIES>();

    [NonSerialized] public Sprite hat;
    public string hatName;
    public Hat.TYPE hatType;

    public SerializableColor hatColor;

    [System.Serializable]
    public struct AbilityChargeData
    {
        public ABILITIES ability;
        public int charges;
    }

    public PlayerData(Inventory inv)
    {
        coins = inv.coins;
        currentLevel = inv.currentLevel;

        zoom = inv.zoom;
        masterVol = inv.masterVol;
        musicVol = inv.musicVol;
        SFXVol = inv.SFXVol;
        ambienceVol = inv.ambienceVol;

        coinsCollected = inv.coinsCollected;
        levelPopups = inv.levelPopups;
        upgradeLevels = inv.upgradeLevels;

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
