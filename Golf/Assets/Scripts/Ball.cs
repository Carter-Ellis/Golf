using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using System.Threading;

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


    [Header("Ball Properties")]
    Ball ball;
    Rigidbody2D rb;
    LineRenderer lineRenderer;
    CameraController camController;
    public Animator animator;
    public GameObject cursor;
    public Selectable objectSelected;
    

    [Header("Burst")]
    public GameObject ballClone;
    public Transform burstPos1;
    public Transform burstPos2;
    public Transform burstPos3;
    public Transform burstPos4;


    [Header("Variables")]
    public bool isBallLocked;
    bool isAiming;
    bool hasShot;
    bool hasClickedBall;
    int numPoints = 50;
    float timeStep = .05f;
    public bool canPutt;
    private bool isMouseButton1Held;
    public bool isBurst;
    private bool takingDamage;
    private float damageTimer = 0f;
    public float maxHitSpeed = 15f;

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
        cursor.GetComponent<SpriteRenderer>().enabled = false;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.startWidth = .25f;
        lineRenderer.endWidth = .25f;
        canPutt = true;
    }

    private void Start()
    {
        ballRollSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.ballRollSFX);
    }

    void Update()
    {
        checkDead();
        AnimateBall();
        UpdateSound();

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

        if (isBallLocked)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
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
        
        isMouseButton1Held = Input.GetMouseButton(1);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, 20f);
        if (rb.velocity.magnitude < .5f)
        {
            rb.velocity = Vector2.zero;
            wallHits = 0;
        }
        

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
        if (component != null && (Selectable)component != objectSelected)
        {
            ((Selectable)component).onSelect();
            newSelected = component;
        }

        if (newSelected == null || newSelected is not Selectable) {

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
        
        healthTxt.text = "Health: " + health;
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
        if (canPutt)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            wallHits++;
            float volume = rb.velocity.magnitude / 10;
            if (volume > maxSFXVolume)
            {
                volume = maxSFXVolume;
            }
            AudioManager.instance.PlayOneShot(FMODEvents.instance.wallHit, transform.position);
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

            if (isMouseButton1Held && hit.collider != null && rb.velocity == new Vector2(0, 0) && hit.collider.tag == "Ball" && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                hasClickedBall = true;
                break;
            }
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
