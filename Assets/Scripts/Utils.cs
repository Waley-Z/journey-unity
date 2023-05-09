using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PathCreation;

public class Utils : MonoBehaviour
{
    public static IEnumerator ImageFade(Image image, float duration, float targetAlpha, float brightness = 1)
    {
        if (image == null)
            yield break;

        float currentTime = 0;
        float startAlpha = image.color.a;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);
            image.color = new Color(brightness, brightness, brightness, alpha);
            yield return null;
        }
        image.color = new Color(brightness, brightness, brightness, targetAlpha);
    }

    public static IEnumerator UIFade(CanvasRenderer renderer, float duration, float targetAlpha)
    {
        float currentTime = 0;
        float startAlpha = renderer.GetAlpha();
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);
            renderer.SetAlpha(alpha);
            yield return null;
        }
    }

    public static IEnumerator CanvasGroupFade(CanvasGroup group, float duration, float targetAlpha)
    {
        float currentTime = 0;
        float startAlpha = group.alpha;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);
            group.alpha = alpha;
            yield return null;
        }
        group.alpha = targetAlpha;
    }

    public static IEnumerator UIScale(RectTransform transform, float duration, Vector2 _targetScale)
    {
        Vector3 targetScale = new(_targetScale.x, _targetScale.y, transform.localScale.z);
        float currentTime = 0;
        Vector2 startScale = transform.localScale;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, currentTime / duration);
            yield return null;
        }
    }

    public static IEnumerator ObjectMoveWithDuration(Transform transform, float duration, Vector2 _targetPos)
    {
        Vector3 targetPos = new(_targetPos.x, _targetPos.y, transform.localPosition.z);
        float currentTime = 0;
        Vector2 startPos = transform.localPosition;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, currentTime / duration);
            yield return null;
        }
    }

    public static IEnumerator ObjectMoveWithSpeed(Transform transform, float speed, Vector2 _targetPos)
    {
        Vector3 targetPos = new(_targetPos.x, _targetPos.y, transform.localPosition.z);
        while (transform.localPosition != targetPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.unscaledDeltaTime);
            yield return null;
        }
    }

    public static IEnumerator ObjectFollowPath(Transform transform, float speed, PathCreator path)
    {
        if (path == null)
            yield break;
        float distanceTravelled = 0;
        while (distanceTravelled < path.path.length)
        {
            distanceTravelled += speed * Time.unscaledDeltaTime;
            transform.position = path.path.GetPointAtDistance(distanceTravelled);
            yield return null;
        }
    }

    //public static IEnumerator UIMove(RectTransform transform, float duration, Vector2 _targetPos, AnimationCurve curve)
    //{
    //    Vector3 targetPos = new(_targetPos.x, _targetPos.y, transform.localPosition.z);
    //    float currentTime = 0;
    //    Vector2 startPos = transform.localPosition;
    //    while (currentTime < duration)
    //    {
    //        currentTime += Time.unscaledDeltaTime;
    //        transform.localPosition = Vector3.Lerp(startPos, targetPos, curve.Evaluate(currentTime / duration));
    //        yield return null;
    //    }
    //}

    public static IEnumerator UIEaseIn(RectTransform transform, Vector2 _targetPos)
    {
        Vector3 targetPos = new(_targetPos.x, _targetPos.y, transform.localPosition.z);
        while (Vector3.Distance(transform.localPosition, targetPos) > 0.1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.unscaledDeltaTime * 2.5f);
            yield return null;
        }
        transform.localPosition = targetPos;
    }

    public static IEnumerator UIEaseOut(RectTransform transform, Vector2 _targetPos)
    {
        Vector3 targetPos = new(_targetPos.x, _targetPos.y, transform.localPosition.z);
        Vector3 startPos = transform.localPosition;
        Vector3 dir = Vector3.Normalize(targetPos - startPos);
        transform.localPosition += dir * 0.2f;
        while (Vector3.Distance(startPos, transform.localPosition) < Vector3.Distance(startPos, targetPos))
        {
            float speed = Vector3.Distance(transform.localPosition, startPos) * 3.8f;
            transform.localPosition += dir * speed * Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localPosition = targetPos;
    }

    public static IEnumerator UIMove(Transform transform, Vector2 _targetPos, float duration)
    {
        float currentTime = 0;
        Vector3 targetPos = new(_targetPos.x, _targetPos.y, transform.localPosition.z);
        Vector3 startPos = transform.localPosition;
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, curve.Evaluate(currentTime / duration));
            yield return null;
        }
    }
}
