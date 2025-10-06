using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    Ball ball;
    public float blowingPower = .03f;
    private BoxCollider2D boxCollider;
    private ParticleSystem particleSys;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    private ParticleSystem.MainModule mainModule;
    [SerializeField] private float particleLifetime = .5f;
    private ParticleSystem.EmissionModule emissionModule;
    [SerializeField] private float particlesPerSecond = 300f;
    void Start()
    {
        ball = FindObjectOfType<Ball>();
        particleSys = transform.GetChild(0).GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainModule = particleSys.main;
        velocityModule = particleSys.velocityOverLifetime;

    }

    void Update()
    {
      
        mainModule.startLifetime = particleLifetime;

        emissionModule = particleSys.emission;
        emissionModule.rateOverTime = particlesPerSecond;

        velocityModule.speedModifier = blowingPower * 20f;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == null)
        {
            return;
        }

        GameObject obj = collision.gameObject;
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            return;
        }

        float rad = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        Vector2 direction = (new Vector2((float)Mathf.Cos(rad), (float)Mathf.Sin(rad))).normalized;
        bool isBall = obj.gameObject.GetComponent<Ball>() != null;
        bool isInteractable = obj.gameObject.tag == "Interactable";

        if (isBall && rb.linearVelocity.magnitude > .5f)
        {
            rb.linearVelocity += blowingPower * direction / Vector2.Distance(transform.position, ball.transform.position);
        }
        else if (isInteractable && rb.linearVelocity.magnitude > .5f)
        {
            rb.linearVelocity += blowingPower * direction;

        }
        else if (isBall || isInteractable)
        {
            rb.linearVelocity = direction;
        }
        else
        {
            rb.linearVelocity += blowingPower * direction;
        }

    }

}
