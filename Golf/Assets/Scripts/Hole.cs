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
    public float timeToBeat = 5f;
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
    public int holeNum;
    private int runFinalHole = 2;
    public UnityEngine.UI.Button nextLevelButton;

    [SerializeField] private TextMeshProUGUI winTxt;
    [SerializeField] private TextMeshProUGUI holeOnWinScreenTxt;
    [SerializeField] private TextMeshProUGUI parOnWinScreenTxt;
    [SerializeField] private TextMeshProUGUI strokesTxt;
    [SerializeField] private TextMeshPro signTxt;
    [SerializeField] private TextMeshPro signLevelTxt;
    [SerializeField] private TextMeshProUGUI upgradeAvailableTxt;

    [Header("Achievement Objects")]
    [SerializeField] private GameObject appleAchievement;
    [SerializeField] private Button hole9Button;

    private CursorController cursor;
    public TextMeshProUGUI timeToBeatTxt;
    private int[] costs = { 2, 5, 8, 12 };
    private void Awake()
    {
        ball = GameObject.FindObjectOfType<Ball>();
        inv = ball.GetComponent<Inventory>();
        camController = FindObjectOfType<CameraController>();

        if (animator != null)
        {
            cursor = animator.GetComponentInChildren<CursorController>();
            cursor.gameObject.SetActive(false);
        }

        GameObject timeObject = GameObject.Find("Time To Beat");

        if (timeObject != null)
        {
            timeToBeatTxt = timeObject.GetComponent<TextMeshProUGUI>();
        }

        holeNum = int.Parse(getCourseNumber());
        currentLevel = holeNum;

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
        Inventory inv = ball.GetComponent<Inventory>();
        if (inv.isFreeplayMode)
        {
            inv.levelsCompleted[holeNum] = true;
        }
        
        if (appleAchievement != null)
        {
            Achievement.Give(Achievement.TYPE.APPLE_TEE);
        }

        if (hole9Button != null && !hole9Button.isPressed)
        {
            Achievement.Give(Achievement.TYPE.HOLE9_GUESS);
        }

        if (ball.strokes <= par)
        {
            foreach (int coin in inv.tempCollectedCoins)
            {
                if (!inv.coinsCollected[currentLevel].Contains(coin)) {
                    inv.coins++;
                    inv.totalCoins++;
                    inv.coinsCollected[holeNum].Add(coin);
                }
                
            }
        }

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
        else if (inv.isCampSpeedMode && !inv.isWalkMode)
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
        else if (inv.isClassicSpeedMode && !inv.isWalkMode)
        {
            inv.classicSpeedCurrScore[holeNum] = inv.timer;
            if (holeNum == runFinalHole)
            {
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
        else if (inv.isCampHardMode)
        {
            inv.campHardCurrScore[holeNum] = ball.strokes;
            if (holeNum == runFinalHole)
            {
                if (inv.campHardHighScore != null && inv.campHardHighScore.Count == runFinalHole)
                {
                    int highscore = 0;
                    int currScore = 0;

                    foreach (var kvp in inv.campHardHighScore)
                    {
                        int score = kvp.Value;
                        int level = kvp.Key;
                        highscore += score;
                    }
                    foreach (var kvp in inv.campHardCurrScore)
                    {
                        int score = kvp.Value;
                        int level = kvp.Key;
                        currScore += score;
                    }
                    if (currScore <= highscore)
                    {

                        inv.campHardHighScore = inv.campHardCurrScore;
                    }

                }
                else
                {
                    inv.campHardHighScore = inv.campHardCurrScore;
                }


            }
        }
        else if (inv.isClassicHardMode)
        {
            inv.classicHardCurrScore[holeNum] = ball.strokes;
            if (holeNum == runFinalHole)
            {
                if (inv.classicHardHighScore != null && inv.classicHardHighScore.Count == runFinalHole)
                {
                    int highscore = 0;
                    int currScore = 0;

                    foreach (var kvp in inv.classicHardHighScore)
                    {
                        int score = kvp.Value;
                        int level = kvp.Key;
                        highscore += score;
                    }
                    foreach (var kvp in inv.classicHardCurrScore)
                    {
                        int score = kvp.Value;
                        int level = kvp.Key;
                        currScore += score;
                    }
                    if (currScore <= highscore)
                    {

                        inv.classicHardHighScore = inv.classicHardCurrScore;
                    }

                }
                else
                {
                    inv.classicHardHighScore = inv.classicHardCurrScore;
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
            isPlayingVoiceLine = true;

            if (ball.strokes <= par)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.inHoleSound, transform.position);
            }

            if (ball.strokes == 0 && !inv.isWalkMode)
            {
                Achievement.Give(Achievement.TYPE.HOLEINNONE);

            }

            if (ball.strokes == 1)
            {
                Achievement.Give(Achievement.TYPE.HOLEINONE);
            }

            if (ball.strokes > par && inv.tempCollectedCoins.Count == 0)
            {
                Achievement.Give(Achievement.TYPE.JORDAN);
            }

            inv.SavePlayer();

            if (!inv.isFreeplayMode)
            {
                UnlockNewLevel();
            }

            if (ball.strokes <= par)
            {
                if (inv.isFreeplayMode)
                {
                    UnlockNewLevel();
                }

                if (inv.coinsCollected.ContainsKey(currentLevel) && inv.coinsCollected[currentLevel].Contains(1) && inv.coinsCollected[currentLevel].Contains(2) && inv.coinsCollected[currentLevel].Contains(3))
                {  
                    int level = holeNum;
                    
                    ball.GetComponent<Inventory>().unlockedHats[(Hat.TYPE)level] = true;
                    inv.SavePlayer();
                }
            }
            camController.isWinScreen = true;
            camController.cam.Follow = null;
            camController.cam.m_Lens.OrthographicSize = camController.mapViewSize;
            camController.cam.transform.position = camController.mapViewPos.position;

            
            if (((holeNum == 18 && (inv.isCampSpeedMode || inv.isClassicSpeedMode)) && inv.timer < timeToBeat) || (!inv.isCampSpeedMode && !inv.isClassicSpeedMode && holeNum == 18))
            {
                animator.SetBool("RunFinished", true);
            }
            else if (inv.isFreeplayMode)
            {
                animator.SetBool("Won", true);
            }
            else if(inv.isCampSpeedMode || inv.isClassicSpeedMode)
            {
                animator.SetBool("SpeedrunWon", true);
            }
            else
            {
                animator.SetBool("RunWon", true);
            }

            Ability ability = inv.getCurrentAbility();
            if (ability != null)
            {
                ability.onBallDisabled(ball);
            }
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

                if (level < costs.Length && inv.coins >= costs[level])
                {
                    upgradeAvailableTxt.enabled = true;
                }
                if (upgradeAvailableTxt.enabled)
                {
                    break;
                }

            }

            cursor.gameObject.SetActive(true);


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

                if (!inv.coinsCollected[currentLevel].Contains(3) && (inv.isCampaignMode || inv.isFreeplayMode || inv.isCampHardMode))
                {
                    inv.coins += 1;
                    inv.totalCoins += 1;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.coinCollect, transform.position);
                }
                if (inv.isCampaignMode || inv.isFreeplayMode || inv.isCampHardMode)
                {
                    inv.coinsCollected[currentLevel].Add(3);
                }
                

            }

            if (inv.isFreeplayMode && ball.strokes <= par)
            {
                if (holeNum == 18 && SceneManager.GetActiveScene().name == "Level 18 Official")
                {
                    Achievement.Give(Achievement.TYPE.BEAT_CAMP_FREEPLAY);
                }
                else if (holeNum == 18 && SceneManager.GetActiveScene().name == "Classic 18")
                {
                    Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_FREEPLAY);
                }
                winTxt.fontSize = 90;
                winTxt.text = "YOU WIN!";
            }
            else if (inv.isFreeplayMode && ball.GetComponent<Inventory>().levelsCompleted.ContainsKey(holeNum) && ball.GetComponent<Inventory>().levelsCompleted[holeNum])
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "You Lose! Too Many Strokes!";
            }
            else if (inv.isCampaignMode)
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Campaign 18 Holes";

                if (holeNum == 18)
                {
                    Achievement.Give(Achievement.TYPE.BEAT_CAMP_18);
                    winTxt.text = "You finished Campaign 18 Holes!";
                }
            }
            else if (inv.isCampSpeedMode)
            {
                nextLevelButton.interactable = false;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Too Slow!";
                if (inv.timer < timeToBeat)
                {
                    winTxt.text = "Campaign Speedrun";
                    nextLevelButton.interactable = true;
                    inv.campSpeedGoalsBeat[holeNum] = true;
                }

                if (holeNum == 18 && inv.timer < timeToBeat)
                {
                    if (inv.isWalkMode)
                    {
                        Achievement.Give(Achievement.TYPE.BEAT_CAMP_CLUBLESS);
                    }
                    else
                    {
                        Achievement.Give(Achievement.TYPE.BEAT_CAMP_SPEEDRUN);
                    }
                    
                    winTxt.text = "You finished Campaign Speedrun!";
                }

            }
            else if (inv.isCampHardMode)
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Campaign Hardcore";

                if (ball.strokes > par)
                {
                    winTxt.text = "Too Many Strokes!";
                    nextLevelButton.interactable = false;

                }
                else 
                {
                    if (holeNum == 18)
                    {
                        Achievement.Give(Achievement.TYPE.BEAT_CAMP_HARDCORE);
                        winTxt.text = "You finished Campaign Hardcore!";
                    }
                    
                }
                

            }
            else if (inv.isClassicMode)
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Classic 18 Holes";
                print("Hello");
                if (holeNum == 18)
                {
                    Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_HARDCORE);
                    winTxt.text = "You finished Classic 18 Holes!";
                }

            }
            else if (inv.isClassicSpeedMode)
            {
                nextLevelButton.interactable = false;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Too Slow!";

                print(inv.timer);
                print(timeToBeat);
                if (inv.timer < timeToBeat)
                {
                    winTxt.text = "Classic Speedrun";
                    nextLevelButton.interactable = true;
                    inv.campSpeedGoalsBeat[holeNum] = true;
                }

                if (holeNum == 18 && inv.timer < timeToBeat)
                {
                    if (inv.isWalkMode)
                    {
                        Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_CLUBLESS);
                    }
                    else
                    {
                        Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_SPEEDRUN);
                    }
                        winTxt.text = "You finished Classic Speedrun!";
                }

            }
            else if (inv.isClassicHardMode)
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Classic Hardcore";

                if (ball.strokes > par)
                {
                    winTxt.text = "Too Many Strokes!";
                    nextLevelButton.interactable = false;

                }
                else
                {
                    if (holeNum == 18)
                    {
                        Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_HARDCORE);
                        winTxt.text = "You finished Classic Hardcore!";
                    }
                }
                

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
        else if (!inv.achievements[(int)Achievement.TYPE.SLOW_THERE_BUDDY] && collision.gameObject.tag == "Ball" && collision.gameObject.GetComponent<Ball>() is Ball && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude >= ballOverHoleSpeed)
        {
            Achievement.Give(Achievement.TYPE.SLOW_THERE_BUDDY);
            inv.SavePlayer();
        }
        
    }

    private void PlayVoiceLine()
    {
        if (ball.strokes == 1)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.parfect, transform.position);
            return;
        }
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
        else
        {
            if (ball.strokes > par)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.parthetic, transform.position);
            }
            else
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.excellent, transform.position);
            }
            
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
