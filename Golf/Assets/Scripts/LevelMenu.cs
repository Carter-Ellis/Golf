using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public UnityEngine.UI.Button[] buttons;
    public GameObject levelButtons;
    private Inventory inv;
    [SerializeField] private TextMeshProUGUI totalCoinsTxt;
    [SerializeField] private Image totalCoinsImg;
    private int totalCoins;
    private void Start()
    {
        inv = FindObjectOfType<Ball>().GetComponent<Inventory>();
    }
    private void Update()
    {
        if (totalCoinsTxt != null)
        {
            totalCoinsTxt.text = inv.totalCoins + "/" + totalCoins;
        }
        
    }

    private void OnEnable()
    {
        ButtonsToArray();
        MainMenu menu = FindObjectOfType<MainMenu>();
        Inventory inv = FindObjectOfType<Inventory>();
        Map.TYPE map = menu.GetMap();
        GameMode.TYPE mode = GameMode.current;
        bool isFreePlay = mode == GameMode.TYPE.FREEPLAY;
        for (int i = 0; i < buttons.Length; i++)
        {
            bool unlocked = inv.isLevelUnlocked(map, mode, i+1);
            buttons[i].interactable = unlocked;
            buttons[i].GetComponent<ButtonAudio>().enabled = unlocked;
            buttons[i].GetComponent<HoverFlag>().enabled = isFreePlay;
            if (map == Map.TYPE.CLASSIC)
            {
                buttons[i].GetComponent<HoverFlag>().enabled = false;
            }
        }

        totalCoinsImg.enabled = isFreePlay;
        totalCoinsTxt.enabled = isFreePlay;

        if (map == Map.TYPE.CLASSIC)
        {
            totalCoinsImg.enabled = false;
            totalCoinsTxt.enabled = false;
        }

    }

    private void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new UnityEngine.UI.Button[childCount];  
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Button>();
        }
        totalCoins = buttons.Length * 3;
        
    }

}
