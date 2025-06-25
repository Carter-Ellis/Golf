using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
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

    public static string Name(TYPE type)
    {
        return mapNames[(int)type];
    }

    public static TYPE getCurrent()
    {
        string name = SceneManager.GetActiveScene().name;
        name = name.Substring(0, name.IndexOf(' '));
        TYPE output = TYPE.MAX;
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

}
