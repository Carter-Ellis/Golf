using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using UnityEngine.UI;
using static Item;
using UnityEngine.SceneManagement;

public class UI_Shop : MonoBehaviour
{
    public List<ItemType> shopItems = new List<ItemType>();
    private List<GameObject> itemGameObj = new List<GameObject>();
    public Color hoverColor;
    public Color boughtOutColor;
    private Transform container;
    private Transform shopItemTemplate;
    private Inventory inv;
    [SerializeField] Canvas exitCanvas;
    private bool isActive;

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        inv = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        foreach (ItemType type in shopItems)
        {
            CreateItemButton(type);
        }
        
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.menuClose, transform.position);
    }

    private void CreateItemButton(Item.ItemType itemType)
    {
        string itemName = Item.GetName(itemType);
        string info = Item.GetDescription(itemType);
        Sprite sprite = Item.GetSprite(itemType);
        int itemCost = Item.GetCost(itemType);
        
        
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);

        itemGameObj.Add(shopItemTransform.gameObject);

        TextMeshProUGUI nameTxt = shopItemTransform.Find("NameTxt").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI priceTxt = shopItemTransform.Find("PriceTxt").GetComponent<TextMeshProUGUI>();
        GameObject description = shopItemTransform.Find("Description").gameObject;
        Image itemIcon = shopItemTransform.Find("ItemIcon").GetComponent<Image>();
        Image background = shopItemTransform.Find("Background").GetComponent<Image>();


        description.SetActive(false);



        RectTransform shopItemRect = shopItemTransform.GetComponent<RectTransform>();
        bool isBought = false;
        
        
        float shopItemHeight = shopItemRect.rect.height * 1.5f;
        shopItemRect.anchoredPosition = new Vector2(0, -shopItemHeight * (itemGameObj.Count - 1));

        nameTxt.SetText(itemName);
        nameTxt.color = Color.black;
        itemIcon.sprite = sprite;     
        priceTxt.SetText(itemCost.ToString());
        Color origColor = background.color;

        shopItemTransform.GetComponent<Button_UI>().MouseOverOnceFunc = () =>
        {
            if (!isBought)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.menuBlip, transform.position);
                background.color = hoverColor;
            }
            description.SetActive(true);
            description.GetComponent<TextMeshProUGUI>().SetText(info);
        };

        shopItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            //Clicked on shop item button
            if (TryBuyItem(itemType))
            {
                isBought = true;
                background.color = boughtOutColor;
            }
        };

        shopItemTransform.GetComponent<Button_UI>().MouseOutOnceFunc = () =>
        {
            if (!isBought)
            {
                background.color = origColor;
            }
            description.SetActive(false);
        };
        if (Item.IsAbility(itemType))
        {
            ABILITIES abilityType = Item.GetAbility(itemType);
            foreach (Ability abil in inv.unlockedAbilities)
            {
                if (abil.type == abilityType)
                {
                    isBought = true;
                    background.color = boughtOutColor;
                }
            }
        }
    }

    private bool TryBuyItem(Item.ItemType itemType)
    {
        if (inv.coins < Item.GetCost(itemType))
        {
            print("Not enough coins.");
            return false;
        }
        if (Item.IsAbility(itemType))
        {
            ABILITIES abilityType = Item.GetAbility(itemType);
            foreach (Ability abil in inv.unlockedAbilities)
            {
                if (abil.type == abilityType)
                {
                    print("Already have this ability.");
                    return false;
                }
            }
            inv.AddAbility(Ability.Create(abilityType, Color.black));           
        }
        AudioManager.instance.PlayOneShot(FMODEvents.instance.shopPurchase, transform.position);
        inv.coins -= Item.GetCost(itemType);
        return true;
    }

}
