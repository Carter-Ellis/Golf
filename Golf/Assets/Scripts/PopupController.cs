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
            
            if(inv.isCampSpeedMode || inv.isClassicSpeedMode || inv.isCampHardMode || inv.isClassicHardMode)
            {
                popup.SetActive(false);
                return;
            }
            
            int level = SceneManager.GetActiveScene().buildIndex;
            if (!inv.levelPopups.ContainsKey(level)) { return; }

            if (inv.levelPopups[level])
            {
                disablePopup();
            }
            else
            {
                enablePopup();
            }
            inv.levelPopups[level] = true;
            return;
        }        

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
