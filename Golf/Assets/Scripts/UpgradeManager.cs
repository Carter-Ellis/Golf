using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [HideInInspector]
    public UpgradeButton selectedUpgrade;

    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unequipButton;
    private GameObject buyButton;

    void Awake()
    {
        Instance = this;
        buyButton = FindObjectOfType<BuyButton>().gameObject;
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // Left click
        {
            GameObject clicked = GetClickedUI();

            if (clicked != null && clicked.name == "Buy")
            {
                return;
            }

            SelectUpgrade(null);

            // If you clicked outside any upgrade or the Buy button
            if (clicked == null || (!clicked.CompareTag("Item") && clicked.name != "Buy" && !clicked.CompareTag("Cosmetic")) )
            {
                if (buyButton.GetComponent<BuyButton>())
                {
                    buyButton.GetComponent<BuyButton>().SetItemSelected(false);
                }      
            }
            else if (clicked.CompareTag("Cosmetic"))
            {
                if (FindObjectOfType<BuyButton>() == null) { return; }

                GameObject buyButton = FindObjectOfType<BuyButton>().gameObject;         
                buyButton.SetActive(false);
                if (FindObjectOfType<CosmeticsManager>().isEquipped)
                {
                    unequipButton.SetActive(true);
                }
                else
                {
                    equipButton.SetActive(true);
                }     
                
            }
            else
            {
                //Upgrade selected
                SelectUpgrade(clicked);
                
                if (!buyButton.activeSelf)
                {
                    buyButton.SetActive(true);
                    equipButton.SetActive(false);
                    unequipButton.SetActive(false);
                }
                buyButton.GetComponent<BuyButton>().SetItemSelected(true);
            }
        }
    }

    GameObject GetClickedUI()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        if (results.Count > 0)
            return results[0].gameObject;

        return null;
    }

    public void SelectUpgrade(GameObject obj)
    {
        if (selectedUpgrade != null)
        {
            selectedUpgrade.GetComponent<Image>().color = Color.white;
        }
        if (obj == null)
        {
            selectedUpgrade = null;
            return;
        }
        selectedUpgrade = obj.GetComponent<UpgradeButton>();
        if (selectedUpgrade == null)
        {
            selectedUpgrade = obj.GetComponentInParent<UpgradeButton>();
        }
        selectedUpgrade.GetComponent<Image>().color = new Color(.7f, .7f, .7f);

    }

    public void TryPurchaseSelected()
    {
        if (selectedUpgrade != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
            selectedUpgrade.GetComponentInParent<UpgradeButton>().TryPurchase();
        }
        
    }
}

