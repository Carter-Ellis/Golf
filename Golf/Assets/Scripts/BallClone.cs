using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BallClone : MonoBehaviour
{
    private float timer;
    private float lifeTime = 4f;
    Ball ball;
    Rigidbody2D rb;
    public Animator animator;
    void Start()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        ball.isBurst = true;
        if (timer > lifeTime)
        {
            ball.isBurst = false;
            Destroy(gameObject);
        }
        AnimateBall();
    }
    void AnimateBall()
    {
        if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x))
        {
            //Roll Up
            animator.SetBool("isHorizontal", false);
            animator.SetBool("isVertical", true);
            animator.SetFloat("SpeedY", rb.velocity.y);

        }
        else if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(rb.velocity.x))
        {
            //Roll Right
            animator.SetBool("isVertical", false);
            animator.SetBool("isHorizontal", true);
            animator.SetFloat("SpeedX", rb.velocity.x);
        }
        else
        {
            animator.SetBool("isVertical", false);
            animator.SetBool("isHorizontal", false);
            animator.SetFloat("SpeedX", 0);
            animator.SetFloat("SpeedY", 0);
        }

    }
}

