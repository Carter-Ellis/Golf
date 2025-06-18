using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private Ball ball;

    private float timer = 0f;
    private float onSpikeTimer = 0f;
    public float onSpikeDamageTimer = 2f;
    public float idleTime = 2f;
    public float setTime = 2f;
    public float attackTime = 3f;
    private bool isAttacking;
    private bool isSet;
    public bool isAlwaysAttacking;
    private Animator anim;
    private bool entered;

    private FMOD.Studio.EventInstance instanceSet;
    private FMOD.Studio.EventInstance instanceAttack;
    private FMOD.Studio.EventInstance instanceContract;

    public FMODUnity.EventReference spikeSetAudio;
    public FMODUnity.EventReference spikeAttackAudio;
    public FMODUnity.EventReference spikeContractAudio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ball = FindObjectOfType<Ball>();
        onSpikeTimer = onSpikeDamageTimer;

        instanceSet = FMODUnity.RuntimeManager.CreateInstance(spikeSetAudio);

        instanceAttack = FMODUnity.RuntimeManager.CreateInstance(spikeAttackAudio);

        instanceContract = FMODUnity.RuntimeManager.CreateInstance(spikeContractAudio);

    }

    private void Update()
    {
        instanceSet.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        instanceAttack.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        instanceContract.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        
        if (ball != null)
        {
            float distance = Vector2.Distance(
                new Vector2(ball.transform.position.x, ball.transform.position.y),
                new Vector2(transform.position.x, transform.position.y)
            );

            if (distance < 1.5 && isAttacking)
            {
                ball.closeToSpike = true;
                entered = true;
            }
            else
            {
                if (entered)
                {
                    ball.closeToSpike = false;
                    entered = false;
                }
            }
        } 

        timer += Time.deltaTime;
        if (timer > idleTime && !isSet)
        {
            isSet = true;

            instanceSet.start();
            anim.SetTrigger("SetSpike");
            timer = 0f;
        }
        else if (timer > setTime && isSet && !isAttacking)
        {
            instanceAttack.start();
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
        if (isAttacking && timer > attackTime && isSet && !isAlwaysAttacking)
        {
            instanceContract.start();
            anim.SetTrigger("AttackIdle");
            anim.SetTrigger("Attack");
            timer = 0f;
            isAttacking = false;
            isSet = false;
        }
        if (!isAttacking)
        {
            onSpikeTimer = onSpikeDamageTimer;
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
                ball.TakeDamage(100);
                onSpikeTimer = 0f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        onSpikeTimer = onSpikeDamageTimer;
    }
}