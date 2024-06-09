using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    GameObject obj;
    Ball ball;
    public float blowingPower = .03f;
    private bool isBlowing;

    void Start()
    {
        ball = FindObjectOfType<Ball>();
    }

    void Update()
    {
        float rad = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        Vector2 direction = (new Vector2((float)Mathf.Cos(rad), (float)Mathf.Sin(rad))).normalized;

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

    private void OnTriggerStay2D(Collider2D collision)
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
