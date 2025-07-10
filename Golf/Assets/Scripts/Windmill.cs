using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Windmill : MonoBehaviour
{
    Transform blades;
    public float rotationsPerMinute = 40f;

    private SoundEffect windSFX;

    void Start()
    {
        windSFX = new SoundEffect(FMODEvents.instance.windmill);
        windSFX.play(this);
        blades = transform.GetChild(0);
    }

    void Update()
    {
        blades.Rotate(0, 0, Time.deltaTime * rotationsPerMinute);
    }

    private void OnDestroy()
    {
        windSFX.stop();
    }
}
