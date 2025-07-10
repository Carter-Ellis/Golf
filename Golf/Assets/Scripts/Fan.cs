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
        float input = PlayerInput.get(PlayerInput.Axis.Horizontal) * (PlayerInput.isController ? 1 : -1);
        bool left =  input > 0;
        bool right = input < 0;
        UpdateSprite();

        if (left == right) return;

        float dir = left ? 1 : -1;
        float rotationIncrement = dir * rotationSpeed * Time.deltaTime;

        Quaternion rotMat = Quaternion.AngleAxis(rotationIncrement, Vector3.forward);
        Quaternion newRot = center.rotation * rotMat;

        if (Quaternion.Angle(newRot, origRotation) >= rotationBounds)
        {
            Quaternion max = origRotation * Quaternion.AngleAxis(rotationBounds * dir, Vector3.forward);
            center.rotation = max;
        }
        else
        {
            center.rotation = newRot;

            Quaternion spriteRot = spriteObj.transform.rotation * rotMat;
            Vector3 angles = spriteRot.eulerAngles;
            float zAngle = angles.z;

            if (zAngle >= 180) zAngle -= 360;

            if (zAngle > 22.5f)
                zAngle -= 45f;
            else if (zAngle < -22.5)
                zAngle += 45f;

            angles.z = zAngle;
            // spriteObj.transform.rotation = Quaternion.Euler(angles);
        }
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
