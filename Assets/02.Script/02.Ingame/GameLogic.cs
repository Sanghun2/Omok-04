using UnityEngine;
using System;
using System.Text;

public class GameLogic
{
    public BoardController boardController;
    public enum GameResult { NONE, WIN, LOSE, DRAW }

    public BasePlayerState firstPlayerState;            // Player A
    public BasePlayerState secondPlayerState;           // Player B
    private BasePlayerState currentPlayerState;         // 현재 턴의 Player
    private Cell[,] board;                              // 보드의 상태 정보
    public Cell[,] Board => board;

    public GameLogic(BoardController boardController, Cell[,] board, Define.Type.Game gameType)
    {
        this.boardController = boardController;
        this.board = board;

        switch (gameType)
        {
            case Define.Type.Game.Single:
                firstPlayerState = new PlayerState(true);
                secondPlayerState = new AIState();

                // 게임 시작
                SetState(firstPlayerState);
                break;
            case Define.Type.Game.Local:
                firstPlayerState = new PlayerState(true);
                secondPlayerState = new PlayerState(false);

                // 게임 시작
                SetState(firstPlayerState);
                break;
            case Define.Type.Game.Multi:
                break;
        }
    }    

    /// <summary>
    /// 턴이 바뀔 때, 기존 진행하던 상태를 Exit하고,
    /// 이번 턴의 상태를 currentPlayerState에 할당하고
    /// 이번 턴의 상태의 Enter를 호출
    /// </summary>
    /// <param name="state"></param>
    public void SetState(BasePlayerState state)
    {
        currentPlayerState?.OnExit(this);
        currentPlayerState = state;
        currentPlayerState?.OnEnter(this);
    }

    /// <summary>
    /// board 배열에 새로운 Marker 값을 할당
    /// </summary>
    /// <returns></returns>
    public bool SetNewBoardValue(Cell.StoneType stoneType, int row, int col)
    {
        if (board[row, col].Stone != Cell.StoneType.None)
        {
            boardController.ActiveX_Marker(row, col);
            return false;
        }

        if (stoneType == Cell.StoneType.Black)
        {
            if (OmokAI.CheckRenju(stoneType, board, row, col))
            {
                boardController.ActiveX_Marker(row, col);
                Debug.Log("### DEV_JSH 렌주룰상 금수 ###");
                return false;
            }
            else
            {
                board[row, col].SetMarker(stoneType);
                boardController.PlaceMarker(stoneType, row, col);
                boardController.onMarkerSettedDelegate?.Invoke(stoneType);
                return true;
            }
        }
        else if (stoneType == Cell.StoneType.White)
        {
            board[row, col].SetMarker(stoneType);
            boardController.PlaceMarker(stoneType, row, col);
            boardController.onMarkerSettedDelegate?.Invoke(stoneType);
            return true;
        }
        else
            return false;
    }

    // Game Over 처리
    public void EndGame(GameResult gameResult)
    {
        SetState(null);
        firstPlayerState = null;
        secondPlayerState = null;

        Debug.Log("### DEV_JSH Game Over ###");
    }

    // 게임의 결과를 확인하는 함수
    public GameResult CheckGameResult(Cell.StoneType stoneType, int row, int col)
    {
        if (OmokAI.CheckGameWin(stoneType, board, row, col)) 
        {
            if(stoneType == Cell.StoneType.Black)
                return GameResult.WIN;
            else if(stoneType == Cell.StoneType.White)
                return GameResult.LOSE;
        }
        else if (OmokAI.CheckGameDraw(board))
        {
            return GameResult.DRAW;
        }
        return GameResult.NONE;
    }
}
