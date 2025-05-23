using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static bool isShopLoad;

    public void LoadShopScene()
    {
        isShopLoad = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
