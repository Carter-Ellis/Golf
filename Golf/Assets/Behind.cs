using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behind : MonoBehaviour
{
    private Ball ball;
    private SpriteRenderer spriteRenderer;
    private float fadingSpeed = 0.05f;
    private float fadeToTransparentAmount = .5f;
    private float fadedAmount;
    private bool exited;
    private Color color;
    private SpriteRenderer childSr;

    void Start()
    {
        gameObject.SetActive(true);
        ball = GameObject.FindObjectOfType<Ball>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        color = Color.white;
        exited = true;

        if (transform.childCount == 1)
        {
            childSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

    }

    IEnumerator FadeToTransparent()
    {
        for (float i = 1f; i >= fadeToTransparentAmount; i -= 0.05f)
        {
            if (exited)
            {
                break;
            }
            fadedAmount = i;

            spriteRenderer.color = new Color(color.r, color.g, color.b, i);
            if (transform.childCount == 1)
            {
                childSr.color = new Color(color.r, color.g, color.b, i);
            }
            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    IEnumerator FadeToSolid()
    {
        for (float i = fadedAmount; i <= 1.05; i += 0.05f)
        {
            if (!exited)
            {
                break;
            }
            spriteRenderer.color = new Color(color.r, color.g, color.b, i);
            if (transform.childCount == 1)
            {
                childSr.color = new Color(color.r, color.g, color.b, i);
            }
            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Make transparent when behind

        if (collision.gameObject != null && collision.gameObject.tag == "Ball")
        {
            exited = false;
            StartCoroutine(FadeToTransparent());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.tag == "Ball")
        {
            //Make transparent when not behind
            exited = true;
            StartCoroutine(FadeToSolid());
        }
    }
}
