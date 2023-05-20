using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTwo : MonoBehaviour
{
    [SerializeField] List<Image> objectsToShowFirst, objectsToShowSecond;
    [SerializeField] List<UIMove> objectsToMove = new();
    [SerializeField] List<GameObject> objectsToFadeOut = new();

    [SerializeField] TypeWritter typeWritter;
    [SerializeField, TextArea] string[] texts = new string[3];

    [SerializeField] GameObject woolBallPrefab;
    [SerializeField] GameObject woolBallBoundary;
    [SerializeField] List<Sprite> normalWoolBallSprites = new();
    [SerializeField] Sprite specialWoolBallSprite;

    [SerializeField] GameObject PaperBalls;
    [SerializeField] Image note;

    bool specialWoolBallSpawned = false;
    int normalWoolBallSpawned = 0;
    Coroutine showNoteCoroutine;
    GameObject catSnoreSound;

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
        StartCoroutine(SoundManager.Instance.CrossFadeBGM(SoundManager.Sound.MusicLevelTwo, 1f));
        catSnoreSound = SoundManager.PlaySound(SoundManager.Sound.CatSnore, loop: true);

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
        foreach (PaperBall paperBall in PaperBalls.GetComponentsInChildren<PaperBall>())
        {
            paperBall.OnCollision += () => startShowNote(paperBall.note);
        }

        foreach (Image img in objectsToShowFirst)
        {
            StartCoroutine(Utils.ImageFade(img, 1f, 1f));
        }

        foreach (GameObject go in objectsToFadeOut)
        {
            go.SetActive(false);
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

    void startShowNote(Sprite noteSprite)
    {
        if (showNoteCoroutine != null)
            StopCoroutine(showNoteCoroutine);

        note.sprite = noteSprite;
        showNoteCoroutine = StartCoroutine(ShowNote());
    }

    IEnumerator ShowNote()
    {
        StartCoroutine(Utils.ImageFade(note, 1, 1));
        yield return new WaitForSeconds(5f);
        StartCoroutine(Utils.ImageFade(note, 1, 0));
    }

    public IEnumerator EndLevel()
    {
        StartCoroutine(SoundManager.StartFade(catSnoreSound.GetComponent<AudioSource>(), 1f, 0));
        foreach (GameObject go in objectsToFadeOut)
        {
            foreach (Image img in go.GetComponentsInChildren<Image>())
            {
                StartCoroutine(Utils.ImageFade(img, 1f, 0f));
            }
        }

        typeWritter.StartTypeWrite(texts[2]);

        yield return new WaitForSeconds(15f);

        foreach (UIMove move in objectsToMove)
        {
            move.MoveOut();
        }

        GameManager.Instance.LoadSceneInSeconds(SceneType.MainMenu, 3f);

        StartCoroutine(SoundManager.StartFade(SoundManager.BGM_loop.GetComponent<AudioSource>(), 2f, 0));
        yield return new WaitForSeconds(2f);
        SoundManager.PlaySound(SoundManager.Sound.NewLevel);

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void OnCatClicked()
    {
        SoundManager.PlaySound(SoundManager.Sound.CatClick);

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
            woolBall.GetComponent<Button>().onClick.AddListener(() => SoundManager.PlaySound(SoundManager.Sound.DestroyWoolBall));
            normalWoolBallSpawned++;
        }
        Vector3 randomVector3 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        woolBall.GetComponent<Rigidbody2D>().velocity = randomVector3.normalized * Random.Range(1f, 2.5f);
        woolBall.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        woolBall.transform.localScale *= Random.Range(0.5f, 0.7f);
    }

    void OnWoolBallClicked()
    {
        SoundManager.PlaySound(SoundManager.Sound.DestroyWoolBall);

        foreach (GameObject go in objectsToFadeOut)
        {
            go.SetActive(true);
            foreach (Image img in go.GetComponentsInChildren<Image>())
            {
                StartCoroutine(Utils.ImageFade(img, 0f, 0f));
                StartCoroutine(Utils.ImageFade(img, 1f, 1f));
            }
        }
    }
}
