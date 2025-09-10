using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public enum BTNType
{
    STARTSOLO,
    STARTMULTI,
    SETTING,
    EXIT,
}
public class GameUIManger : MonoBehaviour
{


    public GameObject popup;
    public GameObject introPopup;
    public GameObject dim;


    public Button settingButton;
    public Button exitButton;

    //intro 화면
    public GameObject ButtonPopup;


    public bool isStart; //게임 오버 후 인트로로 돌아오면 다시 false로 켜줘야 됨
    public bool isSetting;

    // 팝업 버튼들
    public GameObject settingPopup;
    public GameObject storyPopup;
    public GameObject tipPopup; //디벨로퍼

    [Header("사운드 셋팅 UI")]
    public SoundManager soundManager;
    public Slider bgmVolumeSlider;
    public Slider SfxVolumeSlider;
    public Button bgmTestButton;
    public Button SfxTestButton;

    public GameObject MainButton;

    bool isStory;
    bool isTip;

    public GameObject[] background = new GameObject[3];



    void Start()
    {
        Time.timeScale = 0f;
        if (settingButton != null)
            settingButton.onClick.AddListener(OnSettingPopup);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitSetting);


        //사운드
        OnSoundSetting();


        isStart = false;
    }

    void RandomBackground()
    {
        //모든 백그라운드 비활성화
        for (int i = 0; i < background.Length; i++)
        {
            if (background[i] != null)
                background[i].SetActive(false);
        }
        //랜덤으로 하나 활성화
        int randomIndex = UnityEngine.Random.Range(0, background.Length);

        if (background[randomIndex] != null)
            background[randomIndex].SetActive(true);
    }

    public void OnSoundSetting()
    {
        if (soundManager == null) return;

        //Bgm 슬라이더
        if (bgmVolumeSlider != null)
        {
            bgmVolumeSlider.value = soundManager.GetBGMVolume();
            bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        }

        if (SfxVolumeSlider != null)
        {
            SfxVolumeSlider.value = soundManager.GetSFXVolume();
            SfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    }

    private void OnSFXVolumeChanged(float Value)
    {
        if (soundManager != null)
            soundManager.SetSFXVolume(Value);
    }

    //사운드
    private void OnBGMVolumeChanged(float Value)
    {
        if (soundManager != null)
            soundManager.SetBGMVolume(Value);
    }

    //팝업
    public void OnStartSetting()
    {
        introPopup.SetActive(false);
        RandomBackground();

        Time.timeScale = 1f;
        isStart = true;

        if (soundManager != null)
            soundManager.SetBGMSound("Play");

    }

    public void OnPopup()
    {
        soundManager.BGMTestSound();
        Time.timeScale = 0f;

        popup.SetActive(true);
        dim.SetActive(true);

        if (!isStart)
        {
            ButtonPopup.SetActive(false);
        }
        isSetting = true;

    }
    public void OnExitSetting()
    {
        if (popup)
        {
            popup.SetActive(false);
            dim.SetActive(false);

            settingPopup.SetActive(false);
            ButtonPopup.SetActive(true);
        }


        if (isStart)
            Time.timeScale = 1f;

        if (!isStart)
        {
            introPopup.SetActive(true);
            ButtonPopup.SetActive(true);
            Time.timeScale = 0f;
        }
        isSetting = false;

        if (isStory || isTip)
        {
            storyPopup.SetActive(false);
            tipPopup.SetActive(false);

            isStory = false;
            isTip = false;
        }

        // gameButtonManager.buttonScale.localScale = defaultScale;

    }

    public void OnStroyPopup()
    {
        storyPopup.SetActive(true);
        ButtonPopup.SetActive(false);
        isStory = true;
    }
    public void OnTipPopup()
    {
        tipPopup.SetActive(true);
        ButtonPopup.SetActive(false);
        isTip = true;
        popup.SetActive(false);

    }

    public void OnSettingPopup()
    {
        OnPopup();
        settingPopup.SetActive(true);
    }

    public void OnToMain()
    {
        isStart = false;

        if (MainButton != null && !MainButton.activeSelf)
            MainButton.SetActive(true);

        Time.timeScale = 0f;
    }
}
