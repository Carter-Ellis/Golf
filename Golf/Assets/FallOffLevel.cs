using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallOffLevel : MonoBehaviour
{
    private bool isFalling;
    private Ball ball;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private float bounceForce = 3f;
    private float bounceTimer = 0f;
    private float bounceTime = .15f;
    private bool isBouncing;
    public float gravity = 5f;
    public int level = 1;
    public int levelFallAmount = 1;
    GameObject tilemap;



    void Start()
    {
        ball = FindObjectOfType<Ball>();
        rb = ball.GetComponent<Rigidbody2D>();
        tilemap = GameObject.Find("Foreground");
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            tilemap.GetComponent<TilemapCollider2D>().enabled = false;
            rb.velocity = new Vector2(rb.velocity.x, -gravity);

            if (ball.transform.position.y <= targetPosition.y)
            {
                isFalling = false;
                rb.velocity = new Vector2(rb.velocity.x, bounceForce);
                isBouncing = true;
                
            }
            
        }
        if (!isFalling && isBouncing)
        {
            bounceTimer += Time.deltaTime;
            if (bounceTimer > bounceTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, -gravity / 2.5f);
                if (ball.transform.position.y <= targetPosition.y)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    isBouncing = false;
                    bounceTimer = 0f;
                    ball.GetComponent<Inventory>().currentHeight -= (int)levelFallAmount;
                    tilemap.GetComponent<TilemapCollider2D>().enabled = true;
                }
            }
        }
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            isFalling = true;
            targetPosition = ball.transform.position + Vector3.down * 3 * levelFallAmount;
        }
    }

}
