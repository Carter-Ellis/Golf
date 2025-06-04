using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHighscore : MonoBehaviour
{
    private Inventory inv;
    [SerializeField] private List<TextMeshProUGUI> campaign18Scores;
    [SerializeField] private TextMeshProUGUI campaign18Best;

    [SerializeField] private List<TextMeshProUGUI> campSpeedScores;
    [SerializeField] private TextMeshProUGUI campSpeedBest;

    [SerializeField] private List<TextMeshProUGUI> classic18Scores;
    [SerializeField] private TextMeshProUGUI classic18Best;

    [SerializeField] private List<TextMeshProUGUI> classicSpeedScores;
    [SerializeField] private TextMeshProUGUI classicSpeedBest;
    void Awake()
    {
        inv = FindObjectOfType<Inventory>();
    }

    public void campaign18()
    {
        if (inv.campaignHighScore != null && inv.campaignHighScore.Count != 18)
        {
            int totalScore = 0;
            foreach (var kvp in inv.campaignHighScore)
            {
                int score = kvp.Value;
                int level = kvp.Key;
                totalScore += score;

                campaign18Scores[level - 1].text = score.ToString();
                
            }
            
            if (inv.campaignHighScore.Count > 0)
            {
                campaign18Best.text = "Best Score: " + totalScore.ToString();
            }
            
        }
    }

    public void campSpeed()
    {
        if (inv.campSpeedHighScore != null && inv.campSpeedHighScore.Count != 18)
        {
            float totalScore = 0;
            foreach (var kvp in inv.campSpeedHighScore)
            {
                float score = kvp.Value;
                int level = kvp.Key;
                totalScore += score;

                TimeSpan timeSpan = TimeSpan.FromSeconds(score);
                campSpeedScores[level - 1].text = timeSpan.ToString(@"mm\:ss"); ;

            }

            if (inv.campSpeedHighScore.Count > 0)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(totalScore);
                campSpeedBest.text = "Best Score: " + timeSpan.ToString(@"mm\:ss\.ff");
            }

        }
    }

    public void classic18()
    {
        
        if (inv.classicHighScore != null && inv.classicHighScore.Count != 18)
        {
            int totalScore = 0;
            
            foreach (var kvp in inv.classicHighScore)
            {
                int score = kvp.Value;
                int level = kvp.Key;
                totalScore += score;
                
                classic18Scores[level - 1].text = score.ToString();
            }
            if (inv.classicHighScore.Count > 0)
            {
                classic18Best.text = "Best Score: " + totalScore.ToString();
            }
        }
    }

    public void classicSpeed()
    {
        if (inv.classicSpeedHighScore != null && inv.classicSpeedHighScore.Count != 18)
        {
            float totalScore = 0;
            foreach (var kvp in inv.classicSpeedHighScore)
            {
                float score = kvp.Value;
                int level = kvp.Key;
                totalScore += score;

                TimeSpan timeSpan = TimeSpan.FromSeconds(score);
                classicSpeedScores[level - 1].text = timeSpan.ToString(@"mm\:ss"); ;

            }

            if (inv.classicSpeedHighScore.Count > 0)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(totalScore);
                classicSpeedBest.text = "Best Score: " + timeSpan.ToString(@"mm\:ss\.ff");
            }

        }
    }

}
