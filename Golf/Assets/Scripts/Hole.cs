using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Hole : MonoBehaviour
{
    Ball ball;
    public int par = 4;
    public float ballOverHoleSpeed = 10f;
    private bool inHole;
    private Vector3 scaleChange = new Vector3(-.01f, -.01f, 0);
    private void Awake()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
    }

    private void Update()
    {

        if (!inHole)
        {
            return;
        }

        if (ball.transform.localScale.x <= 0) {
            Destroy(ball.gameObject);
            inHole = false;
        }
        else if (ball != null)
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .1f), .01f);
            ball.transform.root.localScale += scaleChange;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" && collision.gameObject.GetComponent<Ball>() is Ball && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < ballOverHoleSpeed)
        {
            if (ball.strokes <= par)
            {
                print("On Par! Strokes: " + ball.strokes);
            }
            else if (ball.strokes > par)
            {
                print("You are so bad! Strokes: " + ball.strokes);
            }

            inHole = true;
            
        }
        
    }
}
