using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using Cinemachine.Utility;

public enum Direction
{
    North,
    East,
    South,
    West,
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}
public class Ball : MonoBehaviour
{

    [Header("Ball Components")]
    public int strokes = 0;

    public int health = 100;
    public int _maxHealth = 100;

    public int clickDamage = 5;
    public int _maxClickDamage = 100;

    public int hitDamage = 20;
    public int _maxHitDamage = 100;

    public int wallHits = 0;
    public int _minWallHitCombo = 2;
    public LayerMask layerMask;
    public LayerMask ignoreRaycast;

    public float moveSpeed = 5f;
    private Vector2 movement;

    [Header("Ball Properties")]
    Ball ball;
    Rigidbody2D rb;
    LineRenderer lineRenderer;
    CameraController camController;
    public Animator animator;
    public GameObject cursor;
    public Selectable objectSelected;
    public Direction CurrentDirection { get; private set; }

    [Header("Burst")]
    public GameObject ballClone;
    public Transform burstPos1;
    public Transform burstPos2;
    public Transform burstPos3;
    public Transform burstPos4;



    [Header("Variables")]
    public bool isBattleMode;
    public bool isBallLocked;
    public bool isTraveling;
    bool isAiming;
    bool hasShot;
    
    int numPoints = 50;
    float timeStep = .05f;
    public bool hasClickedBall;
    public bool canPutt;
    public bool isPuttCooldown;
    public bool isTeleportReady;
    private bool isMouseButton1Held;
    public bool isBurst;
    public bool isSelectFan;
    private bool takingDamage;
    private float damageTimer = 0f;
    public float maxHitSpeed = 15f;
    private ParticleSystem ps;

    [Header("Swing Cooldown")]
    private float swingTimer = 0;
    private float swingCooldownTime = 3f;
    [SerializeField] private TextMeshProUGUI swingCooldownTxt;

    [Header("TextDisplay")]
    [SerializeField] private TextMeshProUGUI healthTxt;

    [Header("Sounds")]
    public float maxSFXVolume = .45f;
    public float minPitch = 0.5f;  // Minimum pitch value
    public float maxPitch = 2.0f;  // Max pitch
    public float maxVelocity = 10.0f;
    [SerializeField] private AudioClip[] rollSFX;
    [SerializeField] private AudioClip[] softSwingClips;
    [SerializeField] private AudioClip[] mediumSwingClips;
    [SerializeField] private AudioClip[] hardSwingClips;
    [SerializeField] private AudioClip[] wallHitClips;
    private EventInstance ballRollSFX;

    void Awake()
    {
        ball = GetComponent<Ball>();
        rb = GetComponent<Rigidbody2D>();
        camController = GameObject.FindAnyObjectByType<CameraController>();
        hasShot = true;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.startWidth = .25f;
        lineRenderer.endWidth = .25f;
        canPutt = true;
        isPuttCooldown = true;
        //ps = GetComponentInChildren<ParticleSystem>();
        //ps.gameObject.SetActive(false);
        cursor.GetComponent<SpriteRenderer>().enabled = false;


    }

