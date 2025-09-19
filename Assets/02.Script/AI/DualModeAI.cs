using System;
using System.Collections.Generic;
using UnityEngine;

public class DualModeAI
{
    private struct LevelParam
    {
        public int maxDepth;
        public int maxCandidates;
        public float selfWeight;
        public float oppWeight;
        public bool blockThree;

        // 새 가중치
        public float winWeight;
        public float blockWeight;
        public float open3SelfWeight;
        public float open3OppWeight;

        public LevelParam(int d, int c, float sw, float ow, bool b3,
                          float wW, float bW, float o3S, float o3O)
        {
            maxDepth = d;
            maxCandidates = c;
            selfWeight = sw;
            oppWeight = ow;
            blockThree = b3;
            winWeight = wW;
            blockWeight = bW;
            open3SelfWeight = o3S;
            open3OppWeight = o3O;
        }
    }

    private static LevelParam GetLevelParam(Define.Type.GameLevel level)
    {
        // 난이도별 가중치 예시
        switch (level)
        {
            case Define.Type.GameLevel.Easy:
                return new LevelParam(1, 12, 1.0f, 1.5f, true,
                    5000f, 4000f, 800f, 800f);
            case Define.Type.GameLevel.Hard:
                return new LevelParam(2, 12, 1.2f, 1.0f, true,
                    7000f, 6000f, 1200f, 1000f);
            case Define.Type.GameLevel.Normal:
                return new LevelParam(3, 20, 1.4f, 1.0f, true,
                    10000f, 8000f, 1500f, 1200f);
            default:
                return new LevelParam(2, 12, 1.0f, 1.0f, true,
                    7000f, 6000f, 1000f, 1000f);
        }
    }

    private static List<(int row, int col)> GetCandidateMoves(Cell[,] board)
    {
        List<(int, int)> moves = new();
        int rows = board.GetLength(0), cols = board.GetLength(1);
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
            {
                if (board[r, c].Stone != Define.Type.StoneColor.None) continue;
                bool near = false;
                for (int dr = -1; dr <= 1 && !near; dr++)
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int nr = r + dr, nc = c + dc;
                        if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) continue;
                        if (board[nr, nc].Stone != Define.Type.StoneColor.None) { near = true; break; }
                    }
                if (near) moves.Add((r, c));
            }
        return moves;
    }

    public static (int row, int col)? GetBestMove(Cell[,] board,
        Define.Type.StoneColor ai, Define.Type.GameLevel level)
    {
        var param = GetLevelParam(level);
        var candidates = GetCandidateMoves(board);
        if (candidates.Count == 0)
            return (board.GetLength(0) / 2, board.GetLength(1) / 2);

        // 모든 후보를 “가중치 포함 평가” 후 정렬
        candidates.Sort((a, b) =>
            EvaluateMove(board, b, ai, param).CompareTo(EvaluateMove(board, a, ai, param)));

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
                                    param);
            board[r, c].SetMarker(Define.Type.StoneColor.None);
            if (score > bestScore) { bestScore = score; best = (r, c); }
        }
        return best;
    }

    private static float EvaluateMove(Cell[,] board, (int r, int c) move,
        Define.Type.StoneColor ai, LevelParam param)
    {
        Define.Type.StoneColor opp = ai == Define.Type.StoneColor.Black
                                     ? Define.Type.StoneColor.White
                                     : Define.Type.StoneColor.Black;

        board[move.r, move.c].SetMarker(ai);

        float s = EvaluateBoard(board, ai, param);

        // === 추가된 가중치 평가 ===
        if (CheckAnyWin(ai, board))
            s += param.winWeight;  // AI 즉시승리 가치

        // 상대가 즉시 이기는 수를 막는 가치
        board[move.r, move.c].SetMarker(opp);
        if (CheckAnyWin(opp, board))
            s += param.blockWeight;
        board[move.r, move.c].SetMarker(ai);

        // 열린 3목(자신/상대) 가중치
        if (IsOpenThree(board, ai, move.r, move.c))
            s += param.open3SelfWeight;
        if (param.blockThree && IsStrictOpenThree(board, opp, move.r, move.c))
            s += param.open3OppWeight;

        board[move.r, move.c].SetMarker(Define.Type.StoneColor.None);
        return s;
    }

    private static float DoMiniMax(Cell[,] board, int depth, bool isMax,
        Define.Type.StoneColor ai, int maxDepth, float alpha, float beta, LevelParam param)
    {
        Define.Type.StoneColor opp = ai == Define.Type.StoneColor.Black ? Define.Type.StoneColor.White : Define.Type.StoneColor.Black;
        if (CheckAnyWin(ai, board)) return 10000 - depth;
        if (CheckAnyWin(opp, board)) return -10000 + depth;
        if (depth >= maxDepth) return EvaluateBoard(board, ai, param);

        var moves = GetCandidateMoves(board);
        if (moves.Count == 0) return 0;

        float best = isMax ? float.NegativeInfinity : float.PositiveInfinity;
        foreach (var (r, c) in moves)
        {
            board[r, c].SetMarker(isMax ? ai : opp);
            float sc = DoMiniMax(board, depth + 1, !isMax, ai, maxDepth, alpha, beta, param);
            board[r, c].SetMarker(Define.Type.StoneColor.None);
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


    private static float EvaluateBoard(Cell[,] board, Define.Type.StoneColor ai, LevelParam param)
    {
        Define.Type.StoneColor opp = ai == Define.Type.StoneColor.Black ? Define.Type.StoneColor.White : Define.Type.StoneColor.Black;
        return ScoreFor(board, ai) * param.selfWeight - ScoreFor(board, opp) * param.oppWeight;
    }

    private static int ScoreFor(Cell[,] board, Define.Type.StoneColor m)
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
                    else if (cnt == 4) score += 50000;
                    else if (cnt == 3) score += 5000;
                    else if (cnt == 2) score += 500;
                }
            }
        return score;
    }

    private static bool CheckAnyWin(Define.Type.StoneColor marker, Cell[,] board)
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


    private static bool IsOpenThree(Cell[,] board, Define.Type.StoneColor m, int r, int c)
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
        board[r, c].SetMarker(Define.Type.StoneColor.None);
        return found;
    }

    private static bool IsStrictOpenThree(Cell[,] board, Define.Type.StoneColor m, int r, int c)
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

            int end1r = nr, end1c = nc;

            nr = r - d.x; nc = c - d.y;
            while (nr >= 0 && nr < board.GetLength(0) &&
                   nc >= 0 && nc < board.GetLength(1) &&
                   board[nr, nc].Stone == m) { cnt++; nr -= d.x; nc -= d.y; }

            int end2r = nr, end2c = nc;

            if (cnt == 3)
            {
                bool end1Open = end1r >= 0 && end1r < board.GetLength(0) &&
                                end1c >= 0 && end1c < board.GetLength(1) &&
                                board[end1r, end1c].Stone == Define.Type.StoneColor.None;

                bool end2Open = end2r >= 0 && end2r < board.GetLength(0) &&
                                end2c >= 0 && end2c < board.GetLength(1) &&
                                board[end2r, end2c].Stone == Define.Type.StoneColor.None;

                if (end1Open && end2Open) { found = true; break; }
            }
        }
        board[r, c].SetMarker(Define.Type.StoneColor.None);
        return found;
    }

}