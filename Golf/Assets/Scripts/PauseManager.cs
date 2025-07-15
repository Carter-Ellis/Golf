using TMPro;
using UnityEngine;
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
    [SerializeField] private GameObject resetButton;
    [SerializeField] private GameObject shopButton;

    [Header("Level Description")]
    [SerializeField] private TextMeshProUGUI holeNumberTxt;
    [SerializeField] private TextMeshProUGUI parTxt;
    [SerializeField] private TextMeshProUGUI strokesTxt;
    [SerializeField] private TextMeshProUGUI upgradeAvailableTxt;
    private int[] costs = { 4, 6, 9, 12 };
    public void initialize(GameObject menu)
    {
        inv = FindObjectOfType<Inventory>();
        Hole hole = FindObjectOfType<Hole>();
        ball = FindObjectOfType<Ball>();
        resetButton = menu.gameObject.transform.Find("Audio").gameObject.transform.Find("RestartLevelButton").gameObject;
        shopButton = menu.gameObject.transform.Find("Audio").gameObject.transform.Find("ShopButton").gameObject;

        if (holeNumberTxt == null || parTxt == null || strokesTxt == null || 
           ball == null || hole == null) { Debug.Log("PauseManager has null."); return; }

        holeNumberTxt.text = "Hole " + FindObjectOfType<Hole>().holeNum;
        parTxt.text = "Par " + hole.par;
        strokesTxt.text = "Stroke " + ball.strokes;

    }

    public void UpdatePauseMenu()
    {

        if ((GameMode.current != GameMode.TYPE.FREEPLAY) && !GameMode.isAnySpeedrun())
        {
            resetButton.SetActive(false);
        }

        if (GameMode.isAnySpeedrun() || Map.current == Map.TYPE.CLASSIC)
        {
            coinDisplay1.enabled = false;
            coinDisplay2.enabled = false;
            coinDisplay3.enabled = false;
        }

        for (int i = 0; i < 3; i++)
        {
            upgradeAvailableTxt.enabled = false;
            int level = 0;
            if (inv.upgradeLevels.ContainsKey(i+1))
            {
                level = inv.upgradeLevels[i+1];
            }

            if (level < costs.Length && inv.coins >= costs[level]
                && inv.unlockedAbilities.Count > i)
            {
                upgradeAvailableTxt.enabled = true;
            }
            if (upgradeAvailableTxt.enabled)
            {
                break;
            }
            
        }

        int currentLevel = FindObjectOfType<Hole>().holeNum;
        MapData mapData = Map.getCurrent();
        if (mapData.isCoinCollected(currentLevel, 1) && goldCoin != null && coinDisplay1 != null)
        {
            coinDisplay1.GetComponent<Image>().sprite = goldCoin;
        }
        if (mapData.isCoinCollected(currentLevel, 2) && goldCoin != null && coinDisplay1 != null)
        {
            coinDisplay2.GetComponent<Image>().sprite = goldCoin;
        }
        if (mapData.isCoinCollected(currentLevel, 3) && goldCoin != null && coinDisplay1 != null)
        {
            coinDisplay3.GetComponent<Image>().sprite = goldCoin;
        }

        if (GameMode.current != GameMode.TYPE.FREEPLAY)
        {
            upgradeAvailableTxt.enabled = false;
            shopButton.SetActive(false);
        }

        if (ball == null)
        {
            
            return;
        }
        strokesTxt.text = "Stroke " + ball.strokes;
    }

}
