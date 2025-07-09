using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{

    public Collider2D[] collectors = new Collider2D[0];
    public Coin coin;
    public bool isAchievement;
    private bool allCollected = false;
    private Inventory inv;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        if (allCollected) { return; }
        bool notCollected = false;
        for (int i = 0; i < collectors.Length; i++)
        {
            if (collectors[i] != null)
            {
                notCollected = true;
                break;
            }
        }
        allCollected = !notCollected;
        if (allCollected)
        {
            coin.transform.position = this.transform.position;
            Audio.playSFX(FMODEvents.instance.shopPurchase, transform.position);

            if (isAchievement && !inv.achievements[(int)Achievement.TYPE.CASEOH])
            {
                Achievement.Give(Achievement.TYPE.CASEOH);
                inv.SavePlayer();
            }

        }
    }

}
