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

    private SoundEffect squawkSFX = new SoundEffect(FMODEvents.instance.squak);
    private SoundEffect flapSFX = new SoundEffect(FMODEvents.instance.flapWing);

    private void Start()
    {
        anim = GetComponent<Animator>();
        ball = FindObjectOfType<Ball>();
        rb = GetComponent<Rigidbody2D>();
        ball.GetComponent<Inventory>().seagullInScene = true;
    }

    private void Update()
    {
        if (isFlying || ball == null)
            return;

        float distance = Vector2.Distance(transform.position, ball.transform.position);

        if (distance < radius)
        {
            Inventory.hitBird = true;
            Destroy(gameObject, 20f);
            anim.SetTrigger("Fly");
            isFlying = true;

            // Create and attach sound to this GameObject so it moves with the bird
            squawkSFX.play(this);
            flapSFX.play(this);
           
        }
    }

    private void FixedUpdate()
    {
        if (!isFlying)
            return;

        squawkSFX.updatePosition(this);
        flapSFX.updatePosition(this);

        elapsedTime += Time.fixedDeltaTime;

        float wobble = Mathf.Sin(elapsedTime * wobbleFrequency) * wobbleAmplitude;
        float verticalVelocity = upwardSpeed + wobble;

        rb.velocity = new Vector2(forwardSpeed, verticalVelocity);
    }

    private void OnDestroy()
    {
        squawkSFX.stop();
        flapSFX.stop();
    }

}
