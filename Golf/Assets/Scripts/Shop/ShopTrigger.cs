using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private UI_Shop uishop;
    private bool isActive;
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
        if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.tag == "Shop")
        {
            isActive = !isActive;
            uishop.gameObject.SetActive(isActive);
            
        }
        /*else if (Input.GetKey)
        {
            isActive = false;
            uishop.gameObject.SetActive(isActive);
        }*/
        if (Vector2.Distance((Vector2)ball.transform.position, (Vector2)transform.position) > shopRadius)
        {
            uishop.gameObject.SetActive(false);
        }
    }

}
