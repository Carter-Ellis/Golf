using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeSway : MonoBehaviour
{
    private Ball ball;
    private float timer;
    private float rand;
    private float swayRange = 25f;
    private bool isSwaying;
    public Animator leavesAnim;
    public Animator trunkAnim;
    void Start()
    {
        rand = Random.Range(0, swayRange);
        ball = GameObject.FindObjectOfType<Ball>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (leavesAnim == null)
        {
            return;
        }
        if (timer > rand)
        {
            leavesAnim.SetBool("IsSwaying", true);
            trunkAnim.SetBool("IsSwaying", true);
            isSwaying = true;
        }
        if (isSwaying)
        {
            AnimatorStateInfo stateInfo = leavesAnim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("LeavesSway") && stateInfo.normalizedTime > 1f)
            {
                isSwaying = false;
                leavesAnim.SetBool("IsSwaying", false);
                trunkAnim.SetBool("IsSwaying", false);
                rand = Random.Range(0, swayRange);
                timer = 0;
            }
        }

        if (ball == null)
        {
            return;
        }
        //Put tree in front of ball or behind.
        if (ball.transform.position.y > transform.position.y - .75f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10f);
        }     

    }
}
