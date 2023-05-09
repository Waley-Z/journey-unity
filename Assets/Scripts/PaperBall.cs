using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBall : MonoBehaviour
{
    public Sprite note;
    public Action OnCollision;

    bool ifShown = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        print("on coliision 2d");
        if (!ifShown && collision.gameObject.CompareTag("MazeBall"))
        {
            OnCollision.Invoke();
            ifShown = true;
        }
    }
}
