using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private float timer = 0f;
    private float onSpikeTimer = 0f;
    public float onSpikeDamageTimer = 2f;
    public float idleTime = 2f;
    public float setTime = 2f;
    public float attackTime = 3f;      
    private bool isAttacking;
    private bool isSet;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        onSpikeTimer = onSpikeDamageTimer;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > idleTime && !isSet)
        {
            isSet = true;
            anim.SetTrigger("SetSpike");
            timer = 0f;
        }
        else if (timer > setTime && isSet && !isAttacking)
        {
            isAttacking = true;
            timer = 0f;
            anim.SetTrigger("Attack");
        }
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("SpikeAttack"))
        {
            if (stateInfo.normalizedTime >= 1.0f)
            {
                anim.SetTrigger("AttackIdle");
            }

        }
        if (isAttacking && timer > attackTime && isSet)
        {
            anim.SetTrigger("AttackIdle");
            anim.SetTrigger("Attack");
            timer = 0f;
            isAttacking = false;
            isSet = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (isAttacking && collision.gameObject != null)
        {
            onSpikeTimer += Time.deltaTime;
            if (collision.gameObject.tag == "Ball" && onSpikeTimer > onSpikeDamageTimer)
            {
                Ball ball = collision.gameObject.GetComponent<Ball>();
                ball.health -= 10;
                onSpikeTimer = 0f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        onSpikeTimer = onSpikeDamageTimer;
    }
}
