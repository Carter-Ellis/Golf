using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BallClone : MonoBehaviour
{
    private float timer;
    private float lifeTime = 4f;
    Ball ball;
    Inventory inv;
    Rigidbody2D rb;
    public Animator animator;
    public bool isInteractable;
    void Start()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        inv = ball.GetComponent<Inventory>();
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
        if (rb.linearVelocity.magnitude < .5f)
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (Mathf.Abs(rb.linearVelocity.y) > Mathf.Abs(rb.linearVelocity.x))
        {
            //Roll Up
            animator.SetBool("isHorizontal", false);
            animator.SetBool("isVertical", true);
            animator.SetFloat("SpeedY", rb.linearVelocity.y);

        }
        else if (Mathf.Abs(rb.linearVelocity.y) < Mathf.Abs(rb.linearVelocity.x))
        {
            //Roll Right
            animator.SetBool("isVertical", false);
            animator.SetBool("isHorizontal", true);
            animator.SetFloat("SpeedX", rb.linearVelocity.x);
        }
        else
        {
            animator.SetBool("isVertical", false);
            animator.SetBool("isHorizontal", false);
            animator.SetFloat("SpeedX", 0);
            animator.SetFloat("SpeedY", 0);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameMode.current != GameMode.TYPE.CLUBLESS)
        {
            Audio.playSFX(FMODEvents.instance.wallHit, transform.position);
        }
    }
       

}

