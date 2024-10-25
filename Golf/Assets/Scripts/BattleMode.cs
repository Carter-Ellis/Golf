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
    private List<CircleCollider2D> punchRegions = new List<CircleCollider2D>();

    public LayerMask enemyLayerMask;
    public float moveSpeed = 3f;
    public int punchDamage = 10;
    public float dodgeSpeed = 7f;
    public float knockbackForce = 50f;

    private bool isLeftPunchNext = true;
    private float dodgeTimer = 0f;
    private float dodgeTime = .15f;
    private bool hasDodged;
    private float dodgeCooldownTimer = 0f;
    private float dodgeCooldown = 2f;
    private bool isDodging;
    private Transform punchRegionsParent;

    void Start()
    {
        punchRegionsParent = transform.Find("AttackRegion");
        foreach (Transform region in punchRegionsParent.transform)
        {
            punchRegions.Add(region.GetComponent<CircleCollider2D>());
        }
        ball = GetComponent<Ball>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (ball == null || !ball.isBattleMode)
        {
            if (dodgeTxt == null)
            {
                return;
            }
            dodgeTxt.text = "";
            dodgeCooldownTimer = 2f;
            return;
        }
        HandlePunching();
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

    private void Punch()
    {
        CircleCollider2D activeCollider = punchRegions[(int)(ball.CurrentDirection)];
        Vector2 colliderCenter = activeCollider.transform.position;
        float colliderRadius = activeCollider.radius * activeCollider.transform.lossyScale.x;  // Adjust for scale if necessary

        // Check if any enemies are inside the active collider using OverlapCircle
        Collider2D[] enemies = Physics2D.OverlapCircleAll(colliderCenter, colliderRadius, enemyLayerMask);

        if (enemies.Length > 0)
        {
            foreach (Collider2D enemy in enemies)
            {
                ApplyKnockbackToEnemy(enemy.gameObject);
            }
        }

    }

    private void HandlePunching()
    {
        // Check for left punch (alternating) and punch with left mouse button
        if (isLeftPunchNext && Input.GetMouseButtonDown(0))
        {
            Punch();
            isLeftPunchNext = false;  // Next punch will be with the right hand
        }
        // Check for right punch and punch with right mouse button
        else if (!isLeftPunchNext && Input.GetMouseButtonDown(1))
        {
            Punch();
            isLeftPunchNext = true;  // Next punch will be with the left hand
        }
    }

    private void ApplyKnockbackToEnemy(GameObject enemy)
    {
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

        if (enemyRb != null)
        {
            // Get the knockback direction from the last move direction (enum)
            Vector2 knockbackDirection = GetDirectionVector(ball.CurrentDirection);
            Vector2 knockback = knockbackDirection * knockbackForce;

            // Apply the knockback force to the enemy's Rigidbody2D
            enemyRb.AddForce(knockback, ForceMode2D.Impulse);
            enemy.GetComponent<Enemy>().health -= punchDamage;

            Debug.Log("Knockback applied to " + enemy.name + " in direction: " + ball.CurrentDirection);
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
    private Vector2 GetDirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Vector2.up;
            case Direction.South:
                return Vector2.down;
            case Direction.East:
                return Vector2.right;
            case Direction.West:
                return Vector2.left;
            case Direction.NorthEast:
                return new Vector2(1, 1).normalized; // Diagonal NE
            case Direction.NorthWest:
                return new Vector2(-1, 1).normalized; // Diagonal NW
            case Direction.SouthEast:
                return new Vector2(1, -1).normalized; // Diagonal SE
            case Direction.SouthWest:
                return new Vector2(-1, -1).normalized; // Diagonal SW
            default:
                return Vector2.zero; // Default to zero, though South is your default
        }
    }
}
