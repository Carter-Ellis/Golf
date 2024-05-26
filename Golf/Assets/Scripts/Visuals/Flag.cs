using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private float liftAmount = 1.5f;
    private float startPos;
    private float currentLift;
    private float liftingSpeed = .001f;
    private float origLiftingSpeed;
    private bool exited;
    private SpriteRenderer sr;
    private Ball ball;
    private float fadingSpeed = 0.05f;
    private float fadeToTransparentAmount = .5f;
    private float fadedAmount;
    Transform flagTransform;
    public GameObject flag;

    private void Awake()
    {
        startPos = transform.position.y;
        gameObject.SetActive(true);
        ball = GameObject.FindObjectOfType<Ball>();
        flagTransform = flag.transform;
        sr = flag.GetComponent<SpriteRenderer>();
        exited = true;
        origLiftingSpeed = liftingSpeed;
    }

    IEnumerator MoveUp()
    {
        for (float i = startPos; i < liftAmount + startPos; i +=.05f)
        {
            if (exited)
            {
                break;
            }
            currentLift = i;
            flagTransform.position = new Vector3(flagTransform.position.x, i, flagTransform.position.z);
            liftingSpeed = Mathf.Pow(liftingSpeed, .96f);        
            yield return new WaitForSeconds(liftingSpeed);
        }
        liftingSpeed = origLiftingSpeed;
    }

    IEnumerator MoveDown()
    {
        for (float i = currentLift; i > startPos - 0.05f; i -= 0.05f)
        {
            if (!exited)
            {
                break;
            }
            flagTransform.position = new Vector3(flagTransform.position.x, i, flagTransform.position.z);
            liftingSpeed = Mathf.Pow(liftingSpeed, .96f);
            yield return new WaitForSeconds(liftingSpeed);
        }
        liftingSpeed = origLiftingSpeed;
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
            if (!exited)
            {
                break;
            }
            sr.color = new Color(1f, 1f, 1f, i);
            yield return new WaitForSeconds(fadingSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Move up
        if (collision.gameObject != null && collision.gameObject.tag == "Ball")
        {
            exited = false;
            StartCoroutine(MoveUp());
        }

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
            //Move down
            exited = true;
            StartCoroutine(MoveDown());
        }

        if (collision.gameObject != null && collision.gameObject.tag == "Ball")
        {
            //Make transparent when not behind
            exited = true;
            StartCoroutine(FadeToSolid());
        }
    }
    
}
