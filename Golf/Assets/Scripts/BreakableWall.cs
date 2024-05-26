using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private Ball ball;
    private Rigidbody2D ballRB;
    private Rigidbody2D rb;
    private float threshold = 8;
    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        ballRB = ball.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        
        float ballSpeed = calculateIncomingSpeed();
        if (collision.gameObject.tag == "Ball" && ballSpeed > threshold)
        {
            print("Owa");
            transform.parent.gameObject.SetActive(false);
        }
     
    }
    float calculateIncomingSpeed()
    {
        if (ballRB == null)
        {
            return 0;
        }
        return Mathf.Abs(ballRB.velocity.y);
    }

    private void BreakWall()
    {

    }

}
