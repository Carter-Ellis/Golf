using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine.UI;

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
    Inventory inv;
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

    private Slider swingCooldownSlider;
    public Slider swingPowerSlider;


    [Header("Ball Properties")]
    Ball ball;
    Rigidbody2D rb;
    LineRenderer lineRenderer;
    CameraController camController;
    public Animator animator;
    public GameObject cursor;
    public Selectable objectSelected;
    private Fan[] allFans;

    public Direction CurrentDirection { get; private set; }
    [SerializeField] private float minSpeedForParticles = 5f;

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

    public bool hasClickedBall;
    public bool canPutt;
    public bool isPuttCooldown;
    public bool isTeleportReady;
    private bool isInputPaused;
    private bool pendingUnpause;
    private bool isMouseButton1Held;
    public bool isBurst;
    public bool isSelectFan;
    private bool takingDamage;
    private float damageTimer = 0f;
    public float maxHitSpeed = 15f;
    private ParticleSystem ps;
    private Map.TYPE currentMap;

    [Header("Dots")]
    public GameObject dotPrefab;
    private int numDots = 10;
    private float speed = 4f;

    private List<DotData> dots = new List<DotData>();
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 direction;

    [Header("Swing Cooldown")]
    private float swingTimer = 0;
    private float swingCooldownTime = 2f;

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

    private bool isMainMenu;

    [Header("Achievement Variables")]
    private HashSet<GameObject> moleHits = new HashSet<GameObject>();
    public bool closeToSpike = false;

    void Awake()
    {
        isMainMenu = (SceneManager.GetActiveScene().name == "Main Menu");
        ball = GetComponent<Ball>();
        rb = GetComponent<Rigidbody2D>();
        camController = GameObject.FindAnyObjectByType<CameraController>();
        hasShot = true;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numDots;
        lineRenderer.startWidth = .25f;
        lineRenderer.endWidth = .25f;
        canPutt = true;
        isPuttCooldown = true;
        //ps = GetComponentInChildren<ParticleSystem>();
        //ps.gameObject.SetActive(false);
        if (cursor != null)
        {
            cursor.GetComponent<SpriteRenderer>().enabled = false;
        }


    }

    private void Start()
    {
        currentMap = Map.current;
        CurrentDirection = Direction.South;
        inv = GetComponent<Inventory>();
        // Instantiate dots spaced out behind startPos
        for (int i = 0; i < numDots; i++)
        {
            GameObject dot = Instantiate(dotPrefab);
            dot.GetComponent<SpriteRenderer>().color = inv.ballColor;
            dot.transform.parent = transform;
            dot.SetActive(false);
            float offset = i / (float)numDots;
            dots.Add(new DotData(dot, offset));
        }

        swingCooldownSlider = GameObject.Find("Swing Cooldown")?.GetComponent<Slider>();
        if (swingCooldownSlider != null)
        {
            swingCooldownSlider.gameObject.SetActive(false);
        }
        
        swingPowerSlider = GameObject.Find("Swing Power")?.GetComponent<Slider>();

        if (swingPowerSlider != null)
        {
            swingPowerSlider.gameObject.SetActive(false);
        }
        
        moveSpeed = 7;
        allFans = GameObject.FindObjectsByType<Fan>(FindObjectsSortMode.InstanceID);
        //ballRollSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.ballRollSFX);
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

    public void pause(bool isPaused)
    {
        pendingUnpause = !isPaused;
        if (isPaused)
        {
            isInputPaused = true;
        }
    }

    void Update()
    {
        if (isMainMenu)
        {
            return;
        }
        if (isInputPaused) {
            if (pendingUnpause)
            {
                isInputPaused = false;
            }
            return;
        }

        isBattleMode = (GameMode.current == GameMode.TYPE.CLUBLESS);
        didEnter = false;
        didExit = false;
        GameMode.TYPE currentMode = GameMode.current;
        checkDead();
        AnimateBall();
        //UpdateSound();
        UpdateDirection(rb.velocity);

        if (inv.numResets >= 50 && !inv.achievements[(int)Achievement.TYPE.BACK_TO_THE_BACK])
        {
            Achievement.Give(Achievement.TYPE.BACK_TO_THE_BACK);
            inv.SavePlayer();

        }

        if (PlayerInput.isDown(PlayerInput.Axis.Reset) && currentMode != GameMode.TYPE.HOLE18 && currentMode != GameMode.TYPE.HARDCORE) 
        {
            AudioManager.instance.StopAllSFXEvents();
            inv.numResets++;
            inv.SavePlayer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isBattleMode)
        {
            movement.x = PlayerInput.get(PlayerInput.Axis.Horizontal);
            movement.y = PlayerInput.get(PlayerInput.Axis.Vertical);
            movement *= PlayerInput.isController ? -1 : 1;
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

        if (isBattleMode) { return; }

        if (PlayerInput.isController)
        {
            if (!hasClickedBall && !camController.isViewMode && PlayerInput.isDown(PlayerInput.Axis.Fire2))
            {
                int index = -1;
                if (objectSelected != null)
                {
                    for (int i = 0; i < allFans.Length; i++)
                    {
                        if (allFans[i] as Selectable == objectSelected)
                        {
                            index = i;
                            break;
                        }
                    }
                }
                pickFan(index);
            }
        }
        else if (PlayerInput.isDown(PlayerInput.Axis.Fire1) && !camController.isViewMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(PlayerInput.cursorPosition);
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
       
        //ClickEnemy();
        
        isMouseButton1Held = PlayerInput.get(PlayerInput.Axis.Fire1) != 0;

        if (!camController.isViewMode)
        {
            setPutt();
        }  
        
    }

    private void pickFan(int currentIndex)
    {
        bool found = false;
        for (int i = currentIndex + 1; i < allFans.Length; i++)
        {
  
            if (Vector2.Distance(transform.position, allFans[i].transform.position) <= allFans[i].controlRadius)
            {
                if (Select(allFans[i]))
                {
                    found = true;
                    break;
                }
            }
        }
        if (!found)
        {
            Select(null);
        }
    }

    public bool Select(Component component)
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
            return false;
        }
        else
        {
            objectSelected = (Selectable)newSelected;
            camController.cam.Follow = newSelected.transform;
            ball.canPutt = false;
            return true;
        }
    }
    public void SwingCooldown()
    {
        if (!isPuttCooldown)
        {
            
            swingTimer += Time.deltaTime;
            swingCooldownSlider.value = (swingCooldownTime - swingTimer) / swingCooldownTime;

            if (swingTimer > swingCooldownTime || rb.velocity.magnitude == 0)
            {
                isPuttCooldown = true;
                swingTimer = 0f;
                swingCooldownSlider.gameObject.SetActive(false);
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
        Ray ray = Camera.main.ScreenPointToRay(PlayerInput.cursorPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (PlayerInput.isDown(PlayerInput.Axis.Fire1) && hit.collider != null && hit.collider.tag == "Enemy")
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
        else if (collision.gameObject.tag == "Cobblestone")
        {
            wallHits++;
            float volume = rb.velocity.magnitude / 10;
            if (volume > maxSFXVolume)
            {
                volume = maxSFXVolume;
            }
            if (!didExit || !didEnter)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.cobbleHit, transform.position);
            }
        }
        else if (collision.gameObject.tag == "Mole" && !inv.achievements[(int)Achievement.TYPE.WHACK_A_MOLE])
        {
            moleHits.Add(collision.gameObject);
            if (moleHits.Count >= 5)
            {
                Achievement.Give(Achievement.TYPE.WHACK_A_MOLE);
                inv.SavePlayer();
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
        startPos = transform.position;
        targetPos = cursor.transform.position;
        direction = (targetPos - startPos).normalized;

        float distance = Vector3.Distance(startPos, targetPos);
        if (distance < 0.01f) distance = 0.01f; // Avoid division by zero

        float progressSpeed = speed / distance;

        foreach (var dotData in dots)
        {
            dotData.progress += progressSpeed * Time.deltaTime;
            dotData.progress %= 1f;

            float t = (dotData.progress + dotData.offset) % 1f;
            Vector3 pos = Vector3.Lerp(startPos, targetPos, t);
            pos.z = -9.7f;  // set z to desired value

            dotData.dot.transform.position = pos;
            dotData.dot.SetActive(true);
        }

        cursor.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void ClearDots()
    {
        foreach (var dotData in dots)
        {
            dotData.dot.SetActive(false);
        }
        cursor.GetComponent<SpriteRenderer>().enabled = false;
    }

    void GrabBall()
    {
        if (!PlayerInput.isController)
        {
            Ray ray = Camera.main.ScreenPointToRay(PlayerInput.cursorPosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity, layerMask);

            foreach (RaycastHit2D hit in hits)
            {

                if (isMouseButton1Held && hit.collider != null && hit.collider.tag == "Ball" && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
                {
                    hasClickedBall = true;
                    break;
                }
            }
        }
        else if (!hasClickedBall && PlayerInput.isDown(PlayerInput.Axis.Fire1))
        {
            hasClickedBall = true;
            PlayerInput.resetCursor();
        }
        if (PlayerInput.isDown(PlayerInput.Axis.Fire2))
        {
            hasClickedBall = false;
            cursor.GetComponent<SpriteRenderer>().enabled = false;
            swingPowerSlider.gameObject.SetActive(false);
            ClearDots();
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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(PlayerInput.cursorPosition);
            Vector2 ballPos = ball.transform.position;

            Vector2 mousePosNormalized = (mousePos - ballPos).normalized;

            Vector2 mirroredCursorPos = ballPos - (mousePos - ballPos);
            cursor.transform.position = new Vector3(mirroredCursorPos.x, mirroredCursorPos.y, -9.71f);

            float distFromBall = Vector2.Distance(mousePos, ballPos);
            if (distFromBall > maxHitSpeed)
            {
                distFromBall = maxHitSpeed;
            }

            Vector2 force = -mousePosNormalized * distFromBall;

            swingPowerSlider.gameObject.SetActive(true);
            swingPowerSlider.value = force.magnitude / maxHitSpeed;

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
        FindObjectOfType<GhostRecorder>().recordFrame();
        ClearDots();
        swingPowerSlider.gameObject.SetActive(false);
        swingCooldownSlider.gameObject.SetActive(true);
        hasClickedBall = false;
        isPuttCooldown = false;

        if (!inv.achievements[(int)Achievement.TYPE.WHACK_A_MOLE])
        {
            moleHits.Clear();
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(PlayerInput.cursorPosition);
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
        DisplayParticles();
    }

    public void DisplayParticles()
    {
        var particles = transform.Find("Grass Particles").GetComponent<ParticleSystem>();
        if (particles != null)
        {
            if (rb.velocity.magnitude > minSpeedForParticles && !particles.isPlaying)
            {
                var velocityModule = particles.velocityOverLifetime;
                velocityModule.enabled = true;

                // Set constant velocity based on ball velocity
                velocityModule.x = new ParticleSystem.MinMaxCurve(-(rb.velocity.x / 2));
                velocityModule.y = new ParticleSystem.MinMaxCurve(-(rb.velocity.y / 2));
                velocityModule.z = new ParticleSystem.MinMaxCurve(0f);
                particles.Play();
            }
            else if (rb.velocity.magnitude <= minSpeedForParticles && particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }

    public void DisplayWindParticles()
    {
        var particles = transform.Find("Wind Particles").GetComponent<ParticleSystem>();
        if (particles != null)
        {
            if (rb.velocity.magnitude > minSpeedForParticles && !particles.isPlaying)
            {
                var velocityModule = particles.velocityOverLifetime;
                velocityModule.enabled = true;

                // Set constant velocity based on ball velocity
                velocityModule.x = new ParticleSystem.MinMaxCurve(-(rb.velocity.x / 2));
                velocityModule.y = new ParticleSystem.MinMaxCurve(-(rb.velocity.y / 2));
                velocityModule.z = new ParticleSystem.MinMaxCurve(0f);
                particles.Play();
            }
            else if (rb.velocity.magnitude <= minSpeedForParticles && particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }

    public void DisplayFreezeParticles()
    {
        var particles = transform.Find("Freeze Particles").GetComponent<ParticleSystem>();
        if (particles != null)
        {
            if (!particles.isPlaying)
            {
                particles.Play();
            }
            else if (particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }

    public void DisplayTeleportParticles()
    {
        var particles = transform.Find("Teleport Particles").GetComponent<ParticleSystem>();
        if (particles != null)
        {
            if (!particles.isPlaying)
            {
                particles.Play();
            }
            else if (particles.isPlaying)
            {
                particles.Stop();
            }
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

    public class DotData
    {
        public GameObject dot;
        public float progress;
        public float offset = 1f;  // how much to delay its start

        public DotData(GameObject dot, float offset)
        {
            this.dot = dot;
            this.offset = offset;
            this.progress = offset;  // start with the offset
        }
    }

}
