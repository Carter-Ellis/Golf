using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerScroll : MonoBehaviour
{
    
    [SerializeField]
    private ScrollRect scrollRect;
    private float scrollSens = 3000f;

    void Update()
    {
        if (!PlayerInput.isController || scrollRect ==  null)
        {
            return;
        }

        float scrollInput = PlayerInput.get(PlayerInput.Axis.ScrollWheel);
        if (Mathf.Approximately(scrollInput, 0f))
        {
            return;
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Vector2.zero,
            scrollDelta = new Vector2(0, scrollInput * scrollSens * Time.deltaTime),
        };
        ExecuteEvents.Execute(scrollRect.gameObject, pointerData, ExecuteEvents.scrollHandler);

    }
}
