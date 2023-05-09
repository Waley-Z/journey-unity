using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTwo : MonoBehaviour
{
    [SerializeField] List<Image> objectsToShowFirst, objectsToShowSecond;
    [SerializeField] List<UIMove> objectsToMove = new();

    [SerializeField] TypeWritter typeWritter;
    [SerializeField, TextArea] string[] texts = new string[3];

    [SerializeField] GameObject woolBallPrefab;
    [SerializeField] GameObject woolBallBoundary;
    [SerializeField] List<Sprite> normalWoolBallSprites = new();
    [SerializeField] Sprite specialWoolBallSprite;

    bool specialWoolBallSpawned = false;
    int normalWoolBallSpawned = 0;

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
            StartCoroutine(EndLevel());
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
        yield return new WaitForSeconds(10f);

        typeWritter.StartTypeWrite(texts[1]);
    }

    IEnumerator EndLevel()
    {
        typeWritter.StartTypeWrite(texts[2]);

        yield return new WaitForSeconds(7f);
        foreach (UIMove move in objectsToMove)
        {
            move.MoveOut();
        }

        GameManager.Instance.LoadSceneInSeconds(SceneType.MainMenu, 3f);

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public void OnCatClicked()
    {
        GameObject woolBall = Instantiate(woolBallPrefab, woolBallBoundary.transform);
        int random = specialWoolBallSpawned ?
            Random.Range(0, normalWoolBallSprites.Count) : Random.Range(0, normalWoolBallSprites.Count + 1);

        print(random);

        if (random == normalWoolBallSprites.Count || (!specialWoolBallSpawned && normalWoolBallSpawned >= 8))
        {
            woolBall.GetComponent<Image>().sprite = specialWoolBallSprite;
            woolBall.GetComponent<Button>().onClick.AddListener(OnWoolBallClicked);
            specialWoolBallSpawned = true;
        }
        else
        {
            woolBall.GetComponent<Image>().sprite = normalWoolBallSprites[random];
            normalWoolBallSpawned++;
        }
        Vector3 randomVector3 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        woolBall.GetComponent<Rigidbody2D>().velocity = randomVector3.normalized * Random.Range(1f, 2.5f);
        woolBall.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        woolBall.transform.localScale *= Random.Range(0.8f, 1f);
    }

    void OnWoolBallClicked()
    {

    }
}
