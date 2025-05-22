using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenTog, vsyncTog;
    [SerializeField] private TMP_Text resLabel;
    [SerializeField] List<ResItem> resolutions = new List<ResItem>();
    private int selectedRes;
    

    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }
        UpdateResLabel();
    }

    void Update()
    {
        
    }

    public void ResLeft()
    {
        selectedRes--;
        if (selectedRes < 0)
        {
            selectedRes = resolutions.Count - 1;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedRes++;
        if (selectedRes >= resolutions.Count)
        {
            selectedRes = 0;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resLabel.text = resolutions[selectedRes].horizontal.ToString() + " x " + resolutions[selectedRes].vertical.ToString();
    }

    public void ApplyGraphics()
    {

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreenTog.isOn);

    }

}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
