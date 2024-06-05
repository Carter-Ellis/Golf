using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour, ClickTarget
{
    GameObject obj;
    Ball ball;
    CinemachineVirtualCamera cam;
    public float blowingPower = .03f;
    private bool isBlowing;
    private bool isSelected;
    public float rotationSpeed = 50f;
    public float rotationBounds;


    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
        cam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        float rad = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        Vector2 direction = (new Vector2((float)Mathf.Cos(rad), (float)Mathf.Sin(rad))).normalized;

        Rotate();

        if (isBlowing && obj != null)
        {
            if (obj.gameObject.tag == "Ball" && ball.GetComponent<Rigidbody2D>().velocity.magnitude > .5f)
            {
                obj.GetComponent<Rigidbody2D>().velocity += blowingPower * direction;
            }
            else if (obj.gameObject.tag == "Ball")
            {
                obj.GetComponent<Rigidbody2D>().velocity = direction;
            }
            else
            {
                obj.GetComponent<Rigidbody2D>().velocity += blowingPower * direction;
            }
            
        }
    }


    private void Rotate()
    { 
        if (!isSelected)
        {
            return;
        }
        float rotationIncrement = rotationSpeed * Time.deltaTime;
        Vector3 rotationVector = new Vector3(0, 0, rotationIncrement);

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - rotationVector);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationVector);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == null)
        {
            return;
        }
        obj = collision.gameObject;
        isBlowing = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        obj = null;
        isBlowing = false;
    }

    public void onClick()
    {
        isSelected = !isSelected;
        cam.Follow = isSelected ? transform : ball.transform;
        ball.canPutt = isSelected ? false : true;   
        ball.objectControlled = ball.objectControlled == null ? gameObject : null; 
        
        gameObject.GetComponent<SpriteRenderer>().color = isSelected ? Color.green : Color.white;
    }
}
