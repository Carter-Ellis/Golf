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
        CROWN,
        HALO,
        CONE,
        PROPELLER,
        BERET,
        PIRATE,
        FEZ,
        SOMBRERO,
        COWBOY,
        CONICAL,
        SANTA,
        MAX_HATS
    }

    private static string hatSpriteSheetPath = "Hats/Hats";

    private static Sprite[] sprites = new Sprite[(int)TYPE.MAX_HATS];
    private static bool isLoaded;

    public static Sprite GetSprite(TYPE type)
    {
        loadSprites();
        
        return sprites[(int)type];
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
    }

}
