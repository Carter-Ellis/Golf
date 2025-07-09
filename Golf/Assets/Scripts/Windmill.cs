using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Windmill : MonoBehaviour
{
    Transform blades;
    public float rotationsPerMinute = 40f;

    private SoundEffect windSFX = new SoundEffect(FMODEvents.instance.windmill);

    void Start()
    {
        blades = transform.GetChild(0);

        windSFX.play(this);
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
