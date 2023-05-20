using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] levelButtons = new GameObject[3];
    [SerializeField] Image[] levelTexts = new Image[3];
    [SerializeField] List<UIMove> objectsToMove = new();

    Image ghost
    {
        get
        {
            InstantCamera ic = FindObjectOfType<InstantCamera>();
            return ic ? ic.ghost : null;
        }
    }

    void Awake()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int _i = i;
            levelButtons[i].GetComponent<Button>().onClick.AddListener(() => OnLevelButtonPressed(_i));
        }
    }

    void Start()
    {
        StartCoroutine(SoundManager.Instance.CrossFadeBGM(SoundManager.Sound.MusicStartMainMenu, 2f));

        foreach (UIMove move in objectsToMove)
        {
            move.MoveIn();
        }
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
            levelButtons[i].GetComponent<UIMove>().MoveIn();
            if (i > GameManager.Instance.Progress - 1)
            {
                levelButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    void OnLevelButtonPressed(int i) // 0, 1, 2
    {
        foreach (UIMove move in objectsToMove)
        {
            move.MoveOut();
        }
        StartCoroutine(LevelIntro(i));
    }

    IEnumerator LevelIntro(int i)
    {
        for (int k = 0; k < levelButtons.Length; k++)
        {
            if (k != i)
            {
                levelButtons[k].GetComponent<UIMove>().MoveOut();
            }
        }
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(Utils.ImageFade(levelTexts[i], 2f, 1f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(Utils.ImageFade(levelTexts[i], 1f, 0f));
        StartCoroutine(Utils.ImageFade(levelButtons[i].GetComponent<Image>(), 1f, 0f));
        StartCoroutine(Utils.ImageFade(ghost, 1f, 0f));

        GameManager.Instance.LoadSceneInSeconds((SceneType)(i + 2), 3f);

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        InstantCamera ic = FindObjectOfType<InstantCamera>();
        if (ic)
        {
            Destroy(ic.transform.parent.gameObject);
        }
    }
}