    private void Start()
    {
        ballRollSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.ballRollSFX);
        CurrentDirection = Direction.South;
    }

    private void FixedUpdate()
    {
        if (isBattleMode && movement.magnitude > 0)
        {
            float speed = Mathf.Max(moveSpeed, rb.velocity.magnitude);
            rb.velocity = (rb.velocity + movement.normalized * moveSpeed).normalized * speed;
        }
        if (rb.velocity.magnitude < .5f)
        {
            rb.velocity = Vector2.zero;
            wallHits = 0;
        }
        else if (rb.velocity.magnitude > 20f)
        {
            rb.velocity = rb.velocity.normalized * 20f;
        }

    }

    void Update()
    {
        didEnter = false;
        didExit = false;
        checkDead();
        AnimateBall();
        //UpdateSound();
        UpdateDirection(rb.velocity);

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            AudioManager.instance.CreateInstance(FMODEvents.instance.mapOpen);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        /*if (Input.GetKeyDown(KeyCode.B))
        {
            isBattleMode = !isBattleMode;
        }*/

        if (isBattleMode)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        
        if (takingDamage)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > .2f)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                takingDamage = false;
                damageTimer = 0;
            }
        }

        SwingCooldown();

        if (isBallLocked)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && !camController.isViewMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            bool didSelect = false;
            if (hit.collider != null)
            {
                Component[] components = hit.collider.GetComponents<Component>();
                foreach (Component component in components)
                {
                    if (component is Selectable)
                    {
                        Select(component);
                        didSelect = true;
                        break;
                    }
                    
                }
            }
            if (!didSelect) 
            {
                Select(null);
            }
        }
       
        ClickEnemy();
        
        isMouseButton1Held = Input.GetMouseButton(0);
        
        

        if (!camController.isViewMode)
        {
            setPutt();
        }  
        
    }

    public void Select(Component component)
    {

        Component newSelected = null;
        if (objectSelected != null)
        {
            objectSelected.onDeselect();
        }
        bool isSelectable = true;
        if (component != null && (Selectable)component != objectSelected)
        {
            isSelectable = ((Selectable)component).onSelect();
            newSelected = component;
        }

        if (newSelected == null || newSelected is not Selectable || !isSelectable) {

            objectSelected = null;
            camController.cam.Follow = this.transform;
            ball.canPutt = true;
        }
        else
        {
            objectSelected = (Selectable)newSelected;
            camController.cam.Follow = newSelected.transform;
            ball.canPutt = false;
        }
    }
    public void SwingCooldown()
    {
        if (!isPuttCooldown)
        {
            swingTimer += Time.deltaTime;
            swingCooldownTxt.text = (swingCooldownTime - (int)swingTimer).ToString();
            if (swingTimer > swingCooldownTime || rb.velocity.magnitude == 0)
            {
                isPuttCooldown = true;
                swingTimer = 0f;
                swingCooldownTxt.text = "";
            }
        }
    }
    private void UpdateDirection(Vector2 movement)
    {
        if (movement.x > 0)
        {
            CurrentDirection = movement.y > 0 ? Direction.NorthEast : (movement.y < 0 ? Direction.SouthEast : Direction.East);
        }
        else if (movement.x < 0)
        {
            CurrentDirection = movement.y > 0 ? Direction.NorthWest : (movement.y < 0 ? Direction.SouthWest : Direction.West);
        }
        else
        {
            CurrentDirection = movement.y > 0 ? Direction.North : (movement.y < 0 ? Direction.South : CurrentDirection);
        }

    }

    void AnimateBall()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        float origAngle = angle;

        if (angle < 0)
        {
            angle += 180;
        }
        if (angle >= 30 && angle <= 60)
        {
            if (origAngle < 0)
            {
                animator.SetFloat("DiagonalSpeed", -rb.velocity.magnitude);
            }
            else
            {
                animator.SetFloat("DiagonalSpeed", rb.velocity.magnitude);
            }
            animator.SetBool("isDiagonalRight", true);
            animator.SetBool("isVertical", true);
            animator.SetBool("isHorizontal", true);

        }
        else if (angle >= 120 && angle <= 150)
        {

            if (origAngle < 0)
            {
                animator.SetFloat("DiagonalSpeed", -rb.velocity.magnitude);
            }
            else
            {
                animator.SetFloat("DiagonalSpeed", rb.velocity.magnitude);
            }

            animator.SetBool("isDiagonalRight", false);
            animator.SetBool("isVertical", true);
            animator.SetBool("isHorizontal", true);
        }
        else if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x))
        {
            //Roll Up
            animator.SetBool("isHorizontal", false);
            animator.SetBool("isVertical", true);
            animator.SetFloat("SpeedY", rb.velocity.y);
            
        }
        else if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(rb.velocity.x))
        {
            //Roll Right           
            animator.SetBool("isVertical", false);
            animator.SetBool("isHorizontal", true);       
            animator.SetFloat("SpeedX", rb.velocity.x);
        }
        else
        {
            animator.SetBool("isVertical", false);
            animator.SetBool("isHorizontal", false);
            animator.SetBool("isDiagonalRight", false);
            animator.SetFloat("DiagonalSpeed", 0);
            animator.SetFloat("SpeedX", 0);
            animator.SetFloat("SpeedY", 0);
        }
        
        //healthTxt.text = "Health: " + health;
    }
    public void TakeDamage(int damage)
    {
        takingDamage = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255/255, 100/255,100/255);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.ballHurtSFX, transform.position);
        health -= damage;
    }
    void ClickEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.tag == "Enemy")
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.health -= ball.clickDamage;
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, .2f), ForceMode2D.Impulse);
        }
    }

    void setPutt()
    {
        if (canPutt && isPuttCooldown)
        {
            GrabBall();
        }
        else
        {
            lineRenderer.enabled = false;
            hasClickedBall = false;
        }
    }

    void checkDead()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Burst()
    {
        if (isBurst)
        {
            return;
        }
        float burstSpeedMultiplier = 2f;
        GameObject clone;
        clone = Instantiate(ballClone, burstPos1.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().velocity = rb.velocity * burstSpeedMultiplier;
        
        clone = Instantiate(ballClone, burstPos2.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().velocity = rb.velocity * burstSpeedMultiplier;
        clone = Instantiate(ballClone, burstPos3.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().velocity = rb.velocity * burstSpeedMultiplier;
        clone = Instantiate(ballClone, burstPos4.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().velocity = rb.velocity * burstSpeedMultiplier;

    }

    bool didEnter = false;
    bool didExit = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBattleMode)
        {
            return;
        }
        if (collision.gameObject.tag == "Wall")
        {
            wallHits++;
            float volume = rb.velocity.magnitude / 10;
            if (volume > maxSFXVolume)
            {
                volume = maxSFXVolume;
            }
            didEnter = true;
            if (!didExit || !didEnter)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.wallHit, transform.position);
            }
        }
        else if (collision.gameObject.tag == "Wood")
        {
            wallHits++;
            float volume = rb.velocity.magnitude / 10;
            if (volume > maxSFXVolume)
            {
                volume = maxSFXVolume;
            }
            if (!didExit || !didEnter)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.woodHit, transform.position);
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            didExit = true;
        }
    }

    void DrawTrajectory(Vector2 force)
    {
        cursor.GetComponent<SpriteRenderer>().enabled = true;
        lineRenderer.enabled = true;
        Vector3[] points = new Vector3[numPoints];
        Vector3 initialPosition = new Vector3(transform.position.x, transform.position.y, -9f);
        Vector3 velocity = force;

        for (int i = 0; i < numPoints; i++)
        {
            float t = 5 * (int)(i / 5) * timeStep;
            points[i] = initialPosition + velocity;
        }
        cursor.transform.position = initialPosition + velocity;
        //lineRenderer.SetPositions(points);
    }

    void GrabBall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity, layerMask);
        
        foreach (RaycastHit2D hit in hits)
        {

            if (isMouseButton1Held && hit.collider != null && hit.collider.tag == "Ball" && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                hasClickedBall = true;
                break;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            hasClickedBall = false;
            cursor.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }
        if (hasClickedBall)
        {
            Putt();
        }
        if (!isMouseButton1Held)
        {
            hasClickedBall = false;
        }

    }

    void Putt()
    {
        if (isMouseButton1Held)
        {
            isAiming = true;
            hasShot = false;
        }
        else
        {
            isAiming = false;
        }
        if (isAiming)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 ballPos = ball.transform.position;

            Vector2 mousePosNormalized = (mousePos - ballPos).normalized;

            float distFromBall = Vector2.Distance(mousePos, ballPos);
            Vector2 force = -mousePosNormalized * distFromBall;
            DrawTrajectory(force);
        }
        else
        {
            lineRenderer.enabled = false;
        }
        if (!hasShot && !isAiming)
        {
            hasShot = true;
            hasClickedBall = false;
            Shoot();
        }
    }

    void Shoot()
    {
        canPutt = false;
        isPuttCooldown = false;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ballPos = ball.transform.position;

        Vector2 mousePosNormalized = (mousePos - ballPos).normalized;

        float distFromBall = Vector2.Distance(mousePos, ballPos);
        
        if (distFromBall > maxHitSpeed)
        {
            distFromBall = maxHitSpeed;
        }

        Vector2 force = -mousePosNormalized * distFromBall;
        
        if (force.magnitude > 0)
        {
            strokes++;
        }
           
        rb.AddForce(force, ForceMode2D.Impulse);
        cursor.GetComponent<SpriteRenderer>().enabled = false;
        
        if (distFromBall > 0 && distFromBall < 4)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.softSwing, transform.position);
        }
        else if (distFromBall >= 4 && distFromBall < 8.5)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.mediumSwing, transform.position);
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.hardSwing, transform.position);
        }
        
    }

    private void UpdateSound()
    {
        //Start rollSFX event if the ball has a velocity > 0
        if (rb.velocity.magnitude > 0)
        {
            // Get the playback state
            PLAYBACK_STATE playbackState;
            ballRollSFX.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                ballRollSFX.start();
            }
        }
        else
        {
            // Otherwise stop the SFX
            ballRollSFX.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

}
