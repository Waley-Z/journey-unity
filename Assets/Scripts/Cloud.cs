using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] float startPosX, endPosX;
    [SerializeField] float minSpeed = 6f;
    [SerializeField] float maxSpeed = 12f;
    float speed;
    RectTransform rt;

    void Awake()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rt.localPosition.x < endPosX)
        {
            rt.localPosition = new Vector3(rt.localPosition.x + speed * Time.deltaTime, rt.localPosition.y, rt.localPosition.z);
        }
        else
        {
            rt.localPosition = new Vector3(startPosX, rt.localPosition.y, rt.localPosition.z);
            speed = Random.Range(minSpeed, maxSpeed);
            Debug.Log($"new cloud speed: {speed}");
        }
    }
}
