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
    private Vector3 iExitPos;
    private Vector3 scaleChange = new Vector3(-1f, -1f, 0);
    private Vector3 origScale = new Vector3(1, 1, 1);

    
    public bool isTraveling;
    private bool played;
    public int AscendAmount;

    private float travelTime = 2f;
    private float fallTime = .2f;
    private float fallSpeed;
    private float travelSpeed;
    public float ballOverHoleSpeed = 10f;
    public float exitSpeed = 2f;
    public float iExitSpeed = 4f;
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
        iExitPos = exit.transform.GetChild(1).transform.position;
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
                AudioManager.instance.PlayOneShot(FMODEvents.instance.tunnelBounce, transform.position);
                rand = Random.Range(playStartRange, playEndRange);
                timer = 0f;
            }

            if (ball.transform.localScale.x <= 0) {
                ball.transform.position = Vector2.MoveTowards(ball.transform.position, exitPos, travelSpeed * Time.deltaTime);
                Ability ability = ball.GetComponent<Inventory>().getCurrentAbility();
                if (ability != null)
                {
                    ability.onBallDisabled(ball);
                }
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

        if (collision.gameObject.tag == "Interactable")
        {
            GameObject iball = collision.gameObject;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.tunnelEnter, transform.position);
            IExit(iball);
            return;
        }

        if (collision.gameObject.tag != "Ball"|| collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > ballOverHoleSpeed)
        {
            return;
        }
        ball.GetComponent<Inventory>().currentHeight += AscendAmount;
        if (!played)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.tunnelEnter, transform.position);
            played = true;
        }
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        fallSpeed = Vector2.Distance(ball.transform.position, transform.position - new Vector3(0, .1f)) / fallTime;
        isTraveling = true;
        ball.isTraveling = isTraveling;

    }

    private void IExit(GameObject iball)
    {
        float randomY = Random.Range(-.5f, .5f);
        float randomX = Random.Range(-.5f, .5f);
        iball.GetComponent<Rigidbody2D>().position = new Vector2(iExitPos.x + randomX, iExitPos.y + randomY);
        float IExitspeed = iExitSpeed;
        switch (direction)
        {
            case DIRECTION.UP:
                iball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, IExitspeed);
                break;
            case DIRECTION.DOWN:
                iball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -IExitspeed);
                break;
            case DIRECTION.LEFT:
                iball.GetComponent<Rigidbody2D>().velocity = new Vector2(-IExitspeed, 0);
                break;
            case DIRECTION.RIGHT:
                iball.GetComponent<Rigidbody2D>().velocity = new Vector2(IExitspeed, 0);
                break;
        }
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.tunnelExit, transform.position);
        played = false;
        isTraveling = false;
        ball.isTraveling = isTraveling;
    }


}
