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

    void Start()
    {


        suki.SetActive(false);
        StartCoroutine(ShowSuki());
    }

    private void DoBoth(System.Action<Player> action)
    {
        action(player1);
        action(player2);
    }

    private IEnumerator ShowSuki()
    {
        yield return new WaitForSeconds(1);
        DoBoth(p => p.OnKamae());


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
                player1.OnKamae();
                player2.OnKamae();
                StartCoroutine(ShowSuki());
            }
        }
    }
}
