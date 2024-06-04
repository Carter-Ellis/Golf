using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public GameObject exit;
    private Vector3 exitPos;
    private Vector3 scaleChange = new Vector3(-.01f, -.01f, 0);
    private Vector3 origScale = new Vector3(1, 1, 1);
    private Ball ball;
    private bool isTraveling;
    private float travelSpeed = .01f;
    public float ballOverHoleSpeed = 10f;
    public float exitSpeed = 2f;
    void Start()
    {
        exitPos = exit.transform.GetChild(0).transform.position;
        ball = FindObjectOfType<Ball>();
        travelSpeed = travelSpeed + Vector3.Distance(gameObject.transform.position, exitPos) / 4000;
    }

    void Update()
    {
        if (isTraveling)
        {
            
            if (ball.transform.localScale.x <= 0) {
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, exitPos, travelSpeed);
                ball.gameObject.SetActive(false);
            }
            else
            {
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .1f), .01f);
                ball.transform.root.localScale += scaleChange;
            }
            if (ball.transform.position == exitPos)
            {
                Exit();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (exitPos == null) {
            print("Exit not set");
            return;
        }

        if (collision.gameObject.tag != "Ball" && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > ballOverHoleSpeed)
        {
            return;
        }
        
        isTraveling = true;

    }

    private void Exit()
    {
        ball.gameObject.SetActive(true);
        ball.transform.root.localScale = origScale;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(exitSpeed, 0);
        isTraveling = false;
    }


}
