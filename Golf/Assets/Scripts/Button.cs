using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private SpriteRenderer sr;
    public GameObject target;
    private ButtonTarget buttonTarget;
    public Sprite pushedSprite;
    public Sprite unpushedSprite;
    public AudioClip pushClip;
    public AudioClip unpushClip;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (target != null)
        {
            Component[] components = target.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component is ButtonTarget)
                {
                    buttonTarget = (ButtonTarget)component;
                    break;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        sr.sprite = pushedSprite;
        SoundFXManager.instance.PlaySoundFXClip(pushClip, transform, FindObjectOfType<Ball>().maxSFXVolume);
        if (buttonTarget == null)
        {
            return;
        }
        buttonTarget.onPress();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SoundFXManager.instance.PlaySoundFXClip(unpushClip, transform, FindObjectOfType<Ball>().maxSFXVolume);
        sr.sprite = unpushedSprite;
    }
}
