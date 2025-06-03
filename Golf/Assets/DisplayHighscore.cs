using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHighscore : MonoBehaviour
{
    private Inventory inv;
    [SerializeField] private List<TextMeshProUGUI> campaign18Scores;
    [SerializeField] private TextMeshProUGUI campaign18Best;
    [SerializeField] private List<TextMeshProUGUI> classic18Scores;
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
            campaign18Best.text = "Best Score: " + totalScore.ToString();
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
        }
    }

}
