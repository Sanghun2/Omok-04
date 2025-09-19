using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Define.Type.Player PlayerType => playerType;
    public PlaceButton PlaceButton => placeButton;


    [Header("[  Assign  ]")]
    [SerializeField][Tooltip("SetPlayerType���� ���� ���� ����. ���Ǹ� ���� �⺻�� �Ҵ� �� ����")] Define.Type.Player playerType;
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] GameObject turnMarkObj;
    [SerializeField] TextMeshProUGUI gameResultText;
    [SerializeField] PlaceButton placeButton;

    [Space]
    [SerializeField] GameObject thinkingText;

    private PlayerInfo playerInfo;

    public void InitPlayerUI(PlayerInfo playerInfo) {
        this.playerInfo = playerInfo;
        playerNameText.text = playerInfo.PlayerName;

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
        //gameResultText.text = gameResult == Define.State.GameResult.BlackStoneWin ? "��" : "��";
    }

    public void ShowThinkingText(bool aiTurn) {
        thinkingText.gameObject.SetActive(aiTurn);
    }

    public void ActivePlaceButton(bool active) {
        placeButton.gameObject.SetActive(active);
    }
    public void ActiveGameResultText(bool active, string result = "") {
        gameResultText.gameObject.SetActive(active);
        gameResultText.text = result;
    }
}

[Serializable]
public class PlayerInfo // userdata�� �ᵵ �������� ai ���� userdata �ʿ�
{
    public string PlayerName => playerName;
    public string Rank => rank;
    public Define.Type.Player PlayerType => playerType;

    [SerializeField] Define.Type.Player playerType;
    [SerializeField] string playerName;
    [SerializeField] string rank;

    /// <summary>
    /// user name, rank. rank �⺻���� string.empty
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="rank"></param>
    public PlayerInfo(string playerName, string rank) {
        this.playerName = playerName;
        this.rank = rank;
    }

    public PlayerInfo SetPlayerType(Define.Type.Player playerType) {
        this.playerType = playerType;
        return this;
    }
}
