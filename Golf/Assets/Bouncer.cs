using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public Animator animator;
    private bool isBouncing;

    private void Update()
    {
        if (isBouncing)
        {
            animator.SetTrigger("Bounce");
            isBouncing = false;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Bounce") && stateInfo.normalizedTime >= 1f)
        {
            animator.SetTrigger("Bounce");
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isBouncing = true;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.bouncer, transform.position);
    }
}
