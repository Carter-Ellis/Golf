using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfCart : MonoBehaviour
{
    Rigidbody2D cartBody;
    SpriteRenderer spriteRenderer;
    private Inventory inv;

    public float speed = 3f;
    public float travelDistance = 5f;

    private ParticleSystem particleSys;

    private Vector2 startPosition;
    private float ballHits;

    private EventInstance cartSFX;

    void Start()
    {
        // Removed PlayOneShot — we're already creating a manual instance below

        cartBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = cartBody.position;
        inv = FindObjectOfType<Inventory>();

        particleSys = gameObject.transform.Find("Grass Particles Cart").GetComponent<ParticleSystem>();

        cartSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.golfCart);
        cartSFX.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        cartSFX.start();

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

        if (cartSFX.isValid())
        {
            cartSFX.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
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
        velocityModule.x = new ParticleSystem.MinMaxCurve(-speed / 2f);
        velocityModule.y = new ParticleSystem.MinMaxCurve(0f);

        particleSys.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            ballHits++;
            if (ballHits >= 20 && !inv.achievements[(int)Achievement.TYPE.GOLF_CART_JOCKEY])
            {
                Achievement.Give(Achievement.TYPE.GOLF_CART_JOCKEY);
                inv.SavePlayer();
            }
        }
    }

    private void OnDestroy()
    {
        if (cartSFX.isValid())
        {
            cartSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            cartSFX.release();
        }
    }

    private void OnDisable()
    {
        if (cartSFX.isValid())
        {
            cartSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            cartSFX.release();
        }
    }
}
