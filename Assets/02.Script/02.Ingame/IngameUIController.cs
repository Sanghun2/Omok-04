using System;
using System.Collections;
using System.Linq;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIController : MonoBehaviour
{
    [SerializeField] private PlayerUI player1_UI;
    [SerializeField] private PlayerUI player2_UI;

    [SerializeField] private Slider timeSlider;

    //[SerializeField] private Button p2OKButton; // ����� �� ���� ��ư �̰͸� �۵���
    [SerializeField] private TextMeshProUGUI timerText;

    #region Public

    public void InitUIs(Define.Type.Game gameType, Define.Type.Player firstPlayer) {
        // UI
        switch (gameType) {
            case Define.Type.Game.Single:
                if (player2_UI != null) {
                    player2_UI.InitPlayer(new PlayerInfo("AI", string.Empty));
                }

                player2_UI.ActivePlaceButton(false);
                break;
            case Define.Type.Game.Local:
                player1_UI.InitPlayer(new PlayerInfo("P1", string.Empty));
                player2_UI.InitPlayer(new PlayerInfo("P2", string.Empty));
                break;
            case Define.Type.Game.Multi:
                break;
            default:
                break;
        }

        player1_UI.ActiveRankUI(gameType);
        player2_UI.ActiveRankUI(gameType);

        player1_UI.ShowThinkingText(false);
        player2_UI.ShowThinkingText(false);

        player1_UI.ActiveGameResultText(false);
        player2_UI.ActiveGameResultText(false);


        player1_UI.ActivePlaceButton(true);
        player2_UI.ActivePlaceButton(gameType != Define.Type.Game.Single);

        UpdateTurnUI(firstPlayer);

        Debug.LogAssertion($"init ui");
    }

    /// <summary>
    /// ���� ���� �÷��̾� UI ǥ��
    /// </summary>
    /// <param name="aiTurn"></param>
    public void ActiveTurnMarkUI(bool aiTurn) {
        player1_UI.ActiveTurnMarkUI(!aiTurn);
        player2_UI.ActiveTurnMarkUI(aiTurn);

        if (Managers.Game.CurrentGameType == Define.Type.Game.Single) player2_UI.ShowThinkingText(aiTurn);
    }

    #endregion


    #region Private

    private void Start() {
        Timer timer = Managers.Time.GetTimer();
        if (timer != null)
            BindTimeEvent(timer);

        Managers.Turn.OnTurnChanged.AddListener((player) => {
            // Local ������ ���� �� ���� ���� ���� �÷��̾� ǥ�� Ȱ��ȭ
            if (Managers.Game.CurrentGameType == Define.Type.Game.Local) {
                UpdateTurnUI(player);
            }
        });
    }
    // Ÿ�̸� �̺�Ʈ ����
    private void BindTimeEvent(Timer timer) {
        timer.OnTimeChanged -= UpdateTimerUI;
        timer.OnTimeChanged += UpdateTimerUI;
    }

    // TextMeshPro UI ����
    private void UpdateTimerUI(float currentTime, float totalTime) {
        int seconds = Mathf.RoundToInt(currentTime);
        timerText.text = seconds.ToString();
        timeSlider.value = currentTime / totalTime;
    }
    private void UpdateTurnUI(Define.Type.Player playerType) {
        player1_UI.ActiveTurnMarkUI(playerType == Define.Type.Player.Player1);
        player2_UI.ActiveTurnMarkUI(playerType != Define.Type.Player.Player1);
    }

    #endregion
}
