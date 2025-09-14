using UnityEngine;
using System;

public static class OmokAI
{
    private static readonly (int rowDir, int colDir)[] directions =
    {
        (0, 1),   // 가로 방향
        (1, 0),   // 세로 방향
        (1, 1),   // 대각선 ↘ 방향
        (1, -1)   // 대각선 ↙ 방향
    };

    public static bool CheckGameWin(Cell.CellMarker marker, Cell[,] board, int row, int col)
    {
        foreach (var (rowDir, colDir) in directions)
        {
            int count = 1; // 현재 돌 포함

            // 한쪽 방향으로 체크
            count += CountStones(marker, board, row, col, rowDir, colDir);

            // 반대 방향으로 체크
            count += CountStones(marker, board, row, col, -rowDir, -colDir);

            if (count >= 5) return true;
        }
        return false;
    }

    /// <summary>
    /// 연속된 마커 개수 확인
    /// </summary>
    private static int CountStones(Cell.CellMarker marker, Cell[,] board, int row, int col, int rowDir, int colDir)
    {
        int count = 0;
        int r = row + rowDir;
        int c = col + colDir;

        // 한 칸 씩 이동하면서 Marker가 같다면 count++ 아니면 반복 종료
        while (r >= 0 && r < Board.BoardRow && c >= 0 && c < Board.BoardCol && board[r, c].Marker == marker)
        {
            count++;
            r += rowDir;
            c += colDir;
        }
        return count;
    }

    private static int CountRenjuStone(Cell.CellMarker marker, Cell[,] board, int row, int col, int rowDir, int colDir, int moveLimit)
    {
        int stoneCount = 0;
        int r = row + rowDir;
        int c = col + colDir;

        //  제한 범위 내 돌 개수 확인
        while (r >= 0 && r < Board.BoardRow && c >= 0 && c < Board.BoardCol && moveLimit > 0)
        {
            if (board[r, c].Marker == marker)
            {
                stoneCount++;
            }

            r += rowDir;
            c += colDir;
            moveLimit--;
        }

        return stoneCount;
    }

    public static bool CheckRenju(Cell.CellMarker marker, Cell[,] board, int row, int col, int stoneLimit)
    {
        int stoneCount = 0;
        int count = 0;

        foreach (var (rowDir, colDir) in directions)
        {
            // 한쪽 방향으로 체크
            stoneCount += CountRenjuStone(marker, board, row, col, rowDir, colDir , stoneLimit);

            if (stoneCount == (stoneLimit - 1))
            {
                count++;
                stoneCount = 0;
            }

            // 반대 방향으로 체크
            stoneCount += CountRenjuStone(marker, board, row, col, -rowDir, -colDir, stoneLimit);

            if (stoneCount == (stoneLimit - 1)) count++;

            stoneCount = 0;
        }
        if (count >= 2) return true;

        return false;
    }
}
