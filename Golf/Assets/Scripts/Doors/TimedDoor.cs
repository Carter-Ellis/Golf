using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDoor : MonoBehaviour, ButtonTarget
{
    private enum DOOR_STATE
    {
        CLOSED,
        OPEN,
        CLOSING,
        OPENING,

    };

    public float speed = 3f;
    public float openTime = 2f;
    private DOOR_STATE state = DOOR_STATE.CLOSED;
    private Rigidbody2D doorRB;
    private BoxCollider2D doorCollider;
    private Vector2 startPos;
    private Vector2 endPos;
    private float travelDist;
    private float timer = 0;
    private bool isTiming;
    private EventInstance doorTimeSFX;
    void Start()
    {
        doorRB = GetComponent<Rigidbody2D>();
        doorCollider = GetComponent<BoxCollider2D>();
        startPos = transform.position;
        travelDist = Mathf.Sqrt(Mathf.Pow(doorCollider.bounds.size.y, 2) + Mathf.Pow(doorCollider.bounds.size.x, 2));
        endPos = startPos + ((Vector2)(transform.rotation * Vector3.up)).normalized * travelDist;
        //doorTimeSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.door6sec);
    }

    void Update()
    {
        if (isTiming)
        {
            timer += Time.deltaTime;
            if (timer > openTime)
            {
                CloseDoor();
                timer = 0;
                isTiming = false;
            }
        }
        if (state == DOOR_STATE.OPEN || state == DOOR_STATE.CLOSED)
        {
            return;
        }

        if (state == DOOR_STATE.OPENING)
        {
            if (((Vector2)transform.position - startPos).magnitude >= travelDist)
            {
                doorRB.velocity = Vector2.zero;
                state = DOOR_STATE.OPEN;
                isTiming = true;
            }

        }
        else
        {
            if (((Vector2)transform.position - endPos).magnitude >= travelDist)
            {
                doorRB.velocity = Vector2.zero;
                state = DOOR_STATE.CLOSED;
            }
        }

    }

    void OpenDoor()
    {
        Vector3 direction = transform.rotation * Vector3.up;
        doorRB.velocity = direction.normalized * speed;
        state = DOOR_STATE.OPENING;
        
    }
    void CloseDoor()
    {
        Vector3 direction = transform.rotation * Vector3.up;
        doorRB.velocity = -direction.normalized * speed;
        state = DOOR_STATE.CLOSING;
    }

    public void onPress()
    {
        timer = 0;
        if (state == DOOR_STATE.OPEN || state == DOOR_STATE.OPENING)
        {
            doorTimeSFX.stop(STOP_MODE.IMMEDIATE);
            doorTimeSFX.start();
        }
        else
        {
            doorTimeSFX.start();
        }
        if (state == DOOR_STATE.OPENING || state == DOOR_STATE.OPEN)
        {

        }
        else
        {
            OpenDoor();
        }
    }
}
