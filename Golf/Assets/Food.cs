using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.appleBite, transform.position);
            Destroy(gameObject);
        }
    }
    
}
