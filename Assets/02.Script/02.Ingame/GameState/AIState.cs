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
        var ui = Managers.InGameUI;
        // 0.5�� �Ŀ� ���� �� ���¸� ActiveTurnMarkUI(true) ����
        Managers.Coroutine.Wait(0.5f, () =>
        {
            // ���� AIState �������� Ȯ��
            if (gameLogic.CurrentState == this) {
                ui.ActiveTurnMarkUI(true);
            }
        });

        // Ÿ�̸� �ʱ�ȭ �� 25�� ī��Ʈ ����
        Debug.LogAssertion($"on enter timer reset");
        var timer = Managers.Time.GetTimer();
        if (timer != null)
        {
            timer.SetTimeAsDefault();
            timer.StartCount();
        }

        var board = gameLogic.Board;

        var result = await System.Threading.Tasks.Task.Run(() => DualModeAI.GetBestMove(board, Define.Type.StoneColor.White, level));

        if (result.HasValue)
            HandleMove(gameLogic, result.Value.row, result.Value.col);
        else 
        { 
            Debug.Log("### DEV_JSH ���� ��� ###"); 
            Managers.Game.EndGame(Define.State.GameResult.DRAW); 
        }

        if (ui != null)
            ui.ActiveTurnMarkUI(false);
    }

    public override void OnExit(GameLogic gameLogic)
    {
        Managers.InGameUI.ActiveTurnMarkUI(false);
    }

    protected override void HandleNextTurn(GameLogic gameLogic)
    {
        Debug.Log($"### DEV_JSH AI HandleNextTrun Called / �浹 ���� ###");
        gameLogic.SetState(gameLogic.firstPlayerState);
    }
}
