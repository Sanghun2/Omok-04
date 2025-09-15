using UnityEngine;

public class PlayerState : BasePlayerState
{
    private bool isFirstPlayer;
    private Cell.StoneType stoneType;
    private string roomId;
    private bool isMultiplay;

    public Cell.StoneType Stone => stoneType;

    public PlayerState(bool isFirstPlayer)
    {
        this.isFirstPlayer = isFirstPlayer;
        stoneType = isFirstPlayer ?
            Cell.StoneType.Black : Cell.StoneType.White;
        isMultiplay = false;
    }

    #region �ʼ� �޼���
    public override void HandleMove(GameLogic gameLogic, int row, int col)
    {
        Debug.Log("### DEV_JSH Player_HandleMove Called ###");
        ProcessMove(gameLogic, stoneType, row, col);
    }

    public override void OnEnter(GameLogic gameLogic)
    {
        gameLogic.boardController.onCellClickedDelegate = (row, col) =>
        {
            HandleMove(gameLogic, row, col);
        };
    }

    public override void OnExit(GameLogic gameLogic)
    {
        gameLogic.boardController.onCellClickedDelegate = null;
    }

    protected override void HandleNextTurn(GameLogic gameLogic)
    {
        if (isFirstPlayer)
        {
            Debug.Log("### DEV_JSH ���� ���� �÷��̾�� �鵹 ###");
            gameLogic.SetState(gameLogic.secondPlayerState);
        }
        else
        {
            Debug.Log("### DEV_JSH ���� ���� �÷��̾�� �浹 ###");
            gameLogic.SetState(gameLogic.firstPlayerState);
        }
    }
    #endregion 
}
