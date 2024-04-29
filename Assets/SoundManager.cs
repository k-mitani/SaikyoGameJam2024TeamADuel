using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundPlayerInstant playerPrefab;
    [SerializeField] private AudioSource bgmSource;

    [SerializeField] private AudioClip bgmNormal;
    public void PlayBgmNormal() => PlayBgm(bgmNormal, loop: false);
    [SerializeField] private AudioClip bgmRain;
    public void PlayBgmRain() => PlayBgm(bgmRain, true);

    [SerializeField] private AudioClip seShowSuki;
    public void PlaySeShowSuki() => PlaySe(seShowSuki, preCut: 0.08f);

    [SerializeField] private AudioClip seKatana;
    public void PlaySeKatana() => PlaySe(seKatana, preCut: 0.08f);
    [SerializeField] private AudioClip seDraw;
    public void PlaySeDraw() => PlaySe(seDraw, preCut: 0.08f);

    [SerializeField] private AudioClip seNoutou;
    public void PlaySeNoutou() => PlaySe(seNoutou, preCut: 0.08f, dontDestroyOnLoad: true);

    [SerializeField] private AudioClip seBird;
    public void PlaySeBird() => PlaySe(seBird, preCut: 0.08f);

    [SerializeField] private AudioClip seFake;
    public void PlaySeFake() => PlaySe(seFake, preCut: 0.08f);
    [SerializeField] private AudioClip seFake2;
    public void PlaySeFake2() => PlaySe(seFake2, preCut: 0.08f);

    public bool IsPlayingBgm()
    {
        return bgmSource.isPlaying;
    }

    public void PlayBgm(AudioClip clip, bool loop)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
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
