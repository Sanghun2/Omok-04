using System;
using System.Collections.Generic;
using UnityEngine;

public class DualModeAI
{
    private struct LevelParam
    {
        public int maxDepth;
        public int maxCandidates;
        public float evalScale;
        public bool blockThree;   // 3�� ��� ����
        public bool sloppySort;   // �ĺ� ������ �Ϻη� ��� �Ǽ� ����
        public LevelParam(int d, int c, float e, bool block3, bool sloppy)
        { maxDepth = d; maxCandidates = c; evalScale = e; blockThree = block3; sloppySort = sloppy; }
    }

    private static LevelParam GetLevelParam(Define.Type.GameLevel level)
    {
        switch (level)
        {
            case Define.Type.GameLevel.Easy:
                return new LevelParam(1, 12, 0.7f, false, true);   // ��� �Ǽ� ����
            case Define.Type.GameLevel.Normal:
                return new LevelParam(2, 10, 1.0f, false, false);   // �⺻��
            case Define.Type.GameLevel.Hard:
                return new LevelParam(4, 12, 1.2f, false, false);   // ���� ��� �ε� �ð� �þ..(3�� ��� ��)
            default:
                return new LevelParam(2, 10, 1.0f, false, false);
        }
    }

    private static List<(int row, int col)> GetCandidateMoves(Cell[,] board)
    {
        List<(int, int)> moves = new();
        int rows = board.GetLength(0), cols = board.GetLength(1);
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                if (board[r, c].Stone != Cell.StoneType.None) continue;
                bool near = false;
                for (int dr = -1; dr <= 1 && !near; dr++)
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int nr = r + dr, nc = c + dc;
                        if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) continue;
                        if (board[nr, nc].Stone != Cell.StoneType.None) { near = true; break; }
                    }
                if (near) moves.Add((r, c));
            }
        return moves;
    }

    public static (int row, int col)? GetBestMove(Cell[,] board,
        Cell.StoneType ai, Define.Type.GameLevel level)
    {
        var param = GetLevelParam(level);
        var candidates = GetCandidateMoves(board);
        if (candidates.Count == 0)
            return (board.GetLength(0) / 2, board.GetLength(1) / 2);

        Cell.StoneType opp = ai == Cell.StoneType.Black ? Cell.StoneType.White : Cell.StoneType.Black;

        // 1. ��� �¸�
        foreach (var (r, c) in candidates)
        {
            board[r, c].SetMarker(ai);
            if (CheckAnyWin(ai, board)) { board[r, c].SetMarker(Cell.StoneType.None); return (r, c); }
            board[r, c].SetMarker(Cell.StoneType.None);
        }

        // 2. ��� ��� �¸� ���� + ������/3�� ���
        foreach (var (r, c) in candidates)
        {
            board[r, c].SetMarker(opp);
            if (CheckAnyWin(opp, board) || IsOpenFour(board, opp, r, c) ||
               (param.blockThree && IsOpenThree(board, opp, r, c)))
            {
                board[r, c].SetMarker(Cell.StoneType.None);
                return (r, c);
            }
            board[r, c].SetMarker(Cell.StoneType.None);
        }

        // 3. �ĺ� ���� �� �̴ϸƽ�
        candidates.Sort((a, b) =>
            EvaluateMove(board, b, ai).CompareTo(EvaluateMove(board, a, ai)));

        if (param.sloppySort && candidates.Count > 3)
        {
            // Easy ����: �Ϻη� ���� 3�� �� �����ϰ� ����
            var rng = UnityEngine.Random.Range(0, 3);
            return candidates[rng];
        }

        if (candidates.Count > param.maxCandidates)
            candidates = candidates.GetRange(0, param.maxCandidates);

        float bestScore = float.NegativeInfinity;
        (int row, int col) best = (-1, -1);
        foreach (var (r, c) in candidates)
        {
            board[r, c].SetMarker(ai);
            float score = DoMiniMax(board, 0, false, ai,
                                    param.maxDepth,
                                    float.NegativeInfinity,
                                    float.PositiveInfinity,
                                    param.evalScale);
            board[r, c].SetMarker(Cell.StoneType.None);
            if (score > bestScore) { bestScore = score; best = (r, c); }
        }
        return best;
    }

    private static float DoMiniMax(Cell[,] board, int depth, bool isMax,
        Cell.StoneType ai, int maxDepth, float alpha, float beta, float scale)
    {
        Cell.StoneType opp = ai == Cell.StoneType.Black ? Cell.StoneType.White : Cell.StoneType.Black;
        if (CheckAnyWin(ai, board)) return 10000 - depth;
        if (CheckAnyWin(opp, board)) return -10000 + depth;
        if (depth >= maxDepth) return EvaluateBoard(board, ai) * scale;

        var moves = GetCandidateMoves(board);
        if (moves.Count == 0) return 0;

        float best = isMax ? float.NegativeInfinity : float.PositiveInfinity;
        foreach (var (r, c) in moves)
        {
            board[r, c].SetMarker(isMax ? ai : opp);
            float sc = DoMiniMax(board, depth + 1, !isMax, ai, maxDepth, alpha, beta, scale);
            board[r, c].SetMarker(Cell.StoneType.None);
            if (isMax)
            {
                best = Mathf.Max(best, sc);
                alpha = Mathf.Max(alpha, sc);
            }
            else
            {
                best = Mathf.Min(best, sc);
                beta = Mathf.Min(beta, sc);
            }
            if (beta <= alpha) break;
        }
        return best;
    }

    private static float EvaluateMove(Cell[,] board, (int r, int c) move, Cell.StoneType ai)
    {
        board[move.r, move.c].SetMarker(ai);
        float s = EvaluateBoard(board, ai);
        board[move.r, move.c].SetMarker(Cell.StoneType.None);
        return s;
    }

    private static float EvaluateBoard(Cell[,] board, Cell.StoneType ai)
    {
        Cell.StoneType opp = ai == Cell.StoneType.Black ? Cell.StoneType.White : Cell.StoneType.Black;
        return ScoreFor(board, ai) - ScoreFor(board, opp);
    }

    private static int ScoreFor(Cell[,] board, Cell.StoneType m)
    {
        int score = 0;
        Vector2Int[] dirs = { new(1, 0), new(0, 1), new(1, 1), new(1, -1) };
        int rows = board.GetLength(0), cols = board.GetLength(1);
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                if (board[r, c].Stone != m) continue;
                foreach (var d in dirs)
                {
                    int cnt = 1;
                    int nr = r + d.x, nc = c + d.y;
                    while (nr >= 0 && nr < rows && nc >= 0 && nc < cols && board[nr, nc].Stone == m)
                    { cnt++; nr += d.x; nc += d.y; }
                    if (cnt >= 5) score += 100000;
                    else if (cnt == 4) score += 20000;
                    else if (cnt == 3) score += 3000;
                    else if (cnt == 2) score += 200;
                }
            }
        return score;
    }

    private static bool CheckAnyWin(Cell.StoneType marker, Cell[,] board)
    {
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col].Stone == marker &&
                    OmokAI.CheckGameWin(marker, board, row, col))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool IsOpenFour(Cell[,] board, Cell.StoneType m, int r, int c)
    {
        Vector2Int[] dirs = { new(1, 0), new(0, 1), new(1, 1), new(1, -1) };
        board[r, c].SetMarker(m);
        bool found = false;
        foreach (var d in dirs)
        {
            int cnt = 1;
            int nr = r + d.x, nc = c + d.y;
            while (nr >= 0 && nr < board.GetLength(0) &&
                   nc >= 0 && nc < board.GetLength(1) &&
                   board[nr, nc].Stone == m) { cnt++; nr += d.x; nc += d.y; }
            nr = r - d.x; nc = c - d.y;
            while (nr >= 0 && nr < board.GetLength(0) &&
                   nc >= 0 && nc < board.GetLength(1) &&
                   board[nr, nc].Stone == m) { cnt++; nr -= d.x; nc -= d.y; }
            if (cnt >= 4) { found = true; break; }
        }
        board[r, c].SetMarker(Cell.StoneType.None);
        return found;
    }

    private static bool IsOpenThree(Cell[,] board, Cell.StoneType m, int r, int c)
    {
        Vector2Int[] dirs = { new(1, 0), new(0, 1), new(1, 1), new(1, -1) };
        board[r, c].SetMarker(m);
        bool found = false;
        foreach (var d in dirs)
        {
            int cnt = 1;
            int nr = r + d.x, nc = c + d.y;
            while (nr >= 0 && nr < board.GetLength(0) &&
                   nc >= 0 && nc < board.GetLength(1) &&
                   board[nr, nc].Stone == m) { cnt++; nr += d.x; nc += d.y; }
            nr = r - d.x; nc = c - d.y;
            while (nr >= 0 && nr < board.GetLength(0) &&
                   nc >= 0 && nc < board.GetLength(1) &&
                   board[nr, nc].Stone == m) { cnt++; nr -= d.x; nc -= d.y; }
            if (cnt >= 3) { found = true; break; }
        }
        board[r, c].SetMarker(Cell.StoneType.None);
        return found;
    }
}
