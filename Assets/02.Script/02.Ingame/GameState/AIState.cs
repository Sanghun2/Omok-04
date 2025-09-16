using UnityEngine;

public class AIState : BasePlayerState
{
    private Define.Type.GameLevel level;

    public AIState(Define.Type.GameLevel level)
    {
        this.level = level;
    }

    public override void HandleMove(GameLogic gameLogic, int row, int col)
    {
        Debug.Log("### DEV_JSH AI_HandleMove Called ###");
        ProcessMove(gameLogic, Cell.StoneType.White, row, col);
    }

    public override void OnEnter(GameLogic gameLogic)
    {
        var board = gameLogic.Board;

        var result = DualModeAI.GetBestMove(board,Cell.StoneType.White,level);

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
