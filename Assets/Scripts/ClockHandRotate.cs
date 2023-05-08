using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHandRotate : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.Rotate(0, 0, -Time.unscaledDeltaTime * speed);
    }
}
