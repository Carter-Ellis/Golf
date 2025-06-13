using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{

    private Image cursorImage;
    private HashSet<GameObject> hovered = new HashSet<GameObject>();
    private bool dragging = false;
    private float scrollSens = 3000f;

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
            SimulateClick(position);
            if (PlayerInput.isDown(PlayerInput.Axis.Fire2))
            {
                BackButton(position);
            }
        }
        else if (cursorImage.enabled)
        {
            cursorImage.enabled = false;
            dragging = false;
        }
    }

    private void BackButton(Vector2 screenPosition)
    {

        GameObject backObj = GameObject.Find("Back");
        var backButton = backObj?.GetComponent<UnityEngine.UI.Button>();
        if (backButton == null)
        {
            return;
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition,
        };
        ExecuteEvents.Execute(backObj, pointerData, ExecuteEvents.pointerClickHandler);

    }

    private void SimulateClick(Vector2 screenPosition)
    {

        bool isDown = PlayerInput.isDown(PlayerInput.Axis.Fire1);
        bool isUp = PlayerInput.isUp(PlayerInput.Axis.Fire1) && dragging;
        if (isDown)
        {
            dragging = true;
        }
        else if (isUp)
        {
            dragging = false;
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition,
        };
        float scrollInput = PlayerInput.get(PlayerInput.Axis.ScrollWheel);
        if (scrollInput != 0)
        {
            pointerData.scrollDelta = new Vector2(0, scrollInput * scrollSens * Time.deltaTime);
        }

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        HashSet<GameObject> newHover = new HashSet<GameObject>();

        foreach (var result in results)
        {

            GameObject obj = result.gameObject;

            mouseEvents(obj, pointerData, isDown, isUp, newHover, hovered);

            if (obj.name == "Background")
            {
                GameObject parent = obj.transform.parent.gameObject;
                if (parent != null)
                {
                    mouseEvents(parent, pointerData, isDown, isUp, newHover, hovered);
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

    private void mouseEvents(GameObject target, PointerEventData pointerData, bool isDown, bool isUp, HashSet<GameObject> newHover, HashSet<GameObject> oldHover)
    {
        newHover.Add(target);
        if (isDown)
        {
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.beginDragHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.dragHandler);
        }
        else if (isUp)
        {
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.endDragHandler);
        }
        else if (dragging)
        {
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.dragHandler);
        }
        if (!oldHover.Contains(target))
        {
            ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerEnterHandler);
        }
        ExecuteEvents.Execute(target, pointerData, ExecuteEvents.scrollHandler);
    }

}
