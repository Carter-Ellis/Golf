using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    Ball ball;
    Rigidbody2D rb;
    float timer = 0f;
    float lifetime = 1f;
    public float rotationSpeed = 30f;
    private bool isCopied;
    private Color color;
    private SpriteRenderer sr;
    private float fadingSpeed = 0.04f;
    private float fadeToTransparentAmount = 0f;
    public Sprite eatenApple;
    private bool eaten;
    void Start()
    {
        ball = FindObjectOfType<Ball>();
        rb = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        color = sr.color;

    }

    private void Update()
    {
        if (eaten)
        {
            FlyAway();
        }
        
    }

    private void FlyAway()
    {
        if (!isCopied)
        {
            rb.velocity = new Vector2(-ball.GetComponent<Rigidbody2D>().velocity.normalized.x * 2, -ball.GetComponent<Rigidbody2D>().velocity.normalized.y * 2);
            isCopied = true;
            StartCoroutine(FadeToTransparent());
        }

        rotationSpeed *= .99f;

        transform.Rotate(0, 0, rotationSpeed);


        timer += Time.deltaTime;

        if (timer > lifetime)
        {
            Destroy(gameObject);
        }

    }

    IEnumerator FadeToTransparent()
    {
        for (float i = 1f; i >= fadeToTransparentAmount; i -= 0.05f)
        {

            sr.color = new Color(color.r, color.g, color.b, i);

            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" && !eaten)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = eatenApple;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.appleBite, transform.position);
            eaten = true;
        }
    }

}
