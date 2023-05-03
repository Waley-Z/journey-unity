using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIScale : MonoBehaviour
{
    Vector2 originalScale;

    RectTransform rectTransform;
    IEnumerator scaleCoroutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    public void Scale(bool isUp, float minDuration = 2f, float maxDuration = 4f)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        scaleCoroutine = Utils.UIScale(rectTransform, Random.Range(minDuration, maxDuration), isUp ? originalScale : Vector2.zero);
        StartCoroutine(scaleCoroutine);
    }
}
