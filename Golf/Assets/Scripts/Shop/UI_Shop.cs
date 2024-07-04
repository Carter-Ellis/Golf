using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using UnityEngine.UI;
using static Item;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private Inventory inv;
    [SerializeField] Canvas exitCanvas;
    private string info;
    private bool isActive;

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        inv = FindObjectOfType<Inventory>();
        info = "Freeze: Stops all velocity of the ball instantly. Press space to use.";
    }

    private void Start()
    {
        CreateItemButton(Item.ItemType.Freeze, "Freeze", Item.GetSprite(Item.ItemType.Freeze), Item.GetCost(Item.ItemType.Freeze), 0, info);
        CreateItemButton(Item.ItemType.Wind, "Wind", Item.GetSprite(Item.ItemType.Wind), Item.GetCost(Item.ItemType.Wind), 1, info);
        CreateItemButton(Item.ItemType.Teleport, "Teleport", Item.GetSprite(Item.ItemType.Teleport), Item.GetCost(Item.ItemType.Teleport), 2, info);
        CreateItemButton(Item.ItemType.Burst, "Burst", Item.GetSprite(Item.ItemType.Burst), Item.GetCost(Item.ItemType.Burst), 3, info);
        exitCanvas.gameObject.SetActive(isActive);
        shopItemTemplate.gameObject.SetActive(false);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            exitCanvas.gameObject.SetActive(isActive);
        }
    }

    public void exitShop() 
    {
        SceneManager.LoadSceneAsync("Level 1");
    }

    private void CreateItemButton(Item.ItemType itemType, string itemName, Sprite sprite, int itemCost, int positionIndex, string info)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.Find("Info").gameObject.SetActive(false);
        RectTransform shopItemRect = shopItemTransform.GetComponent<RectTransform>();
        bool isBought = false;
        
        
        float shopItemHeight = 150f;
        shopItemRect.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);
        shopItemTransform.Find("NameTxt").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("NameTxt").GetComponent<TextMeshProUGUI>().color = Color.black;
        shopItemTransform.Find("ItemIcon").GetComponent<Image>().sprite = sprite;     
        shopItemTransform.Find("PriceTxt").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        Color origColor = shopItemTransform.GetChild(0).GetComponent<Image>().color;
        shopItemTransform.GetComponent<Button_UI>().MouseOverOnceFunc = () =>
        {
            if (!isBought)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.menuBlip, transform.position);
                shopItemTransform.GetChild(0).GetComponent<Image>().color = new Color(36 / 255f, 163 / 255f, 203 / 255f, .5f);
            }
            shopItemTransform.Find("Info").gameObject.SetActive(true);
            shopItemTransform.Find("Info").GetComponent<TextMeshProUGUI>().SetText(info);
        };
        shopItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //Clicked on shop item button
            if (TryBuyItem(itemType))
            {
                isBought = true;
                shopItemTransform.GetChild(0).GetComponent<Image>().color = new Color(37 / 255f, 42 / 255f, 53 / 255f, .5f);
            }
            

        };
        shopItemTransform.GetComponent<Button_UI>().MouseUpdate = () =>
        {
            
            foreach (Ability abil in inv.unlockedAbilities)
            {
                if (abil.type == ((ABILITIES)itemType) - 1)
                {
                    isBought = true;
                    shopItemTransform.GetChild(0).GetComponent<Image>().color = new Color(37 / 255f, 42 / 255f, 53 / 255f, .5f);
                }
            }
            
        };
        shopItemTransform.GetComponent<Button_UI>().MouseOutOnceFunc = () =>
        {
            if (!isBought)
            {
                shopItemTransform.GetChild(0).GetComponent<Image>().color = origColor;
            }
            shopItemTransform.Find("Info").gameObject.SetActive(false);
        };
        
    }

    private bool TryBuyItem(Item.ItemType itemType)
    {
        if (inv.coins < Item.GetCost(itemType))
        {
            print("Not enough coins.");
            return false;
        }
        foreach (Ability abil in inv.unlockedAbilities)
        {
            if(abil.type == ((ABILITIES)itemType) - 1)
            {
                print("Already have this ability.");
                return false;
            }
        }
        AudioManager.instance.PlayOneShot(FMODEvents.instance.shopPurchase, transform.position);
        inv.AddAbility(Ability.Create(((ABILITIES)itemType) - 1, Color.black));
        inv.coins -= Item.GetCost(itemType);
        return true;
    }

}
