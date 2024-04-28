using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float waitShowSukiDurationMax = 3;
    [SerializeField] private GameObject suki;
    [SerializeField] private SoundManager sounds;
    [SerializeField] private UIManager ui;

    [SerializeField] private Player player1;
    [SerializeField] private Player player2;

    [SerializeField] private bool isInSuki = false;
    [SerializeField] private bool isInGameEnd = false;
    private bool p1Win = false;
    private bool p2Win = false;

    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer bloodEffect;

    [Header("歩き調整")]
    private float preBattleFadeoutDuration = 0.3f;
    private float preBattleWaitAfterFadeout = 0.1f;
    private float preBattleFadeinDuration = 0.3f;
    private float preBattleWaitAfterMove = 0.5f;


    private float sukiAt = 0;
    private float reactAt = 0;

    private void Awake()
    {
        StartCoroutine(ui.HideCurtain(0.5f));
    }

    void Start()
    {

        ui.HideWinLabels();
        ui.UpdateSpeed(null);
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
        sukiAt = Time.time;
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
        ui.ShowP1Win();

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
            player1.objectKiru.color = new Color(1, 1, 1, v);
            player2.objectLoseBody.color = new Color(1, 1, 1, v);
            player2.objectLoseHead.color = new Color(1, 1, 1, v);
            yield return null;
        }
        background.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.5f);

        isInGameEnd = true;
        if (!sounds.IsPlayingBgm())
        {
            sounds.PlayBgmNormal();
        }

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
        StartCoroutine(DryBlood());
        IEnumerator DryBlood()
        {
            //yield return new WaitForSeconds(0.4f);
            var duraMax = 2.5f;
            var dura = 0f;
            while (dura < duraMax)
            {
                dura += Time.deltaTime;
                var v = Mathf.Lerp(0, 1, dura / duraMax);
                bloodEffect.color = new Color(1, v, v, 1);
                yield return null;
            }
            bloodEffect.color = new Color(1, 1, 1, 1);
        }
    }

    private IEnumerator P2Win()
    {
        isInSuki = false;
        suki.SetActive(false);
        sounds.SetBgmVolume(0.1f);
        sounds.PlaySeKatana();

        player2.transform.localPosition = new Vector2(-3.9f, 0);
        player1.transform.localPosition = new Vector2(-1.41f, 0);
        player2.OnWin();
        player1.OnLose();
        ui.ShowP2Win();

        background.color = new Color(0, 0, 0, 1);
        bloodEffect.color = new Color(1, 0, 0, 1);
        bloodEffect.flipX = true;
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
            player2.objectKiru.color = new Color(1, 1, 1, v);
            player1.objectLoseBody.color = new Color(1, 1, 1, v);
            player1.objectLoseHead.color = new Color(1, 1, 1, v);
            yield return null;
        }
        background.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.5f);

        isInGameEnd = true;
        if (!sounds.IsPlayingBgm())
        {
            sounds.PlayBgmNormal();
        }

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
        yield return Util.Fadeout(player1.objectLoseHead, headFade);
        player1.objectLoseHead.transform.localPosition = new Vector2(2.24f, -0.42f);
        player1.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, 61.7f);
        yield return Util.Fadein(player1.objectLoseHead, headFade);

        yield return Util.Fadeout(player1.objectLoseHead, headFade);
        player1.objectLoseHead.transform.localPosition = new Vector2(3.10f, -1.55f);
        player1.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, -149.6f);
        yield return Util.Fadein(player1.objectLoseHead, headFade);

        yield return Util.Fadeout(player1.objectLoseHead, headFade);
        player1.objectLoseHead.transform.localPosition = new Vector2(3.6f, -2.81f);
        player1.objectLoseHead.transform.localEulerAngles = new Vector3(0, 0, -210.5f);
        yield return Util.Fadein(player1.objectLoseHead, headFade);
        StartCoroutine(DryBlood());
        IEnumerator DryBlood()
        {
            //yield return new WaitForSeconds(0.4f);
            var duraMax = 2.5f;
            var dura = 0f;
            while (dura < duraMax)
            {
                dura += Time.deltaTime;
                var v = Mathf.Lerp(0, 1, dura / duraMax);
                bloodEffect.color = new Color(1, v, v, 1);
                yield return null;
            }
            bloodEffect.color = new Color(1, 1, 1, 1);
        }
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
                p1Win = true;
                reactAt = Time.time;
                ui.UpdateSpeed(reactAt - sukiAt);
                StartCoroutine(P1Win());
            }
            // p2の勝ち
            else if (!p1pushed && p2pushed)
            {
                p2Win = true;
                reactAt = Time.time;
                ui.UpdateSpeed(reactAt - sukiAt);
                StartCoroutine(P2Win());
            }
            // 同着、仕切り直し
            else if (p1pushed && p2pushed)
            {
                reactAt = Time.time;
                ui.UpdateSpeed(reactAt - sukiAt);
                isInSuki = false;
                player1.FadeinKamae(0.3f);
                player2.FadeinKamae(0.3f);
                StartCoroutine(ShowSuki());
            }
        }
        if (isInGameEnd)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isInGameEnd = false;
                StartCoroutine(Retry());
                IEnumerator Retry()
                {
                    sounds.PlaySeNoutou();
                    yield return ui.ShowCurtain(0.2f);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                return;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                isInGameEnd = false;
                StartCoroutine(Exit());
                IEnumerator Exit()
                {
                    yield return ui.ShowCurtain(0.1f);
                    Application.Quit();
                }
                return;
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