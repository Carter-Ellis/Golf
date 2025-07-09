using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyColors : MonoBehaviour
{
    [SerializeField] int price = 10;
    private Inventory inv;
    void Start()
    {
        inv = FindObjectOfType<Inventory>();
        
    }
    private void Update()
    {
        if (inv.isColorUnlocked)
        {
            Destroy(gameObject);
        }
    }
    public void onClick()
    {
        if (inv.coins >= price)
        {
            inv.isColorUnlocked = true;
            inv.coins -= price;
            //FindObjectOfType<CosmeticsManager>().changeBallColor(0);
            //FindObjectOfType<CosmeticsManager>().changeHat(0);
            FindObjectOfType<CosmeticsManager>().changeBallColor(0);
            inv.SavePlayer();
            Audio.playSFX(FMODEvents.instance.shopPurchase, transform.position);
            Destroy(gameObject);
        }
        else
        {
            Audio.playSFX(FMODEvents.instance.error, transform.position);
        }
        
    }

}
