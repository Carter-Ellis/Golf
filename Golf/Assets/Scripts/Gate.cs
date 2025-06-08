using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, ButtonTarget
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void onPress()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        anim.SetTrigger("OpenGate");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.gateOpen, transform.position);
    }
}
