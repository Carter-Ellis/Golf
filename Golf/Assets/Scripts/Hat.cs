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
        MAX_HATS
    }

    public enum ANIM_TYPE
    {
        NONE,
        FIRE,
        GOLD,
        WINGS,
        MAX_ANIM_HATS
    }

    private static string hatSpriteSheetPath = "Hats/Hats";

    private static string animHatPath = "Hats/AnimatedHats";

    private static Sprite[] sprites = new Sprite[(int)TYPE.MAX_HATS];

    private static GameObject[] animHats = new GameObject[(int)ANIM_TYPE.MAX_ANIM_HATS];

    private static bool isLoaded;

    public static Sprite GetSprite(TYPE type)
    {
        loadSprites();
        
        return sprites[(int)type];
    }

    public static GameObject GetAnimHat(ANIM_TYPE type)
    {
        loadSprites();
        return animHats[(int)type];
    }

    private static void loadSprites()
    {
        if (isLoaded) { return; }
        isLoaded = true;

        Sprite[] spriteSheet = Resources.LoadAll<Sprite>(hatSpriteSheetPath);

        for (int i = 1; i < (int)TYPE.MAX_HATS; i++)
        {
            sprites[i] = spriteSheet[i - 1];
        }

        GameObject[] hats = Resources.LoadAll<GameObject>(animHatPath);

        for (int i = 1; i < (int)ANIM_TYPE.MAX_ANIM_HATS; i++)
        {
            print("CREATING HAT");
            animHats[i] = hats[i - 1];
        }

    }

}
