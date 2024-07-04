using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        None,
        Freeze,
        Wind,
        Teleport,
        Burst
    }

    public static int GetCost(ItemType itemType)
    {
        switch(itemType)
        {
            default:
                case ItemType.None: return 0;
                case ItemType.Freeze: return 3;
                case ItemType.Wind: return 4;
                case ItemType.Teleport: return 7;
                case ItemType.Burst: return 4;
        }
    }
    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.None: return null;
            case ItemType.Freeze: return GameAssets.instance.freezeAbilitySprite;
            case ItemType.Wind: return GameAssets.instance.windAbilitySprite;
            case ItemType.Teleport: return GameAssets.instance.teleportAbilitySprite;
            case ItemType.Burst: return GameAssets.instance.burstAbilitySprite;
        }
    }
}
