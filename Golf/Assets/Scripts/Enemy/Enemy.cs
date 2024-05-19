using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Components")]
    int _maxHealth = 100;
    public int health = 100;
    int damage = 10;
    int damageThreshold = 8;

    [Header("Enemy Properties")]
    Rigidbody2D rb;
    Ball ball;
    Rigidbody2D ballRB;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ball = FindObjectOfType<Ball>();
        ballRB = ball.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Dead();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isWallCombo = ball.wallHits >= ball._minWallHitCombo;

        float ballSpeed = calculateIncomingSpeed();

        if (isWallCombo && collision.gameObject.tag == "Ball" && ballSpeed > damageThreshold)
        {
            health -= ball.hitDamage * 2;
            print("WALL COMBO");
        }
        else if (collision.gameObject.tag == "Ball" && ballSpeed > damageThreshold)
        {
            health -= ball.hitDamage;
            print("Owa!");
        }
    }
    float calculateIncomingSpeed()
    {
        if (ballRB == null) 
        {
            return 0;
        }
        Vector2 ballSpeed;
        Vector2 RelativeVelocity = ballRB.velocity - rb.velocity;
        Vector2 Normal = ballRB.position - rb.position;
        float dot = Vector2.Dot(RelativeVelocity, Normal);
        dot *= ballRB.mass + rb.mass;
        Normal *= dot;
        ballSpeed = Normal / ballRB.mass;
        return ballSpeed.magnitude;
    }
    void Dead()
    {
        if (health < 1)
        {
            Destroy(gameObject);
        }
    }

}
