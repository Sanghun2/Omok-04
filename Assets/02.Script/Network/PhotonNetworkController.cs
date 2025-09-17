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
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions:new RoomOptions() { MaxPlayers= maxPlayers });
    }
    public override void OnJoinedRoom() {
        Debug.LogAssertion($"<color=cyan>room ����. ���� �ο�:{PhotonNetwork.CurrentRoom.PlayerCount}, �ִ� �ο�: {PhotonNetwork.CurrentRoom.MaxPlayers}</color>");
        gameObject.AddComponent<PhotonView>();
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
            Debug.LogAssertion($"���� ���� �õ�");
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
    /// Place Stone�� ������ �� �ִ��� �Ǵ�
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
        // player �Ҵ� �� ���� ����
        if (PhotonNetwork.IsMasterClient) {
            var fistPlayerIndex = Random.Range(1,3);
            ChooseFirstPlayer(
                PhotonNetwork.LocalPlayer.ActorNumber == fistPlayerIndex ? 
                Define.Type.Player.Player1 : 
                Define.Type.Player.Player2);
        }

        // �ڽ��� �Ҵ���� player type���� set. master���� rpc�� ȣ���ؼ� ���ÿ� set
        Managers.Player.InitPlayer(Define.Type.Player.Player1, new PlayerInfo(PhotonNetwork.NickName, "1��"));

        Debug.LogAssertion($"actor number: {PhotonNetwork.LocalPlayer.ActorNumber}");
        Managers.Game.EnterMultiPlay();
        Managers.Game.SetStatePlay();
        Debug.LogAssertion($"<color=magenta>���� ����!</color>");
    }

    #endregion
}
