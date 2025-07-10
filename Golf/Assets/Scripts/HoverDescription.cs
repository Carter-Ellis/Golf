using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject description;
    private MainMenu menu;

    private void Start()
    {
        menu = FindObjectOfType<MainMenu>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);
        MainMenu.State state = menu.GetState();
        bool isSpeedrunSelect = state == MainMenu.State.SPEEDRUN_SELECT;
        var text = description.GetComponentInChildren<TextMeshProUGUI>();
        if (state == MainMenu.State.MODE_SELECT || isSpeedrunSelect)
        {
            GameMode.TYPE type = GameMode.getByName(gameObject.name);
            text.text = GameMode.description(type, MainMenu.selectedMap, isSpeedrunSelect);
        }
        else if (state == MainMenu.State.MAP_SELECT)
        {
            Map.TYPE map = Map.getByName(gameObject.name);
            text.text = Map.description(map);
        }
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
