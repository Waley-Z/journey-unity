using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string parameter;
    [SerializeField] float minX, maxX;
    [SerializeField] RectTransform slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        audioMixer.SetFloat(parameter, Mathf.Log10((slider.localPosition.x - minX) / (maxX - minX)) * 20);
    }
}
