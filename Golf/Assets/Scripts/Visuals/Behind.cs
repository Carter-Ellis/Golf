using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behind : MonoBehaviour
{
    Ball ball;
    void Start()
    {
        ball = GameObject.FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.transform.position.y > transform.position.y - .75f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10f);
        }
    }
}
