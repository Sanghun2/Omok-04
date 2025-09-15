using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager
{
    public void SpawnPlayer(Define.Type.Player playerType, PlayerInfo playerInfo) {
        GameObject playerPrefab = Resources.Load<GameObject>("InGame/Player");
        GameObject playerObj = GameObject.Instantiate(playerPrefab, GetPlayerTr(playerType));
        Player player = playerObj.GetComponent<Player>();
        player.InitPlayer(playerInfo);
    }

    private Transform GetPlayerTr(Define.Type.Player playerType) {
        Debug.LogAssertion($"<color=orange>player tr get 구현 필요</color>");
        return null;
    }
}
