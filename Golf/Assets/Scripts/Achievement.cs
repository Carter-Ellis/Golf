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
        APPLE_A_DAY,
        VAMPIRE,
        ALAKAZAA,
        SPLURSH,
        BIRD_THAT_I_HATE,
        WHO_LIVES_IN_PINEAPPLE,
        MOMENTEMUMS,
        ANNOYING_ORANGE,
        MAUI_2026,
        MR_K,
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
        "Whack-A-Mole",
        "Jordan's Achievement",
        "Is This It?",
        "Golf Cart Jockey",
        "Close Call",
        "All My Power Combine",
        "Plants Vs. Golfballs",
        "Slow There Buddy",
        "CaseOh",
        "Digital Style",
        "Pixel Penny Parfect",
        "The Big One",
        "Back To The Back",
        "How About A Tornado Patrick?",
        "An Apple A Day",
        "Am I A Vampire?",
        "ALAKAZAA!!!",
        "SPLURSHHH!",
        "That Bird That I Hate",
        "Who Lives In A Pineapple",
        "Momentemums",
        "Annoying Orange",
        "Maui 2026",
        "Mr. K",
};

    private static string[] descriptions =
    {
        "Complete a hole without ever hitting the ball. Clubless mode does not count.",
        "Complete a hole with only one stroke.",
        "Complete a Hardcore Campaign run.",
        "Complete all 18 holes in a Campaign run.",
        "Complete a Campaign Speedrun.",
        "Complete a Campaign Speedrun without using any clubs.",
        "Complete Campaign Freeplay mode.",
        "Complete a Hardcore Classic run.",
        "Complete all 18 holes in a Classic run.",
        "Complete a Classic Speedrun.",
        "Complete a Classic Speedrun without using any clubs.",
        "Complete a Classic Freeplay mode.",
        "Complete Campaign hole 11 with par or less without eating the apple on the tee.",
        "Score a hole-in-one on every hole.",
        "Complete Campaign hole 9 with par or less without pressing the button.",
        "Use a bouncer 100 times in one life.",
        "Hit 5 different moles in 1 putt.",
        "Get over par and collect no coins.",
        "On Campaign hole 9 go into the wrong goal.",
        "Get hit 20 times by the same golf cart.",
        "Use the freeze ability next to a spike trap that is attacking.",
        "Max out all of the abilities.",
        "Run into a tree 10 times.",
        "Go too fast over the hole.",
        "Eat all the apples on Campaign hole 10.",
        "Unlock all cosmetics.",
        "Obtain all coins.",
        "Eat the giant apple.",
        "Reset 50 times.",
        "Click on a fan 20 times.",
        "???",
        "???",
        "Use all abilities when they are maxed.",
        "Sink into a watery grave.",
        "Complete a hole without scaring seagulls.",
        "Collect the hidden pineapples.",
        "Break through 10 walls.",
        "Equip the orange ball color and a green hat.",
        "Unlock the beach map.",
        "Hit Mr. K 5 times."

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
        
        if (GameMode.current == GameMode.TYPE.CLUBLESS && (type != Achievement.TYPE.BEAT_CAMP_CLUBLESS || type != Achievement.TYPE.BEAT_CLASSIC_CLUBLESS))
        {
            return;
        }

        int index = (int)type;

        if (!SteamIntegration.IsThisSteamAchUnlocked(type))
        {
            SteamIntegration.UnlockSteamAch(type);
        }
        if (inv.achievements[index])
        {
            return;
        }
        
        
        inv.achievements[index] = true;
        AchievementGet.PlayAchievementGet(type);
    }

}
