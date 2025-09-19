using UnityEngine;
using UnityEngine.UI;

public enum BTNType
{
    STARTSINGLE,   
    STARTMULTI,    
    SETTING,      
    EXIT,          
}

public class GameUIManger : UIBase
{

    [SerializeField] GameObject popup;        // 일반 팝업(설정창 등) 루트 오브젝트
    [SerializeField] GameObject introPopup;   // 인트로(초기 안내) 팝업
    [SerializeField] GameObject dim;          // 배경을 어둡게 하는 딤(팝업 시 활성화)

    [SerializeField] Button settingButton;    // 인스펙터에 연결된 설정 버튼(우상단 등)
    [SerializeField] Button exitButton;       // 인스펙터에 연결된 종료 버튼

    // intro(인트로) 화면에 노출되는 버튼들을 감싼 오브젝트
    [SerializeField] GameObject ButtonPopup;

    // 런타임 상태 관리 플래그
    [SerializeField] bool isStart;            // 게임이 실제로 시작되었는지(인트로 이후)
    [SerializeField] bool isSetting;          // 설정 팝업이 열려있는지 여부

    // 팝업 종류들 (인스펙터에 각각 연결)
    [SerializeField] GameObject startLevelPopup;
    [SerializeField] GameObject settingPopup; // 설정 상세 팝업
    [SerializeField] GameObject MultiPopup;   // 멀티 플레이 팝업 (방 생성/참가 UI)

    [Header("사운드 셋팅 UI")]
    [SerializeField] SoundManager soundManager;  // 재생/볼륨 제어를 위한 사운드 매니저 참조
    [SerializeField] Slider bgmVolumeSlider;     // BGM 볼륨 슬라이더
    [SerializeField] Slider SfxVolumeSlider;     // SFX 볼륨 슬라이더

    [SerializeField] Button bgmTestButton;       // BGM 테스트 버튼 (옵션) 아직 미사용
    [SerializeField] Button SfxTestButton;       // SFX 테스트 버튼 (옵션)

    [SerializeField] GameObject MainButton;      // 메인(인트로) 버튼 그룹(다시 보이게 할 때 사용)
    [SerializeField] GameObject winloselog;      //승리, 패배 전적


    // 내부 상태 변수
    bool isMulti;   
    bool isTip;
    bool iswinloselog;

    #region 안 쓰는 기능
    // 배경들을 미리 할당해두고 랜덤으로 하나를 켜는 용도
    // [SerializeField] GameObject[] background = new GameObject[3];
    #endregion

    void Start()
    {
        // Time.timeScale = 0f;

        // 버튼을 Inspector에 연결해놓았다면 클릭 리스너를 등록
        if (settingButton != null)
            settingButton.onClick.AddListener(OnSettingPopup);

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitSetting);

        // 사운드 설정 초기화(슬라이더 값에 사운드매니저의 현재 값 반영)
        OnSoundSetting();

