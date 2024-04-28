using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundPlayerInstant playerPrefab;

    [SerializeField] private AudioClip seShowSuki;
    public void PlaySeShowSuki() => PlaySe(seShowSuki, preCut: 0.08f);

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

}
