using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOne : MonoBehaviour
{
    [SerializeField] List<Image> objectsToShowFirst, objectsToShowSecond;
    [SerializeField] TypeWritter typeWritter;
    [SerializeField, TextArea] string[] texts = new string[4];

    [SerializeField] Image doorLight;
    [SerializeField] Pen pen;

    [SerializeField] Transform greenBubbles, redBubbles;

    [SerializeField] StickMan man1, man2;

    [SerializeField] Image guide, bottle;

    [SerializeField] Bottle bottleLarge;

    [SerializeField] MainMenu mainMenu;

    IEnumerator flickering;
    int bubbleDestroyed = 0;

    void Start()
    {
        foreach (Image img in objectsToShowFirst)
        {
            StartCoroutine(Utils.ImageFade(img, 0f, 0f));
        }

        foreach (Image img in objectsToShowSecond)
        {
            StartCoroutine(Utils.ImageFade(img, 0, 0f));
        }
        man1.gameObject.SetActive(false);
        man2.gameObject.SetActive(false);

        //Init();
    }

    public void Init()
    {
        mainMenu.gameObject.SetActive(false);
        StartCoroutine(StartLevel());
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
        man1.gameObject.SetActive(true);
        man2.gameObject.SetActive(true);
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
        bubbleDestroyed++;
        if (bubbleDestroyed == redBubbles.GetComponentsInChildren<UIScale>().Length)
        {
            typeWritter.StartTypeWrite(texts[3]);
        }
    }
}
