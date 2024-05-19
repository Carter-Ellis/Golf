using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    public float openSpeed = 3f;
    private float topPosition;
    private float bottomPosition;
    private float leftPosition;
    private float rightPosition;
    private bool isOpen;
    private Rigidbody2D doorRB;
    void Start()
    {
        doorRB = door.GetComponent<Rigidbody2D>();
        topPosition = Mathf.Abs(door.transform.position.y + (door.GetComponent<Renderer>().bounds.size.y / 2f));
        bottomPosition = Mathf.Abs(door.transform.position.y - (door.GetComponent<Renderer>().bounds.size.y / 2f));
        leftPosition = Mathf.Abs(door.transform.position.x - (door.GetComponent<Renderer>().bounds.size.x / 2f));
        rightPosition = Mathf.Abs(door.transform.position.x + (door.GetComponent<Renderer>().bounds.size.x / 2f));
        isOpen = false;
    }

    void Update()
    {
        if (isOpen)
        {
            topPosition = Mathf.Abs(door.transform.position.y + (door.GetComponent<Renderer>().bounds.size.y / 2f));
            leftPosition = Mathf.Abs(door.transform.position.x - (door.GetComponent<Renderer>().bounds.size.x / 2f));
            if (bottomPosition >= topPosition || rightPosition >= leftPosition)
            {
                doorRB.velocity = Vector2.zero;
                isOpen = false;
            }
        }
        else
        {
            rightPosition = Mathf.Abs(door.transform.position.x + (door.GetComponent<Renderer>().bounds.size.x / 2f));
            bottomPosition = Mathf.Abs(door.transform.position.y - (door.GetComponent<Renderer>().bounds.size.y / 2f));
            if (bottomPosition >= topPosition || rightPosition >= leftPosition)
            {
                doorRB.velocity = Vector2.zero;
                isOpen = true;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (door == null) 
        {
            return;
        }
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        

    }

    void OpenDoor()
    {
        float zAngle = door.transform.eulerAngles.z + 90;
        float zAngleRad = zAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(zAngleRad), Mathf.Sin(zAngleRad));
        topPosition = Mathf.Abs(door.transform.position.y + (door.GetComponent<Renderer>().bounds.size.y / 2f));     
        doorRB.velocity = direction.normalized * openSpeed;       
    }
    void CloseDoor()
    {
        float zAngle = door.transform.eulerAngles.z - 90;
        float zAngleRad = zAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(zAngleRad), Mathf.Sin(zAngleRad));
        bottomPosition = Mathf.Abs(door.transform.position.y - (door.GetComponent<Renderer>().bounds.size.y / 2f));
        doorRB.velocity = direction.normalized * openSpeed;
    }
}
