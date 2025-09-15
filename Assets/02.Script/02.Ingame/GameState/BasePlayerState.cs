using UnityEngine;

public abstract class BasePlayerState
{
    public abstract void OnEnter(GameLogic gameLogic);                          // ���� ����
    public abstract void OnExit(GameLogic gameLogic);                           // ���� ����
    public abstract void HandleMove(GameLogic gameLogic, int row, int col);     // ��Ŀ ǥ��
    protected abstract void HandleNextTurn(GameLogic gameLogic);                // �� ��ȯ

    // ���� ��� ó��
    protected void ProcessMove(GameLogic gameLogic, Cell.CellMarkerType marker, int row, int col)
    {
        Debug.Log($"### DEV_JSH ProcessMove Called / Marker : {marker.ToString()} / Row : {row} / Col : {col} ###");
        if (gameLogic.SetNewBoardValue(marker, row, col))
        {
            // ������ ������ Marker�� ������� ������ ����� �Ǵ�
            var gameResult = gameLogic.CheckGameResult(row, col);
            if (gameResult == GameLogic.GameResult.NONE)
            {
                HandleNextTurn(gameLogic);
            }
            else
            {
                gameLogic.EndGame(gameResult);
            }
        }
    }
}
