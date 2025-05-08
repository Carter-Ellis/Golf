using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfCart : MonoBehaviour
{
    Rigidbody2D cartBody;
    SpriteRenderer spriteRenderer;

    public float speed = 3f;
    public float travelDistance = 5f;

    private Vector2 startPosition;

    void Start()
    {
        cartBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = cartBody.position;

        cartBody.velocity = new Vector2(speed, 0);
        UpdateSpriteDirection();
    }

    void Update()
    {
        float distanceTraveled = Mathf.Abs(cartBody.position.x - startPosition.x);
        if (distanceTraveled >= travelDistance)
        {
            TurnAround();
        }
    }

    void TurnAround()
    {
        // Reverse direction
        speed *= -1;
        cartBody.velocity = new Vector2(speed, 0);
        startPosition = cartBody.position; // Reset start point for next leg
        UpdateSpriteDirection();
    }

    void UpdateSpriteDirection()
    {
        spriteRenderer.flipX = speed > 0;
    }
}
