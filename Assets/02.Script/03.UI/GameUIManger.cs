using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public enum BTNType
{
    STARTSINGLE,
    STARTMULTI,
    SETTING,
    EXIT,
}
public class GameUIManger : MonoBehaviour
{


    [SerializeField] GameObject popup;
    [SerializeField] GameObject introPopup;
    [SerializeField] GameObject dim;


    [SerializeField] Button settingButton;
    [SerializeField] Button exitButton;

    //intro 화면
    [SerializeField] GameObject ButtonPopup;


    [SerializeField] bool isStart; //게임 오버 후 인트로로 돌아오면 다시 false로 켜줘야 됨
    [SerializeField] bool isSetting;

    // 팝업 버튼들
    [SerializeField] GameObject settingPopup;
    [SerializeField] GameObject MultiPopup;
    [SerializeField] GameObject tipPopup; //디벨로퍼

    [Header("사운드 셋팅 UI")]
    [SerializeField] SoundManager soundManager;
    [SerializeField] Slider bgmVolumeSlider;
    [SerializeField] Slider SfxVolumeSlider;
    [SerializeField] Button bgmTestButton;
    [SerializeField] Button SfxTestButton;

    [SerializeField] GameObject MainButton;

    bool isMulti;
    bool isTip;

    [SerializeField] GameObject[] background = new GameObject[3];



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

        if (isMulti || isTip)
        {
            MultiPopup.SetActive(false);
            tipPopup.SetActive(false);

            isMulti = false;
            isTip = false;
        }

        // gameButtonManager.buttonScale.localScale = defaultScale;

    }

    public void OnmMultiPopup()
    {
        MultiPopup.SetActive(true);
        ButtonPopup.SetActive(false);
        isMulti = true;
    }

    public void OnSettingPopup()
    {
        OnPopup();
        settingPopup.SetActive(true);
    }

    //종료할 기능 넣기
    public void OnExitPopup()
    {
    }

    public void OnToMain()
    {
        isStart = false;

        if (MainButton != null && !MainButton.activeSelf)
            MainButton.SetActive(true);

        Time.timeScale = 0f;
    }
}
