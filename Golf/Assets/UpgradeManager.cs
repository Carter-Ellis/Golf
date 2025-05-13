using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [HideInInspector]
    public UpgradeButton selectedUpgrade;

    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if (Input.GetMouseButton(0)) // Left click
        {
            GameObject clicked = GetClickedUI();

            // If you clicked outside any upgrade or the Buy button
            if (clicked == null || clicked.GetComponent<UpgradeButton>() == null && (clicked.CompareTag("Item") && clicked.name != "Buy"))
            {
                SelectUpgrade(null);
                FindObjectOfType<BuyButton>().SetItemSelected(false);
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

    public void SelectUpgrade(UpgradeButton upgrade)
    {
        selectedUpgrade = upgrade;
    }

    public void TryPurchaseSelected()
    {
        if (selectedUpgrade != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gameObject);
            selectedUpgrade.TryPurchase();
        }
        
    }
}

