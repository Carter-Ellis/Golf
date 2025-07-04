using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Windmill : MonoBehaviour
{
    Transform blades;
    public float rotationsPerMinute = 40f;

    private EventInstance windSFX;

    void Start()
    {
        blades = transform.GetChild(0);

        //windSFX = AudioManager.instance.CreateInstance(FMODEvents.instance.windmill);
        windSFX.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        windSFX.start();
    }

    void Update()
    {
        blades.Rotate(0, 0, Time.deltaTime * rotationsPerMinute);
    }

    private void OnDestroy()
    {
        windSFX.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        windSFX.release();
    }
}
