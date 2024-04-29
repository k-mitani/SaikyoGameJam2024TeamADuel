using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public bool moveToRight = true;
    public float speed = 1;

    public void Initialize()
    {
        moveToRight = Random.value < 0.5f;
        if (moveToRight)
        {
            var pos = transform.position;
            pos.x = -11;
            transform.position = pos;
            TryGetComponent<SpriteRenderer>(out var r);
            r.flipX = true;
        }
        else
        {
            var pos = transform.position;
            pos.x = 11;
            transform.position = pos;
        }
    }

    void FixedUpdate()
    {
        if (moveToRight)
        {
            transform.position += speed * Time.deltaTime * Vector3.right;
        }
        else
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }
    }
}
