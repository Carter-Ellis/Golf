using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeSway : MonoBehaviour
{
    private float timer;
    private float rand;
    private float swayRange = 25f;
    private bool isSwaying;
    public Animator animator;
    void Start()
    {
        rand = Random.Range(0, swayRange);
    }

    void Update()
    {
        
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
