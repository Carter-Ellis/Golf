using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClips;
    public float maxMusicVolume = .05f;
    void Awake()
    {
        MusicManager.instance.PlayRandomMusicClip(musicClips, transform, maxMusicVolume);
    }

}
