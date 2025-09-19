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

    public event INetworkController.PlayerHandler OnChooseFirstPlayer; //
    public event INetworkController.PlayerHandler OnPlayerInit;
    public event INetworkController.PlayerHandler OnPlaceStone;
    public event INetworkController.PlayerHandler OnTurnChanged;
    public event INetworkController.PlayerHandler OnGameFinish;

    public delegate void PlayerHandler(Define.Type.Player targetPlayer, string playerName, int rank);
    public event PlayerHandler OnGameInit;

    #region interface

    public void InitConnect() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomID) {
        Debug.LogAssertion($"<color=cyan>room 생성 시도</color>");
    }
    public void JoinRoom(string roomID) {
        Debug.LogAssertion($"<color=cyan>room 입장 시도</color>");
    }

    public override void OnConnectedToMaster() {
        Debug.LogAssertion($"<color=cyan>master 연결</color>");
        PhotonNetwork.NickName = "Test Player";
    }
    public void OnDisconnected() {
        Debug.LogAssertion($"<color=cyan>master 연결 종료</color>");
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
        Debug.LogAssertion($"<color=cyan>room 입장. 현재 인원:{PhotonNetwork.CurrentRoom.PlayerCount}, 최대 인원: {PhotonNetwork.CurrentRoom.MaxPlayers}</color>");
        gameObject.AddComponent<PhotonView>();
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
            Debug.LogAssertion($"게임 시작 시도");
            OnMatchFound?.Invoke();
            var routineID = Managers.Coroutine.WaitFrame(1, SetFirstPlayer);
        }
    }

    public override void OnLeftRoom() {
        TestLog($"room을 나감");
    }

    #endregion

    #region InGame
    public void SetPlayerAndFirstPlayer(Define.Type.Player playerType, Define.Type.Player firstPlayer) {
        OnChooseFirstPlayer?.Invoke(firstPlayer);
    }

    /// <summary>
    /// Place Stone을 실행할 수 있는지 판단
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
        OnGameInit += UpdatePlayerUI;
        OnPlayerInit += ReadyGame;
        OnAllPlayerReady += SetGameStatePlay;// game start에서 전부 처리 후 ready처리. 모두 ready이면 game state inprogress로 변경
    }

    private void SetFirstPlayer() { //
        photonView.RPC(nameof(RPC_SetFirstPlayer), RpcTarget.AllViaServer);
    }


    /// <summary>
    /// Local의 인게임 PlayerState 및 PlayerUI Init
    /// </summary>
    /// <param name="firstPlayer"></param>
    private void InitGame(Define.Type.Player firstPlayer) { //
        // 이전 step에서 master client가 아니면 return 했으므로 이 코드는 master만 실행함
        photonView.RPC(nameof(RPC_InitGame), RpcTarget.AllViaServer, firstPlayer);
    }

    
    /// <summary>
    /// 다른 플레이어의 PlayerUI를 현재 내가 Init한 정보로 Init
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="playerName"></param>
    /// <param name="rank"></param>
    private void UpdatePlayerUI(Define.Type.Player targetPlayer, string playerName, int rank) {
        photonView.RPC(nameof(RPC_UpdatePlayerUI), RpcTarget.OthersBuffered, targetPlayer, playerName, rank);
    } //


    /// <summary>
    /// 씬 이동 후 game ready RPC 호출
    /// </summary>
    /// <param name="localPayerType"></param>
    private void ReadyGame(Define.Type.Player localPayerType) { //
        Managers.UI.ClosePopUp();
        Managers.Game.EnterMultiPlay();
        photonView.RPC(nameof(RPC_ReadyGame), RpcTarget.MasterClient, (int)localPayerType);
    }

    private void SetGameStatePlay() { //
        if (PhotonNetwork.IsMasterClient) {
            photonView.RPC(nameof(RPC_SetGameStateplay), RpcTarget.AllViaServer);
        }
    }

    [PunRPC] //
    private void RPC_SetFirstPlayer() {

        if (!PhotonNetwork.IsMasterClient) return;

        int firstPlayerIndex = Random.Range(1, 3);
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        TestLog($"선택된 first player index: {firstPlayerIndex}, local actor: {actorNumber}");
        Define.Type.Player firstPlayer = actorNumber == firstPlayerIndex ? Define.Type.Player.Player1 : Define.Type.Player.Player2;

        TestLog($"local player type: {LocalPlayerType}, first player: {firstPlayer == LocalPlayerType}");
        OnChooseFirstPlayer?.Invoke(firstPlayer);
    }

    [PunRPC] //
    private void RPC_InitGame(Define.Type.Player firstPlayer) {
        // 모든 플레이어 인게임 진입

        // 모든 플레이어가 개별적으로 같은 값으로 board 초기화 및 플레이어 초기화
        var gameLogic = new GameLogic(Managers.Board.Board, Define.Type.Game.Multi);

        if (LocalPlayerType == Define.Type.Player.Player1) {
            gameLogic.firstPlayerState = new PlayerState(firstPlayer == LocalPlayerType);
            gameLogic.secondPlayerState = new PlayerState(firstPlayer == LocalPlayerType);
            TestLog("first player로 set");
        }
        else {
            gameLogic.firstPlayerState = new PlayerState(firstPlayer == LocalPlayerType);
            gameLogic.secondPlayerState = new PlayerState(firstPlayer == LocalPlayerType);
            TestLog("second player로 set");
        }
        ;
        var currentUser = Managers.UserInfo.GetCurrentUser();

        // Local Player UI Init 후 rpc 동기화
        Managers.InGameUI.InitPlayerUI(LocalPlayerType, new PlayerInfo(currentUser.username, currentUser.rank.ToString()));
        
        TestLog($"actor number: {PhotonNetwork.LocalPlayer.ActorNumber}");
        OnGameInit?.Invoke(LocalPlayerType, currentUser.username, currentUser.rank);
    }

    [PunRPC] //
    private void RPC_UpdatePlayerUI(Define.Type.Player targetPlayer, string playerName, int rank) {
        Managers.InGameUI.InitPlayerUI(targetPlayer, new PlayerInfo(playerName, rank.ToString()));

        OnPlayerInit?.Invoke(targetPlayer);
    }

    [PunRPC] //
    private void RPC_ReadyGame(int actorNumber) {
        if (PhotonNetwork.IsMasterClient) {
            readiedPlayers.Add(actorNumber);

            if (readiedPlayers.Count == 2) {
                OnAllPlayerReady?.Invoke();
            }
        }
    }

    [PunRPC] //
    private void RPC_SetGameStateplay() {
        Managers.Game.SetGameStatePlay();
        TestLog("게임 시작!", "magenta");
    }

    [PunRPC]
    private void RPC_FinishGame(Define.Type.Player winner) {
        OnGameFinish?.Invoke(winner);
    }

    #endregion

    private void TestLog(string text, string textColor="aqua") {
        Debug.LogAssertion($"<color={textColor}>{text}</color>");
    }
}
