using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWritter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    IEnumerator typeWriteCoroutine;

    public void StartTypeWrite(string text)
    {
        if (typeWriteCoroutine != null)
            StopCoroutine(typeWriteCoroutine);
        typeWriteCoroutine = TypeWrite(text);
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
    }

    public void Clear()
    {
        if (typeWriteCoroutine != null)
            StopCoroutine(typeWriteCoroutine);
        text.text = "";
    }
}
