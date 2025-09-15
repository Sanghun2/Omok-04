using UnityEngine;

public class PlayerState : BasePlayerState
{
    private bool isFirstPlayer;
    private Cell.CellMarkerType marker;
    private string roomId;
    private bool isMultiplay;

    public Cell.CellMarkerType Marker => marker;

    public PlayerState(bool isFirstPlayer)
    {
        this.isFirstPlayer = isFirstPlayer;
        marker = isFirstPlayer ?
            Cell.CellMarkerType.Black : Cell.CellMarkerType.White;
        isMultiplay = false;
    }

    #region �ʼ� �޼���
    public override void HandleMove(GameLogic gameLogic, int row, int col)
    {
        ProcessMove(gameLogic, marker, row, col);
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
            gameLogic.SetState(gameLogic.secondPlayerState);
            Debug.Log("### DEV_JSH ���� ���� �÷��̾�� �鵹 ###");
        }
        else
        {
            gameLogic.SetState(gameLogic.firstPlayerState);
            Debug.Log("### DEV_JSH ���� ���� �÷��̾�� �浹 ###");
        }
    }
    #endregion 
}
