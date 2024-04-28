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

    [Header("歩き調整")]
    private float preBattleFadeoutDuration = 0.3f;
    private float preBattleWaitAfterFadeout = 0.1f;
    private float preBattleFadeinDuration = 0.3f;
    private float preBattleWaitAfterMove = 0.5f;


    void Start()
    {
        sounds.PlayBgmNormal();

        suki.SetActive(false);
        StartCoroutine(PreBattle());
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


        var wait = Random.value * waitShowSukiDurationMax;
        yield return new WaitForSeconds(wait);
        isInSuki = true;
        suki.SetActive(true);
        sounds.PlaySeShowSuki();
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
                isInSuki = false;
                player1.OnWin();
                player2.OnLose();
            }
            // p2の勝ち
            else if (!p1pushed && p2pushed)
            {
                isInSuki = false;
                player1.OnLose();
                player2.OnWin();
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
