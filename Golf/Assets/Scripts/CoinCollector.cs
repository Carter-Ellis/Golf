using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{

    public Collider2D[] collectors = new Collider2D[0];
    public Coin coin;
    private bool allCollected = false;

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
            AudioManager.instance.PlayOneShot(FMODEvents.instance.shopPurchase, transform.position);
        }
    }

}
