using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderNumDisplay : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI volumeTxt;
    [SerializeField] private Slider slider;
    public string sliderType;

    private void Update()
    {

        if (slider == null)
        {
            print("Slider is null.");
            return;
        }
        if (sliderType == "")
        {
            print("Slider type not selected");
            return;
        }
        volumeTxt.text = sliderType + ": " + (int) (slider.value * 100);

    }

}
