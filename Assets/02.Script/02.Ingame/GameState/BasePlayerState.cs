using UnityEngine;

public abstract class BasePlayerState
{
    public abstract void OnEnter(GameLogic gameLogic);                          // 상태 시작
    public abstract void OnExit(GameLogic gameLogic);                           // 상태 종료
    public abstract void HandleMove(GameLogic gameLogic, int row, int col);     // 마커 표시
    protected abstract void HandleNextTurn(GameLogic gameLogic);                // 턴 전환

    // 게임 결과 처리
    protected void ProcessMove(GameLogic gameLogic, Define.Type.StoneColor marker, int row, int col)
    {
        Debug.Log($"### DEV_JSH ProcessMove Called / Marker : {marker.ToString()} / Row : {row} / Col : {col} ###");
        if (gameLogic.SetNewBoardValue(marker, row, col))
        {
            // 새로이 놓여진 Marker를 기반으로 게임의 결과를 판단
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
