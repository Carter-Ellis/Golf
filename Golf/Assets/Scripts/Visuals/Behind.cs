using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Behind : MonoBehaviour
{
    private Ball ball;
    private SpriteRenderer sr;
    private float fadingSpeed = 0.05f;
    private float fadeToTransparentAmount = .5f;
    private float fadedAmount;
    private bool exited;
    void Start()
    {
        gameObject.SetActive(true);
        ball = GameObject.FindObjectOfType<Ball>();
        sr = GetComponent<SpriteRenderer>();
        exited = true;
    }

    void Update()
    {
        if (ball.transform.position.y > transform.position.y - .75f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10f);
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
            sr.color = new Color(1f, 1f, 1f, i);
            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    IEnumerator FadeToSolid()
    {
        for (float i = fadedAmount; i <= 1.05; i += 0.05f)
        {
            
            sr.color = new Color(1f, 1f, 1f, i);
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
