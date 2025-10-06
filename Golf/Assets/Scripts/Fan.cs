using Cinemachine;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(LineRenderer))]
public class Fan : MonoBehaviour, Selectable
{
    private Ball ball;
    private Wind wind;
    private Transform center;
    private CameraController cam;
    public GameObject spriteObj;
    private bool isSelected;
    public float controlRadius = 10f;
    public float rotationSpeed = 50f;
    public float blowingPower = 0.07f;
    private Quaternion origRotation;
    private Animator anim;
    public float rotationBounds = 90f;
    public bool isSelectable;
    private bool firstTouch;

    private SoundEffect fanSFX;
    private LineRenderer line;
    private int clickCount;

    private void Start()
    {
        ball = FindObjectOfType<Ball>();
        wind = GetComponentInChildren<Wind>();
        anim = GetComponentInChildren<Animator>();
        cam = FindObjectOfType<CameraController>();
        center = transform.GetChild(0);
        origRotation = transform.rotation;
        fanSFX = new SoundEffect(FMODEvents.instance.fan);
        fanSFX.play(this);

        if (wind != null)
        {
            wind.blowingPower = blowingPower;
        }

        // Set up LineRenderer
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = 0.07f;
        line.endWidth = 0.07f;
        line.enabled = false;
        line.numCapVertices = 8;

        line.colorGradient = new Gradient
        {
            colorKeys = new GradientColorKey[] {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(Color.white, 1f)
            },
            alphaKeys = new GradientAlphaKey[] {
                new GradientAlphaKey(.5f, 0f),
                new GradientAlphaKey(0f, 1f)
            }
        };

        if (!isSelectable)
        {
            spriteObj.GetComponent<SpriteRenderer>().color = new Color(.6f, .6f, .6f);
        }
        

        UpdateSprite();
    }

    private void Update()
    {
        if (ball == null || !isSelectable) return;

        if (PlayerInput.isDown(PlayerInput.Axis.Fire1)) return;
        if (PlayerInput.isUp(PlayerInput.Axis.Fire1))
        {
            firstTouch = false;
        }
        if (PlayerInput.get(PlayerInput.Axis.Fire1) == 0) return;

        if (!isSelected)
        {
            line.enabled = false;
            return;
        }

        // Show and update the line when selected
        UpdateLine();

        if (Vector2.Distance(ball.transform.position, transform.position) >= controlRadius && !cam.isViewMode)
        {
            ball.Select(null);
            return;
        }

        Rotate();
    }

    public bool onSelect()
    {
        if (!isSelectable) { return false; }

        firstTouch = true;
        isSelected = !isSelected;
        if (isSelected)
        {
            clickCount++;
        }

        gameObject.GetComponentInChildren<SpriteRenderer>().color = isSelected ? Color.green : Color.white;
        line.enabled = isSelected;
        ball.isSelectFan = isSelected;

        if (ball != null && clickCount >= 20 && !ball.GetComponent<Inventory>().achievements[(int)Achievement.TYPE.TORNADO])
        {
            Achievement.Give(Achievement.TYPE.TORNADO);
            ball.GetComponent<Inventory>().SavePlayer();
        }

        return true;
    }

    public void onDeselect()
    {
        isSelected = false;
        ball.isSelectFan = false;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        line.enabled = false;
    }

    private void UpdateSprite()
    {
        int spriteIndex = 8 - (int)((center.rotation.eulerAngles.z / 45f) + 0.5f);
        if (spriteIndex >= 8)
        {
            spriteIndex = 0;
        }
        anim.SetFloat("Angle", spriteIndex);
    }

    private void Rotate()
    {
        UpdateSprite();

        if (Input.touchCount == 0) return;
        if (firstTouch)
        {
            if (Input.touches[0].deltaPosition.magnitude < 10f)
            {
                return;
            }
            firstTouch = false;
        }

        Vector2 dir = (Input.touches[0].position - new Vector2(Screen.width, Screen.height) / 2).normalized;
        center.rotation = Quaternion.FromToRotation(Vector2.down, dir);
    }

    private void UpdateLine()
    {
        Vector3 start = center.position;
        Vector3 direction = -center.up.normalized;
        Vector3 end = start + direction * 4f;

        start.z = -8f;
        end.z = -8f;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    private void OnDestroy()
    {
        fanSFX.stop();
    }

}
