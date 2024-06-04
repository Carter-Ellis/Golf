using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    GameObject obj;
    Ball ball;
    public float blowingPower = .03f;
    private bool isBlowing;


    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
    }

    private void Update()
    {
        float rad = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        print(rad);
        Vector2 direction = (new Vector2((float)Mathf.Cos(rad), (float)Mathf.Sin(rad))).normalized;
        print(direction);
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

}
