using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonWiggle : MonoBehaviour
{
    [SerializeField] Transform button;

    [Header("Wiggle Effect Config")]
    [SerializeField] float speed = 20f;
    [SerializeField] float distance = 6f;
    [SerializeField] float duration = 1f;
    [SerializeField] float interval = 3f;
    [SerializeField] SoundManager.Sound sound = SoundManager.Sound.None;

    float WiggleCountdown = float.PositiveInfinity;

    void Update()
    {
        WiggleCountdown -= Time.deltaTime;
        if (WiggleCountdown <= 0)
        {
            StartCoroutine(WiggleButton());
            WiggleCountdown = interval + duration;
        }
    }

    public void WiggleInSeconds(float sec)
    {
        WiggleCountdown = sec;
    }

    public void OnClicked()
    {
        WiggleCountdown = float.PositiveInfinity;
    }

    IEnumerator WiggleButton()
    {
        if (sound != SoundManager.Sound.None)
            SoundManager.PlaySound(sound);

        float time = 0f;
        Vector3 rot = button.localEulerAngles;

        while (time <= duration)
        {
            rot.z = Mathf.Lerp(Mathf.Sin(time * speed) * distance, 0, time / duration);
            button.localEulerAngles = rot;
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        button.localEulerAngles = Vector3.zero;
    }
}
