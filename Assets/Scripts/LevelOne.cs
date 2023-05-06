using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelOne : MonoBehaviour
{
    [SerializeField] List<Image> objectsToShowFirst, objectsToShowSecond;
    [SerializeField] List<UIMove> objectsToMove = new();

    [SerializeField] TypeWritter typeWritter;
    [SerializeField, TextArea] string[] texts = new string[4];

    [SerializeField] Image doorLight;
    [SerializeField] Pen pen;

    [SerializeField] Transform greenBubbles, redBubbles;

    [SerializeField] Image guide, bottle;

    [SerializeField] Bottle bottleLarge;

    [SerializeField] GameObject man1Prefab, man2Prefab;

    StickMan man1, man2;
    IEnumerator flickering;

    void Awake()
    {
        foreach (Image img in objectsToShowFirst)
        {
            StartCoroutine(Utils.ImageFade(img, 0f, 0f));
        }

        foreach (Image img in objectsToShowSecond)
        {
            StartCoroutine(Utils.ImageFade(img, 0, 0f));
        }
    }

    void Start()
    {
        StartCoroutine(StartLevel());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LevelOneEnd());
        }
    }

    IEnumerator StartLevel()
    {
        foreach (Image img in objectsToShowFirst)
        {
            StartCoroutine(Utils.ImageFade(img, 1f, 1f));
        }

        yield return new WaitForSeconds(2f);

        foreach (Image img in objectsToShowSecond)
        {
            StartCoroutine(Utils.ImageFade(img, 1f, 1f));
        }

        yield return new WaitForSeconds(2f);

        typeWritter.StartTypeWrite(texts[0]);
        flickering = StartFlickering(12f);
        StartCoroutine(flickering);
        yield return null;
    }

    IEnumerator StartFlickering(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        while (true)
        {
            flickerLight(true);
            yield return new WaitForSeconds(0.5f);
            flickerLight(false);
            yield return new WaitForSeconds(0.8f);
            flickerLight(true);
            yield return new WaitForSeconds(0.5f);
            flickerLight(false);
            yield return new WaitForSeconds(0.8f);
            yield return new WaitForSeconds(2f);
        }
    }

    public void StopFlickering()
    {
        if (flickering != null)
        {
            StopCoroutine(flickering);
            flickering = null;
            flickerLight(true);
            StartCoroutine(StartStickMen());
        }
    }

    void flickerLight(bool on)
    {
        doorLight.enabled = on;
        pen.TurnLight(on);
    }

    IEnumerator StartStickMen()
    {
        man1 = Instantiate(man1Prefab).GetComponent<StickMan>();
        man2 = Instantiate(man2Prefab).GetComponent<StickMan>();

        yield return new WaitForSeconds(3f);
        Coroutine walk2 = StartCoroutine(man2.StartWalk());
        yield return new WaitForSeconds(1f);
        Coroutine walk1 = StartCoroutine(man1.StartWalk());

        StartCoroutine(ScaleBubbles(true, greenBubbles.GetComponentsInChildren<UIScale>()));

        yield return walk1;
        yield return walk2;

        typeWritter.StartTypeWrite(texts[1]);

        yield return new WaitForSeconds(10f);

        typeWritter.StartTypeWrite(texts[2]);
        pen.AllowClick = true;

        man1.StartDanceRed();
        man2.StartDanceRed();

        StartCoroutine(ScaleBubbles(false, greenBubbles.GetComponentsInChildren<UIScale>(), 0.5f, 1.5f));
        StartCoroutine(ScaleBubbles(true, redBubbles.GetComponentsInChildren<UIScale>(), 0.5f, 1.5f));

        yield return new WaitForSeconds(1f);

        guide.gameObject.SetActive(true);
        bottle.gameObject.SetActive(true);

        bottle.GetComponent<ButtonWiggle>().WiggleInSeconds(10f);

        StartCoroutine(Utils.ImageFade(guide, 1f, 1f));
        StartCoroutine(Utils.ImageFade(bottle, 1f, 1f));
    }

    IEnumerator LevelOneEnd()
    {
        typeWritter.StartTypeWrite(texts[3]);

        yield return new WaitForSeconds(1f);

        if (man1 && man2)
        {
            Coroutine walk1 = StartCoroutine(man1.ReturnWalk());
            Coroutine walk2 = StartCoroutine(man2.ReturnWalk());
            yield return walk1;
            yield return walk2;
            Destroy(man1.gameObject);
            Destroy(man2.gameObject);
        }

        yield return new WaitForSeconds(5f);
        foreach (UIMove move in objectsToMove)
        {
            move.MoveOut();
        }

        GameManager.Instance.LoadSceneInSeconds(SceneType.MainMenu, 3f);

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    IEnumerator ScaleBubbles(bool isUp, UIScale[] scales, float minDuration = 2f, float maxDuration = 4f)
    {
        foreach (var scale in scales)
        {
            scale.Scale(isUp, minDuration, maxDuration);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }

    public void OnBottleClicked()
    {
        bottleLarge.gameObject.SetActive(true);
        bottleLarge.Init();
    }

    public void OnBubbleDestroyed()
    {
        if (GameObject.FindGameObjectsWithTag("RedBubble").Length <= 1)
        {
            StartCoroutine(LevelOneEnd());
        }
    }
}
