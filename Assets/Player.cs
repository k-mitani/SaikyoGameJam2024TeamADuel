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

    private void Awake()
    {
        SetInactiveAll();
        SetActiveObject(objectNoutou);
    }

    public void OnKamae()
    {
        SetInactiveAll();
        SetActiveObject(objectKamae);
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
