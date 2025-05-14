using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour
{
    private bool isItemSelected = false;
    private UnityEngine.UI.Button button;

    void Awake()
    {
        button = GetComponent<UnityEngine.UI.Button>();
    }

    void Start()
    {
        SetInteractable(isItemSelected);
    }

    public void SetItemSelected(bool value)
    {
        isItemSelected = value;
        SetInteractable(value);
    }

    private void SetInteractable(bool value)
    {
        button.interactable = value;
    }

}
