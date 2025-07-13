using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{

    public Collider2D[] collectors = new Collider2D[0];
    public Coin coin;
    public bool isAchievement;
    public bool isPineapple;
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

            if (isAchievement && !inv.achievements[(int)Achievement.TYPE.CASEOH] && !isPineapple)
            {
                Achievement.Give(Achievement.TYPE.CASEOH);
                inv.SavePlayer();
            }
            else if (isAchievement && !inv.achievements[(int)Achievement.TYPE.WHO_LIVES_IN_PINEAPPLE] && isPineapple)
            {
                Achievement.Give(Achievement.TYPE.WHO_LIVES_IN_PINEAPPLE);
                inv.SavePlayer();
            }

        }
    }

}
