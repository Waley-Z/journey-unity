using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : MonoBehaviour
{
    [SerializeField] float spriteInterval = 0.2f;
    [SerializeField] List<Sprite> speakSprites = new();
    [SerializeField] List<Sprite> staticSprites = new();

    [SerializeField] Vector2 endPos;
    [SerializeField] TypeWritter typeWritter;

    [NonSerialized] public Image image;
    float countdown;
    int spriteIndex = 0;

    void Awake()
    {
        image = GetComponent<Image>();
        countdown = spriteInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown <= 0)
        {
            if (typeWritter && typeWritter.IsTyping)
                image.sprite = speakSprites[spriteIndex++ % speakSprites.Count];
            else
                image.sprite = staticSprites[spriteIndex++ % staticSprites.Count];

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
