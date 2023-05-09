using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteLoop : MonoBehaviour
{
    [SerializeField] float spriteInterval = 0.1f;
    [SerializeField] List<Sprite> sprites = new();

    public float countdown;

    Image image;
    int spriteIndex = 0;

    void Awake()
    {
        image = GetComponent<Image>();
        countdown = spriteInterval;
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
