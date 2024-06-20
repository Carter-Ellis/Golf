using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClips;
    public float maxMusicVolume = .05f;
    void Start()
    {
        MusicManager.instance.PlayMusicClip(musicClips[5], transform, maxMusicVolume);
    }

}
