using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SoundManager.Instance.CrossFadeBGM(SoundManager.Sound.MusicStartMainMenu, 1f));

        foreach (UIMove move in FindObjectOfType<Background>().moves)
        {
            move.MoveIn();
        }

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        StartCoroutine(Utils.CanvasGroupFade(canvasGroup, 2f, 1f));
    }
}
