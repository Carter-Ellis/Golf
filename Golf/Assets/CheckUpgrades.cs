using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUpgrades : MonoBehaviour
{
    public void checkUpgrades()
    {
        int count = 0;
        foreach (RectTransform child in transform)
        {
            int level = child.GetComponent<UpgradeButton>().upgradeLevel;
            if (level == 4)
            {
                count++;
            }
        }
        if (count == 3)
        {
            Achievement.Give(Achievement.TYPE.ALL_MY_POWER);
            FindObjectOfType<Inventory>().SavePlayer();
        }
        
    }

}
