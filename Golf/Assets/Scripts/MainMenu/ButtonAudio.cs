using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public EventReference clickSound;
    public bool isSecretButton;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSecretButton || !gameObject.GetComponent<UnityEngine.UI.Button>().interactable) {  return; }

        Audio.playSFX(FMODEvents.instance.menuBlip, transform.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gameObject.GetComponent<UnityEngine.UI.Button>().interactable) { return; }
        if (clickSound.IsNull) { return; }
        Audio.playSFX(clickSound, transform.position);
    }
}
