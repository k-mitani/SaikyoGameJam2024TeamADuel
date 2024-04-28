using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float waitShowSukiDurationMax = 3;
    [SerializeField] private GameObject suki;
    [SerializeField] private SoundManager sounds;

    [SerializeField] private Player player1;
    [SerializeField] private Player player2;

    [SerializeField] private bool isInSuki = false;

    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer bloodEffect;

    [Header("歩き調整")]
    private float preBattleFadeoutDuration = 0.3f;
    private float preBattleWaitAfterFadeout = 0.1f;
    private float preBattleFadeinDuration = 0.3f;
    private float preBattleWaitAfterMove = 0.5f;


    void Start()
    {
        sounds.PlayBgmNormal();

        suki.SetActive(false);
        //StartCoroutine(PreBattle());

        player1.transform.localPosition = new Vector2(-3.5f, 0);
        player2.transform.localPosition = new Vector2(3.5f, 0);
        StartCoroutine(ShowSuki());
    }

    private IEnumerator MoveTo(Player player, Vector2 pos)
    {
        yield return player.MoveTo(
            pos,
            preBattleFadeoutDuration,
            preBattleWaitAfterFadeout,
            preBattleFadeinDuration,
            preBattleWaitAfterMove);
    }

    /// <summary>
    /// 戦闘前の移動シーン
    /// </summary>
    /// <returns></returns>
    private IEnumerator PreBattle()
    {
        var p1End = false;
        IEnumerator DoP1()
        {
            player1.transform.localPosition = new Vector2(-25, 0);
            yield return MoveTo(player1, new Vector2(-18.8f, 0));
            yield return MoveTo(player1, new Vector2(-13.8f, 0));
            yield return MoveTo(player1, new Vector2(-8.8f, 0));
            yield return MoveTo(player1, new Vector2(-3.5f, 0));
            p1End = true;
        }
        StartCoroutine(DoP1());

        var p2End = false;
        IEnumerator DoP2()
        {
            player2.transform.localPosition = new Vector2(25, 0);
            //yield return MoveTo(player2, new Vector2(18.8f, 0));
            //yield return MoveTo(player2, new Vector2(13.8f, 0));
            //yield return MoveTo(player2, new Vector2(8.8f, 0));
            yield return MoveTo(player2, new Vector2(14.8f, 0));
            yield return MoveTo(player2, new Vector2(10.8f, 0));
            yield return MoveTo(player2, new Vector2(6.8f, 0));
            yield return MoveTo(player2, new Vector2(3.5f, 0));
            p2End = true;
        }
        StartCoroutine(DoP2());
        while (!p1End || !p2End)
        {
            yield return null;
        }

        yield return new WaitForSeconds(Random.value * 2);
        yield return DoBoth(p => p.FadeoutNoutou(0.3f));
        yield return new WaitForSeconds(0.1f);
        yield return DoBoth(p => p.FadeinKamae(0.2f));
        yield return ShowSuki();
    }


    private IEnumerator ShowSuki()
    {
        //var wait = Random.value * waitShowSukiDurationMax;
        var wait = 0.3f;
        yield return new WaitForSeconds(wait);
        isInSuki = true;
        suki.SetActive(true);
        sounds.PlaySeShowSuki();
    }

    private IEnumerator P1Win()
    {
        isInSuki = false;
        suki.SetActive(false);
        sounds.SetBgmVolume(0.1f);
        sounds.PlaySeKatana();

        player1.transform.localPosition = new Vector2(3.7f, 0);
        player2.transform.localPosition = new Vector2(1.41f, 0);
        player1.OnWin();
        player2.OnLose();

        background.color = new Color(0, 0, 0, 1);
        bloodEffect.color = new Color(1, 0, 0, 1);
        //yield return new WaitForSeconds(0.2f);
        bloodEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        var duraMax = 0.3f;
        var dura = 0f;
        while (dura < duraMax)
        {
            dura += Time.deltaTime;
            var v = Mathf.Lerp(0, 1, dura / duraMax);
            background.color = new Color(v, v, v, 1);
            bloodEffect.color = new Color(1, v, v, 1);
            player1.objectKiru.color = new Color(1, 1, 1, v);
            player2.objectLoseBody.color = new Color(1, 1, 1, v);
            player2.objectLoseHead.color = new Color(1, 1, 1, v);
            yield return null;
        }
        background.color = new Color(1, 1, 1, 1);
        bloodEffect.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RestoreBgm());
        IEnumerator RestoreBgm()
        {
            yield return new WaitForSeconds(0.5f);
            var d = 0f;
            var dmax = 0.5f;
            while (d < dmax)
            {
                d += Time.deltaTime;
                var v = Mathf.Lerp(0.1f, 1, d / dmax);
                sounds.SetBgmVolume(v);
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.75f);

        var headFade = 0.2f;
        yield return Util.Fadeout(player2.objectLoseHead, headFade);
        player2.objectLoseHead.transform.localPosition = new Vector2(-2.44f, -0.62f);
        player2.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, 61.7f);
        yield return Util.Fadein(player2.objectLoseHead, headFade);
        
        yield return Util.Fadeout(player2.objectLoseHead, headFade);
        player2.objectLoseHead.transform.localPosition = new Vector2(-2.85f, -2.02f);
        player2.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, 119.6f);
        yield return Util.Fadein(player2.objectLoseHead, headFade);

        yield return Util.Fadeout(player2.objectLoseHead, headFade);
        player2.objectLoseHead.transform.localPosition = new Vector2(-3.67f, -2.55f);
        player2.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, 161.8f);
        yield return Util.Fadein(player2.objectLoseHead, headFade);
        //yield return new WaitForSeconds(0.2f);
        //yield return Util.Fadeout(player2.objectLoseHead, headFade);
        //player2.objectLoseHead.transform.localPosition = new Vector2(-3.79f, -2.55f);
        //player2.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, -161.8f);
        //yield return Util.Fadein(player2.objectLoseHead, headFade * 2);
        //yield return new WaitForSeconds(0.4f);
        //yield return Util.Fadeout(player2.objectLoseHead, headFade * 1);
        //player2.objectLoseHead.transform.localPosition = new Vector2(-3.61f, -2.55f);
        //player2.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, 161.8f);
        //yield return Util.Fadein(player2.objectLoseHead, headFade * 2);

        //yield return Util.Fadeout(player2.objectLoseHead, headFade * 2);
        //player2.objectLoseHead.transform.localPosition = new Vector2(-3.79f, -2.55f);
        //player2.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, -161.8f);
        //yield return Util.Fadein(player2.objectLoseHead, headFade * 2);
    }

    private IEnumerator P2Win()
    {
        isInSuki = false;
        player1.OnWin();
        player2.OnLose();
        yield return Util.Fadein(background, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInSuki)
        {
            var p1pushed = Input.GetKey(KeyCode.A);
            var p2pushed = Input.GetKey(KeyCode.L);
            // p1の勝ち
            if (p1pushed && !p2pushed)
            {
                StartCoroutine(P1Win());
            }
            // p2の勝ち
            else if (!p1pushed && p2pushed)
            {
                StartCoroutine(P2Win());
            }
            // 同着、仕切り直し
            else if (p1pushed && p2pushed)
            {
                isInSuki = false;
                player1.FadeinKamae(0.3f);
                player2.FadeinKamae(0.3f);
                StartCoroutine(ShowSuki());
            }
        }
    }

    //private void DoBoth(System.Action<Player> action)
    //{
    //    action(player1);
    //    action(player2);
    //}

    private IEnumerator DoBoth(System.Func<Player, IEnumerator> action)
    {
        var p1End = false;
        IEnumerator DoP1()
        {
            yield return action(player1);
            p1End = true;
        }
        StartCoroutine(DoP1());

        var p2End = false;
        IEnumerator DoP2()
        {
            yield return action(player2);
            p2End = true;
        }
        StartCoroutine(DoP2());
        while (!p1End || !p2End)
        {
            yield return null;
        }
    }
}

public static class Util
{
    public static IEnumerator Fadein(SpriteRenderer obj, float duration)
    {
        obj.color = new Color(1, 1, 1, 0);
        obj.gameObject.SetActive(true);
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

    public static IEnumerator Fadeout(SpriteRenderer obj, float duration)
    {
        var start = Time.time;
        var end = start + duration;
        while (Time.time < end)
        {
            var rate = (Time.time - start) / duration;
            obj.color = new Color(1, 1, 1, 1 - rate);
            yield return null;
        }
        obj.gameObject.SetActive(false);
        obj.color = new Color(1, 1, 1, 1);
    }
}