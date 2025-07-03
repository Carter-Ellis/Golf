using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public List<GhostFrame> currFrames = new List<GhostFrame>();
    public float timeElapsed = 0f;
    private float lastFrameTime = 0f;
    private float frameCaptureRate = 0.08f;
    public bool isRecordingTunnel = false;
    Animator anim;
    Hole hole;
    Inventory inv;
    bool timeMaxed = false;
    public bool isRecording
    {
        get { return _isRecording; }
        set
        {
            _isRecording = value;
            if (!_isRecording && currFrames.Count > 0)
            {
                timeElapsed = currFrames[currFrames.Count - 1].GetTime();
            }
        }
    }
    private bool _isRecording = true;

    private void Start()
    {
        anim = GameObject.Find("LevelFinishedCanvas").GetComponent<Animator>();
        hole = FindObjectOfType<Hole>();
        inv = FindObjectOfType<Inventory>();
        if (!GameMode.isAnySpeedrun())
        {
            isRecording = false;
        }
        recordFrame();
    }

    void FixedUpdate()
    {
        if (timeMaxed)
        {
            return;
        }

        if (timeElapsed > 300f)
        {
            if (GameMode.current == GameMode.TYPE.SPEEDRUN && inv.isLevelUnlocked(Map.current, GameMode.TYPE.SPEEDRUN, hole.holeNum + 1))
            {
                hole.nextLevelButton.interactable = true;
            }
            else if (GameMode.current == GameMode.TYPE.CLUBLESS && inv.isLevelUnlocked(Map.current, GameMode.TYPE.CLUBLESS, hole.holeNum + 1))
            {
                hole.nextLevelButton.interactable = true;
            }
            else
            {
                hole.nextLevelButton.interactable = false;
            }
            hole.winTxt.fontSize = 50f;
            hole.winTxt.text = "Wayyyy Too Slow!";
            hole.holeOnWinScreenTxt.text = "";
            hole.parOnWinScreenTxt.text = "";
            hole.strokesTxt.text = "";
            anim.SetBool("SpeedrunWon", true);
            
            Ball ball = FindObjectOfType<Ball>();

            if (ball != null)
            {
                ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                ball.isBallLocked = true;
                ball.isBattleMode = false;
                ball.hasClickedBall = false;
                ball.ClearDots();
                ball.swingPowerSlider?.gameObject.SetActive(false);
                ball.powerTxt?.gameObject.SetActive(false);
                ball.cancelImage?.SetActive(false);
                SpriteRenderer cursorSprite = ball.cursor?.GetComponent<SpriteRenderer>();
                if (cursorSprite != null)
                {
                    cursorSprite.enabled = false;
                }
            }

            timeMaxed = true;

            return;
        }

        recordFrame();
    }

    public void recordFrame()
    {
        if (!isRecording)
        {
            return;
        }
        timeElapsed += Time.fixedDeltaTime;

        if (lastFrameTime + frameCaptureRate > timeElapsed)
        {
            return;
        }

        lastFrameTime = timeElapsed;
        currFrames.Add(new GhostFrame(transform.position, timeElapsed, isRecordingTunnel));
        if (isRecordingTunnel)
        {
            isRecordingTunnel = false;
        }
    }

}

[System.Serializable]
public struct GhostFrame
{
    public SerializableVector3 position;
    public float time;
    public bool isTunnelEntrance;  // NEW flag

    public GhostFrame(Vector3 pos, float t, bool tunnelEntrance = false)
    {
        position = new SerializableVector3(pos);
        time = t;
        isTunnelEntrance = tunnelEntrance;
    }

    public Vector3 GetPosition() => position.ToVector3();
    public float GetTime() => time;
    public bool IsTunnelEntrance() => isTunnelEntrance;
}

[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

