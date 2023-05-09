using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeExit : MonoBehaviour
{
    [SerializeField] LevelTwo levelTwo;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MazeBall"))
        {
            StartCoroutine(levelTwo.EndLevel());
        }
    }
}
