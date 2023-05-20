using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantCamera : MonoBehaviour
{
    [SerializeField] ICScene[] iCScenes;
    [SerializeField] string startText;
    [SerializeField] GameObject cameraButton;
    [SerializeField] Image flash, mainImage, itemLarge;
    [SerializeField] TypeWritter typeWritter;
    [SerializeField] List<UIMove> objectsToMove = new();
    [SerializeField] Outro outro;

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
            if (!outro)
                StartCoroutine(IntroEnd());
            else
                StartCoroutine(OutroEnd());
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
        Debug.Log("OnCameraButtonPressed sceneIdx = " + sceneIdx + ", length = " + iCScenes.Length);
        if (CurrentScene != null && !CurrentScene.ScenePassed)
        {
            SoundManager.PlaySound(SoundManager.Sound.InvalidClick);
            return;
        }
        SoundManager.PlaySound(SoundManager.Sound.CameraButton);

        // end of intro/outro
        if (sceneIdx + 1 == iCScenes.Length)
        {
            cameraButton.GetComponent<Button>().onClick.RemoveAllListeners();
            if (outro != null)
                StartCoroutine(OutroEnd());
            else
                StartCoroutine(IntroEnd());
            return;
        }

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
        foreach (UIMove move in FindObjectOfType<Background>().moves)
            move.MoveOut();
        ghost.GetComponent<Ghost>().MoveToSide();

        GameManager.Instance.LoadSceneInSeconds(SceneType.MainMenu, 3f);
    }

    IEnumerator OutroEnd()
    {
        foreach (var obj in objectsToMove)
            obj.MoveOut();
        foreach (UIMove move in FindObjectOfType<Background>().moves)
            move.MoveOut();

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(outro.OutroEnd());
    }

    IEnumerator NewScene()
    {
        sceneIdx++;
        yield return Utils.ImageFade(flash, 0.1f, 1);
        yield return new WaitForSeconds(0.7f);

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
                SoundManager.PlaySound(SoundManager.Sound.Click);
                itemLarge.enabled = true;
                itemLarge.sprite = item.ItemDetailSprite;
                item.Pressed = true;
                if (item.ItemText != "")
                    typeWritter.StartTypeWrite(item.ItemText);
                if (CurrentScene.ScenePassed)
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
    [TextArea]
    public string ItemText;
    [NonSerialized]
    public bool Pressed = false;
}
