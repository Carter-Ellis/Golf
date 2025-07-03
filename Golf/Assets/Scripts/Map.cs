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

    public static void setAll(List<MapData> data)
    {
        mapData = data;
    }

    public static string name(TYPE type)
    {
        return mapNames[(int)type];
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
