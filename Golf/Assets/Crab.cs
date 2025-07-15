using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Crab : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float moveDuration = 2f;
    public Vector2 waitTimeRange = new Vector2(1f, 3f);

    public bool mrCrab;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isMoving = false;
    private int moveDirection = -1;

    private EventInstance walkInstance;     // FMOD event instance for CrabWalk
    private bool isSoundPlaying = false;    // To track sound state

    private int hitCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Create the sound instance (replace with your actual FMOD event reference)
        walkInstance = RuntimeManager.CreateInstance(FMODEvents.instance.crabWalk);
        RuntimeManager.AttachInstanceToGameObject(walkInstance, transform, rb);

        StartCoroutine(MovementRoutine());

        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);

            if (!isSoundPlaying)
            {
                walkInstance.start();
                isSoundPlaying = true;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;

            if (isSoundPlaying)
            {
                walkInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                isSoundPlaying = false;
            }
        }
    }

    IEnumerator MovementRoutine()
    {
        while (true)
        {
            isMoving = true;
            animator.SetTrigger("Moving");

            yield return new WaitForSeconds(moveDuration);

            isMoving = false;
            animator.SetTrigger("Moving");

            yield return new WaitForSeconds(Random.Range(waitTimeRange.x, waitTimeRange.y));

            moveDirection *= -1;
            FlipCrab();
        }
    }

    private void FlipCrab()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;
        transform.localScale = scale;
    }

    private void OnDestroy()
    {
        walkInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        walkInstance.release();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ball") { return; }
        if (mrCrab && hitCount < 5)
        {
            hitCount++;

            if (hitCount >= 5) 
            {
                if (!FindObjectOfType<Inventory>().achievements[(int)Achievement.TYPE.MR_K])
                {
                    Inventory inv = FindObjectOfType<Inventory>();
                    Hat.give(Hat.TYPE.CRAB);
                    Achievement.Give(Achievement.TYPE.MR_K);
                    inv.SavePlayer();
                }
            }
        }
    }

}
