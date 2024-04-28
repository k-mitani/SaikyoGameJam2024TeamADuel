using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject labelP1Win;
    [SerializeField] public GameObject labelP2Win;
    [SerializeField] public GameObject labelDraw;
    [SerializeField] public TextMeshProUGUI labelDraw1;
    [SerializeField] public TextMeshProUGUI labelDraw2;
    [SerializeField] public TextMeshProUGUI labelSpeed;
    [SerializeField] public Image curtain;

    public IEnumerator ShowCurtain(float durationMax)
    {
        curtain.gameObject.SetActive(true);
        var duration = 0f;
        while (duration < durationMax)
        {
            duration += Time.deltaTime;
            curtain.color = new Color(0, 0, 0, duration / durationMax);
            yield return null;
        }
    }

    public IEnumerator HideCurtain(float durationMax)
    {
        curtain.gameObject.SetActive(true);
        var duration = 0f;
        while (duration < durationMax)
        {
            duration += Time.deltaTime;
            curtain.color = new Color(0, 0, 0, 1 - duration / durationMax);
            yield return null;
        }
        curtain.gameObject.SetActive(false);
    }

    public void HideWinLabels()
    {
        labelP1Win.SetActive(false);
        labelP2Win.SetActive(false);
        labelDraw.SetActive(false);
    }

    public void ShowP1Win()
    {
        labelP1Win.SetActive(true);
        labelP2Win.SetActive(false);
        labelDraw.SetActive(false);
    }

    public void ShowP2Win()
    {
        labelP1Win.SetActive(false);
        labelP2Win.SetActive(true);
        labelDraw.SetActive(false);
    }

    public void ShowDraw()
    {
        labelP1Win.SetActive(false);
        labelP2Win.SetActive(false);
        labelDraw.SetActive(true);
    }

    public void UpdateSpeed(float? speedSeconds)
    {
        if (!speedSeconds.HasValue)
        {
            labelSpeed.text = "";
            return;
        }
        labelSpeed.text = (speedSeconds.Value * 1000).ToString("0") + "ms";
    }
}
