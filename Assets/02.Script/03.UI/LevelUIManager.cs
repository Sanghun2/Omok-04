using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : UIBase
{
    public Define.Type.GameLevel level;

    [SerializeField] Button EasyModeButton;
    [SerializeField] Button NormalModeButton;
    [SerializeField] Button HardModeButton;

    [SerializeField] private UIBase inGameUI;

    // UI 초기화 시 버튼 이벤트 연결
    public override void InitUI()
    {
        base.InitUI();

        EasyModeButton.onClick.AddListener(() => SetLevel(Define.Type.GameLevel.Easy));
        NormalModeButton.onClick.AddListener(() => SetLevel(Define.Type.GameLevel.Normal));
        HardModeButton.onClick.AddListener(() => SetLevel(Define.Type.GameLevel.Hard));
    }

    private void SetLevel(Define.Type.GameLevel selectedLevel)
    {
        level = selectedLevel;
        Debug.Log($"선택된 난이도 : {level}");

        // UI 전환
        this.CloseUI();         
        inGameUI.OpenUI();

        //Managers.Game.StartSinglePlay(level);
    }
}
