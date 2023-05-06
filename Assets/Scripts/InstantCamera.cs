using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantCamera : MonoBehaviour
{
    [SerializeField] ICScene[] iCScenes = new ICScene[3];
    [SerializeField] String startText;
    [SerializeField] GameObject cameraButton;
    [SerializeField] Image flash, mainImage, itemLarge;
    [SerializeField] TypeWritter typeWritter;
    [SerializeField] List<UIMove> objectsToMove = new();

    public Image ghost;

    int sceneIdx = -1;
    ICScene CurrentScene
    {
        get
        {
            if (sceneIdx >= 0 && sceneIdx < iCScenes.Length)
                return iCScenes[sceneIdx];
            return null;
        }
    }

    void Awake()
    {
        mainImage.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(IntroEnd());
        }
    }

    public void StartGame()
    {
        typeWritter.StartTypeWrite(startText);
        cameraButton.GetComponent<ButtonWiggle>().WiggleInSeconds(8f);
        cameraButton.GetComponent<Button>().onClick.AddListener(OnCameraButtonPressed);
    }

    void OnCameraButtonPressed()
    {
        // end of intro
        if (sceneIdx + 1 == iCScenes.Length)
        {
            Debug.Log("intro ends");
            cameraButton.GetComponent<Button>().onClick.RemoveAllListeners();
            StartCoroutine(IntroEnd());
            return;
        }

        if (CurrentScene != null && !CurrentScene.ScenePassed)
            return;

        if (CurrentScene != null)
        {
            // clear previous scene items
            foreach (var item in CurrentScene.items)
            {
                item.ItemGameObject.SetActive(false);
            }
            itemLarge.enabled = false;
        }
        // clear text
        typeWritter.Clear();

        StartCoroutine(NewScene());
    }

    IEnumerator IntroEnd()
    {
        yield return Utils.ImageFade(ghost, 2f, 1f);
        foreach (var obj in objectsToMove)
            obj.MoveOut();
        ghost.GetComponent<Ghost>().MoveToSide();

        GameManager.Instance.LoadSceneInSeconds(SceneType.MainMenu, 3f);
    }

    IEnumerator NewScene()
    {
        sceneIdx++;
        yield return Utils.ImageFade(flash, 0.1f, 1);
        yield return new WaitForSeconds(0.5f);

        mainImage.enabled = true;
        mainImage.sprite = CurrentScene.ICImage;
        foreach (var item in CurrentScene.items)
        {
            item.ItemGameObject.SetActive(true);
            Button button = item.ItemGameObject.GetComponent<Button>();
            button.onClick.AddListener(() => OnObjectClicked(item.ItemGameObject));
            ButtonWiggle wiggle = item.ItemGameObject.GetComponent<ButtonWiggle>();
            wiggle.WiggleInSeconds(10f);
        }

        typeWritter.StartTypeWrite(CurrentScene.Text);
        yield return Utils.ImageFade(flash, 1f, 0);
    }

    void OnObjectClicked(GameObject gameObject)
    {
        foreach (var item in CurrentScene.items)
        {
            if (gameObject == item.ItemGameObject)
            {
                itemLarge.enabled = true;
                itemLarge.sprite = item.ItemDetailSprite;
                item.Pressed = true;
                if (CurrentScene.ScenePassed && sceneIdx + 1 != iCScenes.Length)
                {
                    Debug.Log("wiggle camera button in 5 seconds");
                    cameraButton.GetComponent<ButtonWiggle>().WiggleInSeconds(8f);
                }
                return;
            }
        }
    }
}

[System.Serializable]
public class ICScene
{
    public Sprite ICImage;
    [TextArea]
    public string Text;
    public List<ICItem> items = new();

    public bool ScenePassed
    {
        get
        {
            foreach (var item in items)
            {
                if (!item.Pressed)
                    return false;
            }
            return true;
        }
    }
}

[System.Serializable]
public class ICItem
{
    public GameObject ItemGameObject;
    public Sprite ItemDetailSprite;
    [System.NonSerialized]
    public bool Pressed = false;
}
