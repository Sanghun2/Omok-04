using System;
using System.Text;
using UnityEngine;
using UnityEngine.Timeline;

public class GameLogic
{
    public BasePlayerState firstPlayerState;            // Player A
    public BasePlayerState secondPlayerState;           // Player B
    public Cell[,] Board => board;

    private BasePlayerState currentPlayerState;         // 현재 턴의 Player
    private Cell[,] board;                              // 보드의 상태 정보
    private Define.Type.Game gameType;                  // 현재 게임 타입 상태

    public GameLogic(Cell[,] board, Define.Type.Game gameType,Define.Type.GameLevel level = Define.Type.GameLevel.Easy)
    {
        this.board = board;
        this.gameType = gameType;

        switch (gameType)
        {
            case Define.Type.Game.Single:
                firstPlayerState = new PlayerState(true);
                secondPlayerState = new AIState(level);

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
    public bool SetNewBoardValue(Define.Type.StoneColor stoneType, int row, int col)
    {
        if (board[row, col].Stone != Define.Type.StoneColor.None)
        {
            Managers.Board.ActiveX_Marker(row,col);
            Managers.Board.ResetCurretCell();
            return false;
        }

        if (stoneType == Define.Type.StoneColor.Black)
        {
            if (board[row,col].IsRenju)
            {
                Managers.Board.ResetCurretCell();
                Debug.Log("### DEV_JSH 렌주룰상 금수 ###");
                return false;
            }
            else
            {
                board[row, col].SetMarker(stoneType);
                Managers.Board.PlaceMarker(stoneType, row, col);
                Managers.Board.ResetCurretCell();
                Managers.Board.onStoneSettedDelegate?.Invoke(stoneType);

                foreach (var cell in board)
                {
                    if (cell.Stone == Define.Type.StoneColor.None)
                        OmokAI.CheckRenju(Define.Type.StoneColor.Black, board, cell.CellRow, cell.CellCol);
                }

                Managers.Board.ShowAllRenju(board);

                return true;
            }
        }
        else if (stoneType == Define.Type.StoneColor.White)
        {
            board[row, col].IsRenju = false;
            board[row, col].OnX_Marker = false;
            board[row, col].SetMarker(stoneType);
            Managers.Board.DestroyX_Marker(row, col);
            Managers.Board.PlaceMarker(stoneType, row, col);
            Managers.Board.ResetCurretCell();
            Managers.Board.onStoneSettedDelegate?.Invoke(stoneType);
            return true;
        }
        else
            return false;
    }

    // 게임의 결과를 확인하는 함수
    public Define.State.GameResult CheckGameResult(Define.Type.StoneColor stoneType, int row, int col)
    {
        if (OmokAI.CheckGameWin(stoneType, board, row, col)) 
        {
            if(stoneType == Define.Type.StoneColor.Black)
                return Define.State.GameResult.BlackStoneWin;
            else if(stoneType == Define.Type.StoneColor.White)
                return Define.State.GameResult.WhiteStoneWin;
        }
        else if (OmokAI.CheckGameDraw(board))
        {
            return Define.State.GameResult.DRAW;
        }
        return Define.State.GameResult.NONE;
    }
}
