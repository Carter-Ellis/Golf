using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopTrigger : MonoBehaviour
{
    private bool isActive;
    bool isSoundPlayed;
    bool isInRange;
    Ball ball;
    private float shopRadius = 7f;
    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
    }
    private void Update()
    {
        if (ball == null)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.tag == "Shop" && isInRange)
        {
            SceneManager.LoadSceneAsync("Shop");
            isActive = !isActive;
            if (isActive)
            {
                isSoundPlayed = false;
                Audio.playSFX(FMODEvents.instance.menuOpen, transform.position);
            }
            else
            {
                Audio.playSFX(FMODEvents.instance.menuClose, transform.position);
            }

        }
        if (Vector2.Distance((Vector2)ball.transform.position, (Vector2)transform.position) > shopRadius && isActive)
        {
            isInRange = false;
            isActive = false;
            if (!isSoundPlayed)
            {
                Audio.playSFX(FMODEvents.instance.menuClose, transform.position);
                isSoundPlayed = true;
            }
           
        }
        else if (Vector2.Distance((Vector2)ball.transform.position, (Vector2)transform.position) > shopRadius)
        {
            isInRange = false;
        }
        else
        {
            isInRange = true;
        }
        
    }

}
