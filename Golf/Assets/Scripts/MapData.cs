using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{

    public Map.TYPE type { get; private set; }
    public string name {  get
        {
            return Map.name(type);
        }
    }
    private bool[,] unlockedLevels = new bool[3, 18];
    private List<GhostFrame>[,] ghostFrames = new List<GhostFrame>[2, 18];
    private bool[,] coinsCollected = new bool[18, 3];

    public bool isHardcoreUnlocked
    {
        get
        {
            if (type == Map.TYPE.CLASSIC)
            {
                Inventory inv = Object.FindAnyObjectByType<Inventory>();
                return inv.classicHighScore != null && inv.classicHighScore.Count == 18;
            }
            else
            {
                return coinsUnlocked >= 40;
            }
        }
    }

    public int coinsUnlocked
    {
        get
        {
            int total = 0;
            for (int i = 0; i < 18; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (coinsCollected[i, j])
                    {
                        total++;
                    }
                }
            }
            return total;
        }
    }

    public MapData(Map.TYPE type)
    {

        this.type = type;

    }

    public bool isCoinCollected(int hole, int coinNum)
    {
        if (hole < 1 || hole > 18 || coinNum < 1 || coinNum > 3)
        {
            return false;
        }
        return coinsCollected[hole - 1, coinNum - 1];
    }

    public void setCoinCollected(int hole, int coinNum, bool value = true)
    {
        if (hole < 1 || hole > 18 || coinNum < 1 || coinNum > 3)
        {
            return;
        }
        coinsCollected[hole - 1, coinNum - 1] = value;
    }

    public bool isLevelUnlocked(GameMode.TYPE mode, int hole)
    {
        return getLevelUnlockedBool(mode, hole);
    }

    public void setLevelUnlocked(GameMode.TYPE mode, int hole, bool isUnlocked = true)
    {
        getLevelUnlockedBool(mode, hole) = isUnlocked;
    }

    private bool blankBool;
    private ref bool getLevelUnlockedBool(GameMode.TYPE mode, int hole)
    {
        if (hole == 1)
        {
            blankBool = true;
            return ref blankBool;
        }
        if (hole < 1 || hole > 18)
        {
            blankBool = false;
            return ref blankBool;
        }
        switch(mode)
        {
            case GameMode.TYPE.SPEEDRUN:
                return ref unlockedLevels[0, hole - 1];
            case GameMode.TYPE.CLUBLESS:
                return ref unlockedLevels[1, hole - 1];
            case GameMode.TYPE.FREEPLAY:
                return ref unlockedLevels[2, hole - 1];
        }

        blankBool = true;
        return ref blankBool;
    }

    public List<GhostFrame> getGhostFrames(GameMode.TYPE mode, int hole)
    {
        return getGhostFrameRef(mode, hole);
    }

    public void setGhostFrames(List<GhostFrame> frames, GameMode.TYPE type, int hole)
    {
        getGhostFrameRef(type, hole) = frames;
    }

    private List<GhostFrame> blankFrames = null;
    private ref List<GhostFrame> getGhostFrameRef(GameMode.TYPE mode, int hole)
    {

        if (hole < 1 || hole > 18)
        {
            return ref blankFrames;
        }
        switch (mode)
        {
            case GameMode.TYPE.SPEEDRUN:
                return ref ghostFrames[0, hole - 1];
            case GameMode.TYPE.CLUBLESS:
                return ref ghostFrames[1, hole - 1];
        }
        return ref blankFrames;
    }

}
