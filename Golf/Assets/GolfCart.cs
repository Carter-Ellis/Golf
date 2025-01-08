using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfCart : MonoBehaviour
{
    Rigidbody2D cartBody;
    SpriteRenderer spriteRenderer;
    private float driveTimer;
    public float driveTimeThreshold = 3f;
    public float speed = 3f;
    void Start()
    {
        cartBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cartBody.velocity = new Vector2(speed, 0);
        spriteRenderer.flipX = true;
    }

    void Update()
    {
        driveTimer += Time.deltaTime;
        if (driveTimer >= driveTimeThreshold)
        {
            TurnAround();
            driveTimer = 0f;
        }
    }

    void TurnAround()
    {
        if (cartBody.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
            cartBody.velocity = new Vector2(-speed, 0);
        }
        else if (cartBody.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
            cartBody.velocity = new Vector2(speed, 0);
        }
        
    }

}
