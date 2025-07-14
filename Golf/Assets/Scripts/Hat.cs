using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hat : MonoBehaviour
{
    public enum TYPE
    {
        NONE,
        NINJA,
        TOPHAT,
        WIZARD,
        FROG,
        VIKING,
        CHEF,
        MUSHROOM,
        HALO,
        CONE,
        PROPELLER,
        BERET,
        PIRATE,
        FEZ,
        SOMBRERO,
        COWBOY,
        STRAW,
        SANTA,
        CROWN,
        FIRE,
        GOLD,
        WINGS,
        MAX_HATS
    }

    private static string[] names =
    {
        "None",
        "Ninja",
        "Top Hat",
        "Wizard",
        "Frog",
        "Viking",
        "Chef",
        "Mushroom",
        "Halo",
        "Traffic Cone",
        "Propeller",
        "Beret",
        "Pirate",
        "Fez",
        "Sombrero",
        "Cowboy",
        "Straw",
        "Santa",
        "Crown",
        "Fire",
        "Shiny Hat",
        "Wings"
    };

    private static string defaultDescription = "Collect all 3 coins in a level to unlock a unique hat! There is one hat per CAMPAIGN level.";

    private static string hatSpriteSheetPath = "Hats/Hats";
    private static string animSpriteSheetPath = "Hats/AnimatedHats/";

    private static Sprite[] sprites = new Sprite[(int)TYPE.MAX_HATS];
    private static RuntimeAnimatorController[] animators = new RuntimeAnimatorController[(int)TYPE.MAX_HATS];
    private static RuntimeAnimatorController[] uiAnimators = new RuntimeAnimatorController[(int)TYPE.MAX_HATS];

    private static bool isLoaded;

    public static Sprite getSprite(TYPE type)
    {
        loadSprites();
        
        return sprites[(int)type];
    }

    public static RuntimeAnimatorController getAnimator(TYPE type, bool isUI)
    {
        loadSprites();
        if (isUI)
        {
            return uiAnimators[(int)type];
        }
        return animators[(int)type];
    }

    public static bool isAnimated(TYPE type)
    {
        switch(type)
        {
            case TYPE.FIRE:
            case TYPE.GOLD:
            case TYPE.WINGS:
                return true;
            default:
                return false;
        }
    }

    public static string getDescription(TYPE type)
    {
        switch (type)
        {
            case TYPE.FIRE:
                return "Complete a hardcore run on campaign to unlock!";
            case TYPE.GOLD:
                return "Get all achievements to unlock!";
            case TYPE.WINGS:
                return "Complete all speedrun levels on campaign to unlock!";
            default:
                return defaultDescription;
        }
    }

    public static string getName(TYPE type)
    {
        return names[(int)type];
    }

    private static void loadSprites()
    {
        if (isLoaded) { return; }
        isLoaded = true;

        Sprite[] spriteSheet = Resources.LoadAll<Sprite>(hatSpriteSheetPath);

        for (int i = 1; i < (int)TYPE.MAX_HATS; i++)
        {
            if (isAnimated((TYPE)i))
            {
                string path = animSpriteSheetPath + ((TYPE)i).ToString().ToLower();
                sprites[i] = Resources.LoadAll<Sprite>(path)[0];
                animators[i] = Resources.Load<RuntimeAnimatorController>(path);
                uiAnimators[i] = Resources.Load<RuntimeAnimatorController>(path + "_ui");
            }
            else
            {
                sprites[i] = spriteSheet[i - 1];
            }
        }

    }

}
