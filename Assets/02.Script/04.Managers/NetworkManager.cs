using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public interface INetworkController
{
    #region Room Set & Network Set
    void InitConnect();
    void OnDisconnected();

    #endregion

    #region Match Making

    void QuickMatch(int matchPlayers);

    void OnConnectedToMaster();
    void OnJoinedRoom();


    #endregion

    #region InGame

    void ChooseFirstPlayer(Define.Type.Player firstPlayer);

    void StartGame();
    bool PlaceReady(Define.Type.Player turnPlayer);
    void PlaceStone(Define.Type.Player playerType, Vector2Int stonePos);
    void SetTimer(Define.Type.Player playerType, float time);

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

    public void QuickMatch() {
        networkController.QuickMatch(matchPlayers:2);
    }

    public void ChooseFirstPlayer(Define.Type.Player firstPlayer) {
        networkController.ChooseFirstPlayer(firstPlayer);
    }


    public void StartGame() {
        networkController.StartGame();
    }
    public void PlaceStone(Define.Type.Player playerType, Vector2Int stonePos) {
        networkController.PlaceStone(playerType, stonePos);
    }
    public void SetTimer(Define.Type.Player playerType, float time) {
        networkController.SetTimer(playerType, time);
    }


    public void FinishGame(Define.Type.Player winner) {
        networkController.FinishGame(winner);
    }

    public void Initialize() {
        if (IsInit) return;

        networkController = Photon;
        networkController.InitConnect();
        isInit = true;
    }
}
