using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundPlayerInstant playerPrefab;
    [SerializeField] private AudioSource bgmSource;

    [SerializeField] private AudioClip bgmNormal;
    public void PlayBgmNormal() => PlayBgm(bgmNormal);


    [SerializeField] private AudioClip seShowSuki;
    public void PlaySeShowSuki() => PlaySe(seShowSuki, preCut: 0.08f);

    [SerializeField] private AudioClip seKatana;
    public void PlaySeKatana() => PlaySe(seKatana, preCut: 0.08f);

    [SerializeField] private AudioClip seNoutou;
    public void PlaySeNoutou() => PlaySe(seNoutou, preCut: 0.08f, dontDestroyOnLoad: true);


    public void PlayBgm(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    private void PlaySe(
        AudioClip clip,
        float? volume = null,
        bool dontDestroyOnLoad = false,
        float? preCut = null)
    {
        var player = Instantiate(playerPrefab);
        if (dontDestroyOnLoad) DontDestroyOnLoad(player);
        player.Play(clip, false, true, preCut);
        if (volume != null)
        {
            player.source.volume = volume.Value;
        }
    }

    public void StopBgm()
    {
        bgmSource.Stop();
    }

    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
    }
}
