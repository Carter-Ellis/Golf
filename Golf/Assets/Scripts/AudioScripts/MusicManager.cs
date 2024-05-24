using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioSource musicObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayMusicClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //Spawn in gameObject
        AudioSource audioSource = Instantiate(musicObject, spawnTransform.position, Quaternion.identity);

        //Assign the audioClip
        audioSource.clip = audioClip;

        //Assign volume
        audioSource.volume = volume;

        //Play sound
        audioSource.Play();

        //Get length of sound FX clip
        float clipLength = audioSource.clip.length;

    }

    public void PlayRandomMusicClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {

        //Assign a random index
        int rand = Random.Range(0, audioClip.Length);

        //Spawn in gameObject
        AudioSource audioSource = Instantiate(musicObject, spawnTransform.position, Quaternion.identity);

        //Assign the audioClip
        audioSource.clip = audioClip[rand];

        //Assign volume
        audioSource.volume = volume;

        //Play sound
        audioSource.Play();

    }
}
