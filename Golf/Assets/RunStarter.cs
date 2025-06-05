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
        inv.isCampSpeedMode = false;
        inv.isClassicMode = false;
        inv.isWalkMode = false;
        inv.isClassicSpeedMode = false;
        inv.isCampHardMode = false;
        inv.isClassicHardMode = false;
        inv.isFreeplayMode = false;

        inv.isCampaignMode = true;

        inv.campaignCurrScore = new Dictionary<int, int>();
        inv.SavePlayer();

        string levelName = "Level 1 Official";
        SceneManager.LoadScene(levelName);
    }

    public void StartClassic()
    {
        inv.isCampSpeedMode = false;
        inv.isCampaignMode = false;
        inv.isWalkMode = false;
        inv.isClassicSpeedMode = false;
        inv.isCampHardMode = false;
        inv.isClassicHardMode = false;
        inv.isFreeplayMode = false;

        inv.isClassicMode = true;

        inv.classicCurrScore = new Dictionary<int, int>();
        inv.SavePlayer();

        string levelName = "Classic 1";
        SceneManager.LoadScene(levelName);
    }

    public void StartCampSpeedrun()
    {
        inv.isClassicMode = false;
        inv.isCampaignMode = false;
        inv.isWalkMode = false;
        inv.isClassicSpeedMode = false;
        inv.isCampHardMode = false;
        inv.isClassicHardMode = false;
        inv.isFreeplayMode = false;

        inv.isCampSpeedMode = true;

        inv.campSpeedCurrScore = new Dictionary<int, float>();
        inv.SavePlayer();

        string levelName = "Level 1 Official";
        SceneManager.LoadScene(levelName);
    }

    public void StartCampClublessSpeedrun()
    {
        inv.isClassicMode = false;
        inv.isCampaignMode = false;
        inv.isClassicSpeedMode = false;
        inv.isCampHardMode = false;
        inv.isClassicHardMode = false;
        inv.isFreeplayMode = false;

        inv.isCampSpeedMode = true;
        inv.isWalkMode = true;

        inv.SavePlayer();

        string levelName = "Level 1 Official";
        SceneManager.LoadScene(levelName);
    }

    public void StartClassicSpeedrun()
    {
        inv.isClassicMode = false;
        inv.isCampaignMode = false;
        inv.isWalkMode = false;
        inv.isCampSpeedMode = false;
        inv.isCampHardMode = false;
        inv.isClassicHardMode = false;
        inv.isFreeplayMode = false;

        inv.isClassicSpeedMode = true;

        inv.classicSpeedCurrScore = new Dictionary<int, float>();
        inv.SavePlayer();

        string levelName = "Classic 1";
        SceneManager.LoadScene(levelName);
    }

    public void StartClassicClublessSpeedrun()
    {
        inv.isClassicMode = false;
        inv.isCampaignMode = false;
        inv.isCampSpeedMode = false;
        inv.isCampHardMode = false;
        inv.isClassicHardMode = false;
        inv.isFreeplayMode = false;

        inv.isClassicSpeedMode = true;
        inv.isWalkMode = true;

        inv.SavePlayer();

        string levelName = "Classic 1";
        SceneManager.LoadScene(levelName);
    }

    public void StartCampHardcore()
    {
        inv.isClassicMode = false;
        inv.isClassicSpeedMode = false;
        inv.isCampaignMode = false;
        inv.isWalkMode = false;
        inv.isCampSpeedMode = false;
        inv.isClassicHardMode = false;
        inv.isFreeplayMode = false;

        inv.isCampHardMode = true;

        inv.campHardCurrScore = new Dictionary<int, int>();
        inv.SavePlayer();

        string levelName = "Level 1 Official";
        SceneManager.LoadScene(levelName);
    }

    public void StartClassicHardcore()
    {
        inv.isClassicMode = false;
        inv.isClassicSpeedMode = false;
        inv.isCampaignMode = false;
        inv.isWalkMode = false;
        inv.isCampSpeedMode = false;
        inv.isCampHardMode = false;
        inv.isFreeplayMode = false;

        inv.isClassicHardMode = true;

        inv.classicHardCurrScore = new Dictionary<int, int>();
        inv.SavePlayer();

        string levelName = "Classic 1";
        SceneManager.LoadScene(levelName);
    }

    public void StartFreeplay()
    {
        inv.isClassicMode = false;
        inv.isClassicSpeedMode = false;
        inv.isCampaignMode = false;
        inv.isWalkMode = false;
        inv.isCampSpeedMode = false;
        inv.isClassicHardMode = false;
        inv.isCampHardMode = false;

        inv.isFreeplayMode = true;

        inv.SavePlayer();
    }
}
