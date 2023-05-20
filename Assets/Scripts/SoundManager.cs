using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    [SerializeField] SoundAudioClip[] SoundAudioClipArray;

    public static SoundManager Instance;

    [Serializable]
    class SoundAudioClip
    {
        public Sound sound;
        public AudioMixerGroup mixer;
        public AudioClip audioClip;
    }

    // enumeration of all sounds, added in GameAssets
    public enum Sound
    {
        MusicStartMainMenu,
        MusicIntroOutro,
        MusicLevelOne,
        MusicLevelTwo,
        MusicLevelThree,


        InvalidClick,
        Click,
        CameraButton,
        NewLevel,

        Whisper1,
        Whisper2,
        DestroyBubble,
        BottleWiggle,
        Pen,

        DestroyWoolBall,
        CatClick,
        CatSnore,

        TagFall,
        TagWiggle,
        Bird,

        None
    }

    public static GameObject BGM_loop;

    // last time the soundclip is played
    private static Dictionary<Sound, float> soundTimerDic;
    // soundclips play minimum interval
    private static Dictionary<Sound, float> soundMaxTimerDic;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        soundTimerDic = new() { };
        soundMaxTimerDic = new() { };
        foreach (Sound sound in Enum.GetValues(typeof(Sound)))
        {
            soundTimerDic[sound] = 0;
            soundMaxTimerDic[sound] = 0;
        }
        //soundMaxTimerDic[Sound.TrainHorn] = 10f;
    }

    // play one shot
    public static GameObject PlaySound(Sound sound, float volume = 1f, bool loop = false)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new("Sound");
            soundGameObject.transform.SetParent(Instance.transform);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            SoundAudioClip soundAudioClip = GetSoundAudioClip(sound);
            audioSource.outputAudioMixerGroup = soundAudioClip.mixer;

            if (loop)
            {
                audioSource.clip = soundAudioClip.audioClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(soundAudioClip.audioClip);
                Destroy(soundGameObject, soundAudioClip.audioClip.length); // destroy after length of soundclip
            }

            return soundGameObject;
        }
        return null;
    }

    public static void StartBGM(Sound loop)
    {
        if (BGM_loop) Destroy(BGM_loop);

        BGM_loop = new("BGM Loop");
        BGM_loop.transform.SetParent(Instance.transform);
        AudioSource loopAudioSource = BGM_loop.AddComponent<AudioSource>();
        SoundAudioClip loopClip = GetSoundAudioClip(loop);

        loopAudioSource.outputAudioMixerGroup = loopClip.mixer;

        loopAudioSource.clip = loopClip.audioClip;
        loopAudioSource.loop = true;
        loopAudioSource.Play();
    }

    public IEnumerator CrossFadeBGM(Sound newSound, float fadeDuration = 0f)
    {
        if (BGM_loop)
            yield return StartFade(BGM_loop.GetComponent<AudioSource>(), fadeDuration, 0);
        StartBGM(newSound);
    }

    public void FadeAudio(GameObject audioGameObject, float duration, float targetVolume)
    {
        if (audioGameObject)
            StartCoroutine(StartFade(audioGameObject.GetComponent<AudioSource>(), duration, targetVolume));
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration && audioSource)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public void pauseBGM(float duration = 0)
    {
        FadeAudio(BGM_loop, duration, 0f);
    }

    public void restartBGM(float duration = 0)
    {
        FadeAudio(BGM_loop, duration, 1f);
    }

    // check minimum interval
    private static bool CanPlaySound(Sound sound)
    {
        if (soundTimerDic.ContainsKey(sound))
        {
            float lastTimePlayed = soundTimerDic[sound];
            float timerMax = soundMaxTimerDic[sound];
            if (lastTimePlayed + timerMax <= Time.time)
            {
                soundTimerDic[sound] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private static SoundAudioClip GetSoundAudioClip(Sound sound)
    {
        foreach (SoundAudioClip soundAudioClip in Instance.SoundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip;
            }
        }
        Debug.LogError("Sound not found");
        return null;
    }
}
