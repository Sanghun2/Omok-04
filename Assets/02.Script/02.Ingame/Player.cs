using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] PlayerInfo playerInfo;
    //[SerializeField] TimeUI timeUI;

    public void InitPlayer(PlayerInfo playerInfo) {
        this.playerInfo = playerInfo;
        playerNameText.text = playerInfo.PlayerName;

        if (rankText != null) {
            rankText.text = playerInfo.Rank;
        }
    }
}

[Serializable]
public class PlayerInfo
{
    public string PlayerName => playerName;
    public string Rank => rank;

    [SerializeField] string playerName;
    [SerializeField] string rank;
}
