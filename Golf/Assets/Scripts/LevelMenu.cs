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
    [SerializeField] private TextMeshProUGUI totalCoinsTxt;
    [SerializeField] private Image totalCoinsImg;

    private void OnEnable()
    {
        ButtonsToArray();
        Map.TYPE map = MainMenu.GetMap();
        GameMode.TYPE mode = GameMode.current;
        bool isFreePlay = mode == GameMode.TYPE.FREEPLAY;
        for (int i = 0; i < buttons.Length; i++)
        {
            bool unlocked = Map.get(map).isLevelUnlocked(mode, i+1);
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
        
    }

}
