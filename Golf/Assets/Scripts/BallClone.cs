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
    public bool isInteractable;
    void Start()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateBall();
        if (isInteractable)
        {           
            return;
        }
        timer += Time.deltaTime;
        ball.isBurst = true;
        Debug.Log(ball.isBurst);
        if (timer > lifeTime)
        {
            ball.isBurst = false;
            Destroy(gameObject);
        }
        
    }
    void AnimateBall()
    {
        if (rb.velocity.magnitude < .5f)
        {
            rb.velocity = Vector2.zero;
        }
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

