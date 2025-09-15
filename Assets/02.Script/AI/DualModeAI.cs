using UnityEngine;

public class DualModeAI 
{
    // 난이도별 최대 탐색 깊이 설정
    private static int GetMaxDepth(Define.Type.GameLevel level)
    {
        switch (level)
        {
            case Define.Type.GameLevel.Easy: return 1;      // 얕은 탐색
            case Define.Type.GameLevel.Normal: return 2;    // 기본 탐색
            case Define.Type.GameLevel.Hard: return 3;      // 깊은 탐색
            default: return 2;
        }
    }

    // 현재 상태를 전달하면 다음 최적의 수를 반환하는 메서드
    public static (int row, int col)? GetBestMove(Cell[,] board, Cell.CellMarker aiMarker, Define.Type.GameLevel level)
    {
        int maxDepth = GetMaxDepth(level);
        float bestScore = float.NegativeInfinity;
        (int row, int col) bestMove = (-1, -1);

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col].Marker == Cell.CellMarker.None)
                {
                    board[row, col].SetMarker(aiMarker);
                    float score = DoMiniMax(board, 0, false, aiMarker, maxDepth);
                    board[row, col].SetMarker(Cell.CellMarker.None);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (row, col);
                    }
                }
            }
        }

        if (bestMove != (-1, -1))
            return bestMove;
        return null;
    }

    // 미니맥스 알고리즘
    private static float DoMiniMax(Cell[,] board, int depth, bool isMaximizing, Cell.CellMarker aiMarker, int maxDepth)
    {
        Cell.CellMarker opponent = (aiMarker == Cell.CellMarker.Black) ? Cell.CellMarker.White : Cell.CellMarker.Black;

        // 승패 체크
        if (CheckAnyWin(aiMarker, board)) return 100 - depth;
        if (CheckAnyWin(opponent, board)) return -100 + depth;
        if (CheckGameDraw(board)) return 0;
        if (depth >= maxDepth) return EvaluateBoard(board, aiMarker); // 깊이 제한

        if (isMaximizing)
        {
            float bestScore = float.NegativeInfinity;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col].Marker == Cell.CellMarker.None)
                    {
                        board[row, col].SetMarker(aiMarker);
                        float score = DoMiniMax(board, depth + 1, false, aiMarker, maxDepth);
                        board[row, col].SetMarker(Cell.CellMarker.None);
                        bestScore = Mathf.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            float bestScore = float.PositiveInfinity;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col].Marker == Cell.CellMarker.None)
                    {
                        board[row, col].SetMarker(opponent);
                        float score = DoMiniMax(board, depth + 1, true, aiMarker, maxDepth);
                        board[row, col].SetMarker(Cell.CellMarker.None);
                        bestScore = Mathf.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    // 간단한 평가 함수 
    private static float EvaluateBoard(Cell[,] board, Cell.CellMarker aiMarker)
    {
        float score = 0;
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col].Marker == aiMarker) score += 1;
            }
        }
        return score;
    }

    // 비겼는지 확인
    private static bool CheckGameDraw(Cell[,] board)
    {
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col].Marker == Cell.CellMarker.None) return false;
            }
        }
        return true;
    }

    // 특정 돌이 승리 조건을 만족하는지 확인 (전체 보드 탐색)
    private static bool CheckAnyWin(Cell.CellMarker marker, Cell[,] board)
    {
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col].Marker == marker &&
                    OmokAI.CheckGameWin(marker, board, row, col))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
}
