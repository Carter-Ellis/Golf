using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayControllerButton : MonoBehaviour
{
    [SerializeField] private GameObject buttonsObj;

    private void Update()
    {
        buttonsObj.SetActive(PlayerInput.isController);
    }
}
