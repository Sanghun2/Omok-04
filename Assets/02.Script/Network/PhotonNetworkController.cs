using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class PhotonNetworkController : MonoBehaviourPunCallbacks, INetworkController, IInitializable
{
    public Define.Type.Player LocalPlayerType => (Define.Type.Player)PhotonNetwork.LocalPlayer.ActorNumber;
    public int LocalActorNumber => PhotonNetwork.LocalPlayer.ActorNumber;

    public bool IsInit => isInit;
    private bool isInit;
    private SortedSet<int> readiedPlayers = new SortedSet<int>();

    public event INetworkController.MatchHandler OnMatchFound; //
    public event INetworkController.MatchHandler OnCancelMatch; //
    public event INetworkController.MatchHandler OnAllPlayerReady;
    public event INetworkController.MatchHandler OnGameStart; //

    public event INetworkController.PlayHandler OnChooseFirstPlayer; //
    public event INetworkController.PlayHandler OnGameInit;
    public event INetworkController.PlayHandler OnPlaceStone;
    public event INetworkController.PlayHandler OnTurnChanged;
    public event INetworkController.PlayHandler OnGameFinish;

    #region interface

    public void InitConnect() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomID) {
        Debug.LogAssertion($"<color=cyan>room ���� �õ�</color>");
    }
    public void JoinRoom(string roomID) {
        Debug.LogAssertion($"<color=cyan>room ���� �õ�</color>");
    }

    public override void OnConnectedToMaster() {
        Debug.LogAssertion($"<color=cyan>master ����</color>");
        PhotonNetwork.NickName = "Test Player";
    }
    public void OnDisconnected() {
        Debug.LogAssertion($"<color=cyan>master ���� ����</color>");
    }
    

    #endregion

    #region Match Making

    public void QuickMatch(int maxPlayers) {
        readiedPlayers.Clear();
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions:new RoomOptions() { MaxPlayers= maxPlayers });
    }

    public void CancelFindMatch() {
        PhotonNetwork.LeaveRoom();
        OnCancelMatch?.Invoke();
    }

    public override void OnJoinedRoom() {
        Debug.LogAssertion($"<color=cyan>room ����. ���� �ο�:{PhotonNetwork.CurrentRoom.PlayerCount}, �ִ� �ο�: {PhotonNetwork.CurrentRoom.MaxPlayers}</color>");
        gameObject.AddComponent<PhotonView>();
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
            Debug.LogAssertion($"���� ���� �õ�");
            OnMatchFound?.Invoke();
            var routineID = Managers.Coroutine.WaitFrame(1, SetFirstPlayer);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.LogAssertion($"<color=cyan>left room. ���� �ο�:{PhotonNetwork.CurrentRoom.PlayerCount}, �ִ� �ο�: {PhotonNetwork.CurrentRoom.MaxPlayers}</color>");
    }

    #endregion

    #region InGame
    public void SetPlayerAndFirstPlayer(Define.Type.Player playerType, Define.Type.Player firstPlayer) {
        OnChooseFirstPlayer?.Invoke(firstPlayer);
    }
    public void StartGame() {
        photonView.RPC(nameof(RPC_GameStart), RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Place Stone�� ������ �� �ִ��� �Ǵ�
    /// </summary>
    /// <param name="turnPlayer"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool PlaceReady(Define.Type.Player turnPlayer) {
        throw new System.NotImplementedException();
    }
    public void PlaceStone(Define.Type.Player playerType, Vector2Int pos) {
        OnPlaceStone?.Invoke(playerType);
    }

    public void SetTimer(float time) {
        throw new System.NotImplementedException();
    }

    public void FinishGame(Define.Type.Player winner) {
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC(nameof(RPC_FinishGame), RpcTarget.AllViaServer, winner);
        }
    }

    #endregion

    #region capsulation

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogAssertion($"<color=cyan>disconnected: {cause}</color>");
    }

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    
   

    public void Initialize() {
        InitConnect();
        OnChooseFirstPlayer += InitGame;
        OnGameInit += ReadyPlayer;
        OnAllPlayerReady += SetGameStatePlay;// game start���� ���� ó�� �� readyó��. ��� ready�̸� game state inprogress�� ����
    }

    private void ReadyPlayer(Define.Type.Player localPayerType) {
        photonView.RPC(nameof(RPC_PlayerReady), RpcTarget.MasterClient, (int)localPayerType);
    }

    private void SetFirstPlayer() {
        photonView.RPC(nameof(RPC_SetFirstPlayer), RpcTarget.AllViaServer);
    }

    
    private void InitGame(Define.Type.Player firstPlayer) {
        // ���� step���� master client�� �ƴϸ� return �����Ƿ� �� �ڵ�� master�� ������
        photonView.RPC(nameof(RPC_InitGame), RpcTarget.AllViaServer, firstPlayer);
    }

    private void PlayerReady() {
        photonView.RPC(nameof(RPC_PlayerReady), RpcTarget.MasterClient, LocalActorNumber);
    }
    private void SetGameStatePlay() {
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC(nameof(RPC_SetGameStateplay), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void RPC_SetFirstPlayer() {

        if (!PhotonNetwork.IsMasterClient) return;

        int firstPlayerIndex = Random.Range(1, 3);
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.LogAssertion($"���õ� first player index: {firstPlayerIndex}, local actor: {actorNumber}");
        Define.Type.Player firstPlayer = actorNumber == firstPlayerIndex ? Define.Type.Player.Player1 : Define.Type.Player.Player2;

        Debug.LogAssertion($"local player type: {LocalPlayerType}, first player: {firstPlayer == LocalPlayerType}");
        OnChooseFirstPlayer?.Invoke(firstPlayer);
    }

    [PunRPC]
    private void RPC_InitGame(Define.Type.Player firstPlayer) {
        Managers.UI.ClosePopUp();
        Managers.Game.EnterMultiPlay();

        // ��� �÷��̾ ���������� board �ʱ�ȭ �� �÷��̾� �ʱ�ȭ
        var gameLogic = new GameLogic(Managers.Board.Board, Define.Type.Game.Multi);

        if (LocalPlayerType == Define.Type.Player.Player1) {
            gameLogic.firstPlayerState = new PlayerState(firstPlayer == LocalPlayerType);
            gameLogic.secondPlayerState = new PlayerState(firstPlayer == LocalPlayerType);
            Debug.LogAssertion($"first player�� set");
        }
        else {
            gameLogic.firstPlayerState = new PlayerState(firstPlayer == LocalPlayerType);
            gameLogic.secondPlayerState = new PlayerState(firstPlayer == LocalPlayerType);
            Debug.LogAssertion($"second player�� set");
        }


        Debug.LogAssertion($"actor number: {PhotonNetwork.LocalPlayer.ActorNumber}");
        Managers.Game.EnterMultiPlay();
        OnGameInit?.Invoke(LocalPlayerType);
    }

    [PunRPC]
    private void RPC_GameStart() {
        Managers.UI.ClosePopUp();

        // �ڽ��� �Ҵ���� player type���� set. master���� rpc�� ȣ���ؼ� ���ÿ� set
        Managers.Player.InitPlayerUI(LocalPlayerType, new PlayerInfo(PhotonNetwork.NickName, "1��")); // �� �κ� RPC�� ����ȭ


        PlayerReady();
    }


    [PunRPC]
    private void RPC_PlayerReady(int actorNumber) {
        if (PhotonNetwork.IsMasterClient) {
            readiedPlayers.Add(actorNumber);

            if (readiedPlayers.Count == 2) {
                OnAllPlayerReady?.Invoke();
            }
        }
    }

    [PunRPC]
    private void RPC_SetGameStateplay() {
        Managers.Game.SetGameStatePlay();
        Debug.LogAssertion($"<color=magenta>���� ����!</color>");
    }

    [PunRPC]
    private void RPC_FinishGame(Define.Type.Player winner) {
        OnGameFinish?.Invoke(winner);
    }

    #endregion
}
