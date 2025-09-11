using UnityEngine;
using System;

public static class OmokAI
{
    private static readonly (int dr, int dc)[] directions =
    {
        (0, 1),   // ���� ����
        (1, 0),   // ���� ����
        (1, 1),   // �밢�� �� ����
        (1, -1)   // �밢�� �� ����
    };

    public static bool CheckGameWin(Cell.CellMarker marker, Cell[,] board, int row, int col)
    {
        foreach (var (dr, dc) in directions)
        {
            int count = 1; // ���� �� ����

            // ���� �������� üũ
            count += CountStones(marker, board, row, col, dr, dc);

            // �ݴ� �������� üũ
            count += CountStones(marker, board, row, col, -dr, -dc);

            if (count >= 5) return true;
        }
        return false;
    }

    /// <summary>
    /// ���ӵ� ��Ŀ ���� Ȯ��
    /// </summary>
    private static int CountStones(Cell.CellMarker marker, Cell[,] board, int row, int col, int dr, int dc)
    {
        int count = 0;
        int r = row + dr;
        int c = col + dc;

        // �� ĭ �� �̵��ϸ鼭 Marker�� ���ٸ� count++ �ƴϸ� �ݺ� ����
        while (r >= 0 && r < Board.BoardRow && c >= 0 && c < Board.BoardCol && board[r, c].Marker == marker)
        {
            count++;
            r += dr;
            c += dc;
        }
        return count;
    }

}
