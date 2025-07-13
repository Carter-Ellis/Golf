using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachAchievement : MonoBehaviour
{
    Inventory inv;
    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
    }
    public void unlockedBeachAchievement()
    {
        if (!inv.achievements[(int)Achievement.TYPE.MAUI_2026])
        {
            Achievement.Give(Achievement.TYPE.MAUI_2026);
            inv.SavePlayer();
        }
    }
}
