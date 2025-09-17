using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public Define.Type.Player PlayerType => playerType;

    [SerializeField] Define.Type.Player playerType;
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] PlayerInfo playerInfo;
    //[SerializeField] TimeUI timeUI;

    public void InitPlayer(PlayerInfo playerInfo) {
        this.playerInfo = playerInfo;
        playerNameText.text = playerInfo.PlayerName;
        playerType = playerInfo.PlayerType;

        if (rankText != null) {
            rankText.text = playerInfo.Rank;
        }
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

    public PlayerInfo(string playerName, string rank) {
        this.playerName = playerName;
        this.rank = rank;
    }

    public PlayerInfo SetPlayerType(Define.Type.Player playerType) {
        this.playerType = playerType;
        return this;
    }
}
