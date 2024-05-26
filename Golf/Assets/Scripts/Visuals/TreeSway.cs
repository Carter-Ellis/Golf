using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeSway : MonoBehaviour
{
    private Ball ball;
    private float timer;
    private float rand;
    private float swayRange = 100f;
    private bool isSwaying;
    public Animator animator;
    void Start()
    {
        rand = Random.Range(0, swayRange);
        ball = GameObject.FindObjectOfType<Ball>();
    }

    void Update()
    {

        if (ball.transform.position.y > transform.position.y - .75f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10f);
        }

        timer += Time.deltaTime;
        if (timer > rand)
        {
            animator.SetBool("IsSwaying", true);
            isSwaying = true;            
        }
        if (isSwaying)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("TreeSway") && stateInfo.normalizedTime > .5f)
            {
                isSwaying = false;
                animator.SetBool("IsSwaying", false);
                rand = Random.Range(0, swayRange);
                timer = 0;
            }
        }

    }
}
