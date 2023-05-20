using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thorn : MonoBehaviour
{
    [SerializeField] float targetPosY;
    [SerializeField] RectTransform thornTransform;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Image image;
    [SerializeField] ButtonWiggle tagButton;

    public Action OnDestroyed;

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && thornTransform.localPosition.y > targetPosY)
        {
            scrollRect.enabled = false;

            StartCoroutine(DestroyThorn());
        }
    }

    IEnumerator DestroyThorn()
    {
        StartCoroutine(Utils.ImageFade(image, 1f, 0f));
        tagButton.WiggleInSeconds(0f);
        yield return new WaitForSeconds(1.1f);
        tagButton.GetComponent<Rigidbody2D>().simulated = true;
        SoundManager.PlaySound(SoundManager.Sound.TagFall);
        StartCoroutine(Utils.ImageFade(tagButton.GetComponent<Image>(), 1.1f, 0f));

        yield return new WaitForSeconds(2f);

        OnDestroyed.Invoke();

        Destroy(tagButton.gameObject);
        Destroy(gameObject);
    }
}
