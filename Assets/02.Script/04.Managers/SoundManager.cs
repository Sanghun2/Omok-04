using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    [Header("Bgm")]
    public AudioSource bgmsource;
    public AudioSource sfxSource;
    public AudioClip introBgm;
    public AudioClip playBgm;

    [Header("이벤트 사운드")]
    public AudioClip attackSound; //착수 사운드
    public AudioClip eventSound;

    [Header("볼륨 셋팅")]
    [Range(0f, 1f)]
    public float bgmVolume = 0.1f;

    [Range(0f, 1f)]
    public float sfxVolume = 0.1f;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);

        SetBGMSound("Intro");
        if (bgmsource != null)
            bgmsource.volume = bgmVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;

    }

    public void SetBGMSound(string bgmName)
    {
        if (bgmName == "Intro")
            bgmsource.clip = introBgm;

        else if (bgmName == "Play")
            bgmsource.clip = playBgm;

        bgmsource.loop = true;
        bgmsource.volume = bgmVolume;
        bgmsource.Play();
    }


    public void OnAttackSound()
    {
        if (sfxSource != null && attackSound != null)
            sfxSource.PlayOneShot(attackSound, sfxVolume); // 볼륨 파라미터 추가
    }

    //eventSound
    public void BGMTestSound()
    {
        if (bgmsource != null && eventSound != null)
            bgmsource.PlayOneShot(eventSound, bgmVolume); // 볼륨 파라미터 추가
        
    }
    public void SFMTestSound()
    {
        sfxSource.PlayOneShot(attackSound, sfxVolume);
    }


    //BGM 볼륨 변경
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);  // 0 ~ 1 사이 값으로 제한
        if (bgmsource != null)
            bgmsource.volume = bgmVolume;

    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume); // 0 ~ 1 사이 값으로 제한

        if (sfxSource != null) // SFX 오디오 소스가 연결되어 있다면
            sfxSource.volume = sfxVolume; // 실제 Unity 오디오 소스의 볼륨에 반영
    }
    
    internal float GetBGMVolume()
    {
        return bgmVolume;
    }

    internal float GetSFXVolume()
    {
        return sfxVolume;
    }

}