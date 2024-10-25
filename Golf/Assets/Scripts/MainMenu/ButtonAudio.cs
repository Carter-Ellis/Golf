using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public EventReference clickSound;
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.menuBlip, transform.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot(clickSound, transform.position);
    }
}
