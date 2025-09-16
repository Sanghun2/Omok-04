using UnityEngine;
using System;
using System.Text;

public class GameLogic
{
    public enum GameResult { NONE, BlackStoneWin, WhiteStoneWin, DRAW }

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
    public bool SetNewBoardValue(Cell.StoneType stoneType, int row, int col)
    {
        if (board[row, col].Stone != Cell.StoneType.None)
        {
            Managers.Board.ActiveX_Marker(row,col);
            return false;
        }

        if (stoneType == Cell.StoneType.Black)
        {
            if (OmokAI.CheckRenju(stoneType, board, row, col))
            {
                Managers.Board.ActiveX_Marker(row,col);
                Debug.Log("### DEV_JSH 렌주룰상 금수 ###");
                return false;
            }
            else
            {
                board[row, col].SetMarker(stoneType);
                Managers.Board.PlaceMarker(stoneType, row, col);
                Managers.Board.onStoneSettedDelegate?.Invoke(stoneType);
                return true;
            }
        }
        else if (stoneType == Cell.StoneType.White)
        {
            board[row, col].SetMarker(stoneType);
            Managers.Board.PlaceMarker(stoneType, row, col);
            Managers.Board.onStoneSettedDelegate?.Invoke(stoneType);
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

        Managers.GameResult.EndGame();
        Debug.Log($"### DEV_JSH Game Over Result : {gameResult.ToString()} ###");

        if (gameType == Define.Type.Game.Multi)
        {
            if (gameResult == GameResult.BlackStoneWin)
                Managers.GameResult.SendGameResult(true);
            else if (gameResult == GameResult.WhiteStoneWin)
                Managers.GameResult.SendGameResult(false);
            //else // Draw일 때
        }
    }

    // 게임의 결과를 확인하는 함수
    public GameResult CheckGameResult(Cell.StoneType stoneType, int row, int col)
    {
        if (OmokAI.CheckGameWin(stoneType, board, row, col)) 
        {
            if(stoneType == Cell.StoneType.Black)
                return GameResult.BlackStoneWin;
            else if(stoneType == Cell.StoneType.White)
                return GameResult.WhiteStoneWin;
        }
        else if (OmokAI.CheckGameDraw(board))
        {
            return GameResult.DRAW;
        }
        return GameResult.NONE;
    }
}
