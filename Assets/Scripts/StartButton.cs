using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjectsToFade = new();
    [SerializeField] Image cameraButton;
    [SerializeField] InstantCamera ic;
    [SerializeField] UIMove note;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    public void OnClicked()
    {
        foreach (GameObject go in gameObjectsToFade)
        {
            StartCoroutine(Utils.UIFade(go, 1f, 0f));
        }
        StartCoroutine(Utils.ImageFade(cameraButton, 1f, 1f));
        StartCoroutine(StartGame());
        GetComponent<Button>().onClick.RemoveListener(OnClicked);
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        note.MoveIn();
        yield return new WaitForSeconds(1.5f);
        ic.StartGame();
    }
}
