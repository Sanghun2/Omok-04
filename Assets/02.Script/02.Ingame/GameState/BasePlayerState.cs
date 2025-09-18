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
            if (gameResult == Define.State.GameResult.NONE)
            {
                Managers.Turn.SwitchTurn();
                HandleNextTurn(gameLogic);
            }
            else
            {
                Managers.Game.EndGame(gameResult);
            }
        }
    }
}
