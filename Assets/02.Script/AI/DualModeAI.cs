using UnityEngine;

public class DualModeAI 
{
    // ���̵��� �ִ� Ž�� ���� ����
    private static int GetMaxDepth(Define.Type.GameLevel level)
    {
        switch (level)
        {
            case Define.Type.GameLevel.Easy: return 1;      // ���� Ž��
            case Define.Type.GameLevel.Normal: return 2;    // �⺻ Ž��
            case Define.Type.GameLevel.Hard: return 3;      // ���� Ž��
            default: return 2;
        }
    }

    // ���� ���¸� �����ϸ� ���� ������ ���� ��ȯ�ϴ� �޼���
    public static (int row, int col)? GetBestMove(Cell[,] board, Cell.CellMarkerType aiMarker, Define.Type.GameLevel level)
    {
        int maxDepth = GetMaxDepth(level);
        float bestScore = float.NegativeInfinity;
        (int row, int col) bestMove = (-1, -1);

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col].Marker == Cell.CellMarkerType.None)
                {
                    board[row, col].SetMarker(aiMarker);
                    float score = DoMiniMax(board, 0, false, aiMarker, maxDepth);
                    board[row, col].SetMarker(Cell.CellMarkerType.None);

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

    // �̴ϸƽ� �˰���
    private static float DoMiniMax(Cell[,] board, int depth, bool isMaximizing, Cell.CellMarkerType aiMarker, int maxDepth)
    {
        Cell.CellMarkerType opponent = (aiMarker == Cell.CellMarkerType.Black) ? Cell.CellMarkerType.White : Cell.CellMarkerType.Black;

        // ���� üũ
        if (CheckAnyWin(aiMarker, board)) return 100 - depth;
        if (CheckAnyWin(opponent, board)) return -100 + depth;
        if (CheckGameDraw(board)) return 0;
        if (depth >= maxDepth) return EvaluateBoard(board, aiMarker); // ���� ����

        if (isMaximizing)
        {
            float bestScore = float.NegativeInfinity;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col].Marker == Cell.CellMarkerType.None)
                    {
                        board[row, col].SetMarker(aiMarker);
                        float score = DoMiniMax(board, depth + 1, false, aiMarker, maxDepth);
                        board[row, col].SetMarker(Cell.CellMarkerType.None);
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
                    if (board[row, col].Marker == Cell.CellMarkerType.None)
                    {
                        board[row, col].SetMarker(opponent);
                        float score = DoMiniMax(board, depth + 1, true, aiMarker, maxDepth);
                        board[row, col].SetMarker(Cell.CellMarkerType.None);
                        bestScore = Mathf.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    // ������ �� �Լ� 
    private static float EvaluateBoard(Cell[,] board, Cell.CellMarkerType aiMarker)
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

    // ������ Ȯ��
    private static bool CheckGameDraw(Cell[,] board)
    {
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col].Marker == Cell.CellMarkerType.None) return false;
            }
        }
        return true;
    }

    // Ư�� ���� �¸� ������ �����ϴ��� Ȯ�� (��ü ���� Ž��)
    private static bool CheckAnyWin(Cell.CellMarkerType marker, Cell[,] board)
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
