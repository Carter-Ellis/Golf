using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFall : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ball")
        {

            Collider2D triggerCollider = GetComponent<Collider2D>();
            Bounds triggerBounds = triggerCollider.bounds;

            if (collision.transform.position.y > triggerBounds.max.y)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }

        }
    }


}
