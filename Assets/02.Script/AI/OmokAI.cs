using System;
using UnityEngine;

public static class OmokAI
{
    private static readonly (int rowDir, int colDir)[] directions =
    {
        (0, 1),   // ���� ����
        (1, 0),   // ���� ����
        (1, 1),   // �밢�� �� ����
        (1, -1)   // �밢�� �� ����
    };

    public static bool CheckGameDraw(Cell[,] board)
    {
        foreach (var cell in board)
        {
            if(cell.Stone == Cell.StoneType.None)
                return false;
        }
        
        return true;
    }

    public static bool CheckGameWin(Cell.StoneType stoneType, Cell[,] board, int row, int col)
    {
        foreach (var (rowDir, colDir) in directions)
        {
            int count = 1; // ���� �� ����

            // ���� �������� üũ
            count += CountStones(stoneType, board, row, col, rowDir, colDir);

            // �ݴ� �������� üũ
            count += CountStones(stoneType, board, row, col, -rowDir, -colDir);

            if (count >= 5) return true;
        }
        return false;
    }

    /// <summary>
    /// ���ӵ� ��Ŀ ���� Ȯ��
    /// </summary>
    private static int CountStones(Cell.StoneType stoneType, Cell[,] board, int row, int col, int rowDir, int colDir)
    {
        int count = 0;
        int r = row + rowDir;
        int c = col + colDir;

        // �� ĭ �� �̵��ϸ鼭 Marker�� ���ٸ� count++ �ƴϸ� �ݺ� ����
        while (r >= 0 && r < BoardController.BoardRow && c >= 0 && c < BoardController.BoardCol && board[r, c].Stone == stoneType)
        {
            count++;
            r += rowDir;
            c += colDir;
        }
        return count;
    }

    public static bool CheckRenju(Cell.StoneType stoneType, Cell[,] board, int row, int col)
    {
        int openThreeDirs = 0; // ��� �Ǵܿ�
        int openFourDirs = 0;  // ��� �Ǵܿ�

        foreach (var (rowDir, colDir) in directions)
        {
            // �߽ɿ� marker�� �� ���·� �ش� ������ ���� 9(������ -4..+4) ���ڿ��� �����
            char[] line = BuildLineWithPlacement(board, stoneType, row, col, rowDir, colDir);

            if (HasOpenThreeOrFour(line, 4))
                openFourDirs++;

            if (HasOpenThreeOrFour(line, 3))            
                openThreeDirs++;            
        }

        // ���
        if (openThreeDirs >= 2)
            return true;

        // ���
        if (openFourDirs >= 2)        
            return true;
        


        return false;
    }

    /// <summary>
    /// �߽ɿ� ���� �дٰ� �����ؼ�, ������ -4..+4 ���� ���� �迭�� �����.
    /// 'X' = �� ��(���� ����), '_' = ��ĭ, 'O' = ��뵹 �Ǵ� ���� ��(��).
    /// </summary>
    private static char[] BuildLineWithPlacement(Cell[,] board, Cell.StoneType stoneType, int row, int col, int dr, int dc)
    {
        char[] line = new char[11];
        for (int offset = -5; offset <= 5; offset++)
        {
            int r = row + dr * offset;
            int c = col + dc * offset;
            int idx = offset + 5;

            if (offset == 0)
            {
                // �츮�� ���� �� (����)
                line[idx] = 'X';
            }
            else if (r < 0 || r >= BoardController.BoardRow || c < 0 || c >= BoardController.BoardCol)
            {
                // ���� ���� ������ ���
                line[idx] = 'O';
            }
            else
            {
                var m = board[r, c].Stone;
                if (m == Cell.StoneType.None) line[idx] = '_'; // ��ĭ�� '_'
                else if (m == stoneType) line[idx] = 'X';
                else line[idx] = 'O';
            }
        }
        return line;

    }/// <summary>
     /// �־��� ���ο��� ���� 3 �Ǵ� 4�� �ִ��� �˻�
     /// </summary>
    private static bool HasOpenThreeOrFour(char[] line, int number)
    {
        // ������ �˻�
        for (int s = 5 - number; s <= line.Length - 6; s++)
        {
            int xCount = 0;
            int emptyCount = 0;
            bool hasBlock = false;

            for (int k = s; k < s + 1 + number; k++)
            {
                if (line[k] == 'X')
                    xCount++;
                else if (line[k] == '_') // ��ĭ
                    emptyCount++;
                else // 'O' ���� �� ���� ���¶� �Ұ���
                {
                    hasBlock = true;
                    break;
                }
            }

            // 4���� �� + 1���� ��ĭ
            if (!hasBlock && xCount == number && emptyCount == 1)
            {
                // �ٱ��� �糡 Ȯ��
                bool leftOpen = (s - 1 >= 4 - number && line[s - 1] == '_');
                bool rightOpen = (s + 1 + number < line.Length - 4 + number && line[s + 1 + number] == '_');

                if (leftOpen && rightOpen)
                    return true;
            }
        }
        return false;
    }
}
