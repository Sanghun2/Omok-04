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
        ProcessMove(gameLogic, Define.Type.StoneColor.White, row, col);
    }

    public override async void OnEnter(GameLogic gameLogic)
    {
        var ui = Object.FindObjectOfType<IngameUIController>();
        if (ui != null)
        {
            ui.SetTurnChecks(true);
        }

        // 타이머 초기화 및 25초 카운트 시작
        var timer = Managers.Time.GetTimer();
        if (timer != null)
        {
            timer.SetTimeAsDefault();   // 25초 설정
            timer.StartCount();        // 카운트 시작
        }

        var board = gameLogic.Board;

        var result = await System.Threading.Tasks.Task.Run(() =>
            DualModeAI.GetBestMove(board, Define.Type.StoneColor.White, level)
        );

        if (result.HasValue)
            HandleMove(gameLogic, result.Value.row, result.Value.col);
        else 
        { 
            Debug.Log("### DEV_JSH 게임 비김 ###"); 
            gameLogic.EndGame(Define.State.GameResult.DRAW); 
        }

        if (ui != null)
            ui.SetTurnChecks(false);
    }

    public override void OnExit(GameLogic gameLogic)
    {
        var ui = Object.FindObjectOfType<IngameUIController>();
        if (ui != null)
        {
            ui.SetTurnChecks(false);
        }

        // 타이머 초기화 및 25초 카운트 시작
        var timer = Managers.Time.GetTimer();
        if (timer != null)
        {
            timer.SetTimeAsDefault();   // 25초 설정
            timer.StartCount();        // 카운트 시작
        }
    }

    protected override void HandleNextTurn(GameLogic gameLogic)
    {
        Debug.Log($"### DEV_JSH AI HandleNextTrun Called / 흑돌 차례 ###");
        gameLogic.SetState(gameLogic.firstPlayerState);
    }
}
