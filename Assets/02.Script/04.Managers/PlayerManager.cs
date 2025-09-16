using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : IInitializable
{
    private Player player1;
    private Player player2;

    public bool IsInit => isInit;
    private bool isInit;

    #region Interface

    public void InitPlayer(Define.Type.Player playerType, PlayerInfo playerInfo) {
        GetPlayer(playerType).InitPlayer(playerInfo);
    }

    #endregion

    #region Private

    public void Initialize() {
        if (IsInit) return;

        FindPlayers();
        isInit = true;
    }
    private void FindPlayers() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) {
            Player targetPlayer = players[i].GetComponent<Player>();
            if (targetPlayer.PlayerType == Define.Type.Player.Player1) {
                player1 = targetPlayer;
                Debug.LogAssertion($"<color=green>player1 found</color>");
            }
            else {
                player2 = targetPlayer;
                Debug.LogAssertion($"<color=green>player2 found</color>");
            }
        }
    }
    private Player GetPlayer(Define.Type.Player playerType) {
        if (IsInit == false) Initialize();
        if (playerType == Define.Type.Player.Player1) return player1;
        else if (playerType == Define.Type.Player.Player2) return player2;
        return null;
    }
    private Transform GetPlayerTr(Define.Type.Player playerType) {
        Debug.LogAssertion($"<color=orange>player tr get 구현 필요</color>");
        return null;
    }
    private void SpawnPlayer(Define.Type.Player playerType, PlayerInfo playerInfo) {
        GameObject playerPrefab = Resources.Load<GameObject>("InGame/Player");
        GameObject playerObj = GameObject.Instantiate(playerPrefab, GetPlayerTr(playerType));
        Player player = playerObj.GetComponent<Player>();
        player.InitPlayer(playerInfo);
    }

    #endregion
}
