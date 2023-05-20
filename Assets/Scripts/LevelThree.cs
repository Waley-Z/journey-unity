using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelThree : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToShowFirst, objectsToShowSecond;
    [SerializeField] List<UIMove> objectsToMove = new();

    [SerializeField] TypeWritter typeWritter;
    [SerializeField, TextArea] string[] texts = new string[3];

    [SerializeField] GameObject tags;
    [SerializeField] GameObject vines;
    [SerializeField] Bird bird;

    void Awake()
    {
        FadeAll(objectsToShowFirst, 0, 0);
        FadeAll(objectsToShowSecond, 0, 0);
    }

    void Start()
    {
        StartCoroutine(SoundManager.Instance.CrossFadeBGM(SoundManager.Sound.MusicLevelThree, 1f));

        foreach (Button button in tags.GetComponentsInChildren<Button>())
        {
            button.onClick.AddListener(() => typeWritter.StartTypeWrite(texts[0]));
        }

        foreach (Button button in vines.GetComponentsInChildren<Button>())
        {
            button.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
            button.onClick.AddListener(() => typeWritter.StartTypeWrite(texts[1]));
        }

        foreach (Thorn thorn in vines.GetComponentsInChildren<Thorn>())
        {
            thorn.OnDestroyed += () => CheckEndLevel();
        }

        StartCoroutine(StartLevel());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(EndLevel());
        }
    }

    IEnumerator StartLevel()
    {
        FadeAll(objectsToShowFirst, 1, 1);
        yield return new WaitForSeconds(2f);
        FadeAll(objectsToShowSecond, 1, 1);
    }

    void CheckEndLevel()
    {
        if (vines.GetComponentsInChildren<Thorn>().Count() <= 1)
        {
            StartCoroutine(EndLevel());
        }
    }

    IEnumerator EndLevel()
    {
        StartCoroutine(SoundManager.StartFade(SoundManager.BGM_loop.GetComponent<AudioSource>(), 10f, 0));

        typeWritter.StartTypeWrite(texts[2]);
        yield return StartCoroutine(bird.Fly());

        yield return new WaitForSeconds(3f);

        foreach (UIMove move in objectsToMove)
        {
            move.MoveOut();
        }

        GameManager.Instance.LoadSceneInSeconds(SceneType.Outro, 3f);

        yield return new WaitForSeconds(2f);
        SoundManager.PlaySound(SoundManager.Sound.NewLevel);

        yield return new WaitForSeconds(1f);
    }

    void FadeAll(List<GameObject> gos, float duration, float targetAlpha)
    {
        foreach (GameObject go in gos)
        {
            foreach (Image img in go.GetComponentsInChildren<Image>())
            {
                StartCoroutine(Utils.ImageFade(img, duration, targetAlpha));
            }
        }
    }
}
