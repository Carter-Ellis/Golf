using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject title;
    private Canvas parentCanvas;
    private void Start()
    {
        parentCanvas = mainMenu.GetComponentInParent<Canvas>();
    }
    private void Update()
    {
        
        if (!mainMenu.activeSelf || !parentCanvas.enabled)
        {
            title.SetActive(false);
            return;
        }
        else
        {
            title.SetActive(true);
        }
    }
}
