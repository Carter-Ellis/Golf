using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class Hole : MonoBehaviour, ButtonTarget
{
    Ball ball;
    Inventory inv;
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
    private int currentLevel;
    private int holeNum;
    private int runFinalHole = 2;

    public UnityEngine.UI.Button nextLevelButton;

    [SerializeField] private TextMeshProUGUI winTxt;
    [SerializeField] private TextMeshProUGUI holeOnWinScreenTxt;
    [SerializeField] private TextMeshProUGUI parOnWinScreenTxt;
    [SerializeField] private TextMeshProUGUI strokesTxt;
    [SerializeField] private TextMeshPro signTxt;
    [SerializeField] private TextMeshPro signLevelTxt;
    [SerializeField] private TextMeshProUGUI upgradeAvailableTxt;
    private int[] costs = { 2, 5, 8, 12 };
    private void Awake()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        inv = ball.GetComponent<Inventory>();
        camController = FindObjectOfType<CameraController>();

        holeNum = int.Parse(getCourseNumber());

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (signTxt != null)
        {
            signTxt.text = "Par " + par;
            signLevelTxt.text = "Hole " + holeNum;
        }
        
    }

    private string getCourseNumber()
    {

        string name = SceneManager.GetActiveScene().name;
        int start = -1;
        int end = name.Length;
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            bool isNum = (c >= '0' && c <= '9');
            if (start == -1)
            {
                if (isNum)
                {
                    start = i;
                }
            }
            else
            {
                if (!isNum)
                {
                    end = i;
                    break;
                }
            }
        }
        if (start < 0 || end - start < 0)
        {
            return "0";
        }
        return name.Substring(start, end - start);

    }

    private void UnlockNewLevel()
    {
        if (holeNum >= PlayerPrefs.GetInt("ReachedIndex")) {
            PlayerPrefs.SetInt("ReachedIndex", holeNum + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            
        }
        Inventory inv = ball.GetComponent<Inventory>();
        inv.levelsCompleted[holeNum] = true;
        if (inv.isCampaignMode)
        {
            inv.campaignCurrScore[holeNum] = ball.strokes;
            if (holeNum == runFinalHole)
            {
                if (inv.campaignHighScore != null && inv.campaignHighScore.Count == runFinalHole)
                {
                    int highscore = 0;
                    int currScore = 0;
                    
                    foreach (var kvp in inv.campaignHighScore)
                    {
                        int score = kvp.Value;
                        int level = kvp.Key;
                        highscore += score;
                    }
                    foreach (var kvp in inv.campaignCurrScore)
                    {
                        int score = kvp.Value;
                        int level = kvp.Key;
                        currScore += score;
                    }
                    if (currScore <= highscore)
                    {
                        
                        inv.campaignHighScore = inv.campaignCurrScore;
                    }

                }
                else
                {
                    inv.campaignHighScore = inv.campaignCurrScore;
                }


            }
        }
        else if (inv.isClassicMode)
        {
            inv.classicCurrScore[holeNum] = ball.strokes;
            
            if (holeNum == runFinalHole)
            {
                if (inv.classicHighScore != null && inv.classicHighScore.Count == runFinalHole)
                {
                    int highscore = 0;
                    int currScore = 0;
                    foreach (var kvp in inv.classicHighScore)
                    {
                        int score = kvp.Value;
                        int level = kvp.Key;
                        highscore += score;
                    }
                    foreach (var kvp in inv.classicCurrScore)
                    {
                        int score = kvp.Value;
                        int level = kvp.Key;
                        currScore += score;
                    }
                    if (currScore <= highscore)
                    {
                        inv.classicHighScore = inv.classicCurrScore;
                    }
                }
                else
                {
                    inv.classicHighScore = inv.classicCurrScore;
                }
            } 
        }
        else if (inv.isCampSpeedMode)
        {
            inv.campSpeedCurrScore[holeNum] = inv.timer;
            if (holeNum == runFinalHole)
            {
                if (inv.campSpeedHighScore != null && inv.campSpeedHighScore.Count == runFinalHole)
                {
                    float highscore = 0;
                    float currScore = 0;

                    foreach (var kvp in inv.campSpeedHighScore)
                    {
                        float score = kvp.Value;
                        int level = kvp.Key;
                        highscore += score;
                    }
                    foreach (var kvp in inv.campSpeedCurrScore)
                    {
                        float score = kvp.Value;
                        int level = kvp.Key;
                        currScore += score;
                    }

                    if (currScore <= highscore)
                    {
                        inv.campSpeedHighScore = inv.campSpeedCurrScore;
                    }

                }
                else
                {
                    inv.campSpeedHighScore = inv.campSpeedCurrScore;
                }
            }

        }
        else if (inv.isClassicSpeedMode)
        {
            inv.classicSpeedCurrScore[holeNum] = inv.timer;
            if (holeNum == runFinalHole)
            {
                print("classic speed saved;");
                if (inv.classicSpeedHighScore != null && inv.classicSpeedHighScore.Count == runFinalHole)
                {
                    float highscore = 0;
                    float currScore = 0;

                    foreach (var kvp in inv.classicSpeedHighScore)
                    {
                        float score = kvp.Value;
                        int level = kvp.Key;
                        highscore += score;
                    }
                    foreach (var kvp in inv.classicSpeedCurrScore)
                    {
                        float score = kvp.Value;
                        int level = kvp.Key;
                        currScore += score;
                    }

                    if (currScore <= highscore)
                    {
                        inv.classicSpeedHighScore = inv.classicSpeedCurrScore;
                    }

                }
                else
                {
                    inv.classicSpeedHighScore = inv.classicSpeedCurrScore;
                }
            }

        }
        inv.SavePlayer();

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
            
            if (inv.isCampaignMode)
            {
                UnlockNewLevel();
            }

            if (ball.strokes <= par)
            {
                if (inv.coinsCollected.ContainsKey(currentLevel) && inv.coinsCollected[currentLevel].Contains(1) && inv.coinsCollected[currentLevel].Contains(2) && inv.coinsCollected[currentLevel].Contains(3))
                {
                    int level = holeNum;
                    ball.GetComponent<Inventory>().unlockedHats[(Hat.TYPE)level] = true;
                }
                if (!inv.isCampaignMode)
                {
                    UnlockNewLevel();
                }
                
            }
            camController.isWinScreen = true;
            camController.cam.Follow = null;
            camController.cam.m_Lens.OrthographicSize = camController.mapViewSize;
            camController.cam.transform.position = camController.mapViewPos.position;
            animator.SetBool("Won", true);
            ball.gameObject.SetActive(false);
            inHole = false;
            for (int i = 0; i < 3; i++)
            {
                upgradeAvailableTxt.enabled = false;
                int level = 0;
                if (inv.upgradeLevels.ContainsKey(i))
                {
                    level = inv.upgradeLevels[i];
                }

                if (inv.coins >= costs[level])
                {
                    upgradeAvailableTxt.enabled = true;
                }
                if (upgradeAvailableTxt.enabled)
                {
                    break;
                }

            }

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
                currentLevel = holeNum;
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
                    inv.totalCoins += 1;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.coinCollect, transform.position);
                }
                inv.coinsCollected[currentLevel].Add(3);
                
                
            }

            if (ball.strokes <= par)
            {
                winTxt.fontSize = 90;
                winTxt.text = "YOU WIN!";
            }
            else if (ball.GetComponent<Inventory>().levelsCompleted.ContainsKey(holeNum) && ball.GetComponent<Inventory>().levelsCompleted[holeNum])
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "You Lose! Too Many Strokes!";
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
                parOnWinScreenTxt.text = "Par " + par;
            }

            holeOnWinScreenTxt.text = "Hole " + holeNum;

            strokesTxt.text = "Strokes " + ball.strokes;
            

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

    public void onPress()
    {
        this.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
}
