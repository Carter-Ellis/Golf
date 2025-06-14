using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int upgradeLevel = 0;
    public int[] costs = { 2, 5, 8, 12 };
    public Image[] progressSquares;
    public Sprite purchasedSquare;
    public UnityEngine.UI.Button button;
    private Inventory inv;
    private TextMeshProUGUI costTxt;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI descriptionTxt;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
        costTxt = GetComponentInChildren<TextMeshProUGUI>();
        descriptionTxt.text = "Upgrade Ability: \n" + Ability.GetUpgradeDescription((ABILITIES)transform.GetSiblingIndex());
        bool hasAbility = inv.unlockedAbilities.Exists(a => a.type == (ABILITIES)transform.GetSiblingIndex());
        if (!hasAbility)
        {
            button.interactable = false;
            ColorBlock cb = button.colors;
            cb.disabledColor = new Color(0,0,0,.5f);
            button.colors = cb;
        }
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

            if ((ABILITIES)transform.GetSiblingIndex() == ABILITIES.TELEPORT)
            {
                inv.teleportRange += 2;
            }
            else
            {
                inv.maxChargesByType[(ABILITIES)transform.GetSiblingIndex()]++;
            }
            

            inv.SavePlayer();
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.error, transform.position);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    { 
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }

}
