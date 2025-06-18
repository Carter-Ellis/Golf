using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSway : MonoBehaviour
{
    private Ball ball;
    private float timer;
    private float rand;
    private float swayRange = 30f;
    private bool isSwaying;
    private int ballHitCount = 0;

    public Animator leavesAnim;
    public Animator trunkAnim;

    // Add particle system for drizzle effect
    public ParticleSystem drizzleParticles;

    void Start()
    {
        rand = Random.Range(0, swayRange);
        ball = GameObject.FindObjectOfType<Ball>();

        if (drizzleParticles != null)
        {
            drizzleParticles.Stop();
        }
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

            // Start particles when sway starts
            if (drizzleParticles != null && !drizzleParticles.isPlaying)
            {
                var velocityModule = drizzleParticles.velocityOverLifetime;
                velocityModule.enabled = true;
                velocityModule.x = new ParticleSystem.MinMaxCurve(1f);
                velocityModule.y = new ParticleSystem.MinMaxCurve(-1f);
                velocityModule.z = new ParticleSystem.MinMaxCurve(0f);

                drizzleParticles.Play();
            }
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

                // Stop particles when sway ends
                if (drizzleParticles != null && drizzleParticles.isPlaying)
                {
                    drizzleParticles.Stop();
                }
            }
        }

        if (ball == null)
        {
            return;
        }
        // Put tree in front of ball or behind.
        if (ball.transform.position.y > transform.position.y - .75f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            ballHitCount++;
            if (ballHitCount == 10 && !FindObjectOfType<Inventory>().achievements[(int)Achievement.TYPE.PLANTS_VS_GOLFBALLS])
            {
                Achievement.Give(Achievement.TYPE.PLANTS_VS_GOLFBALLS);
                FindObjectOfType<Inventory>().SavePlayer();
            }
        }
    }

}
