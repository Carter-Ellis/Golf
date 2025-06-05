using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private Inventory inv;
    private Ball ball;

    [SerializeField] private Image coinDisplay1;
    [SerializeField] private Image coinDisplay2;
    [SerializeField] private Image coinDisplay3;
    [SerializeField] private Sprite goldCoin;
    [SerializeField] private Sprite grayCoin;

    [Header("Level Description")]
    [SerializeField] private TextMeshProUGUI holeNumberTxt;
    [SerializeField] private TextMeshProUGUI parTxt;
    [SerializeField] private TextMeshProUGUI strokesTxt;
    [SerializeField] private TextMeshProUGUI upgradeAvailableTxt;
    private int[] costs = { 2, 5, 8, 12 };
    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
        Hole hole = FindObjectOfType<Hole>();
        ball = FindObjectOfType<Ball>();

        if (holeNumberTxt == null || parTxt == null || strokesTxt == null || 
           ball == null || hole == null) { Debug.Log("PauseManager has null."); return; }

        holeNumberTxt.text = "Hole " + FindObjectOfType<Hole>().holeNum;
        parTxt.text = "Par " + hole.par;
        strokesTxt.text = "Stroke " + ball.strokes;

    }

    public void UpdatePauseMenu()
    {
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

        int currentLevel = FindObjectOfType<Hole>().holeNum;
        if (inv.coinsCollected != null && inv.coinsCollected.ContainsKey(currentLevel))
        {
            if (inv.coinsCollected[currentLevel].Contains(1) && goldCoin != null && coinDisplay1 != null)
            {
                coinDisplay1.GetComponent<Image>().sprite = goldCoin;
            }
            if (inv.coinsCollected[currentLevel].Contains(2) && goldCoin != null && coinDisplay1 != null)
            {
                coinDisplay2.GetComponent<Image>().sprite = goldCoin;
            }
            if (inv.coinsCollected[currentLevel].Contains(3) && goldCoin != null && coinDisplay1 != null)
            {
                coinDisplay3.GetComponent<Image>().sprite = goldCoin;
            }
        }
        if (ball == null)
        {
            Debug.Log("No ball found");
            return;
        }
        strokesTxt.text = "Stroke " + ball.strokes;
    }

}
