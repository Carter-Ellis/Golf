using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    Ball ball;
    Rigidbody2D rb;
    bool isOnSlope;
    public float steepness = 0.04f;
    private bool enterFromBottom;
    void Start()
    {
        ball = FindObjectOfType<Ball>();
        rb = ball.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        //print("Height: " + ball.GetComponent<Inventory>().currentHeight);
        if (isOnSlope)
        {
            if (rb.velocity.magnitude > 0.4f)
            {
                rb.velocity += steepness * Vector2.down;
            }
            else
            {
                rb.velocity = Vector2.down;
            }     
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            isOnSlope = true;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Collider2D triggerCollider = GetComponent<Collider2D>();
            Bounds triggerBounds = triggerCollider.bounds;

            if (collision.transform.position.y > triggerBounds.max.y)
            {
                enterFromBottom = false;
                print("Enter from bottom: " + enterFromBottom);
            }
            else
            {
                enterFromBottom = true;
                print("Enter from bottom: " + enterFromBottom);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Collider2D triggerCollider = GetComponent<Collider2D>();
            Bounds triggerBounds = triggerCollider.bounds;

            if (collision.transform.position.y > triggerBounds.max.y)
            {
                if (enterFromBottom)
                {
                    ball.GetComponent<Inventory>().currentHeight += 1;
                }
            }
            else
            {
                if (!enterFromBottom)
                {
                    ball.GetComponent<Inventory>().currentHeight -= 1;
                }
            }
            isOnSlope = false;
        }
    }

}
