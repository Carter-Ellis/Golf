using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map
{
    public enum TYPE
    {
        CAMPAIGN,
        CLASSIC,
        BEACH,
        MAX
    }

    private static string[] mapNames =
    {
        "Campaign",
        "Classic",
        "Beach",
    };

    private static string[] descriptions =
    {
        "Play through the main set of golf levels. Use abilities to overcome obstacles and collect coins.",
        "Classic mini golf levels. No abilities or coins.",
        "Play through beach themed levels. Use abilities to overcome tropical obstacles and collect coins.",
    };

    private static string[] mapUnlockDescriptions =
    {
        "",
        "",
        "Complete a Campaign 18 hole run to unlock."
    };

    public static bool isBeachUnlocked
    {
        
        get
        {
            Inventory inv = Object.FindAnyObjectByType<Inventory>();
            if (inv.campaignHighScore.Count >= 18)
            {
                return true;
            }
            return false;
        }
    }

    private static List<MapData> mapData = new List<MapData>((int)Map.TYPE.MAX);

    public static MapData get(Map.TYPE type)
    {
        return mapData[(int)type];
    }

    public static MapData getCurrent()
    {
        return get(current);
    }

    public static List<MapData> getAll()
    {
        return mapData;
    }

    public static TYPE getByName(string name)
    {
        for (int i = 0; i < (int)TYPE.MAX; i++)
        {
            if (name.Equals(mapNames[i]))
            {
                return (TYPE)i;
            }
        }
        return TYPE.CAMPAIGN;
    }

    public static void setAll(List<MapData> data)
    {
        mapData = data;
    }

    public static string name(TYPE type)
    {
        return mapNames[(int)type];
    }

    public static string description(TYPE type)
    {
        string result = descriptions[(int)type];

        // Beach not unlocked
        if (type == TYPE.BEACH && !isBeachUnlocked)
        {
            result += " " + mapUnlockDescriptions[(int)type];
        }

        return result;
    }

    public static TYPE current
    {
        get { return getCurrentMap(); }
    }

    public static int hole
    {
        get { return getHole(); }
    }

    private static TYPE getCurrentMap()
    {
        string name = SceneManager.GetActiveScene().name;
        name = name.Substring(0, name.IndexOf(' '));
        TYPE output = TYPE.CAMPAIGN;
        for (int i = 0; i < mapNames.Length; i++)
        {
            if (name.Equals(mapNames[i]))
            {
                output = (TYPE)i;
                break;
            }
        }
        return output;
    }

    private static int getHole()
    {
        string name = SceneManager.GetActiveScene().name;
        int numIndex = name.LastIndexOf(' ') + 1;
        if (numIndex >= name.Length)
        {
            return 0;
        }
        string level = name.Substring(numIndex, name.Length - numIndex);
        try
        {
            return int.Parse(level);
        }
        catch (System.Exception)
        {
            return 0;
        }
    }

}
