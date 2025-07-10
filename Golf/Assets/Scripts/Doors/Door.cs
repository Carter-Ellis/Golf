using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ButtonTarget
{
    private enum DOOR_STATE
    {
        CLOSED,
        OPEN,
        CLOSING,
        OPENING,

    };

    public float speed = 3f;
    private DOOR_STATE state = DOOR_STATE.CLOSED;
    private Rigidbody2D doorRB;
    private BoxCollider2D doorCollider;
    private Vector2 startPos;
    private Vector2 endPos;
    private float travelDist;
    private SoundEffect doorSFX;

    void Start()
    {
        doorSFX = new SoundEffect(FMODEvents.instance.door6sec);
        doorRB = GetComponent<Rigidbody2D>();
        doorCollider = GetComponent<BoxCollider2D>();
        startPos = transform.position;
        travelDist = Mathf.Sqrt(Mathf.Pow(doorCollider.bounds.size.y, 2) + Mathf.Pow(doorCollider.bounds.size.x, 2));
        endPos = startPos + ((Vector2)(transform.rotation * Vector3.up)).normalized * travelDist;

    }

    void Update()
    {
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
                doorSFX.stop();
            }

        }
        else
        {
            if (((Vector2)transform.position - endPos).magnitude >= travelDist)
            {
                doorRB.velocity = Vector2.zero;
                state = DOOR_STATE.CLOSED;
                doorSFX.stop();
            }
        }

    }

    void OpenDoor()
    {
        Vector3 direction = transform.rotation * Vector3.up;
        doorRB.velocity = direction.normalized * speed;
        state = DOOR_STATE.OPENING;
        doorSFX.play(this);
    }
    void CloseDoor()
    {
        Vector3 direction = transform.rotation * Vector3.up;
        doorRB.velocity = -direction.normalized * speed;
        state = DOOR_STATE.CLOSING;
        doorSFX.play(this);
    }

    public void onPress()
    {
        if (state == DOOR_STATE.OPENING || state == DOOR_STATE.OPEN)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OnDestroy()
    {
        doorSFX.stop();
    }

}
