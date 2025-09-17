using UnityEngine;

public abstract class BasePlayerState
{
    public abstract void OnEnter(GameLogic gameLogic);                          // ���� ����
    public abstract void OnExit(GameLogic gameLogic);                           // ���� ����
    public abstract void HandleMove(GameLogic gameLogic, int row, int col);     // ��Ŀ ǥ��
    protected abstract void HandleNextTurn(GameLogic gameLogic);                // �� ��ȯ

    // ���� ��� ó��
    protected void ProcessMove(GameLogic gameLogic, Define.Type.StoneColor marker, int row, int col)
    {
        Debug.Log($"### DEV_JSH ProcessMove Called / Marker : {marker.ToString()} / Row : {row} / Col : {col} ###");
        if (gameLogic.SetNewBoardValue(marker, row, col))
        {
            // ������ ������ Marker�� ������� ������ ����� �Ǵ�
            var gameResult = gameLogic.CheckGameResult(marker, row, col);
            if (gameResult == GameLogic.GameResult.NONE)
            {
                HandleNextTurn(gameLogic);
                Managers.Turn.SwitchTurn();
            }
            else
            {
                gameLogic.EndGame(gameResult);
                Define.Type.Player player = marker == Define.Type.StoneColor.Black ?
                    Define.Type.Player.Player1 : Define.Type.Player.Player2;
                Managers.Turn.EndGame(player);
            }
        }
    }
}
