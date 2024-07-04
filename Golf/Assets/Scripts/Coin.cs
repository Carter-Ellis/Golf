using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Ball ball;
    Inventory inventory;

    private void Start()
    {
        ball = FindObjectOfType<Ball>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.tag == "Ball")
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.coinCollect, transform.position);
            inventory = ball.GetComponent<Inventory>();
            inventory.coins++;
            Destroy(gameObject);
        }
    }
}
