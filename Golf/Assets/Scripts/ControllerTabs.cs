using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerTabs : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button[] buttons;
    private int currentTab = 0;

    void Update()
    {
        if (!PlayerInput.isController)
        {
            return;
        }

        if (buttons == null || buttons.Length < 2)
        {
            return;
        }

        int tabChange = 0;
        if (PlayerInput.isDown(PlayerInput.Axis.SwapUp))
        {
            tabChange = 1;
            
        }
        else if(PlayerInput.isDown(PlayerInput.Axis.SwapDown))
        {
            tabChange = buttons.Length - 1;
        }
        else
        {
            return;
        }
        
        currentTab = (currentTab + tabChange) % buttons.Length;
        GameObject buttonObj = buttons[currentTab].gameObject;
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Vector2.zero,
        };
        ExecuteEvents.Execute(buttonObj, pointerData, ExecuteEvents.pointerClickHandler);

    }
}
