using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outro : MonoBehaviour
{
    [SerializeField] InstantCamera ic;

    void Start()
    {
        foreach (UIMove move in FindObjectOfType<Background>().moves)
        {
            move.MoveIn();
        }

        StartCoroutine(StartOutro());
    }

    IEnumerator StartOutro()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        float currentTime = 0;

        while (currentTime < 2f)
        {
            currentTime += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, currentTime / 2f);
            yield return null;
        }

        ic.StartGame();
    }

    public IEnumerator OutroEnd()
    {
        yield return null;
    }
}
