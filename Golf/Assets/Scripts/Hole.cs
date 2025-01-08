using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    Ball ball;
    CameraController camController;
    public int par = 4;
    public float ballOverHoleSpeed = 10f;
    public bool inHole;
    private float fallTime = .2f;
    private float fallSpeed;
    private Vector3 scaleChange = new Vector3(-1f, -1f, 0f);
    private float voiceLineDelayTimer = 0f;
    private float voiceLineDelay = .55f;
    private bool voicePlayed = false;
    private float applauseDelayTimer = 0f;
    private float applauseDelay = .7f;
    private bool isPlayingVoiceLine;
    public AudioClip inHoleSFX;
    public Animator animator;

    public UnityEngine.UI.Button nextLevelButton;

    [SerializeField] private TextMeshProUGUI winTxt;
    [SerializeField] private TextMeshProUGUI parOnWinScreenTxt;
    [SerializeField] private TextMeshProUGUI parUITxt;
    [SerializeField] private TextMeshProUGUI strokesTxt;
    [SerializeField] private TextMeshPro signTxt;
    [SerializeField] private TextMeshPro signLevelTxt;
    private void Awake()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        camController = FindObjectOfType<CameraController>();
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (signTxt != null)
        {
            signTxt.text = "Par " + par;
            parUITxt.text = "Par " + par;
            signLevelTxt.text = "Hole " + SceneManager.GetActiveScene().buildIndex;
        }
        else if (parUITxt != null)
        {
            signLevelTxt.text = "Hole " + SceneManager.GetActiveScene().buildIndex;
        }
        
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
        
        if (isPlayingVoiceLine)
        {
            voiceLineDelayTimer += Time.deltaTime;
            if (voiceLineDelayTimer >= voiceLineDelay) 
            {
                if (!voicePlayed)
                {
                    PlayVoiceLine();
                    voicePlayed = true;
                } 
                applauseDelayTimer += Time.deltaTime;
                if (applauseDelayTimer >= applauseDelay)
                {
                    PlayApplause();
                    applauseDelayTimer = 0;
                    voiceLineDelayTimer = 0f;
                    isPlayingVoiceLine = false;
                    voicePlayed = false;
                }
            }
        }
        if (!inHole)
        {
            return;
        }
        if (ball.transform.localScale.x <= 0)
        {
            // Play inhole audio
            AudioManager.instance.PlayOneShot(FMODEvents.instance.inHoleSound, transform.position);
            isPlayingVoiceLine = true;
            
            if (ball.strokes <= par)
            {
                UnlockNewLevel();
            }
            camController.isWinScreen = true;
            camController.cam.Follow = null;
            camController.cam.m_Lens.OrthographicSize = camController.mapViewSize;
            camController.cam.transform.position = camController.mapViewPos.position;
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
            if (ball.strokes < par)
            {
                int currentLevel = SceneManager.GetActiveScene().buildIndex;
                Inventory inv = ball.GetComponent<Inventory>();
                if (inv.coinsCollected == null)
                {
                    inv.coinsCollected = new Dictionary<int, List<int>>();
                }

                if (!inv.coinsCollected.ContainsKey(currentLevel))
                {
                    inv.coinsCollected[currentLevel] = new List<int>();
                }
                if (!inv.coinsCollected[currentLevel].Contains(3))
                {
                    inv.coins += 1;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.coinCollect, transform.position);
                }
                inv.coinsCollected[currentLevel].Add(3);
                
                
            }

            if (ball.strokes <= par)
            {
                winTxt.fontSize = 80;
                winTxt.text = "YOU WIN!";
            }
            else
            {
                nextLevelButton.interactable = false;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = false;
                winTxt.fontSize = 50;
                winTxt.text = "You Lose! Too Many Strokes!";
            }

            if (parOnWinScreenTxt != null)
            {
                parOnWinScreenTxt.text = "Par: " + par;
            }
            
            strokesTxt.text = "Strokes: " + ball.strokes;
            

            inHole = true;
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            fallSpeed = Vector2.Distance(ball.transform.position, transform.position - new Vector3(0, .1f)) / fallTime;
        }
        
    }

    private void PlayVoiceLine()
    {
        if (ball.strokes == par + 1)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bogey, transform.position);
        }
        else if (ball.strokes == par + 2)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.doubleBogey, transform.position);
        }
        else if (ball.strokes == par)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.par, transform.position);
        }
        else if (ball.strokes == par - 1)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.birdie, transform.position);
        }
        else if (ball.strokes == par - 2)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.eagle, transform.position);
            
        }
    }


    private void PlayApplause()
    {
        if (ball.strokes <= par)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.applause, transform.position);
        }
    }

}
