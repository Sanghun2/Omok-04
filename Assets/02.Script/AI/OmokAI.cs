using System;
using UnityEngine;

public static class OmokAI
{
    private static readonly (int rowDir, int colDir)[] directions =
    {
        (0, 1),   // 가로 방향
        (1, 0),   // 세로 방향
        (1, 1),   // 대각선 ↘ 방향
        (1, -1)   // 대각선 ↙ 방향
    };

    public static bool CheckGameDraw(Cell[,] board)
    {
        foreach (var cell in board)
        {
            if(cell.Stone == Define.Type.StoneColor.None)
                return false;
        }
        
        return true;
    }

    public static bool CheckGameWin(Define.Type.StoneColor stoneColor, Cell[,] board, int row, int col)
    {
        foreach (var (rowDir, colDir) in directions)
        {
            int count = 1; // 현재 돌 포함

            // 한쪽 방향으로 체크
            count += CountStones(stoneColor, board, row, col, rowDir, colDir);

            // 반대 방향으로 체크
            count += CountStones(stoneColor, board, row, col, -rowDir, -colDir);

            if (count >= 5) return true;
        }
        return false;
    }

    /// <summary>
    /// 연속된 마커 개수 확인
    /// </summary>
    private static int CountStones(Define.Type.StoneColor stoneColor, Cell[,] board, int row, int col, int rowDir, int colDir)
    {
        int count = 0;
        int r = row + rowDir;
        int c = col + colDir;

        // 한 칸 씩 이동하면서 Marker가 같다면 count++ 아니면 반복 종료
        while (r >= 0 && r < BoardController.BoardRow && c >= 0 && c < BoardController.BoardCol && board[r, c].Stone == stoneColor)
        {
            count++;
            r += rowDir;
            c += colDir;
        }
        return count;
    }

    public static bool CheckRenju(Define.Type.StoneColor stoneColor, Cell[,] board, int row, int col)
    {
        if (stoneColor == Define.Type.StoneColor.White)
            return false;

        int openThreeDirs = 0; // 삼삼 판단용
        int openFourDirs = 0;  // 사사 판단용

        foreach (var (rowDir, colDir) in directions)
        {
            // 중심에 marker를 둔 상태로 해당 방향의 길이 9(오프셋 -4..+4) 문자열을 만든다
            char[] line = BuildLineWithPlacement(board, stoneColor, row, col, rowDir, colDir);

            if (HasOpenThreeOrFour(line, 4))
                openFourDirs++;

            if (HasOpenThreeOrFour(line, 3))            
                openThreeDirs++;            
        }

        // 삼삼
        if (openThreeDirs >= 2)
            return true;

        // 사사
        if (openFourDirs >= 2)        
            return true;
        


        return false;
    }

    /// <summary>
    /// 중심에 돌을 둔다고 가정해서, 오프셋 -4..+4 으로 문자 배열을 만든다.
    /// 'X' = 내 돌(가정 포함), '_' = 빈칸, 'O' = 상대돌 또는 보드 밖(벽).
    /// </summary>
    private static char[] BuildLineWithPlacement(Cell[,] board, Define.Type.StoneColor stoneColor, int row, int col, int dr, int dc)
    {
        char[] line = new char[11];
        for (int offset = -5; offset <= 5; offset++)
        {
            int r = row + dr * offset;
            int c = col + dc * offset;
            int idx = offset + 5;

            if (offset == 0)
            {
                // 우리가 놓는 돌 (가정)
                line[idx] = 'X';
            }
            else if (r < 0 || r >= BoardController.BoardRow || c < 0 || c >= BoardController.BoardCol)
            {
                // 보드 밖은 벽으로 취급
                line[idx] = 'O';
            }
            else
            {
                var m = board[r, c].Stone;
                if (m == Define.Type.StoneColor.None) line[idx] = '_'; // 빈칸은 '_'
                else if (m == stoneColor) line[idx] = 'X';
                else line[idx] = 'O';
            }
        }
        return line;

    }/// <summary>
     /// 주어진 라인에서 열린 3 또는 4가 있는지 검사
     /// </summary>
    private static bool HasOpenThreeOrFour(char[] line, int number)
    {
        // 윈도우 검사
        for (int s = 5 - number; s <= line.Length - 6; s++)
        {
            int xCount = 0;
            int emptyCount = 0;
            bool hasBlock = false;

            for (int k = s; k < s + 1 + number; k++)
            {
                if (line[k] == 'X')
                    xCount++;
                else if (line[k] == '_') // 빈칸
                    emptyCount++;
                else // 'O' 포함 → 막힌 형태라 불가능
                {
                    hasBlock = true;
                    break;
                }
            }

            // 4개의 돌 + 1개의 빈칸
            if (!hasBlock && xCount == number && emptyCount == 1)
            {
                // 바깥쪽 양끝 확인
                bool leftOpen = (s - 1 >= 4 - number && line[s - 1] == '_');
                bool rightOpen = (s + 1 + number < line.Length - 4 + number && line[s + 1 + number] == '_');

                if (leftOpen && rightOpen)
                    return true;
            }
        }
        return false;
    }
}
