using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMan : MonoBehaviour
{
    [SerializeField] Vector2 targetPos;
    [SerializeField] string walkAnim, danceAnim0, danceAnim1;
    [SerializeField] Animator animator;

    Vector2 startPos;

    public IEnumerator StartWalk()
    {
        startPos = transform.localPosition;
        yield return StartCoroutine(Utils.ObjectMoveWithSpeed(transform, 1f, targetPos));
        animator.Play(danceAnim0);
    }

    public void StartDanceRed()
    {
        animator.Play(danceAnim1);
    }

    public IEnumerator ReturnWalk()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        animator.Play(walkAnim);
        yield return StartCoroutine(Utils.ObjectMoveWithSpeed(transform, 1f, startPos));
    }
}
