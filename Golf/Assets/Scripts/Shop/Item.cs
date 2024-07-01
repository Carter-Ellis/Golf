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

}
