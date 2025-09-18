using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public interface INetworkController : IInitializable
{
    public Define.Type.Player LocalPlayerType { get; }

    public delegate void MatchHandler();
    public event MatchHandler OnMatchFound;
    public event MatchHandler OnCancelMatch;
    public event MatchHandler OnAllPlayerReady;
    public event MatchHandler OnGameStart;

    public delegate void PlayerHandler(Define.Type.Player targetPlayer);
    public event PlayerHandler OnChooseFirstPlayer; // who place stone
    public event PlayerHandler OnPlayerInit;
    public event PlayerHandler OnPlaceStone; // who place stone
    public event PlayerHandler OnTurnChanged; // current turn player input
    public event PlayerHandler OnGameFinish; // win player

    #region Room Set & Network Set
    void InitConnect();
    void OnDisconnected();

    #endregion

    #region Match Making

    void QuickMatch(int matchPlayers);
    void CancelFindMatch();

    void OnConnectedToMaster();
    void OnJoinedRoom();


    #endregion

    #region InGame

    void SetPlayerAndFirstPlayer(Define.Type.Player playerType, Define.Type.Player firstPlayer);

    bool PlaceReady(Define.Type.Player turnPlayer);
    void PlaceStone(Define.Type.Player playerType, Vector2Int stonePos);
    void SetTimer(float time);

    void FinishGame(Define.Type.Player winner);

    #endregion
}

public class NetworkManager : IInitializable
{
    public PhotonNetworkController Photon
    {
        get
        {
            if (photonNetworkController == null) {
                photonNetworkController = Managers.Instance.GetComponentInChildren<PhotonNetworkController>();
            }

            return photonNetworkController;
        }
    }

    public bool IsInit => isInit;

    private bool isInit;
    private INetworkController networkController;
    private PhotonNetworkController photonNetworkController;

    private Define.State.Match currentMatchState;

    public void FindMatch() {
        currentMatchState = Define.State.Match.MatchMaking;
        networkController.QuickMatch(matchPlayers:2);
    }
    public void CancelFindMatch() {
        currentMatchState = Define.State.Match.None;
        networkController.CancelFindMatch();
        Debug.LogAssertion($"<color=orange>대전 상대 탐색 중지</color>");
    }

    public void SetPlayerAndFirstPlayer(Define.Type.Player playerType, Define.Type.Player firstPlayer) {
        networkController.SetPlayerAndFirstPlayer(playerType, firstPlayer);
    }


    public void StartGame() {

    }
    public void PlaceStone(Define.Type.Player playerType, Vector2Int stonePos) {
        networkController.PlaceStone(playerType, stonePos);
    }
    public void SetTimer(float time) {
        networkController.SetTimer(time);
    }


    public void FinishGame(Define.Type.Player winner) {
        networkController.FinishGame(winner);
    }

    #region Capsule

    public void Initialize() {
        if (IsInit) return;

        networkController = Photon;
        networkController.Initialize();
        isInit = true;
    }

    #endregion
}
