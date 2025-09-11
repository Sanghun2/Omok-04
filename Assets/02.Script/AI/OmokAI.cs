using UnityEngine;
using System;

public static class OmokAI
{
    private static readonly (int dr, int dc)[] directions =
    {
        (0, 1),   // 가로 방향
        (1, 0),   // 세로 방향
        (1, 1),   // 대각선 ↘ 방향
        (1, -1)   // 대각선 ↙ 방향
    };

    public static bool CheckGameWin(Cell.CellMarker marker, Cell[,] board, int row, int col)
    {
        foreach (var (dr, dc) in directions)
        {
            int count = 1; // 현재 돌 포함

            // 한쪽 방향으로 체크
            count += CountStones(marker, board, row, col, dr, dc);

            // 반대 방향으로 체크
            count += CountStones(marker, board, row, col, -dr, -dc);

            if (count >= 5) return true;
        }
        return false;
    }

    /// <summary>
    /// 연속된 마커 개수 확인
    /// </summary>
    private static int CountStones(Cell.CellMarker marker, Cell[,] board, int row, int col, int dr, int dc)
    {
        int count = 0;
        int r = row + dr;
        int c = col + dc;

        // 한 칸 씩 이동하면서 Marker가 같다면 count++ 아니면 반복 종료
        while (r >= 0 && r < Board.BoardRow && c >= 0 && c < Board.BoardCol && board[r, c].Marker == marker)
        {
            count++;
            r += dr;
            c += dc;
        }
        return count;
    }

}
