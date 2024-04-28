using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float waitShowSukiDurationMax = 3;
    [SerializeField] private GameObject suki;
    [SerializeField] private SoundManager sounds;

    void Start()
    {
        suki.SetActive(false);
        StartCoroutine(ShowSuki());
    }

    private IEnumerator ShowSuki()
    {
        var wait = Random.value * waitShowSukiDurationMax;
        yield return new WaitForSeconds(wait);
        suki.SetActive(true);
        sounds.PlaySeShowSuki();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
