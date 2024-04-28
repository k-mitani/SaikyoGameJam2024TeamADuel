using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textSuki;
    private int isShowingSuki = 0;

    [SerializeField] private Rigidbody2D head1p;
    [SerializeField] private Rigidbody2D head2p;
    [SerializeField] private Rigidbody2D body1p;
    [SerializeField] private Rigidbody2D body2p;

    [SerializeField] private float HeadUpForce = 10;
    [SerializeField] private float HeadUpWaitSec = 1;

    [SerializeField] private 


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoSomething());
        textSuki.gameObject.SetActive(false);
    }
    IEnumerator DoSomething()
    {
        yield return new WaitForSeconds(Random.value*5);
        transform.position = new Vector2(0, 0);
        textSuki.gameObject.SetActive(true);
        isShowingSuki = 1;
    }
    // Update is called once per frame
    void Update()
    {
        var p = transform.position;
        p.x += 0.1f;
        transform.position = p;
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector2(0, 0);
            if (isShowingSuki == 1)
            {
                textSuki.text = "1P WIN";
                isShowingSuki = 0;
                StartCoroutine(HeadUp(head2p, body2p));
            }
        }

        // 2P—p
        if (Input.GetKey(KeyCode.L))
        {
            if (isShowingSuki == 1)
            {
                textSuki.text = "2P WIN";
                isShowingSuki = 0;
                StartCoroutine(HeadUp(head1p, body1p));
            }

        }
    }

    IEnumerator HeadUp(Rigidbody2D head, Rigidbody2D body)
    {
        yield return new WaitForSeconds(HeadUpWaitSec);
        head.simulated = true;
        head.AddForce(Vector2.up * HeadUpForce, ForceMode2D.Impulse);

        body.simulated = true;

    }

}
