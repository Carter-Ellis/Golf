using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{

    private Image cursorImage;

    private void Start()
    {
        cursorImage = this.GetComponent<Image>();
    }
    void Update()
    {
        if (PlayerInput.isController)
        {
            if (!cursorImage.enabled)
            {
                cursorImage.enabled = true;
            }
            Vector2 position = new Vector2(Screen.width, Screen.height) - PlayerInput.cursorPosition;
            transform.position = position;
            if (PlayerInput.isDown(PlayerInput.Axis.Fire1))
            {
                SimulateClick(position);
            }
        }
        else if (cursorImage.enabled)
        {
            cursorImage.enabled = false;
        }
    }

    private void SimulateClick(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.dragHandler);
            if (result.gameObject.name == "Background")
            {
                GameObject parent = result.gameObject.transform.parent.gameObject;
                if (parent != null)
                {
                    ExecuteEvents.Execute(parent, pointerData, ExecuteEvents.pointerClickHandler);
                    ExecuteEvents.Execute(parent, pointerData, ExecuteEvents.dragHandler);
                }
            }
        }
    }

}
