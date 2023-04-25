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
        for (int k = 0; k < levelButtons.Length; k++)
        {
            if (k != i)
            {
                levelButtons[k].GetComponent<UIMove>().MoveOut();
            }
        }
        StartCoroutine(Utils.ImageFade(levelTexts[i], 2f, 1f));
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
