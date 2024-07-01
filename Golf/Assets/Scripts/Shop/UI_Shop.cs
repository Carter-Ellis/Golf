using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private Inventory inv;

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        inv = FindObjectOfType<Inventory>();
        gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton(Item.ItemType.Freeze, "Freeze", Item.GetCost(Item.ItemType.Freeze), 0);
        CreateItemButton(Item.ItemType.Wind, "Wind", Item.GetCost(Item.ItemType.Wind), 1);
        CreateItemButton(Item.ItemType.Teleport, "Teleport", Item.GetCost(Item.ItemType.Teleport), 2);
        CreateItemButton(Item.ItemType.Burst, "Burst", Item.GetCost(Item.ItemType.Burst), 3);
    }

    private void CreateItemButton(Item.ItemType itemType, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRect = shopItemTransform.GetComponent<RectTransform>();
        float shopItemHeight = 100f;
        shopItemRect.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);
        shopItemTransform.Find("NameTxt").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("PriceTxt").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //Clicked on shop item button
            TryBuyItem(itemType);
        };
    }

    private void TryBuyItem(Item.ItemType itemType)
    {
        if (inv.coins < Item.GetCost(itemType))
        {
            print("Not enough coins.");
            return;
        }
        inv.AddAbility(Ability.Create((ABILITIES)itemType - 1, Color.black));
        inv.coins -= Item.GetCost(itemType);
    }

}
