using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMove : MonoBehaviour
{
    public Vector2 inPosition, outPosition;

    RectTransform rectTransform;
    IEnumerator moveCoroutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();    
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))        
    //        MoveIn();
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //        MoveOut();
    //}

    public void MoveIn()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = Utils.UIEaseIn(rectTransform, inPosition);
        StartCoroutine(moveCoroutine);
    }

    public void MoveOut()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = Utils.UIEaseOut(rectTransform, outPosition);
        StartCoroutine(moveCoroutine);
    }
}
