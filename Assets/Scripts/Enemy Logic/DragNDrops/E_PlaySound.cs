using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class E_PlaySound
{
    public string name;
    public AudioSource audioSource;
    public AudioClip clip;

    public bool loop;

    public void PlaySound()
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
