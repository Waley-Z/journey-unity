using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Outro : MonoBehaviour
{
    [SerializeField] InstantCamera ic;
    [SerializeField] CanvasGroup theEnd;

    void Start()
    {
        foreach (UIMove move in FindObjectOfType<Background>().moves)
        {
            move.MoveIn();
        }
        StartCoroutine(Utils.CanvasGroupFade(theEnd, 0, 0));

        theEnd.gameObject.SetActive(false);

        StartCoroutine(StartOutro());
    }

    IEnumerator StartOutro()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        yield return StartCoroutine(Utils.CanvasGroupFade(canvasGroup, 2f, 1f));

        ic.StartGame();
    }

    public IEnumerator OutroEnd()
    {
        theEnd.gameObject.SetActive(true);
        theEnd.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(OnTheEndClicked()));
        yield return StartCoroutine(Utils.CanvasGroupFade(theEnd, 2f, 1f));
    }

    IEnumerator OnTheEndClicked()
    {
        yield return StartCoroutine(Utils.CanvasGroupFade(theEnd, 2f, 0f));
        GameManager.Instance.LoadSceneInSeconds(SceneType.Intro, 3f);
        Destroy(gameObject);
    }
}
