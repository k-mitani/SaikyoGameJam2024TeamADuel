using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer objectNoutou;
    [SerializeField] private SpriteRenderer objectKamae;
    [SerializeField] private SpriteRenderer objectKiru;
    [SerializeField] private SpriteRenderer objectLoseBody;
    [SerializeField] private Rigidbody2D objectLoseHead;

    public IEnumerator MoveTo(
        Vector2 localPosition,
        float fadeoutDuration,
        float waitAfterFadeoutDuration,
        float fadeinDuration,
        float waitAfterMove
    )
    {
        yield return FadeoutNoutou(fadeoutDuration);
        yield return new WaitForSeconds(waitAfterFadeoutDuration);
        transform.localPosition = localPosition;
        yield return FadeinNoutou(fadeinDuration);
        yield return new WaitForSeconds(waitAfterMove);
    }

    public IEnumerator FadeoutNoutou(float duration)
    {
        var start = Time.time;
        var end = start + duration;
        while (Time.time < end)
        {
            var rate = (Time.time - start) / duration;
            objectNoutou.color = new Color(1, 1, 1, 1 - rate);
            yield return null;
        }
        objectNoutou.gameObject.SetActive(false);
        objectNoutou.color = new Color(1, 1, 1, 1);
    }
    public IEnumerator FadeinNoutou(float duration)
    {
        yield return Fadein(objectNoutou, duration);
    }

    public IEnumerator FadeinKamae(float duration)
    {
        yield return Fadein(objectKamae, duration);
    }


    private IEnumerator Fadein(SpriteRenderer obj, float duration)
    {
        obj.color = new Color(1, 1, 1, 0);
        SetActiveObject(obj);
        var start = Time.time;
        var end = start + duration;
        while (Time.time < end)
        {
            var rate = (Time.time - start) / duration;
            obj.color = new Color(1, 1, 1, rate);
            yield return null;
        }
        obj.color = new Color(1, 1, 1, 1);
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

    private void SetInactiveAll()
    {
        objectNoutou.gameObject.SetActive(false);
        objectKamae.gameObject.SetActive(false);
        objectKiru.gameObject.SetActive(false);
        objectLoseBody.gameObject.SetActive(false);

    }

}
