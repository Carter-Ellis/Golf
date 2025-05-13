using System.Collections.Generic;
using System.Linq;

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
    public List<AbilityChargeData> maxChargesList = new List<AbilityChargeData>();

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
    }
}
