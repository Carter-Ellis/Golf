using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuWarning : MonoBehaviour
{

    [SerializeField] private GameObject menuWarning;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winCanvas;

    public void warningOpen()
    {
        if (GameMode.isAnySpeedrun() || GameMode.current == GameMode.TYPE.FREEPLAY)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            winCanvas.SetActive(false);
            pauseMenu.SetActive(false);
            menuWarning.SetActive(true);
        }

    }
}
