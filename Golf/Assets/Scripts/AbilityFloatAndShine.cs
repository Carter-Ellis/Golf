using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AbilityFloatAndShine : MonoBehaviour
{
    private float liftHeight = .25f;
    private float liftSpeed = .05f;
    private float startPos;
    private float timer;
    private float shineTime = 5f;
    private bool isShining;
    private Animator animator;
    public string shineAnimName;
    void Start()
    {
        startPos = transform.position.y;
        animator = GetComponent<Animator>();
        StartCoroutine(MoveUp());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > shineTime)
        {
            animator.SetBool("isShining", true);
            isShining = true;
        }
        if (isShining)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(shineAnimName) && stateInfo.normalizedTime > 1f)
            {
                isShining = false;
                animator.SetBool("isShining", false);
                timer = 0;
            }
        }
    }

    IEnumerator MoveUp()
    {
        
        for (float i = 0; i < liftHeight; i += .01f)
        {
            transform.position = new Vector3(transform.position.x, startPos + i, transform.position.z);
            yield return new WaitForSeconds(liftSpeed);
        }
        StartCoroutine(MoveDown());
    }
    IEnumerator MoveDown()
    {
        for (float i = startPos + liftHeight; i > startPos - 0.01f; i -= 0.01f)
        {
            transform.position = new Vector3(transform.position.x, i, transform.position.z);
            yield return new WaitForSeconds(liftSpeed);
        }
        StartCoroutine(MoveUp());
    }
}
