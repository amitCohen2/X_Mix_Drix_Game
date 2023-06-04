using Ex02;
using System;

namespace Ex02
{
    public class GameLogic
    {

        Board m_Board = null;


        // < ------------------ INIT METHODS ------------------ >
        public void InitGameLogicBoard(Board i_Board)
        {
            m_Board = i_Board;
        }

        // < ------------------ CHECK METHODS ------------------ >

        public bool CheckTie(ref bool rv_FullMatrixFlag) 
        {
            bool isFullFlag = false;

            if (m_Board.IsBoardFull() == true)
            {
                rv_FullMatrixFlag = true;
                isFullFlag = true;
            }

            return isFullFlag;
        }

        public bool CheckIfPlayerWin(int i_Row, int i_Col, Cell playerShape)
        {
            bool v_Result = false;

            v_Result = m_Board.CheckRowWin(i_Row, playerShape) || m_Board.CheckColWin(i_Col, playerShape) || m_Board.CheckDiagonalWin(playerShape);

            return v_Result;
        }

        // < ------------------ LOGIC METHODS ------------------ >

        public void SwitchPlayer(ref short ro_PlayerNum)
        {
            if (ro_PlayerNum == 1)
            {
                ro_PlayerNum++;
            }
            else
            {
                ro_PlayerNum--;
            }
        }

        public void FindBestCell(Cell[,] i_Mat, int i_Size, Cell i_PlayerShape, out int o_BestRow, out int o_Bestcol)
        {

            int minRow = 0, minCol = 0, minCount = i_Size * i_Size, count = 0;
            for (int i = 0; i < i_Size; i++)
            {
                count = 0;
                for (int j = 0; j < i_Size; j++)
                {
                    count = 0;
                    if (i_Mat[i, j] == Cell.Empty)
                    {
                        count += CountInRow(i_Mat, i_Size, i_PlayerShape, i);
                        count += CountInCol(i_Mat, i_Size, i_PlayerShape, j);
                        if (i == j)
                        {
                            count += CountInDiagonal(i_Mat, i_Size, i_PlayerShape);
                        }
                        if (j == i_Size - i - 1)
                        {
                            count += CountInDiagonal(i_Mat, i_Size, i_PlayerShape);
                        }
                        if (count < minCount)
                        {
                            minCount = count;
                            minRow = i;
                            minCol = j;
                        }
                    }

                }

            }
            o_BestRow = minRow;
            o_Bestcol = minCol;
        }

        public int CountInDiagonal(Cell[,] i_Mat, int i_Size, Cell i_Shape)
        {
            int count = 0;
            for (int i = 0; i < i_Size; i++)
            {
                if (i_Mat[i, i] == i_Shape)
                {
                    count++;
                }
            }



            for (int i = 0; i < i_Size; i++)
            {
                if (i_Mat[(i_Size - 1) - i, i] == i_Shape)
                {
                    count++;
                }
            }
            return count;
        }

        public int CountInRow(Cell[,] i_Mat, int i_Size, Cell shape, int i_Row)
        {
            int count = 0;
            for (int i = 0; i < i_Size; i++)
            {
                if (i_Mat[i_Row, i] == shape)
                {
                    count++;
                }
            }
            return count;
        }

        public int CountInCol(Cell[,] i_Mat, int i_Size, Cell i_Shape, int i_Col)
        {
            int count = 0;
            for (int i = 0; i < i_Size; i++)
            {
                if (i_Mat[i, i_Col] == i_Shape)
                {
                    count++;
                }
            }
            return count;
        }

    }
}
