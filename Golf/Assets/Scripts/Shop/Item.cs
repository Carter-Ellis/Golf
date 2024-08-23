using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Freeze,
        Wind,
        Teleport,
        Burst,
        MAX
    }

    public static int GetCost(ItemType itemType)
    {
        switch(itemType)
        {
            
            case ItemType.Freeze: return 3;
            case ItemType.Wind: return 4;
            case ItemType.Teleport: return 7;
            case ItemType.Burst: return 4;
            default: return 0;
        }
    }
    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {      
            case ItemType.Freeze: return GameAssets.instance.freezeAbilitySprite;
            case ItemType.Wind: return GameAssets.instance.windAbilitySprite;
            case ItemType.Teleport: return GameAssets.instance.teleportAbilitySprite;
            case ItemType.Burst: return GameAssets.instance.burstAbilitySprite;
            default: return null;
        }
    }

    public static string GetName(ItemType itemType)
    {
        if (IsAbility(itemType))
        {
            return Ability.GetName(GetAbility(itemType));
        }
        switch (itemType)
        {
            default: return "";
        }
    }

    public static string GetDescription(ItemType itemType)
    {
        if (IsAbility(itemType))
        {
            return Ability.GetDescription(GetAbility(itemType));
        }
        switch (itemType)
        {
            default: return "";
        }
    }

    public static ABILITIES GetAbility(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Freeze: return ABILITIES.FREEZE;
            case ItemType.Wind: return ABILITIES.WIND;
            case ItemType.Teleport: return ABILITIES.TELEPORT;
            case ItemType.Burst: return ABILITIES.BURST;
            default: return ABILITIES.FREEZE;
        }
    }

    public static bool IsAbility(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Freeze:
            case ItemType.Wind: 
            case ItemType.Teleport:
            case ItemType.Burst: 
                return true;
            default: return false;
        }
    }

}
