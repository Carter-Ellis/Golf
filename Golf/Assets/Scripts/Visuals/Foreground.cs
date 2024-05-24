using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foreground : MonoBehaviour
{
    [Header("Sounds")]
    public float maxSFXVolume = .5f;
    [SerializeField] private AudioClip[] wallHitClips;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(wallHitClips, transform, maxSFXVolume);
    }
}
