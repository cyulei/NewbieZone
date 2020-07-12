using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public AudioSource Source => mySource;
    AudioSource mySource;
    [Header("会随机播放的音频片段")]
    public AudioClip[] clips;
    [Header("AudioSource音量属性")]
    public float pitchMin = 0.8f;
    public float pitchMax = 1.1f;

    void Awake()
    {
        mySource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放音频片段
    /// </summary>
    /// <param name="clip">片段</param>
    /// <param name="pitchMin"></param>
    /// <param name="pitchMax"></param>
    public void PlayClip(AudioClip clip, float pitchMin, float pitchMax)
    {
        mySource.pitch = Random.Range(pitchMin, pitchMax);
        mySource.PlayOneShot(clip);
    }

    /// <summary>
    /// 获取随机的音频片段
    /// </summary>
    /// <returns></returns>
    public AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    /// <summary>
    /// 播放随机的音频片段
    /// </summary>
    public void PlayRandom()
    {
        if (clips.Length == 0)
            return;

        PlayClip(GetRandomClip(), pitchMin, pitchMax);
    }
}
