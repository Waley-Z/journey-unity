using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pen : MonoBehaviour
{
    [SerializeField] float spriteInterval = 0.2f;
    [SerializeField] List<Sprite> spritesLightOn = new();
    [SerializeField] List<Sprite> spritesLightOff = new();

    [NonSerialized] public bool AllowClick = false;
    Image image;
    float countdown;
    int spriteIndex = 0;
    bool isLightOn = false;
    GameObject sound;

    public void TurnLight(bool on)
    {
        isLightOn = on;
        updateSprite();
    }

    void Awake()
    {
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.5f;
        countdown = spriteInterval;
        sound = SoundManager.PlaySound(SoundManager.Sound.Pen, loop: true);
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
        image.sprite = isLightOn ? spritesLightOn[spriteIndex % spritesLightOn.Count]
            : spritesLightOff[spriteIndex % spritesLightOff.Count];
    }

    public void OnClicked()
    {
        if (AllowClick)
        {
            countdown = float.PositiveInfinity;
            Destroy(sound);
        }
    }
}
