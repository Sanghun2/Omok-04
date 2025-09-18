using System;
using UnityEngine;
using UnityEngine.Events;

public class Debugger : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Define.Type.Player targetPlayer;
    [SerializeField] string playerName;

    [Space]
    [Header("Scene")]
    [SerializeField] Define.Type.Scene targetScene;

    [Space]
    [Header("Timer")]
    [SerializeField] float customTimeScale = 1;
    [SerializeField] Timer testTimer;
    [SerializeField] float testTime = 3f;

    #region UI

    public void Test_OpenPopUpFront() {
        Managers.UI.OpenPopUp(new PopUpInfo(
            "대전 상대를 찾는중..", 
            string.Empty,
            new PopUpButtonInfo(
                "취소", () => Managers.UI.CloseUI<PopUpUI>()                
            )), Define.Type.PopUpParent.Front);

    }
    public void Test_OpenPopUpMain() {
        Managers.UI.OpenPopUp(new PopUpInfo(
            "대전 상대를 찾는중..",
            string.Empty,
            new PopUpButtonInfo(
                "취소", () => Managers.UI.CloseUI<PopUpUI>()
            )));
    }
    public void Test_HidePopUp() {
        Managers.UI.ClosePopUp();
    }

    #endregion

    #region Match Making

    public void Test_QuickMatch() {
        Managers.Network.FindMatch();
        Managers.UI.OpenPopUp(PopUpInfo.FindMatchPlayer, Define.Type.PopUpParent.Front);
    }

    #endregion

    #region Player

    public void Test_InitPlayer() {
        var p = new PlayerInfo(playerName, "1");
        Managers.Player.InitPlayerUI(targetPlayer, p);
    }

    #endregion

    #region Scene

    public void Test_ShowScene() {
        Managers.Scene.ShowScene(targetScene);
    }
    public void Test_GoToMenu() {
        Managers.Game.GoToMainMenu();
    }
    public void Test_GoToLogIn() {
        Managers.Game.GoToLogIn();
    }
    public void Test_GoToInGame() {
        Managers.Game.EnterLocalPlay();
    }

    #endregion

    #region Time

    public void Test_ToggleTimerObj() {
        var timerUI = Managers.UI.GetUI<SampleTimeUI>();
        if (timerUI.IsOpened) timerUI.CloseUI();
        else timerUI.OpenUI();
    }
    public void Test_SetTimeAsDefault() {
        Managers.Time.RegisterTimer(testTimer);
        Managers.UI.GetUI<SampleTimeUI>().AddTimerEvent(testTimer);
        testTimer.SetTime(testTime, testTime);
    }

    public void Test_StartCount() {
        testTimer.StartCount();
    }

    private void Test_CheckTime(float currentTime, float totalTime) {
        Debug.LogAssertion($"{currentTime} / {totalTime}");
    }

    public void Test_ChangeTimeScale() {
        Managers.Time.Timer.CustomTimeScale = customTimeScale;
    }

    #endregion

    private void Awake() {
        if (testTimer != null) {
            testTimer.OnTimeChanged += Test_CheckTime;
            testTimer.OnTimeOver += Test_CheckTimeOver;
        }

        //Managers.UI.GetUI<SampleTimeUI>().CloseUI();
    }

    private void Test_CheckTimeOver() {
        Debug.LogAssertion($"<color=magenta>time over</color>");
    }
}

public partial class UIManager
{
    public void OpenPopUp(PopUpInfo popUpInfo, Define.Type.PopUpParent popUpParent=Define.Type.PopUpParent.Main) {
        var ui = Managers.UI.OpenUI<PopUpUI>(Define.Path.POP_UP_UI_PATH);
        PopUpUI popUpUI = ui as PopUpUI;
        Managers.UI.FrontCanvas.ActiveTouchBlockPanel(popUpParent == Define.Type.PopUpParent.Front);
        popUpUI.transform.SetParent(popUpParent == Define.Type.PopUpParent.Front ? Managers.UI.FrontCanvas.transform : Managers.UI.MainCanvas.transform);
        popUpUI.transform.SetAsLastSibling();
        popUpUI.transform.localPosition = Vector3.zero;
        popUpUI.transform.localScale = Vector3.one;
        popUpUI.InitPopUp(popUpInfo);
    }

    public void ClosePopUp() {
        Managers.UI.FrontCanvas.ActiveTouchBlockPanel(false);
        Managers.UI.CloseUI<PopUpUI>();
    }
}