using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWritter : MonoBehaviour
{
    public bool IsTyping { get { return typeWriteCoroutine != null; } }

    IEnumerator typeWriteCoroutine;
    Text text;
    string currentFullText;

    void Awake()
    {
        text = GetComponent<Text>();
        Clear();
    }

    public void StartTypeWrite(string _text)
    {
        if (currentFullText == _text)
            return;
        currentFullText = _text;
        if (typeWriteCoroutine != null)
            StopCoroutine(typeWriteCoroutine);
        typeWriteCoroutine = TypeWrite(_text);
        StartCoroutine(typeWriteCoroutine);
    }

    IEnumerator TypeWrite(string fullText)
    {
        text.text = "";
        yield return new WaitForSeconds(1f);
        while (text.text != fullText)
        {
            text.text = fullText.Substring(0, text.text.Length + 1);
            yield return new WaitForSeconds(Random.Range(0.12f, 0.35f));
        }
        typeWriteCoroutine = null;
    }

    public void Clear() 
    {
        if (typeWriteCoroutine != null)
            StopCoroutine(typeWriteCoroutine);
        text.text = "";
        typeWriteCoroutine = null;
    }
}
