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
        WHACK_A_MOLE,
        JORDAN,
        IS_THIS_IT,
        GOLF_CART_JOCKEY,
        CLOSE_CALL,
        ALL_MY_POWER,
        PLANTS_VS_GOLFBALLS,
        SLOW_THERE_BUDDY,
        CASEOH,
        DIGITAL_STYLE,
        PIXEL_PENNY_PARFECT,
        THE_BIG_ONE,
        BACK_TO_THE_BACK,
        TORNADO,
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
        "Whack-a-mole",
        "Jordan's Achievement",
        "Is This It?",
        "Golf Cart Jockey",
        "Close Call",
        "All My Power Combine",
        "Plants vs. Golfballs",
        "Slow There Buddy",
        "CaseOh",
        "Digital Style",
        "Pixel Penny Parfect",
        "The Big One",
        "Back To The Back",
        "How About A Tornado Patrick?"
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
        "Hit 3 different moles in 1 putt.",
        "Get over par and collect no coins.",
        "On Campaign hole 9 go into the wrong goal.",
        "Get hit 20 times by the same golf cart.",
        "Use the freeze ability next to a spike trap.",
        "Max out abilities.",
        "Run into a tree 10 times.",
        "Go too fast over the hole.",
        "Eat all the apples on Campaign hole 10.",
        "Unlock all cosmetics.",
        "Obtain all coins.",
        "Eat the giant apple.",
        "Reset 50 times.",
        "Click on a fan 100 times."
    };

    public static string GetName(TYPE type)
    {
        return names[(int)type];
    }

    public static string GetDescription(TYPE type)
    {
        return descriptions[(int)type];
    }

    public static void Give(TYPE type)
    {
        Inventory inv = GameObject.FindAnyObjectByType<Inventory>();
        int index = (int)type;
        if (inv.achievements[index])
        {
            return;
        }
        inv.achievements[index] = true;
        AchievementGet.PlayAchievementGet(type);
    }

}
