using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;

public class Bird : MonoBehaviour
{
    [SerializeField] float spriteInterval = 0.1f;
    [SerializeField] List<Sprite> sprites = new();
    [SerializeField] GameObject birdPath;

    public float countdown;

    PathCreator path;
    Image image;
    int spriteIndex = 0;
    bool flying = false;
    GameObject sound;

    public IEnumerator Fly()
    {
        if (flying) yield break;
        flying = true;
        sound = SoundManager.PlaySound(SoundManager.Sound.Bird);
        countdown = 0;
        yield return new WaitForSeconds(2f);
        StartCoroutine(Utils.ObjectFollowPath(transform, 2f, path));
        yield return new WaitForSeconds(1f);
        StartCoroutine(Utils.UIScale(GetComponent<RectTransform>(), 4f, 0.1f * Vector2.one));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Utils.ImageFade(image, 2f, 0f));
        Destroy(path.gameObject);
    }

    void Awake()
    {
        image = GetComponent<Image>();
        countdown = float.PositiveInfinity;
    }

    void Start()
    {
        path = Instantiate(birdPath).GetComponent<PathCreator>();
        transform.position = path.path.GetPointAtDistance(0);
    }

    void Update()
    {
        if (countdown <= 0)
        {
            spriteIndex++;
            updateSprite();
            countdown = spriteInterval;
        }
        else
        {
            countdown -= Time.deltaTime;
        }
    }

    void updateSprite()
    {
        image.sprite = sprites[spriteIndex % sprites.Count];
    }
}
