using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIFloat : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    float originalY;
    float offset;

    void Start()
    {
        originalY = transform.localPosition.y;
        offset = Random.Range(0f, 1f) * Mathf.PI * 2;
    }

    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, originalY + Mathf.Sin(Time.time * speed + offset) * 5f, transform.localPosition.z);
    }
}
