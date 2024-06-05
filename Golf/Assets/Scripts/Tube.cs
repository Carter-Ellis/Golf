using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public enum DIRECTION
    {
        UP,
        DOWN, 
        LEFT, 
        RIGHT,
    };

    public GameObject exit;
    private Ball ball;

    private Vector3 exitPos;
    private Vector3 scaleChange = new Vector3(-1f, -1f, 0);
    private Vector3 origScale = new Vector3(1, 1, 1);

    
    private bool isTraveling;
    private bool played;

    private float travelTime = 1f;
    private float fallTime = .2f;
    private float fallSpeed;
    private float travelSpeed;
    public float ballOverHoleSpeed = 10f;
    public float exitSpeed = 2f;
    public float timer = 0f;
    public float rand = 0;
    private float playEndRange = 1f;
    private float playStartRange = .2f;
    public DIRECTION direction;

    public AudioClip enterSFX;
    public AudioClip exitSFX;
    public AudioClip[] tunnelHitSFX;
    void Start()
    {
        exitPos = exit.transform.GetChild(0).transform.position;
        travelSpeed = Vector2.Distance(exitPos, transform.position) / travelTime;
        ball = FindObjectOfType<Ball>();
        travelSpeed = travelSpeed + Vector2.Distance(gameObject.transform.position, exitPos) / 4000;
        rand = Random.Range(playStartRange, playEndRange);
    }

    void Update()
    {
        if (isTraveling)
        {
            timer += Time.deltaTime;

            if (timer > rand)
            {
                SoundFXManager.instance.PlayRandomSoundFXClip(tunnelHitSFX, transform, ball.maxSFXVolume - .35f);
                rand = Random.Range(playStartRange, playEndRange);
                timer = 0f;
            }

            if (ball.transform.localScale.x <= 0) {
                ball.transform.position = Vector2.MoveTowards(ball.transform.position, exitPos, travelSpeed * Time.deltaTime);
                ball.gameObject.SetActive(false);
            }
            else
            {
                ball.transform.position = Vector2.MoveTowards(ball.transform.position, (Vector2)transform.position - new Vector2(0, .1f), fallSpeed * Time.deltaTime);
                ball.transform.root.localScale += scaleChange / fallTime * Time.deltaTime;
            }
            if (ball.transform.position.x == exitPos.x && ball.transform.position.y == exitPos.y)
            {
                Exit();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (exitPos == null) {
            print("Exit not set");
            return;
        }

        if (collision.gameObject.tag != "Ball" || collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > ballOverHoleSpeed)
        {
            return;
        }
        if (!played)
        {
            SoundFXManager.instance.PlaySoundFXClip(enterSFX, transform, ball.maxSFXVolume + .1f);
            played = true;
        }
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        fallSpeed = Vector2.Distance(ball.transform.position, transform.position - new Vector3(0, .1f)) / fallTime;
        isTraveling = true;

    }

    private void Exit()
    {
        ball.gameObject.SetActive(true);
        ball.transform.root.localScale = origScale;
        switch(direction)
        {
            case DIRECTION.UP:
                ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, exitSpeed);
                break;
            case DIRECTION.DOWN:
                ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -exitSpeed);
                break;
            case DIRECTION.LEFT:
                ball.GetComponent<Rigidbody2D>().velocity = new Vector2(-exitSpeed, 0);
                break;
            case DIRECTION.RIGHT:
                ball.GetComponent<Rigidbody2D>().velocity = new Vector2(exitSpeed, 0);
                break;
        }
        SoundFXManager.instance.PlaySoundFXClip(exitSFX, transform, ball.maxSFXVolume + .1f);
        played = false;
        isTraveling = false;
    }


}
