using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OutOfBounds : MonoBehaviour
{
    Ball ball;

    private Vector3 scaleChange = new Vector3(-1f, -1f, 0f);
    private float fallTime = .2f;
    private bool isFalling;
    private float fallSpeed = 1;
    private float endTime;
    private float triggerCheckDuration = .2f;
    private TilemapCollider2D tilemapCollider;

    void Start()
    {
        ball = FindObjectOfType<Ball>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    void Update()
    {
        if (!isFalling)
        {
            return;
        }

        if (ball.transform.localScale.x <= 0)
        {
            ball.cursor.SetActive(false);
            ball.swingPowerSlider.gameObject.SetActive(false);
            ball.powerTxt.gameObject.SetActive(false);
            ball.cancelImage.SetActive(false);
            ball.TakeDamage(100);
            isFalling = false;
        }
        else
        {
            ball.transform.position = Vector2.MoveTowards(ball.transform.position, (Vector2)transform.position - new Vector2(0, .1f), fallSpeed * Time.deltaTime);
            ball.transform.root.localScale += scaleChange / fallTime * Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isFalling && collision.CompareTag("Ball"))
        {
            if (Time.time < endTime)
            {
                return;
            }

            endTime = Time.time + triggerCheckDuration;

            if (IsFullyInsideCollider(collision, tilemapCollider))
            {
                isFalling = true;
            }
        }
    }

    private bool IsFullyInsideCollider(Collider2D ballCollider, TilemapCollider2D tilemapCollider)
    {
        Bounds b = ballCollider.bounds;
        Vector2[] pointsToCheck = new Vector2[]
        {
            b.min,
            b.max,
            new Vector2(b.min.x, b.max.y),
            new Vector2(b.max.x, b.min.y),
            b.center
        };

        foreach (var point in pointsToCheck)
        {
            if (!tilemapCollider.OverlapPoint(point))
            {
                return false;
            }
        }
        return true;
    }
}
