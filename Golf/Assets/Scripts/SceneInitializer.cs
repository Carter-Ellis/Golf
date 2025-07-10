using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToEnable;
    [SerializeField] private GameObject[] objectsToDisable;

    void Start()
    {

        if (!SceneLoader.isShopLoad || objectsToEnable.Length == 0 || objectsToDisable.Length == 0) return;

        FindObjectOfType<MainMenu>().SetState(MainMenu.State.SHOP);
        Audio.playShopMusic();

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
