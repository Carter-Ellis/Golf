using UnityEngine;
using UnityEngine.EventSystems;

public class SliderHandleSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private bool isHolding = false;

    void Update()
    {
        if (isHolding && Input.GetMouseButtonUp(0))
        {
            isHolding = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHolding)
        {
            Audio.playSFX(FMODEvents.instance.menuBlip, transform.position);
        }
    }

}