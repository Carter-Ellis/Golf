using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class UpgradeButton : MonoBehaviour
{
    public int upgradeLevel = 0;
    public int[] costs = { 2, 5, 8, 12 };
    public Image[] progressSquares;
    public Sprite purchasedSquare;
    public UnityEngine.UI.Button button;
    private Inventory inv;
    private TextMeshProUGUI costTxt;
    

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
        costTxt = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {

        if (costTxt == null)
        {
            Debug.Log("No textmesh pro in children");
            return;
        }
        if (upgradeLevel >= costs.Length) {
            costTxt.text = "Maxed";
            return; 
        }
        else
        {
            costTxt.text = "Cost: " + costs[upgradeLevel];
        }
        
    }

    public void TryPurchase()
    {
        if (upgradeLevel >= costs.Length) return;

        int cost = costs[upgradeLevel];

        if (inv.coins >= cost)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.shopPurchase, transform.position);
            inv.coins -= cost;
            progressSquares[upgradeLevel].sprite = purchasedSquare;
            upgradeLevel++;

            inv.upgradeLevels[transform.GetSiblingIndex()] = upgradeLevel;
            inv.maxChargesByType[(ABILITIES)transform.GetSiblingIndex()]++;
            inv.SavePlayer();
        }
    }

}
