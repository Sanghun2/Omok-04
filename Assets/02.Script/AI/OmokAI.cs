using UnityEngine;
using System;

public static class OmokAI
{
    private static readonly (int rowDir, int colDir)[] directions =
    {
        (0, 1),   // ���� ����
        (1, 0),   // ���� ����
        (1, 1),   // �밢�� �� ����
        (1, -1)   // �밢�� �� ����
    };

    public static bool CheckGameWin(Cell.CellMarker marker, Cell[,] board, int row, int col)
    {
        foreach (var (rowDir, colDir) in directions)
        {
            int count = 1; // ���� �� ����

            // ���� �������� üũ
            count += CountStones(marker, board, row, col, rowDir, colDir);

            // �ݴ� �������� üũ
            count += CountStones(marker, board, row, col, -rowDir, -colDir);

            if (count >= 5) return true;
        }
        return false;
    }

    /// <summary>
    /// ���ӵ� ��Ŀ ���� Ȯ��
    /// </summary>
    private static int CountStones(Cell.CellMarker marker, Cell[,] board, int row, int col, int rowDir, int colDir)
    {
        int count = 0;
        int r = row + rowDir;
        int c = col + colDir;

        // �� ĭ �� �̵��ϸ鼭 Marker�� ���ٸ� count++ �ƴϸ� �ݺ� ����
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

        //  ���� ���� �� �� ���� Ȯ��
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
            // ���� �������� üũ
            stoneCount += CountRenjuStone(marker, board, row, col, rowDir, colDir , stoneLimit);

            if (stoneCount == (stoneLimit - 1))
            {
                count++;
                stoneCount = 0;
            }

            // �ݴ� �������� üũ
            stoneCount += CountRenjuStone(marker, board, row, col, -rowDir, -colDir, stoneLimit);

            if (stoneCount == (stoneLimit - 1)) count++;

            stoneCount = 0;
        }
        if (count >= 2) return true;

        return false;
    }
}
