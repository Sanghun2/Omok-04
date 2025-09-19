using UnityEngine;

public class PlayerState : BasePlayerState
{
    private bool isFirstPlayer;
    private Define.Type.StoneColor stoneColor;

    public Define.Type.StoneColor Stone => stoneColor;

    public PlayerState(bool isFirstPlayer)
    {
        this.isFirstPlayer = isFirstPlayer;
        stoneColor = isFirstPlayer ?
            Define.Type.StoneColor.Black : Define.Type.StoneColor.White;
    }

    #region 필수 메서드
    public override void HandleMove(GameLogic gameLogic, int row, int col)
    {
        Debug.Log("### DEV_JSH Player_HandleMove Called ###");
        ProcessMove(gameLogic, stoneColor, row, col);
    }

    public override void OnEnter(GameLogic gameLogic)
    {
        Managers.Board.OnStonePlace = (row, col) =>
        {
            HandleMove(gameLogic, row, col);
        };
    }

    public override void OnExit(GameLogic gameLogic)
    {
        Managers.Board.OnStonePlace = null;
    }

    protected override void HandleNextTurn(GameLogic gameLogic)
    {
        if (isFirstPlayer)
        {
            Debug.Log("### DEV_JSH 현재 턴의 플레이어는 백돌 ###");
            gameLogic.SetState(gameLogic.secondPlayerState);
        }
        else
        {
            Debug.Log("### DEV_JSH 현재 턴의 플레이어는 흑돌 ###");
            gameLogic.SetState(gameLogic.firstPlayerState);
        }
    }
    #endregion 
}
