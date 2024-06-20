using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TreeEditor;
using UnityEngine;

public class Fan : MonoBehaviour, Selectable
{
    Ball ball;
    Wind wind;
    CinemachineVirtualCamera cam;
    private bool isSelected;
    private float controlRadius = 10f;
    public float rotationSpeed = 50f;
    public float blowingPower = .07f;
    private Quaternion origRotation;
    public float rotationBounds = 90f;



    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        cam = FindObjectOfType<CinemachineVirtualCamera>();
        wind = GetComponentInChildren<Wind>();
        origRotation = transform.rotation;
        if (wind != null)
        {
            wind.blowingPower = blowingPower;
        }
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
        gameObject.GetComponent<SpriteRenderer>().color = isSelected ? Color.green : Color.white;
    }

    public void onDeselect()
    {
        isSelected = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    

    private void Rotate()
    { 
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        if (left == right) 
        {
            return;
        }

        float dir = left ? 1 : -1;
        float rotationIncrement = dir * rotationSpeed * Time.deltaTime;

        Quaternion newRot = transform.rotation * Quaternion.AngleAxis(rotationIncrement, Vector3.forward);

        if (Quaternion.Angle(newRot, origRotation) >= rotationBounds)
        {
            Quaternion max = origRotation * Quaternion.AngleAxis(rotationBounds * dir, Vector3.forward);
            transform.rotation = max;
        }
        else
        {
            transform.rotation = newRot;
        }

    }

}
