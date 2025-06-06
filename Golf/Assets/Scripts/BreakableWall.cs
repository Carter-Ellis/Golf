using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private Ball ball;
    private Rigidbody2D ballRB;
    private SpriteRenderer sr;
    private float threshold = 8;
    private bool isBroken = false;
    [SerializeField] Sprite broken;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] private bool isVertical;
    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        ballRB = ball.GetComponent<Rigidbody2D>();
        sr = GetComponentInParent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (isBroken)
        {
            return;
        }

        float ballSpeed = calculateIncomingSpeed();
        if (collision.gameObject.tag == "Ball" && ballSpeed > threshold)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.wallBreak, transform.position);
            Vector3 ballVel = ballRB.velocity;
            isBroken = true;
            transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            var velocityModule = ps.velocityOverLifetime;
            velocityModule.enabled = true;

            // Set constant velocity based on ball velocity
            velocityModule.x = new ParticleSystem.MinMaxCurve(ballVel.x);
            velocityModule.y = new ParticleSystem.MinMaxCurve(ballVel.y);
            velocityModule.z = new ParticleSystem.MinMaxCurve(0f);

            var main = ps.main;
            main.startColor = gameObject.GetComponentInParent<SpriteRenderer>().color;
            sr.sprite = broken;           
        }
     
    }
    float calculateIncomingSpeed()
    {
        if (ballRB == null)
        {
            return 0;
        }
        if (isVertical)
        {
            return Mathf.Abs(ballRB.velocity.x);
        }
        return Mathf.Abs(ballRB.velocity.y);
    }

}
