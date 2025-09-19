using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Define.Type.Player PlayerType => playerType;

    [SerializeField] Define.Type.Player playerType;
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] PlayerInfo playerInfo;

    [SerializeField] GameObject turnMarkObj;
    [SerializeField] TextMeshProUGUI gameResultText;
    [SerializeField] GameObject thinkingText;

    [SerializeField] PlaceButton placeButton;

    public void InitPlayer(PlayerInfo playerInfo) {
        this.playerInfo = playerInfo;
        playerNameText.text = playerInfo.PlayerName;
        playerType = playerInfo.PlayerType;

        if (rankText != null) {
            rankText.text = playerInfo.Rank;
        }
    }

    public void ActiveTurnMarkUI(bool active) {
        turnMarkObj.SetActive(active);
    }
    public void ActiveRankUI(Define.Type.Game gameType) {
        switch (gameType) {
            case Define.Type.Game.Single:
            case Define.Type.Game.Local:
                rankText.gameObject.SetActive(false);
                break;
            case Define.Type.Game.Multi:
                rankText.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ShowGameResult(Define.State.GameResult gameResult) {
        //gameResultText.text = gameResult == Define.State.GameResult.BlackStoneWin ? "½Â" : "ÆÐ";
    }

    public void ShowThinkingText(bool aiTurn) {
        thinkingText.gameObject.SetActive(aiTurn);
    }

    public void ActivePlaceButton(bool active) {
        placeButton.gameObject.SetActive(active);
    }
    public void ActiveGameResultText(bool active) {
        gameResultText.gameObject.SetActive(active);
    }
}

[Serializable]
public class PlayerInfo // userdata·Î ½áµµ ±¦ÂúÀº°¡ ai Àü¿ë userdata ÇÊ¿ä
{
    public string PlayerName => playerName;
    public string Rank => rank;
    public Define.Type.Player PlayerType => playerType;

    [SerializeField] Define.Type.Player playerType;
    [SerializeField] string playerName;
    [SerializeField] string rank;

    public PlayerInfo(string playerName, string rank) {
        this.playerName = playerName;
        this.rank = rank;
    }

    public PlayerInfo SetPlayerType(Define.Type.Player playerType) {
        this.playerType = playerType;
        return this;
    }
}
