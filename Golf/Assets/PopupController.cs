using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupController : MonoBehaviour
{
    public GameObject popup;
    private Inventory inv;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
        if (popup != null)
        {
            int level = SceneManager.GetActiveScene().buildIndex;
            inv.levelPopups[level] = true;
            return;
        }
        string levelName = SceneManager.GetActiveScene().name;

        if (levelName != "Main Menu") { return; }

        foreach (var levelNumber in inv.levelPopups.Keys.ToList())
        {
            inv.levelPopups[levelNumber] = false;
        }
        inv.SavePlayer();
    }

    public void disablePopup()
    {
        if (popup == null) { Debug.Log("Popup is null"); return; }
        popup.SetActive(false);
    }

    public void enablePopup()
    {
        if (popup == null) { Debug.Log("Popup is null"); return; }

        popup.SetActive(true);
    }

}
