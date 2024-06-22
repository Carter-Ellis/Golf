using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //Spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //Assign the audioClip
        audioSource.clip = audioClip;

        //Assign volume
        audioSource.volume = volume;

        //Play sound
        audioSource.Play();

        //Get length of sound FX clip
        float clipLength = audioSource.clip.length;

        //Destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFXSourceClip(AudioSource source, AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        if (source.isPlaying)
        {
            return;
        }
        int rand = Random.Range(0, audioClip.Length);
        //Assign the audioClip
        source.clip = audioClip[0];

        //Assign volume
        source.volume = volume;

        //Play sound
        source.Play();
        
    }

    public void StopSoundFXClip(AudioSource source)
    {
        //Stop sound
        source.Stop();
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {

        //Assign a random index
        int rand = Random.Range(0, audioClip.Length);
        //Spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //Assign the audioClip
        audioSource.clip = audioClip[rand];

        //Assign volume
        audioSource.volume = volume;

        //Play sound
        audioSource.PlayOneShot(audioClip[rand]);

        //Get length of sound FX clip
        float clipLength = audioSource.clip.length;
        

        //Destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
        
    }

}
