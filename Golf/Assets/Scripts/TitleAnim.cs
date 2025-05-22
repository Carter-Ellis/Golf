using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnim : MonoBehaviour
{
    public Animator animator;
    private float timer = 3f;
    private float timeThreshold = 3f;
    private bool isShining;
    [SerializeField] private GameObject mainMenu;
    void Start()
    {
        isShining = false;
    }
    private void OnEnable()
    {
        isShining = false;
    }
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > timeThreshold && !isShining)
        {
            isShining = true;
            animator.SetBool("IsShining", true);
        }

        if (isShining)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("TitleShine") && stateInfo.normalizedTime > .99f)
            {
                isShining = false;
                animator.SetBool("IsShining", false);
                timer = 0;
            }
        }
    }
}
