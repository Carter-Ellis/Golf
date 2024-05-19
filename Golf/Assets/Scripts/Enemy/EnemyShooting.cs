using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    private GameObject ball;
    private float timer;
    private float range = 10f;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    void Update()
    {
        
        if (ball == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, ball.transform.position);

        if (distance < range)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                timer = 0;
                Shoot();
            }
        }

        

    }

    void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
