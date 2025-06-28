using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    private Inventory inv;
    private List<GhostFrame> frames;
    public float playbackSpeed = 1f;

    private int currentFrame = 0;
    private float playbackTime = 0f;

    private float tunnelTimer = 2f;
    private float tunnelTime = 2f;
    private float timer = .2f;
    private float exitTime = .2f;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();

        frames = inv.getGhostFrames();
        
    }

    void Update()
    {
        if (frames == null || frames.Count < 2 || !GameMode.isAnySpeedrun())
            return;

        if (tunnelTimer < tunnelTime)
        {
            tunnelTimer += Time.deltaTime * playbackSpeed;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            timer = 0f;
            return;
        }

        
        playbackTime += Time.deltaTime * playbackSpeed;


        while (currentFrame < frames.Count - 2 && playbackTime > frames[currentFrame + 1].GetTime())
        {
            if (frames[currentFrame].isTunnelEntrance)
            {
                tunnelTimer = 0f;
            }
            currentFrame++;
        }

        if (timer < exitTime)
        {
            timer += Time.deltaTime;
            return;
        }

        GhostFrame a = frames[currentFrame];
        GhostFrame b = frames[currentFrame + 1];

        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        float t = Mathf.InverseLerp(a.GetTime(), b.GetTime(), playbackTime);
        transform.position = Vector3.Lerp(a.GetPosition(), b.GetPosition(), t);

        // End playback when done
        if (currentFrame >= frames.Count - 2 && playbackTime >= b.GetTime())
        {
            gameObject.SetActive(false);
        }
    }
}
