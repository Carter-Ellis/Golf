using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;
public class RunStarter : MonoBehaviour
{
    Inventory inv;
    private void Awake()
    {
        inv = FindObjectOfType<Inventory>();
    }

    public void StartCampaign()
    {
        inv.isCampaignMode = true;
        inv.campaignCurrScore = new Dictionary<int, int>();
        inv.SavePlayer();

        string levelName = "Level 1 Official";
        SceneManager.LoadScene(levelName);
    }

    public void StartClassic()
    {
        inv.isClassicMode = true;
        inv.classicCurrScore = new Dictionary<int, int>();
        inv.SavePlayer();

        string levelName = "Classic 1";
        SceneManager.LoadScene(levelName);
    }

}
