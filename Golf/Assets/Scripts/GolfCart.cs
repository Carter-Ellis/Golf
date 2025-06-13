using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfCart : MonoBehaviour
{
    Rigidbody2D cartBody;
    SpriteRenderer spriteRenderer;

    public float speed = 3f;
    public float travelDistance = 5f;

    public ParticleSystem particleSys;

    private Vector2 startPosition;

    void Start()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.golfCart, transform.position);
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
        startPosition = cartBody.position;

        PlayParticlesOppositeDirection();
        UpdateSpriteDirection();
    }

    void UpdateSpriteDirection()
    {
        spriteRenderer.flipX = speed > 0;

        // Flip particle system position by mirroring its local X
        Vector3 localPos = particleSys.transform.localPosition;
        localPos.x = -Mathf.Abs(localPos.x) * Mathf.Sign(speed); // flip X based on speed
        particleSys.transform.localPosition = localPos;
    }

    void PlayParticlesOppositeDirection()
    {
        if (particleSys == null)
        {
            Debug.LogWarning("No particle system assigned.");
            return;
        }

        var velocityModule = particleSys.velocityOverLifetime;
        velocityModule.enabled = true;

        // Emit opposite to current velocity
        velocityModule.x = new ParticleSystem.MinMaxCurve(-speed / 2f); // Make the trail more visible
        velocityModule.y = new ParticleSystem.MinMaxCurve(0f);

        particleSys.Play();
    }
}
