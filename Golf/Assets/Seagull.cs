using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Seagull : MonoBehaviour
{
    private Animator anim;
    private Ball ball;
    private float radius = 3f;
    private bool isFlying;

    public float forwardSpeed = 2f;
    public float upwardSpeed = 1f;
    public float wobbleAmplitude = 0.5f;
    public float wobbleFrequency = 3f;

    private Rigidbody2D rb;
    private float elapsedTime;

    private EventInstance squakInstance;  // FMOD event instance
    private EventInstance flapInstance;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ball = FindObjectOfType<Ball>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isFlying || ball == null)
            return;

        float distance = Vector2.Distance(transform.position, ball.transform.position);

        if (distance < radius)
        {
            Destroy(gameObject, 10f);
            anim.SetTrigger("Fly");
            isFlying = true;

            // Create and attach sound to this GameObject so it moves with the bird
            squakInstance = RuntimeManager.CreateInstance(FMODEvents.instance.squak);
            RuntimeManager.AttachInstanceToGameObject(squakInstance, transform, rb);
            squakInstance.start();
            squakInstance.release();  // Release so FMOD cleans it up when done

            flapInstance = RuntimeManager.CreateInstance(FMODEvents.instance.flapWing);
            RuntimeManager.AttachInstanceToGameObject(flapInstance, transform, rb);
            flapInstance.start();
            flapInstance.release();
        }
    }

    private void FixedUpdate()
    {
        if (!isFlying)
            return;

        elapsedTime += Time.fixedDeltaTime;

        float wobble = Mathf.Sin(elapsedTime * wobbleFrequency) * wobbleAmplitude;
        float verticalVelocity = upwardSpeed + wobble;

        rb.velocity = new Vector2(forwardSpeed, verticalVelocity);
    }
}
