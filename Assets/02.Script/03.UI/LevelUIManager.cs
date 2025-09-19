using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : UIBase
{
    public Define.Type.GameLevel level;

    [SerializeField] Button EasyModeButton;
    [SerializeField] Button NormalModeButton;
    [SerializeField] Button HardModeButton;
    [SerializeField] Button GoBackButton;
    [SerializeField] SoundManager soundManager; //명철

    // [SerializeField] private UIBase inGameUI;

    // UI 초기화 시 버튼 이벤트 연결
    public override void InitUI()
    {
        base.InitUI();

        EasyModeButton.onClick.AddListener(() => SetLevel(Define.Type.GameLevel.Easy));
        NormalModeButton.onClick.AddListener(() => SetLevel(Define.Type.GameLevel.Normal));
        HardModeButton.onClick.AddListener(() => SetLevel(Define.Type.GameLevel.Hard));

        GoBackButton.onClick.AddListener(() =>
        {
            // Level UI를 닫음
            this.CloseUI();

            // 부모(Main Menu UI)를 열기
            if (transform.parent != null)
            {
                var parentUI = transform.parent.GetComponent<UIBase>();
                if (parentUI != null)
                {
                    parentUI.OpenUI();
                }
            }
            var introButton = transform.parent.Find("[Intro]Button");
            if (introButton != null)
            {
                introButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("[Intro]Button을 찾을 수 없습니다.");
            }
        });

    }

    public void SetLevel(Define.Type.GameLevel selectedLevel)
    {
        level = selectedLevel;
        Debug.Log($"선택된 난이도 : {level}");

        // UI 전환
        //this.CloseUI();         
        //inGameUI.OpenUI();
        gameObject.SetActive(false);
        Managers.Game.StartSinglePlay(level);

        // BGM 재생 시작 명철
        if (soundManager != null)
            soundManager.SetBGMSound("Play");
    }
}
