using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{

    public static TYPE current;

    public enum TYPE
    {
        HOLE18,
        FREEPLAY,
        SPEEDRUN,
        CLUBLESS,
        HARDCORE,
        MAX
    }

    private static string[] modeNames =
    {
        "18 Holes",
        "Freeplay",
        "Speedrun",
        "Clubless",
        "Hardcore"
    };

    public static string name(TYPE type)
    {
        return modeNames[(int)type];
    }

    public static bool isAnySpeedrun()
    {
        return (current == TYPE.SPEEDRUN) || (current == TYPE.CLUBLESS);
    }

}
