using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.SetActive(false);
    }

    void OnEnable()
    {
        description.SetActive(false);
    }

}
