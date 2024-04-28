using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Sprite spriteNoutou;
    [SerializeField] private Sprite spriteKamae;
    [SerializeField] private Sprite spriteKiru;
    [SerializeField] private Sprite spriteLoseBody;

    [SerializeField] private Rigidbody2D loseHead;

    private new SpriteRenderer renderer;

    private void Awake()
    {
        TryGetComponent(out renderer);
        renderer.sprite = spriteNoutou;
        loseHead.gameObject.SetActive(false);
    }

    public void OnKamae()
    {
        renderer.sprite = spriteKamae;
    }

    public void OnWin()
    {
        renderer.sprite = spriteKiru;
    }

    public void OnLose()
    {
        renderer.sprite = spriteLoseBody;
        loseHead.gameObject.SetActive(true);
    }

}
