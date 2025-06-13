using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    public enum TYPE
    {
        HOLEINNONE,
        HOLEINONE,
        BEAT_CAMP_HARDCORE,
        BEAT_CAMP_18,
        BEAT_CAMP_SPEEDRUN,
        BEAT_CAMP_CLUBLESS,
        BEAT_CAMP_FREEPLAY,
        BEAT_CLASSIC_HARDCORE,
        BEAT_CLASSIC_18,
        BEAT_CLASSIC_SPEEDRUN,
        BEAT_CLASSIC_CLUBLESS,
        BEAT_CLASSIC_FREEPLAY,
        APPLE_TEE,
        ALL_HOLE_IN_ONE,
        HOLE9_GUESS,
        BOUNCER_USE,
        MAX
    }

    private static string[] names =
{
        "Hole In None",
        "Hole In One",
        "Hardcore Campaign",
        "Campaign 18 Holes",
        "Campaign Speedrun",
        "Campaign Clubless",
        "Campaign Freeplay",
        "Classic Hardcore",
        "Classic 18 Holes",
        "Classic Speedrun",
        "Classic Clubless",
        "Classic Freeplay",
        "Appleooza",
        "All Holes In One",
        "Lucky Guess",
        "Pinball Wizard",
};

    private static string[] descriptions =
    {
        "Complete a hole without ever hitting the ball. Clubless mode does not count.",
        "Complete a hole with only one stroke.",
        "Complete a hardcore campaign run.",
        "Complete all 18 holes in a campaign run.",
        "Complete a campaign run within the time limit.",
        "Complete a campaign run without using any clubs.",
        "Complete a campaign run in freeplay mode.",
        "Complete a hardcore run of a classic course.",
        "Complete all 18 holes in a classic course.",
        "Complete a classic course within the time limit.",
        "Complete a classic course without using any clubs.",
        "Complete a classic course in freeplay mode.",
        "Complete hole 11 without eating the apple on the tee.",
        "Score a hole-in-one on every hole.",
        "Complete hole 9 without pressing the button.",
        "Use a bouncer 100 times in one life.",
};

    public static string GetName(TYPE type)
    {
        return names[(int)type];
    }

    public static string GetDescription(TYPE type)
    {
        return descriptions[(int)type];
    }

}
