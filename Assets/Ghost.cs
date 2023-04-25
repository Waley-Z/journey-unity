using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : MonoBehaviour
{
    [SerializeField] float spriteInterval = 0.1f;
    [SerializeField] List<Sprite> sprites = new();
    [SerializeField] Vector2 endPos;

    Image image;
    float countdown;
    int spriteIndex = 0;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0f);
        countdown = spriteInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown <= 0)
        {
            image.sprite = sprites[spriteIndex++ % sprites.Count];
            countdown = spriteInterval;
        }
        else
        {
            countdown -= Time.deltaTime;
        }
    }

    public void MoveToSide()
    {
        StartCoroutine(Utils.UIMove(transform, endPos, 2.5f));
    }
}
