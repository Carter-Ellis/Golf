using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoy : MonoBehaviour
{
    public float floatAmplitude = 0.5f;      
    public float floatFrequency = 1f;        
    public Vector2 currentDrift = new Vector2(0.1f, -0.05f);

    private Rigidbody2D rb;
    private float elapsedTime;
    public bool inWater;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb.bodyType == RigidbodyType2D.Static)
        {
            return;
        }
        //if (!inWater)
            //return;

        elapsedTime += Time.fixedDeltaTime;

        // Vertical float using sine wave velocity
        float offsetY = Mathf.Cos(elapsedTime * floatFrequency) * floatFrequency * floatAmplitude;

        // Combine with steady current drift
        Vector2 floatVelocity = new Vector2(currentDrift.x, currentDrift.y + offsetY);

        rb.velocity = floatVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (GameMode.current != GameMode.TYPE.CLUBLESS)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bouncer, transform.position);
        }

        string otherLayerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            inWater = true;
        }
    }

}
