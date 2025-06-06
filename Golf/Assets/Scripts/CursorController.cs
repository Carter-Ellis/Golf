using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{

    private Image cursorImage;
    private HashSet<GameObject> hovered = new HashSet<GameObject>();

    private void OnEnable()
    {
        PlayerInput.resetCursor();
    }
    private void Awake()
    {
        cursorImage = this.GetComponent<Image>();
        if (PlayerInput.isController)
        {
            if (!cursorImage.enabled)
            {
                cursorImage.enabled = true;
            }
        }
        else if (cursorImage.enabled)
        {
            cursorImage.enabled = false;
        }
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
            SimulateClick(position, PlayerInput.isDown(PlayerInput.Axis.Fire1));
        }
        else if (cursorImage.enabled)
        {
            cursorImage.enabled = false;
        }
    }

    private void SimulateClick(Vector2 screenPosition, bool isDown)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        HashSet<GameObject> newHover = new HashSet<GameObject>();

        foreach (var result in results)
        {

            GameObject obj = result.gameObject;

            mouseEvents(obj, pointerData, isDown, newHover, hovered);

            if (obj.name == "Background")
            {
                GameObject parent = obj.transform.parent.gameObject;
                if (parent != null)
                {
                    mouseEvents(parent, pointerData, isDown, newHover, hovered);
                }
            }
        }

        foreach (GameObject obj in hovered)
        {
            if (!newHover.Contains(obj))
            {
                ExecuteEvents.Execute(obj, pointerData, ExecuteEvents.pointerExitHandler);
            }
        }

        hovered = newHover;

    }

    private void mouseEvents(GameObject target, PointerEventData pointerData, bool isDown, HashSet<GameObject> newHover, HashSet<GameObject> oldHover)
    {
        newHover.Add(target);
        if (isDown)
        {
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.dragHandler);
        }
        if (!oldHover.Contains(target))
        {
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerEnterHandler);
        }
    }

}
