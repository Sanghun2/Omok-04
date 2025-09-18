using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : IInitializable
{
    private PlayerUI player1;
    private PlayerUI player2;

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
        PlayerUI[] players = GameObject.FindGameObjectWithTag(Define.Tag.BACKGROUND_CANVAS_TAG).GetComponentsInChildren<PlayerUI>(true);
        //Debug.LogAssertion($"found player: {players.Length}");

        for (int i = 0; i < players.Length; i++) {
            PlayerUI targetPlayer = players[i];
            if (targetPlayer.PlayerType == Define.Type.Player.Player1) {
                player1 = targetPlayer;
                //Debug.LogAssertion($"<color=green>player1 found</color>");
            }
            else {
                player2 = targetPlayer;
                //Debug.LogAssertion($"<color=green>player2 found</color>");
            }
        }
    }
    private PlayerUI GetPlayer(Define.Type.Player playerType) {
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
        PlayerUI player = playerObj.GetComponent<PlayerUI>();
        player.InitPlayer(playerInfo);
    }

    #endregion
}
