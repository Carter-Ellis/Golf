using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    Ball ball;
    public int par = 4;
    private void Awake()
    {
        ball = GameObject.FindAnyObjectByType<Ball>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" && collision.gameObject.GetComponent<Ball>() is Ball)
        {
            
            if (ball.strokes <= par)
            {
                print("On Par! Strokes: " + ball.strokes);
            }
            else if (ball.strokes > par)
            {
                print("You are so bad! Strokes: " + ball.strokes);
            }

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Ball")
        {
            Destroy(collision.gameObject);
        }
    }
}
