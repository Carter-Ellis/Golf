using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    Ball ball;
    Inventory inventory;
    public bool isRed;
    public int coinNumber;


    private void Start()
    {
        ball = FindObjectOfType<Ball>();
        inventory = ball.GetComponent<Inventory>();

        GameMode.TYPE mode = GameMode.current;
        if ((mode == GameMode.TYPE.SPEEDRUN) || (mode == GameMode.TYPE.CLUBLESS))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject != null && collision.gameObject.tag == "Ball")
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.coinCollect, transform.position);

            if (isRed)
            {
                inventory.redCoinCount++;
                gameObject.SetActive(false);
                return;
            }

            inventory = ball.GetComponent<Inventory>();

            if (inventory.coinsCollected == null)
            {
                inventory.coinsCollected = new Dictionary<int, List<int>>();
            }
  
            int currentLevel = FindObjectOfType<Hole>().holeNum;

            if (!inventory.coinsCollected.ContainsKey(currentLevel))
            {
                inventory.coinsCollected[currentLevel] = new List<int>();
            }

            if (!inventory.coinsCollected[currentLevel].Contains(coinNumber))
            {
                inventory.tempCollectedCoins.Add(coinNumber);
            }

            gameObject.SetActive(false);
        }
    }
}
