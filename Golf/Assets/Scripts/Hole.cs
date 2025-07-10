using FMOD;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour, ButtonTarget
{
    Ball ball;
    Inventory inv;
    CameraController camController;
    GhostRecorder ghostRecorder;
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
    private Map.TYPE currentMap;
    private GameMode.TYPE currentMode;
    public UnityEngine.UI.Button nextLevelButton;

    public TextMeshProUGUI winTxt;
    public TextMeshProUGUI holeOnWinScreenTxt;
    public TextMeshProUGUI parOnWinScreenTxt;
    public TextMeshProUGUI strokesTxt;
    [SerializeField] private TextMeshPro signTxt;
    [SerializeField] private TextMeshPro signLevelTxt;
    [SerializeField] private TextMeshProUGUI upgradeAvailableTxt;
    private int winSayingIndex = 0;
    private string[] winSayings =
    {
        "YOU WIN!",
        "EXCELLENT!",
        "GREAT JOB!",
        "WELL DONE!",
        "NICE WORK!",
        "BRAVO!",
        "TOO EASY!",
        "IMPRESSIVE!",
        "OUTSTANDING!",
    };

    [Header("Achievement Objects")]
    [SerializeField] private GameObject appleAchievement;
    [SerializeField] private Button hole9Button;

    private CursorController cursor;
    public TextMeshProUGUI timeToBeatTxt;
    private int[] costs = { 2, 5, 8, 12 };
    private void Awake()
    {
        currentMap = Map.current;
        currentMode = GameMode.current;
        ball = GameObject.FindObjectOfType<Ball>();
        ghostRecorder = ball.GetComponent<GhostRecorder>();
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

        holeNum = Map.hole;
        currentLevel = holeNum;

        if (signTxt != null)
        {
            signTxt.text = "Par " + par;
            signLevelTxt.text = "Hole " + holeNum;
        }

        winSayingIndex = UnityEngine.Random.Range(0, winSayings.Length);

    }

    private void UnlockNewLevel()
    {
        Inventory inv = ball.GetComponent<Inventory>();
        GameMode.TYPE currentMode = GameMode.current;
        if (currentMode == GameMode.TYPE.FREEPLAY)
        {
            inv.levelsCompleted[holeNum] = true;
        }

        bool shouldUnlock = true;
        if (GameMode.isAnySpeedrun() && ghostRecorder.currFrames != null)
        {
            float time = ghostRecorder.currFrames[ghostRecorder.currFrames.Count - 1].GetTime();
            if (time > timeToBeat)
            {
                shouldUnlock = false;
            }
        }
        if (shouldUnlock)
        {
            Map.get(currentMap).setLevelUnlocked(currentMode, holeNum + 1);
        }
        
        if (appleAchievement != null)
        {
            Achievement.Give(Achievement.TYPE.APPLE_TEE);
        }

        if (hole9Button != null && !hole9Button.isPressed)
        {
            Achievement.Give(Achievement.TYPE.HOLE9_GUESS);
        }

        if (ball.strokes <= par && currentMap != Map.TYPE.CLASSIC)
        {
            MapData mapData = Map.get(currentMap);
            foreach (int coin in inv.tempCollectedCoins)
            {
                if (!mapData.isCoinCollected(currentLevel, coin)) 
                {
                    inv.coins++;
                    inv.totalCoins++;
                    mapData.setCoinCollected(currentLevel, coin);
                }
                
            }
        }

        if ((currentMap == Map.TYPE.CAMPAIGN) && (currentMode == GameMode.TYPE.HOLE18))
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
        else if ((currentMap == Map.TYPE.CLASSIC) && (currentMode == GameMode.TYPE.HOLE18))
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
        else if ((currentMap == Map.TYPE.CAMPAIGN) && (currentMode == GameMode.TYPE.SPEEDRUN))
        {
            List<GhostFrame> ghostFrames = ghostRecorder.currFrames;
            if (inv.campSpeedHighScore != null && inv.campSpeedHighScore.ContainsKey(holeNum))
            {
                if (ghostFrames != null && ghostFrames[ghostFrames.Count - 1].GetTime() < inv.campSpeedHighScore[holeNum])
                {
                    inv.campSpeedCurrScore[holeNum] = ghostFrames[ghostFrames.Count - 1].GetTime();
                    inv.campSpeedHighScore = inv.campSpeedCurrScore;
                }
            }
            else
            {
                inv.campSpeedCurrScore[holeNum] = ghostFrames[ghostFrames.Count - 1].GetTime();
                inv.campSpeedHighScore = inv.campSpeedCurrScore;
            }

        }
        else if ((currentMap == Map.TYPE.CLASSIC) && (currentMode == GameMode.TYPE.SPEEDRUN))
        {
            List<GhostFrame> ghostFrames = ghostRecorder.currFrames;
            if (inv.classicSpeedHighScore != null && inv.classicSpeedHighScore.ContainsKey(holeNum))
            {
                if (ghostFrames != null && ghostFrames[ghostFrames.Count - 1].GetTime() < inv.classicSpeedHighScore[holeNum])
                {
                    inv.classicSpeedCurrScore[holeNum] = ghostFrames[ghostFrames.Count - 1].GetTime();
                    inv.classicSpeedHighScore = inv.classicSpeedCurrScore;
                }
            }
            else
            {
                inv.classicSpeedCurrScore[holeNum] = ghostFrames[ghostFrames.Count - 1].GetTime();
                inv.classicSpeedHighScore = inv.classicSpeedCurrScore;
            }

        }
        else if ((currentMap == Map.TYPE.CAMPAIGN) && (currentMode == GameMode.TYPE.HARDCORE))
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
        else if ((currentMap == Map.TYPE.CLASSIC) && (currentMode == GameMode.TYPE.HARDCORE))
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

            ball.cursor.SetActive(false);
            ball.swingPowerSlider.gameObject.SetActive(false);
            ball.powerTxt.gameObject.SetActive(false);
            ball.cancelImage.SetActive(false);


            // Play inhole audio
            isPlayingVoiceLine = true;

            if (ball.strokes <= par)
            {
                Audio.playSFX(FMODEvents.instance.inHoleSound, transform.position);
            }
            else
            {
                Audio.playSFX(FMODEvents.instance.inHoleBad, transform.position);
            }

            if (ball.strokes == 0 && (GameMode.current != GameMode.TYPE.CLUBLESS))
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

            if (GameMode.current != GameMode.TYPE.FREEPLAY)
            {
                UnlockNewLevel();
            }

            if (ball.strokes <= par)
            {
                if (GameMode.current == GameMode.TYPE.FREEPLAY)
                {
                    UnlockNewLevel();
                }

                MapData mapData = Map.get(currentMap);
                if (mapData.type == Map.TYPE.CAMPAIGN && mapData.isCoinCollected(currentLevel, 1)
                    && mapData.isCoinCollected(currentLevel, 2)
                    && mapData.isCoinCollected(currentLevel, 3))
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

            bool isSpeedrunning = GameMode.isAnySpeedrun();
            if (holeNum == 18 && ((isSpeedrunning && inv.timer < timeToBeat) || (!isSpeedrunning && currentMode != GameMode.TYPE.FREEPLAY)))
            {
                animator.SetBool("RunFinished", true);
            }
            else if (GameMode.current == GameMode.TYPE.FREEPLAY)
            {
                if (holeNum == 18)
                {
                    animator.SetBool("18HoleWin", true);
                }
                else if (Map.current == Map.TYPE.CLASSIC)
                {          
                        animator.SetBool("SpeedrunWon", true);
                }
                else
                {
                        animator.SetBool("Won", true);
                }
            }
            else if (isSpeedrunning)
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
            
            if (ball.strokes < par && currentMap != Map.TYPE.CLASSIC)
            {
                currentLevel = holeNum;
                Inventory inv = ball.GetComponent<Inventory>();
                MapData mapData = Map.get(currentMap);
                if (!mapData.isCoinCollected(currentLevel, 3))
                {
                    inv.coins += 1;
                    inv.totalCoins += 1;
                    mapData.setCoinCollected(currentLevel, 3);
                    Audio.playSFX(FMODEvents.instance.coinCollect, transform.position);
                }

            }

            // Save ghost frames
            if (GameMode.isAnySpeedrun() && !inHole)
            {
                inHole = true;
                
                List<GhostFrame> ghostFrames = Map.get(currentMap).getGhostFrames(currentMode, holeNum);
                if (ghostFrames != null)
                {
                    if (ghostFrames.Count > 0) 
                    {
                        float totalTime = ghostFrames[ghostFrames.Count - 1].GetTime();

                        if (totalTime > ghostRecorder.timeElapsed)
                        {
                            ghostRecorder.recordFrame();
                            Map.get(currentMap).setGhostFrames(new List<GhostFrame>(ghostRecorder.currFrames), currentMode, holeNum);
                        }
                    }
                }
                else
                {
                    ghostRecorder.recordFrame();
                    Map.get(currentMap).setGhostFrames(new List<GhostFrame>(ghostRecorder.currFrames), currentMode, holeNum);
                }
                ghostRecorder.isRecording = false;

            }

            if ((GameMode.current == GameMode.TYPE.FREEPLAY) && ball.strokes <= par)
            {
                if (holeNum == 18 && SceneManager.GetActiveScene().name == "Campaign 18")
                {
                    Achievement.Give(Achievement.TYPE.BEAT_CAMP_FREEPLAY);
                }
                else if (holeNum == 18 && SceneManager.GetActiveScene().name == "Classic 18")
                {
                    Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_FREEPLAY);
                }
                else if (holeNum == 18 && SceneManager.GetActiveScene().name == "Beach 18")
                {
                    //Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_FREEPLAY);
                }
                winTxt.fontSize = 75;
                winTxt.text = winSayings[winSayingIndex];
            }
            else if (currentMap == Map.TYPE.CAMPAIGN && GameMode.current == GameMode.TYPE.HOLE18)
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
            else if (currentMap == Map.TYPE.CAMPAIGN && GameMode.isAnySpeedrun())
            {
                nextLevelButton.interactable = Map.get(currentMap).isLevelUnlocked(currentMode, holeNum + 1);
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Too Slow!";
                List<GhostFrame> currFrames = ghostRecorder.currFrames;
                float time = currFrames[currFrames.Count - 1].GetTime();

                if (time < timeToBeat)
                {
                    winTxt.text = "Campaign Speedrun";
                    nextLevelButton.interactable = true;
                    inv.campSpeedGoalsBeat[holeNum] = true;

                    if (holeNum == 18)
                    {
                        if (GameMode.current == GameMode.TYPE.CLUBLESS)
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

            }
            else if ((currentMap == Map.TYPE.CAMPAIGN) && (currentMode == GameMode.TYPE.HARDCORE))
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
            else if (currentMap == Map.TYPE.CLASSIC && currentMode == GameMode.TYPE.HOLE18)
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Classic 18 Holes";
                if (holeNum == 18)
                {
                    Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_HARDCORE);
                    winTxt.text = "You finished Classic 18 Holes!";
                }

            }
            else if (currentMap == Map.TYPE.CLASSIC && GameMode.isAnySpeedrun())
            {
                nextLevelButton.interactable = Map.get(currentMap).isLevelUnlocked(currentMode, holeNum + 1);
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Too Slow!";

                List<GhostFrame> currFrames = ghostRecorder.currFrames;
                float time = currFrames[currFrames.Count - 1].GetTime();
                if (time < timeToBeat)
                {
                    winTxt.text = "Classic Speedrun";
                    nextLevelButton.interactable = true;
                    inv.campSpeedGoalsBeat[holeNum] = true;

                    if (holeNum == 18)
                    {
                        if (GameMode.current == GameMode.TYPE.CLUBLESS)
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

            }
            else if (currentMap == Map.TYPE.CLASSIC && currentMode == GameMode.TYPE.HARDCORE)
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
            else if (currentMap == Map.TYPE.BEACH && GameMode.current == GameMode.TYPE.HOLE18)
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Beach 18 Holes";

                if (holeNum == 18)
                {
                    //Achievement.Give(Achievement.TYPE.BEAT_CAMP_18);
                    winTxt.text = "You finished Beach 18 Holes!";
                }
            }
            else if (currentMap == Map.TYPE.BEACH && GameMode.isAnySpeedrun())
            {
                nextLevelButton.interactable = Map.get(currentMap).isLevelUnlocked(currentMode, holeNum + 1);
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Too Slow!";

                List<GhostFrame> currFrames = ghostRecorder.currFrames;
                float time = currFrames[currFrames.Count - 1].GetTime();
                if (time < timeToBeat)
                {
                    winTxt.text = "Beach Speedrun";
                    nextLevelButton.interactable = true;
                    inv.campSpeedGoalsBeat[holeNum] = true;

                    if (holeNum == 18)
                    {
                        if (GameMode.current == GameMode.TYPE.CLUBLESS)
                        {
                            //Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_CLUBLESS);
                        }
                        else
                        {
                            // BEACH ACHIEVEMENT Achievement.Give(Achievement.TYPE.BEAT_CLASSIC_SPEEDRUN);
                        }
                        winTxt.text = "You finished Beach Speedrun!";
                    }

                }

            }
            else if ((currentMap == Map.TYPE.BEACH) && (currentMode == GameMode.TYPE.HARDCORE))
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "Beach Hardcore";

                if (ball.strokes > par)
                {
                    winTxt.text = "Too Many Strokes!";
                    nextLevelButton.interactable = false;

                }
                else
                {
                    if (holeNum == 18)
                    {
                        //Achievement.Give(Achievement.TYPE.BEAT_CAMP_HARDCORE);
                        winTxt.text = "You finished Beach Hardcore!";
                    }

                }


            }
            else if (Map.get(currentMap).isLevelUnlocked(currentMode, holeNum + 1))
            {
                nextLevelButton.interactable = true;
                nextLevelButton.GetComponent<ButtonAudio>().enabled = true;
                winTxt.fontSize = 50;
                winTxt.text = "You Lose! Too Many Strokes!";

                

               if (GameMode.isAnySpeedrun())
               {
                    List<GhostFrame> currFrames = ghostRecorder.currFrames;
                    float time = currFrames[currFrames.Count - 1].GetTime();
                    winTxt.text = "Too Slow!";
                    if (time < timeToBeat)
                    {
                        winTxt.fontSize = 75;
                        winTxt.text = winSayings[winSayingIndex];
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
        else if (collision.gameObject.tag == "Ball" && collision.gameObject.GetComponent<Ball>() is Ball && collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude >= ballOverHoleSpeed)
        {
            Audio.playSFX(FMODEvents.instance.overHole, transform.position);
            if (!inv.achievements[(int)Achievement.TYPE.SLOW_THERE_BUDDY])
            {
                Achievement.Give(Achievement.TYPE.SLOW_THERE_BUDDY);
                inv.SavePlayer();
            }
            
        }
        
    }

    private void PlayVoiceLine()
    {
        if (GameMode.isAnySpeedrun())
        {
            return;
        }

        if (ball.strokes == 1)
        {
            Audio.playSFX(FMODEvents.instance.parfect, transform.position);
            return;
        }
        if (ball.strokes == par + 1)
        {
            Audio.playSFX(FMODEvents.instance.bogey, transform.position);
        }
        else if (ball.strokes == par + 2)
        {
            Audio.playSFX(FMODEvents.instance.doubleBogey, transform.position);
        }
        else if (ball.strokes == par)
        {
            Audio.playSFX(FMODEvents.instance.par, transform.position);
        }
        else if (ball.strokes == par - 1)
        {
            Audio.playSFX(FMODEvents.instance.birdie, transform.position);
        }
        else if (ball.strokes == par - 2)
        {
            Audio.playSFX(FMODEvents.instance.eagle, transform.position);
            
        }
        else
        {
            if (ball.strokes > par)
            {
                Audio.playSFX(FMODEvents.instance.parthetic, transform.position);
            }
            else
            {
                Audio.playSFX(FMODEvents.instance.excellent, transform.position);
            }
            
        }
    }


    private void PlayApplause()
    {
        if (ball.strokes <= par)
        {
            Audio.playSFX(FMODEvents.instance.applause, transform.position);
        }
    }

    public void onPress()
    {
        this.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
}
