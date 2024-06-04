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
    private Vector3 scaleChange = new Vector3(-.01f, -.01f, 0);
    private Vector3 origScale = new Vector3(1, 1, 1);

    
    private bool isTraveling;
    private bool played;

    private float travelSpeed = .01f;
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
        ball = FindObjectOfType<Ball>();
        travelSpeed = travelSpeed + Vector3.Distance(gameObject.transform.position, exitPos) / 4000;
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
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, exitPos, travelSpeed);
                ball.gameObject.SetActive(false);
            }
            else
            {
                ball.transform.position = Vector3.MoveTowards(ball.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .1f), .01f);
                ball.transform.root.localScale += scaleChange;
            }
            if (ball.transform.position == exitPos)
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
