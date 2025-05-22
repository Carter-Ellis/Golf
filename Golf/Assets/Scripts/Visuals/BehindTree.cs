using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BehindTree : MonoBehaviour
{
    private SpriteRenderer trunkSr;
    private SpriteRenderer leavesSr;
    private float fadingSpeed = 0.05f;
    private float fadeToTransparentAmount = .5f;
    private float fadedAmount;
    private bool exited;
    private GameObject trunk;
    private GameObject leaves;
    private Color leavesColor;
    private Color trunkColor;
    void Start()
    {
        if (transform.parent.transform.childCount < 2)
        {
            return;
        }
        gameObject.SetActive(true);
        leavesColor = transform.parent.GetChild(0).GetComponent<SpriteRenderer>().color;
        trunkColor = transform.parent.GetChild(1).GetComponent<SpriteRenderer>().color;
        leaves = transform.parent.GetChild(0).gameObject;
        trunk = transform.parent.GetChild(1).gameObject;
        
        trunkSr = trunk.GetComponent<SpriteRenderer>();
        leavesSr = leaves.GetComponent<SpriteRenderer>();
        exited = true;
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
            
            trunkSr.color = new Color(trunkColor.r, trunkColor.g, trunkColor.b, i);
            leavesSr.color = new Color(leavesColor.r, leavesColor.g, leavesColor.b, i);
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
            trunkSr.color = new Color(trunkColor.r, trunkColor.g, trunkColor.b, i);
            leavesSr.color = new Color(leavesColor.r, leavesColor.g, leavesColor.b, i);
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
