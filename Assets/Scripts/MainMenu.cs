using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] levelButtons = new GameObject[3];
    [SerializeField] Image[] levelTexts = new Image[3];
    [SerializeField] List<UIMove> objectsToMove = new();
    [SerializeField] Ghost ghost;
    [SerializeField] LevelOne levelOne;

    void Awake()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int _i = i;
            levelButtons[i].GetComponent<Button>().onClick.AddListener(() => OnLevelButtonPressed(_i));
        }
    }

    void OnLevelButtonPressed(int i)
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
        StartCoroutine(Utils.ImageFade(ghost.image, 1f, 0f));
        yield return new WaitForSeconds(3f);

        switch (i)
        {
            case 0:
                levelOne.gameObject.SetActive(true);
                levelOne.Init();
                break;
            case 1:
                //UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
                break;
            case 2:
                //UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
                break;
        }
    }

    public void Init()
    {
        foreach (UIMove move in objectsToMove)
        {
            move.MoveIn();
        }
        foreach (GameObject btn in levelButtons)
        {
            btn.GetComponent<UIMove>().MoveIn();
        }
    }
}
