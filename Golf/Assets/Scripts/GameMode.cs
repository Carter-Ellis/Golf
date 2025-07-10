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

    private static string[,] descriptions =
    {
        {//Campaign
            "Play all 18 holes with no resets. Coins are earned only when you score par or less.",
            "Unlock levels and earn coins by finishing with par or less. Reset anytime.",
            "Race the clock through all 18 holes to set the fastest record. Reset anytime.",
            "The club is gone...",
            "Score par or less to advance. No resets.",
        },
        {//Classic
            "Play all 18 holes with no resets.",
            "Unlock levels with par or less. Reset anytime.",
            "Race the clock through all 18 holes to set the fastest record. Reset anytime.",
            "The club is gone...",
            "Score par or less to advance. No resets.",
        },
        {//Beach
            "Play all 18 holes with no resets. Coins are earned only when you score par or less.",
            "Unlock levels and earn coins by finishing with par or less. Reset anytime.",
            "Race the clock through all 18 holes to set the fastest record. Reset anytime.",
            "The club is gone...",
            "Score par or less to advance. No resets.",
        },
    };

    private static string speedrunDescription = "Choose between Regular and Clubless speedruns. Race against the clock and your ghost.";

    private static string[] hardcoreDescription =
    {
        "Collect 40 coins to unlock.",
        "Complete an 18 hole run to unlock.",
        "Collect 40 coins to unlock."
    };

    public static string name(TYPE type)
    {
        return modeNames[(int)type];
    }

    public static string description(TYPE type, Map.TYPE map, bool isSpeedrunSelection)
    {
        string result;
        if ((type == TYPE.SPEEDRUN || type == TYPE.CLUBLESS) && !isSpeedrunSelection)
        {
            result = speedrunDescription;
        }
        else
        {
            result = descriptions[(int)map, (int)type];
        }
        
        if (type == TYPE.HARDCORE && !Map.get(map).isHardcoreUnlocked)
        {
            result += " " + hardcoreDescription[(int)map];
        }
        return result;
    }

    public static TYPE getByName(string name)
    {
        for (int i = 0; i < (int)TYPE.MAX; i++)
        {
            if (name.Equals(modeNames[i]))
            {
                return (TYPE)i;
            }
        }
        return TYPE.HOLE18;
    }

    public static bool isAnySpeedrun()
    {
        return (current == TYPE.SPEEDRUN) || (current == TYPE.CLUBLESS);
    }

}
