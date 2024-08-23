using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class BattleMode : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI dodgeTxt;

    private Ball ball;
    private Rigidbody2D body;

    public float moveSpeed = 3f;
    public float dodgeSpeed = 7f;

    private float dodgeTimer = 0f;
    private float dodgeTime = .15f;
    private bool hasDodged;
    private float dodgeCooldownTimer = 0f;
    private float dodgeCooldown = 2f;
    private bool isDodging;

    void Start()
    {
        ball = GetComponent<Ball>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (ball == null || !ball.isBattleMode)
        {
            return;
        }
        CheckDodge();
    }

    void FixedUpdate()
    {
        if (ball == null || !ball.isBattleMode)
        {
            return;
        }
        
        
        if (isDodging)
        {
            Dodge();
        }
        else
        {
            Move();
        }   
    }
    private void DodgeCooldown()
    {
        if (hasDodged)
        {
            dodgeCooldownTimer += Time.deltaTime;
            dodgeTxt.text = (2 - (int)dodgeCooldownTimer).ToString();
            if (dodgeCooldownTimer > dodgeCooldown)
            {
                hasDodged = false;
                dodgeCooldownTimer = 0f;
                dodgeTxt.text = "";
            }
        }
    }
    private void Move()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        body.velocity = direction * moveSpeed;

        
    }
    private void CheckDodge()
    {
        DodgeCooldown();

        if (Input.GetKeyDown(KeyCode.LeftShift) && body.velocity.magnitude > 0f && !hasDodged)
        {
            isDodging = true;
            hasDodged = true;
        }

    }
    private void Dodge()
    {
        dodgeTimer += Time.deltaTime;
        
        if (dodgeTimer > dodgeTime)
        {
            isDodging = false;
            dodgeTimer = 0f;
            return;
        }

        body.velocity = body.velocity.normalized * dodgeSpeed;
    }

}
