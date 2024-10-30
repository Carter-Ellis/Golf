using UnityEngine;

public class Fan : MonoBehaviour, Selectable
{
    private Ball ball;
    private Wind wind;
    private Transform center;
    public GameObject spriteObj;
    private bool isSelected;
    public float controlRadius = 10f;
    public float rotationSpeed = 50f;
    public float blowingPower = .07f;
    private Quaternion origRotation;
    private Animator anim;
    public float rotationBounds = 90f;



    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        wind = GetComponentInChildren<Wind>();
        anim = GetComponentInChildren<Animator>();
        center = transform.GetChild(0);
        origRotation = transform.rotation;
        if (wind != null)
        {
            wind.blowingPower = blowingPower;
        }
        UpdateSprite();
    }


    private void Update()
    {
        if (ball == null) 
            return;
        if (!isSelected)
        {
            return;
        }
        if (Vector2.Distance(ball.transform.position, transform.position) >= controlRadius)
        {
            ball.Select(null);
            return;
        }
        Rotate();
    }

    public void onSelect()
    {
        isSelected = !isSelected;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = isSelected ? Color.green : Color.white;
    }

    public void onDeselect()
    {
        isSelected = false;
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    
    private void UpdateSprite()
    {
        int spriteIndex = 8 - (int)((center.rotation.eulerAngles.z / 45f) + .5f);
        if (spriteIndex >= 8)
        {
            spriteIndex = 0;
        }
        anim.SetFloat("Angle", spriteIndex);
    }
    private void Rotate()
    { 
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        UpdateSprite();
        if (left == right) 
        {
            return;
        }

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
            if (zAngle >= 180)
            {
                zAngle -= 360;
            }

            if (zAngle > 22.5f)
            {
                zAngle -= 45f;
            }
            else if (zAngle < -22.5)
            {
                zAngle += 45f;
            }
            
            angles.z = zAngle;
            //spriteObj.transform.rotation = Quaternion.Euler(angles);
        }

        

    }

}
