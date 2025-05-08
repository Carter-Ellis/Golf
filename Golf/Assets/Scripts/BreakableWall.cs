using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private Ball ball;
    private Rigidbody2D ballRB;
    private SpriteRenderer sr;
    private float threshold = 8;
    [SerializeField] Sprite broken;
    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        ballRB = ball.GetComponent<Rigidbody2D>();
        sr = GetComponentInParent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        
        float ballSpeed = calculateIncomingSpeed();
        if (collision.gameObject.tag == "Ball" && ballSpeed > threshold)
        {
            transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            sr.sprite = broken;
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
