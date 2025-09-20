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


    #region Public

    public void InitUIs(Define.Type.Game gameType, Define.Type.Player firstPlayer) {
        // UI
        switch (gameType) {
            case Define.Type.Game.Single:
                if (player2_UI != null) {
                    player2_UI.InitPlayerUI(new PlayerInfo(Define.Value.DEFAULT_AI_NAME, string.Empty));
                }

                player1_UI.ActivePlaceButton(true);
                player2_UI.ActivePlaceButton(false);

                player1_UI.PlaceButton.ResetPlayerType();
                player2_UI.PlaceButton.ResetPlayerType();
                break;
            case Define.Type.Game.Local:
                player1_UI.InitPlayerUI(new PlayerInfo(Define.Value.DEFAULT_PLAYER1_NAME, string.Empty));
                player2_UI.InitPlayerUI(new PlayerInfo(Define.Value.DEFAULT_PLAYER2_NAME, string.Empty));

                player1_UI.ActivePlaceButton(true);
                player2_UI.ActivePlaceButton(true);

                player1_UI.PlaceButton.ResetPlayerType();
                player2_UI.PlaceButton.ResetPlayerType();
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

        UpdateTurnUI(firstPlayer);

        Debug.LogAssertion($"init ui");
    }
    public void InitPlayerUI(Define.Type.Player targetPlayer, PlayerInfo playerInfo) {
        var targetPlayerUI = targetPlayer == Define.Type.Player.Player1 ? player1_UI : player2_UI;
        targetPlayerUI.InitPlayerUI(playerInfo);
    }
    public void SetPlayerUIAsLeft(Define.Type.Player targetPlayer) {
        var targetPlayerUI = GetPlayerUI(targetPlayer);
        targetPlayerUI.InitPlayerUI(new PlayerInfo(Define.Value.LEFT_PLAYER_NAME, string.Empty));
    }

    public void ShowGameResult(Define.State.GameResult gameResult) {
        switch (gameResult) {
            case Define.State.GameResult.DRAW:
                player1_UI.ActiveGameResultText(true, "무");
                player2_UI.ActiveGameResultText(true, "무");
                break;
            case Define.State.GameResult.BlackStoneWin:
                player1_UI.ActiveGameResultText(true, "승");
                player2_UI.ActiveGameResultText(true, "패");
                break;
            case Define.State.GameResult.WhiteStoneWin:
                player1_UI.ActiveGameResultText(true, "패");
                player2_UI.ActiveGameResultText(true, "승");
                break;
        }
    }
    public void ActivePlaceButton(Define.Type.Player targetPLayer, bool active) {
        var targetPlayerUI = targetPLayer == Define.Type.Player.Player1 ? player1_UI : player2_UI;
        targetPlayerUI.ActivePlaceButton(active);
    }

    /// <summary>
    /// 현재 턴인 플레이어 UI 표시
    /// </summary>
    /// <param name="aiTurn"></param>
    public void ActiveTurnMarkUI(bool aiTurn) {
        player1_UI.ActiveTurnMarkUI(!aiTurn);
        player2_UI.ActiveTurnMarkUI(aiTurn);

        if (Managers.Game.CurrentGameType == Define.Type.Game.Single) player2_UI.ShowThinkingText(aiTurn);
    }


    public PlayerUI GetPlayerUI(Define.Type.Player playerType) {
        return playerType == Define.Type.Player.Player1 ? player1_UI : player2_UI;
    }
    public void UpdateTurnUI(Define.Type.Player playerType) {
        player1_UI.ActiveTurnMarkUI(playerType == Define.Type.Player.Player1);
        player2_UI.ActiveTurnMarkUI(playerType != Define.Type.Player.Player1);
    }


    #endregion


    #region Private

    private void Start() {
        Managers.Turn.OnTurnChanged.AddListener((player) => {
            if (Managers.Game.CurrentGameType == Define.Type.Game.Single) return;
            // Local 게임일 때만 돌 색깔 기준 현재 플레이어 표시 활성화
            UpdateTurnUI(player);
        });


        player1_UI.PlaceButton.ResetPlayerType();
        player2_UI.PlaceButton.ResetPlayerType();
    }


    #endregion
}
