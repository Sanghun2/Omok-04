using UnityEngine;
using System;

public static class OmokAI
{
    public static bool CheckGameWin(Cell.CellMarker marker, Cell[,] board, int row, int col)
    {
        int minRow = Math.Max(row - 4, 0);
        int maxRow = Math.Min(row + 4, 14);
        int minCol = Math.Max(col - 4, 0);
        int maxCol = Math.Min(col + 4, 14);

        // 중앙 7*7 범위에 대해서 확인
        if (row >= 4 && row <= 10 && col >= 4 && col <= 10)
        {
            for (int i = minRow; i <= maxRow - 4; i++)
            {
                for (int j = minCol; j <= maxCol - 4; j++)
                {
                    // 돌을 놓은 지점의 가로 세로 대각선만 확인
                    if (j == col) 
                    { 
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j].Marker == marker &&
                            board[i + 2, j].Marker == marker &&
                            board[i + 3, j].Marker == marker &&
                            board[i + 4, j].Marker == marker)
                            return true;
                    }
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j + 1].Marker == marker &&
                            board[i, j + 2].Marker == marker &&
                            board[i, j + 3].Marker == marker &&
                            board[i, j + 4].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col))
                    {
                        if (board[i,j].Marker == marker &&
                            board[i + 1, j + 1].Marker == marker &&
                            board[i + 2, j + 2].Marker == marker &&
                            board[i + 3, j + 3].Marker == marker &&
                            board[i + 4, j + 4].Marker == marker)
                            return true;
                    }
                    if ((i + j + 4) == (row + col))
                    {
                        if (board[i,j + 4].Marker == marker &&
                            board[i + 1, j +3].Marker == marker &&
                            board[i + 2, j + 2].Marker == marker &&
                            board[i + 3, j + 1].Marker == marker &&
                            board[i + 4, j].Marker == marker)
                            return true;
                    }
                }
            }
        }
        // 왼쪽 아래 5*5 범위에 대해서 확인
        else if(row <= 4 && col <= 4)
        {
            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j + 1].Marker == marker &&
                            board[i, j + 2].Marker == marker &&
                            board[i, j + 3].Marker == marker &&
                            board[i, j + 4].Marker == marker)
                            return true;
                    }

                    if (j == col)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j].Marker == marker &&
                            board[i + 2, j].Marker == marker &&
                            board[i + 3, j].Marker == marker &&
                            board[i + 4, j].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col))
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j + 1].Marker == marker &&
                            board[i + 2, j + 2].Marker == marker &&
                            board[i + 3, j + 3].Marker == marker &&
                            board[i + 4, j + 4].Marker == marker)
                            return true;
                    }

                    if ((row + col) >= 4 && (i + j) == (row + col) && (j -4) >= 0)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j - 1].Marker == marker &&
                            board[i + 2, j - 2].Marker == marker &&
                            board[i + 3, j - 3].Marker == marker &&
                            board[i + 4, j - 4].Marker == marker)
                            return true;
                    }
                }
            }
        }
        // 오른쪽 위 5*5 범위에 대해서 확인
        else if(row >= 10 && col >= 10)
        {
            for (int i = maxRow; i >= minRow; i--)
            {
                for (int j = maxCol; j >= minCol; j--)
                {
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j - 1].Marker == marker &&
                            board[i, j - 2].Marker == marker &&
                            board[i, j - 3].Marker == marker &&
                            board[i, j - 4].Marker == marker)
                            return true;
                    }

                    if (j == col)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i - 1, j].Marker == marker &&
                            board[i - 2, j].Marker == marker &&
                            board[i - 3, j].Marker == marker &&
                            board[i - 4, j].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col))
                    {
                        if (board[i, j].Marker == marker &&
                            board[i - 1, j - 1].Marker == marker &&
                            board[i - 2, j - 2].Marker == marker &&
                            board[i - 3, j - 3].Marker == marker &&
                            board[i - 4, j - 4].Marker == marker)
                            return true;
                    }

                    if ((row + col) <= 24 && (i + j) == (row + col) && (i +4) <= 14)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i+1, j - 1].Marker == marker &&
                            board[i+2, j - 2].Marker == marker &&
                            board[i+3, j - 3].Marker == marker &&
                            board[i+4, j - 4].Marker == marker)
                            return true;
                    }
                }
            }
        }
        // 왼쪽 위 5*5 범위에 대해서 확인
        else if (row <= 4 && col >= 10)
        {
            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = maxCol; j >= minCol; j--)
                {
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j - 1].Marker == marker &&
                            board[i, j - 2].Marker == marker &&
                            board[i, j - 3].Marker == marker &&
                            board[i, j - 4].Marker == marker)
                            return true;
                    }

                    if (j == col)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j].Marker == marker &&
                            board[i + 2, j].Marker == marker &&
                            board[i + 3, j].Marker == marker &&
                            board[i + 4, j].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col) && (i-j) >= -10 && (j + 4) <= 14)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i+ 1, j + 1].Marker == marker &&
                            board[i + 2, j + 2].Marker == marker &&
                            board[i + 3, j + 3].Marker == marker &&
                            board[i + 4, j + 4].Marker == marker)
                            return true;
                    }

                    if ((i + j) == (row + col))
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j - 1].Marker == marker &&
                            board[i + 2, j - 2].Marker == marker &&
                            board[i + 3, j - 3].Marker == marker &&
                            board[i + 4, j - 4].Marker == marker)
                            return true;
                    }
                }
            }
        }
        // 오른쪽 아래 5*5 범위에 대해서 확인
        else if (row  > 10 && col < 4)
        {
            for (int i = maxRow; i >= minRow; i--)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j + 1].Marker == marker &&
                            board[i, j + 2].Marker == marker &&
                            board[i, j + 3].Marker == marker &&
                            board[i, j + 4].Marker == marker)
                            return true;
                    }

                    if (j == col)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i - 1, j].Marker == marker &&
                            board[i - 2, j].Marker == marker &&
                            board[i - 3, j].Marker == marker &&
                            board[i - 4, j].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col) && (i-j) <= 10 && (i + 4) <= 14)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i+ 1, j + 1].Marker == marker &&
                            board[i + 2, j + 2].Marker == marker &&
                            board[i + 3, j + 3].Marker == marker &&
                            board[i + 4, j + 4].Marker == marker)
                            return true;
                    }

                    if ((i + j) == (row + col))
                    {
                        if (board[i, j].Marker == marker &&
                            board[i - 1, j + 1].Marker == marker &&
                            board[i - 2, j + 2].Marker == marker &&
                            board[i - 3, j + 3].Marker == marker &&
                            board[i - 4, j + 4].Marker == marker)
                            return true;
                    }
                }
            }
        }
        // 왼쪽 중앙 4*7 범위에 대해서 확인
        else if (row <= 4)
        {
            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    if (j == col)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j].Marker == marker &&
                            board[i + 2, j].Marker == marker &&
                            board[i + 3, j].Marker == marker &&
                            board[i + 4, j].Marker == marker)
                            return true;
                    }
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j + 1].Marker == marker &&
                            board[i, j + 2].Marker == marker &&
                            board[i, j + 3].Marker == marker &&
                            board[i, j + 4].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col))
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j + 1].Marker == marker &&
                            board[i + 2, j + 2].Marker == marker &&
                            board[i + 3, j + 3].Marker == marker &&
                            board[i + 4, j + 4].Marker == marker)
                            return true;
                    }
                    if ((i + j) == (row + col) && (j - 4) >= 0)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j - 1].Marker == marker &&
                            board[i + 2, j - 2].Marker == marker &&
                            board[i + 3, j - 3].Marker == marker &&
                            board[i + 4, j - 4].Marker == marker)
                            return true;
                    }
                }
            }
        }
        // 오른쪽 중앙 4*7 범위에 대해서 확인
        else if (row >= 10)
        {
            for (int i = maxRow; i >= minRow; i--)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    if (j == col)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i - 1, j].Marker == marker &&
                            board[i - 2, j].Marker == marker &&
                            board[i - 3, j].Marker == marker &&
                            board[i - 4, j].Marker == marker)
                            return true;
                    }
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j + 1].Marker == marker &&
                            board[i, j + 2].Marker == marker &&
                            board[i, j + 3].Marker == marker &&
                            board[i, j + 4].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col))
                    {
                        if (board[i, j].Marker == marker &&
                            board[i - 1, j - 1].Marker == marker &&
                            board[i - 2, j - 2].Marker == marker &&
                            board[i - 3, j - 3].Marker == marker &&
                            board[i - 4, j - 4].Marker == marker)
                            return true;
                    }
                    if ((i + j) == (row + col) && (i + 4) <= 14 && (j - 4) >= 0)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j - 1].Marker == marker &&
                            board[i + 2, j - 2].Marker == marker &&
                            board[i + 3, j - 3].Marker == marker &&
                            board[i + 4, j - 4].Marker == marker)
                            return true;
                    }
                }
            }
        }
        // 아래쪽 중앙 7*4 범위에 대해서 확인
        else if (col <= 4)
        {
            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    if (j == col)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j].Marker == marker &&
                            board[i + 2, j].Marker == marker &&
                            board[i + 3, j].Marker == marker &&
                            board[i + 4, j].Marker == marker)
                            return true;
                    }
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j + 1].Marker == marker &&
                            board[i, j + 2].Marker == marker &&
                            board[i, j + 3].Marker == marker &&
                            board[i, j + 4].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col))
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j + 1].Marker == marker &&
                            board[i + 2, j + 2].Marker == marker &&
                            board[i + 3, j + 3].Marker == marker &&
                            board[i + 4, j + 4].Marker == marker)
                            return true;
                    }
                    if ((i + j) == (row + col) && (j - 4) >= 0)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j - 1].Marker == marker &&
                            board[i + 2, j - 2].Marker == marker &&
                            board[i + 3, j - 3].Marker == marker &&
                            board[i + 4, j - 4].Marker == marker)
                            return true;
                    }                    
                }
            }
        }
        // 위쪽 중앙 7*4 범위에 대해서 확인
        else if (col >= 10)
        {
            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = maxCol; j >= minCol; j--)
                {
                    if (j == col)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j].Marker == marker &&
                            board[i + 2, j].Marker == marker &&
                            board[i + 3, j].Marker == marker &&
                            board[i + 4, j].Marker == marker)
                            return true;
                    }
                    if (i == row)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i, j - 1].Marker == marker &&
                            board[i, j - 2].Marker == marker &&
                            board[i, j - 3].Marker == marker &&
                            board[i, j - 4].Marker == marker)
                            return true;
                    }
                    if ((i - j) == (row - col) && (i - 4) >= 0)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i - 1, j - 1].Marker == marker &&
                            board[i - 2, j - 2].Marker == marker &&
                            board[i - 3, j - 3].Marker == marker &&
                            board[i - 4, j - 4].Marker == marker)
                            return true;
                    }
                    if ((i + j) == (row + col) && (i + 4) <= 14)
                    {
                        if (board[i, j].Marker == marker &&
                            board[i + 1, j - 1].Marker == marker &&
                            board[i + 2, j - 2].Marker == marker &&
                            board[i + 3, j - 3].Marker == marker &&
                            board[i + 4, j - 4].Marker == marker)
                            return true;
                    }
                }
            }
        }

        return false;
    }
}
