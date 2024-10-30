using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Tee : MonoBehaviour
{
    Ball ball;
    Rigidbody2D rb;
    float timer = 0f;
    float lifetime = 1f;
    public float rotationSpeed = 30f;
    private bool isCopied;
    private Color color;
    private SpriteRenderer sr;
    private float fadingSpeed = 0.05f;
    private float fadeToTransparentAmount = 0f;

    void Start()
    {
        ball = FindObjectOfType<Ball>();
        rb = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        color = sr.color;
        
    }

    void Update()
    {

        if (ball.strokes > 0)
        {
            FlyAway();
        }
    }

    private void FlyAway()
    {
        if (!isCopied)
        {
            rb.velocity = new Vector2(-ball.GetComponent<Rigidbody2D>().velocity.x, -ball.GetComponent<Rigidbody2D>().velocity.y);
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

}