        // 기본적으로 게임이 시작되지 않은 상태로 설정
        isStart = false;
    }

    #region 안 쓰는 기능
    // 화면 배경을 랜덤으로 하나만 활성화 / 나머지는 비활성화인데 현재 게임에선 필요 없어보임
    // void RandomBackground()
    // {
    //     // 모든 백그라운드 비활성화
    //     for (int i = 0; i < background.Length; i++)
    //     {
    //         if (background[i] != null)
    //             background[i].SetActive(false);
    //     }

    //     // 랜덤 인덱스를 뽑아서 해당 배경만 활성화
    //     int randomIndex = UnityEngine.Random.Range(0, background.Length);
    //     if (background[randomIndex] != null)
    //         background[randomIndex].SetActive(true);
    // }

    #endregion
    // 사운드 슬라이더와 SoundManager를 연결(초기값 설정 및 이벤트 구독)
    public void OnSoundSetting()
    {
        if (soundManager == null) return;

        if (bgmVolumeSlider != null)
        {
            // 사운드 매니저에서 저장한 현재 볼륨을 슬라이더에 적용
            bgmVolumeSlider.value = soundManager.GetBGMVolume();
            // 슬라이더 값이 변경될 때 OnBGMVolumeChanged를 호출
            bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        }

        // SFX 슬라이더 초기화 및 변경 콜백 설정
        if (SfxVolumeSlider != null)
        {
            SfxVolumeSlider.value = soundManager.GetSFXVolume();
            SfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        // 미구현 버튼 클릭시 테스트 사운드가 나옴
        // if (bgmTestButton != null) bgmTestButton.onClick.AddListener(() => soundManager.PlayTestBGM());
        // if (SfxTestButton != null) SfxTestButton.onClick.AddListener(() => soundManager.PlayTestSFX());
    }

    // 슬라이더에서 볼륨이 변경되었을 때 호출되는 콜백
    private void OnSFXVolumeChanged(float Value)
    {
        if (soundManager != null)
            soundManager.SetSFXVolume(Value); // SoundManager에 변경값 전달
    }

    private void OnBGMVolumeChanged(float Value)
    {
        if (soundManager != null)
            soundManager.SetBGMVolume(Value);
    }

    // 게임을 실제로 시작(인트로에서 플레이로 넘어갈 때)하는 처리
    public void OnSelectLevelSetting()
    {
        
        introPopup.SetActive(false);
        startLevelPopup.SetActive(true);

        #region 안 쓰는 기능
        // 게임 배경 랜덤 선택
        // RandomBackground();

        // 게임 루프 재시작
        // Time.timeScale = 1f;
        #endregion
        isStart = true; // 게임이 시작되었음을 표시

        // BGM 재생 시작
        if (soundManager != null)
            soundManager.SetBGMSound("Play");
    }

    public void OnStartLocalPlay()
    {
        Managers.Game.EnterLocalPlay();

        isStart = true; // 게임이 시작되었음을 표시

        // BGM 재생 시작
        if (soundManager != null)
            soundManager.SetBGMSound("Play");

    }

    //팝업 닫기
    public void OnExitSetting()
    {
        if (popup)
        {
            popup.SetActive(false);
            dim.SetActive(false);

            settingPopup.SetActive(false);
            ButtonPopup.SetActive(true);
        }

        // 게임이 이미 시작된 상태라면 시간 흐름 복구(게임 재개)
        if (isStart)
            // Time.timeScale = 1f;

        // 게임이 시작되지 않은 상태(인트로 화면)로 돌아갈 때
        if (!isStart)
        {
            introPopup.SetActive(true);
            ButtonPopup.SetActive(true);
            // Time.timeScale = 0f;
        }

        isSetting = false;

        // 멀티/팁 팝업이 열려있었다면 닫고 상태 초기화
        if (isMulti || isTip)
        {
            MultiPopup.SetActive(false);

            isMulti = false;
            isTip = false;
        }
    }

    // 멀티 플레이 팝업을 열 때 호출되는 함수
    public void OnMultiPopup()
    {
        MultiPopup.SetActive(true);
        ButtonPopup.SetActive(false);
        isMulti = true;
    }

    // 설정 팝업을 열 때 호출 (기존 OnPopup을 단순화한 버전)
    public void OnSettingPopup()
    {
        settingPopup.SetActive(true);
    }

    public void OnLogPupup()
    {
        if (winloselog != null && !iswinloselog)
        {
            winloselog.SetActive(true);
            iswinloselog = true;
        }

        else
        {
            winloselog.SetActive(false);
            iswinloselog = false;
        }
    }

    public void OnExit()
    {
        Debug.Log("게임 종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //에디터 실행중지
#else
        Application.Quit();
#endif
    }

    //메인(인트로) 화면으로 돌아가는 공통 처리
    // 현재 사용 안 함
     public void OnToMain()
    {
        isStart = false; // 게임 재시작 상태 초기화
        Managers.Game.GoToMainMenu();

        // 메인 버튼 그룹을 활성화 (없으면 null 체크)
        if (MainButton != null && !MainButton.activeSelf)
            MainButton.SetActive(true);

        // BGM을 인트로 트랙으로 변경 (사운드 매니저 싱글톤 사용 가정)
        SoundManager.Instance.SetBGMSound("Intro");
        if (SoundManager.Instance.bgmsource != null)
            SoundManager.Instance.bgmsource.volume = SoundManager.Instance.bgmVolume;

        // 인트로는 시간 정지 상태로 유지
        // Time.timeScale = 0f;
    }
}