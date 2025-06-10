using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupController : MonoBehaviour
{
    public GameObject popup;
    private Inventory inv;
    private Ball ball;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
        ball = FindObjectOfType<Ball>();
        
        if (popup != null)
        {
            if(inv.isCampSpeedMode || inv.isClassicSpeedMode || inv.isCampHardMode || inv.isClassicHardMode)
            {
                popup.SetActive(false);
                return;
            }
            
            int level = SceneManager.GetActiveScene().buildIndex;

            if (inv.levelPopups != null && inv.levelPopups.ContainsKey(level) && inv.levelPopups[level])
            {
                disablePopup();
                return; //Has already seen popup
            }
            else if (inv.levelPopups == null)
            {
                inv.levelPopups = new Dictionary<int, bool>();
            }

            enablePopup();
            inv.levelPopups[level] = true;
            return;
        }        

    }

    private void Update()
    {
        if (popup.activeSelf && PlayerInput.isController && PlayerInput.isDown(PlayerInput.Axis.Fire1))
        {
            disablePopup();
        }
    }

    public void disablePopup()
    {
        if (popup == null) { Debug.Log("Popup is null"); return; }
        ball.pause(false);
        popup.SetActive(false);
    }

    public void enablePopup()
    {
        if (popup == null) { Debug.Log("Popup is null"); return; }
        ball.pause(true);
        popup.SetActive(true);
    }

}
