using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    GameObject obj;
    Ball ball;
    public float blowingPower = .03f;
    private bool isBlowing;
    private BoxCollider2D boxCollider;
    private ParticleSystem particleSys;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;
    private ParticleSystem.MainModule mainModule;
    private float travelDist;
    void Start()
    {
        ball = FindObjectOfType<Ball>();
        particleSys = transform.GetChild(0).GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider2D>();
        travelDist = boxCollider.size.x > boxCollider.size.y ? boxCollider.size.x : boxCollider.size.y;  
        mainModule = particleSys.main;
        velocityModule = particleSys.velocityOverLifetime;

    }

    void Update()
    {
        float rad = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
        Vector2 direction = (new Vector2((float)Mathf.Cos(rad), (float)Mathf.Sin(rad))).normalized;
        float lifetime = travelDist / (blowingPower * 100);
        float particleLife = lifetime;
      
        mainModule.startLifetime = particleLife;
        
        
        
        
        velocityModule.speedModifier = blowingPower * 20f;
        
        if (isBlowing && obj != null)
        {
            if (obj.gameObject.tag == "Ball" && ball.GetComponent<Rigidbody2D>().velocity.magnitude > .5f)
            {
                obj.GetComponent<Rigidbody2D>().velocity += blowingPower * direction / Vector2.Distance(transform.position, ball.transform.position);
            }
            else if (obj.gameObject.tag == "Interactable" && obj.GetComponent<Rigidbody2D>().velocity.magnitude > .5f){
                obj.GetComponent<Rigidbody2D>().velocity += blowingPower * direction;
                
            }
            else if (obj.gameObject.tag == "Ball" || obj.gameObject.tag == "Interactable")
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
