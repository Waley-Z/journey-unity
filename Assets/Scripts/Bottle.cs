using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bottle : MonoBehaviour, IDragHandler
{
    [SerializeField] List<Sprite> sprites = new();
    [SerializeField] float spriteInterval = 0.2f;
    [SerializeField] RectTransform canvas;
    [SerializeField] LevelOne levelOne;

    Image image;
    int spriteIndex = 0;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
        //Init();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, eventData.position, canvas.GetComponent<Canvas>().worldCamera, out position);
        transform.position = canvas.TransformPoint(position);
    }

    public void Init()
    {
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        yield return StartCoroutine(Utils.ImageFade(image, 1f, 1f));
        yield return new WaitForSeconds(0.5f);
        while (spriteIndex < sprites.Count)
        {
            image.sprite = sprites[spriteIndex];
            spriteIndex++;
            yield return new WaitForSeconds(spriteInterval);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RedBubble"))
        {
            Destroy(other.gameObject);
            levelOne.OnBubbleDestroyed();
        }
    }
}
