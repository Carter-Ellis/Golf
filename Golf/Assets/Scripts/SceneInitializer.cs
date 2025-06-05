using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToEnable;
    [SerializeField] private GameObject[] objectsToDisable;

    [SerializeField] private UnityEngine.UI.Button campHardcoreButton;
    [SerializeField] private UnityEngine.UI.Button classicHardcoreButton;

    public int unlockHardcoreAmount = 45;

    void Start()
    {
        
        Inventory inv = FindObjectOfType<Inventory>();

        inv.isClassicMode = false;
        inv.isClassicSpeedMode = false;
        inv.isCampaignMode = false;
        inv.isWalkMode = false;
        inv.isCampSpeedMode = false;
        inv.isClassicHardMode = false;
        inv.isCampHardMode = false;
        inv.isFreeplayMode = false;
        inv.SavePlayer();

        if (inv.totalCoins >= unlockHardcoreAmount)
        {
            
            campHardcoreButton.interactable = true;
            Image childImage = campHardcoreButton.transform.GetChild(0).GetComponent<Image>();
            childImage.enabled = false;
        }

        if (inv.classicHighScore.Count == 18)
        {
            classicHardcoreButton.interactable = true;
            Image childImage = classicHardcoreButton.transform.GetChild(0).GetComponent<Image>();
            childImage.enabled = false;
        }

        if (!SceneLoader.isShopLoad || objectsToEnable.Length == 0 || objectsToDisable.Length == 0) return;

        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        SceneLoader.isShopLoad = false;
    }
}
