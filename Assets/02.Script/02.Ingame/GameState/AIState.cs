using UnityEngine;

public class AIState : BasePlayerState
{
    public override void HandleMove(GameLogic gameLogic, int row, int col)
    {
        Debug.Log("### DEV_JSH AI_HandleMove Called ###");
        ProcessMove(gameLogic, Cell.CellMarkerType.White, row, col);
    }

    public override void OnEnter(GameLogic gameLogic)
    {
        var board = gameLogic.Board;

        var result = DualModeAI.GetBestMove(board,Cell.CellMarkerType.White,Define.Type.GameLevel.Easy);

        if (result.HasValue)
        {
            HandleMove(gameLogic, result.Value.row, result.Value.col);
        }
        else
        {
            Debug.Log("### DEV_JSH °ÔÀÓ ºñ±è ###");
            gameLogic.EndGame(GameLogic.GameResult.DRAW);
        }
    }

    public override void OnExit(GameLogic gameLogic)
    {
    }

    protected override void HandleNextTurn(GameLogic gameLogic)
    {
        Debug.Log($"### DEV_JSH AI HandleNextTrun Called / Èæµ¹ Â÷·Ê ###");
        gameLogic.SetState(gameLogic.firstPlayerState);
    }
}
