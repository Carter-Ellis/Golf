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
    private int totalCoins;
    private void Start()
    {
        inv = FindObjectOfType<Ball>().GetComponent<Inventory>();
    }
    private void Update()
    {
        totalCoinsTxt.text = inv.totalCoins + "/" + totalCoins;
    }

    private void Awake()
    {
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
            
            
            buttons[i].GetComponent<ButtonAudio>().enabled = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].GetComponent<ButtonAudio>().enabled = true;
            buttons[i].interactable = true;
        }
    }
    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId + " Official";
        SceneManager.LoadScene(levelName);
    }

    public void OpenClassicLevel(int levelId)
    {
        string levelName = "Classic " + levelId;
        SceneManager.LoadScene(levelName);
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
