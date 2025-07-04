using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    private Animator anim;
    private Ball ball;
    private float radius = 3f;
    private bool isFlying;


    public float forwardSpeed = 2f;          // Horizontal speed (right)
    public float upwardSpeed = 1f;           // Constant upward movement
    public float wobbleAmplitude = 0.5f;     // How much it wobbles up/down
    public float wobbleFrequency = 3f;       // How fast it wobbles

    private Rigidbody2D rb;
    private float elapsedTime;


    private void Start()
    {
        anim = GetComponent<Animator>();
        ball = FindObjectOfType<Ball>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        
        if (isFlying)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, ball.transform.position);

        if (distance < radius)
        {
            Destroy(gameObject, 10f);
            anim.SetTrigger("Fly");
            isFlying = true;
        }

    }


    void FixedUpdate()
    {
        if (!isFlying)
        {
            return;
        }

        elapsedTime += Time.fixedDeltaTime;

        // Vertical wobble using sine wave
        float wobble = Mathf.Sin(elapsedTime * wobbleFrequency) * wobbleAmplitude;

        // Total vertical speed = constant upward + wobble
        float verticalVelocity = upwardSpeed + wobble;

        // Set velocity (right + vertical)
        rb.velocity = new Vector2(forwardSpeed, verticalVelocity);
    }

}
