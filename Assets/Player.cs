using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public SpriteRenderer objectNoutou;
    [SerializeField] public SpriteRenderer objectKamae;
    [SerializeField] public SpriteRenderer objectKiru;
    [SerializeField] public SpriteRenderer objectLoseBody;
    [SerializeField] public SpriteRenderer objectLoseHead;
    [SerializeField] public Canvas canvasChonbo;

    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material chonboMaterial;

    public IEnumerator MoveTo(
        Vector2 localPosition,
        float fadeoutDuration,
        float waitAfterFadeoutDuration,
        float fadeinDuration,
        float waitAfterMove
    )
    {
        SetInactiveAll();
        objectNoutou.gameObject.SetActive(true);
        yield return FadeoutNoutou(fadeoutDuration);
        yield return new WaitForSeconds(waitAfterFadeoutDuration);
        transform.localPosition = localPosition;
        yield return FadeinNoutou(fadeinDuration);
        yield return new WaitForSeconds(waitAfterMove);
    }

    public IEnumerator FadeoutNoutou(float duration)
    {
        yield return Util.Fadeout(objectNoutou, duration);
    }
    public IEnumerator FadeinNoutou(float duration)
    {
        yield return Util.Fadein(objectNoutou, duration);
    }

    public IEnumerator FadeinKamae(float duration)
    {
        yield return Util.Fadein(objectKamae, duration);
    }


    private void Awake()
    {
        SetInactiveAll();
        SetActiveObject(objectNoutou);
    }

    public void OnWin()
    {
        SetInactiveAll();
        SetActiveObject(objectKiru);
    }

    public void OnLose()
    {
        SetInactiveAll();
        objectLoseBody.gameObject.SetActive(true);
        objectLoseHead.gameObject.SetActive(true);
    }

    private void SetActiveObject(SpriteRenderer obj)
    {
        obj.gameObject.SetActive(true);
    }

    public void SetInactiveAll()
    {
        objectNoutou.gameObject.SetActive(false);
        objectKamae.gameObject.SetActive(false);
        objectKiru.gameObject.SetActive(false);
        objectLoseBody.gameObject.SetActive(false);

    }

    public void SetChonbo()
    {
        objectNoutou.material = chonboMaterial;
        objectKamae.material = chonboMaterial;
        objectKiru.material = chonboMaterial;
        objectLoseBody.material = chonboMaterial;
        objectLoseHead.material = chonboMaterial;
        canvasChonbo.gameObject.SetActive(true);
    }

    public void ResetChonbo()
    {
        objectNoutou.material = normalMaterial;
        objectKamae.material = normalMaterial;
        objectKiru.material = normalMaterial;
        objectLoseBody.material = normalMaterial;
        objectLoseHead.material = normalMaterial;
        canvasChonbo.gameObject.SetActive(false);
    }
}
