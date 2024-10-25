using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Hole : MonoBehaviour
{
    Ball ball;
    public int par = 4;
    public float ballOverHoleSpeed = 10f;
    public bool inHole;
    private float fallTime = .2f;
    private float fallSpeed;
    private Vector3 scaleChange = new Vector3(-1f, -1f, 0f);
    public AudioClip inHoleSFX;
    public Animator animator;
    private void Awake()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex")) {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            
        }
        ball.GetComponent<Inventory>().SavePlayer();
    }

    private void Update()
    {

        if (!inHole)
        {
            return;
        }

        if (ball.transform.localScale.x <= 0)
        {
            // Play inhole audio
            AudioManager.instance.PlayOneShot(FMODEvents.instance.inHoleSound, transform.position);
            UnlockNewLevel();
            animator.SetBool("Won", true);
            ball.gameObject.SetActive(false);
            inHole = false;
            
        }
        else
        {
            
            ball.transform.position = Vector2.MoveTowards(ball.transform.position, (Vector2)transform.position - new Vector2(0, .1f), fallSpeed * Time.deltaTime);
            ball.transform.root.localScale += scaleChange / fallTime * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" && collision.gameObject.GetComponent<Ball>() is Ball && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < ballOverHoleSpeed)
        {
            if (ball.strokes <= par)
            {
                print("On Par! Strokes: " + ball.strokes);
            }
            else if (ball.strokes > par)
            {
                print("You are so bad! Strokes: " + ball.strokes);
            }

            inHole = true;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            fallSpeed = Vector2.Distance(ball.transform.position, transform.position - new Vector3(0, .1f)) / fallTime;
        }
        
    }
}
