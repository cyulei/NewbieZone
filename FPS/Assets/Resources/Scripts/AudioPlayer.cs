using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public AudioSource Source => mySource;

    AudioSource mySource;

    public AudioClip[] clips;
    public float pitchMin = 0.8f;
    public float pitchMax = 1.1f;

    void Awake()
    {
        mySource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip, float pitchMin, float pitchMax)
    {
        mySource.pitch = Random.Range(pitchMin, pitchMax);
        mySource.PlayOneShot(clip);
    }


    public AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    public void PlayRandom()
    {
        if (clips.Length == 0)
            return;

        PlayClip(GetRandomClip(), pitchMin, pitchMax);
    }
}
