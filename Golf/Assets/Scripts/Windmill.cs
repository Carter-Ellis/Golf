using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    Transform blades;
    public float rotationsPerMinute = 40f;
    void Start()
    {
        blades = transform.GetChild(0);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.windmill, transform.position);
    }

    void Update()
    {       
        blades.Rotate(0,0, Time.deltaTime * rotationsPerMinute);
    }
}
