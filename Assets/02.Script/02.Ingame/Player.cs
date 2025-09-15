using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] string playerName;
    [SerializeField] TextMeshProUGUI rankText;
    //[SerializeField] TimeUI timeUI;

    public void InitPlayer(PlayerInfo playerInfo) {
        playerName = playerInfo.PlayerName;
        rankText.text = playerInfo.Rank;
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
