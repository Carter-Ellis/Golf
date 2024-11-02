using System.Collections;
using System.Collections.Generic;
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
    }
}
