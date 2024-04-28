using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerInstant : MonoBehaviour
{
    [SerializeField] public AudioSource source;

    private bool destroyOnEnd;

    public void Play(AudioClip clip, bool loop, bool destroyOnEnd, float? preCut)
    {
        source.loop = loop;
        source.clip = clip;
        this.destroyOnEnd = destroyOnEnd;
        if (preCut != null)
        {
            source.time = preCut.Value;
        }
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyOnEnd && !source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}