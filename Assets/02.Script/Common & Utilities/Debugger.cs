using System;
using UnityEngine;

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
    [SerializeField] Timer testTimer;
    [SerializeField] float testTime = 3f;

    #region UI

    public void Test_OpenPopUpFront() {
        var ui = Managers.UI.OpenUI<PopUpUI>(Define.Value.POP_UP_UI_PATH);
        Managers.UI.FrontCanvas.ActiveTouchBlockPanel(true);
        ui.transform.SetParent(Managers.UI.FrontCanvas.transform);
        ui.transform.SetAsLastSibling();
        ui.transform.localPosition = Vector3.zero;
        var popup = ui as PopUpUI;
        popup.InitPopUp(new PopUpInfo("대전 상대를 찾는중..", string.Empty,
            new PopUpButtonInfo("취소", () => Managers.UI.CloseUI<PopUpUI>())));
    }
    public void Test_OpenPopUpMain() {
        Managers.UI.FrontCanvas.ActiveTouchBlockPanel(false);
        var ui = Managers.UI.OpenUI<PopUpUI>(Define.Value.POP_UP_UI_PATH);
        ui.transform.SetParent(Managers.UI.MainCanvas.transform);
        ui.transform.SetAsLastSibling();
        ui.transform.localPosition = Vector3.zero;
    }
    public void Test_HidePopUp() {
        Managers.UI.FrontCanvas.ActiveTouchBlockPanel(false);
        Managers.UI.CloseUI<PopUpUI>();
    }

    #endregion

    #region Match Making

    public void Test_QuickMatch() {
        //Managers.Network.QuickMatch();
    }

    #endregion

    #region Player

    public void Test_InitPlayer() {
        var p = new PlayerInfo(playerName, "1");
        Managers.Player.InitPlayer(targetPlayer, p);
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


    private void Test_CheckTimeOver(float currentTime, float totalTime) {
        Debug.LogAssertion($"<color=magenta>time over</color>");
    }

    private void Test_CheckTime(float currentTime, float totalTime) {
        Debug.LogAssertion($"{currentTime} / {totalTime}");
    }

    #endregion

    private void Awake() {
        if (testTimer != null) {
            testTimer.OnTimeChanged += Test_CheckTime;
            testTimer.OnTimeOver += Test_CheckTimeOver;
        }

        //Managers.UI.GetUI<SampleTimeUI>().CloseUI();
    }
}
