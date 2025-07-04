using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public float moveSpeed = 1f;            // Speed of the crab
    public float moveDuration = 2f;         // How long it moves in one direction
    public Vector2 waitTimeRange = new Vector2(1f, 3f);  // Random wait time between moves

    private Rigidbody2D rb;
    private Animator animator;

    private bool isMoving = false;
    private int moveDirection = -1; // Start moving left (-1 = left, 1 = right)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(MovementRoutine());
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator MovementRoutine()
    {
        while (true)
        {
            // Start moving
            isMoving = true;
            animator.SetTrigger("Moving");

            yield return new WaitForSeconds(moveDuration);

            // Stop moving
            isMoving = false;
            animator.SetTrigger("Moving");
            print("stopped");
            yield return new WaitForSeconds(Random.Range(waitTimeRange.x, waitTimeRange.y));

            // Flip direction
            moveDirection *= -1;
        }
    }
}
