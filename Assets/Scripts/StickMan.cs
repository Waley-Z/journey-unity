using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMan : MonoBehaviour
{
    [SerializeField] Vector2 targetPos;
    [SerializeField] string walkAnim, danceAnim0, danceAnim1;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator StartWalk()
    {
        yield return StartCoroutine(Utils.ObjectMoveWithSpeed(transform, 1f, targetPos));
        animator.Play(danceAnim0);
    }

    public void StartDanceRed()
    {
        animator.Play(danceAnim1);
    }
}
