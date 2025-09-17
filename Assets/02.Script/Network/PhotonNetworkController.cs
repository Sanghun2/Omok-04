using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class PhotonNetworkController : MonoBehaviourPunCallbacks, INetworkController
{
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
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions:new RoomOptions() { MaxPlayers= maxPlayers });
    }
    public override void OnJoinedRoom() {
        Debug.LogAssertion($"<color=cyan>room 입장. 현재 인원:{PhotonNetwork.CurrentRoom.PlayerCount}, 최대 인원: {PhotonNetwork.CurrentRoom.MaxPlayers}</color>");
        gameObject.AddComponent<PhotonView>();
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
            Debug.LogAssertion($"게임 시작 시도");
            var routineID = Managers.Coroutine.WaitFrame(1, StartGame);
        }
    }

    #endregion

    #region InGame
    public void ChooseFirstPlayer(Define.Type.Player firstPlayer) {
        throw new System.NotImplementedException();
    }
    public void StartGame() {
        photonView.RPC(nameof(RPC_GameStart), RpcTarget.All);
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
        throw new System.NotImplementedException();
    }

    public void SetTimer(Define.Type.Player playerType, float time) {
        throw new System.NotImplementedException();
    }

    public void FinishGame(Define.Type.Player winner) {
        throw new System.NotImplementedException();
    }

    #endregion

    #region capsulation

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogAssertion($"<color=cyan>disconnected: {cause}</color>");
    }

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    [PunRPC]
    private void RPC_GameStart() {
        // player 할당 및 순서 지정
        if (PhotonNetwork.IsMasterClient) {
            var fistPlayerIndex = Random.Range(1,3);
            ChooseFirstPlayer(
                PhotonNetwork.LocalPlayer.ActorNumber == fistPlayerIndex ? 
                Define.Type.Player.Player1 : 
                Define.Type.Player.Player2);
        }

        // 자신이 할당받은 player type으로 set. master에서 rpc로 호출해서 동시에 set
        Managers.Player.InitPlayer(Define.Type.Player.Player1, new PlayerInfo(PhotonNetwork.NickName, "1급"));

        Debug.LogAssertion($"actor number: {PhotonNetwork.LocalPlayer.ActorNumber}");
        Managers.Game.EnterMultiPlay();
        Managers.Game.SetStatePlay();
        Debug.LogAssertion($"<color=magenta>게임 시작!</color>");
    }

    #endregion
}
