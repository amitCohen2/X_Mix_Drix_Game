using System;

namespace Ex02
{
    public enum Cell
    {
        P1,
        P2,
        Empty
    }

    public class Board
    {
        int m_MatrixSize { get; set; }
        int m_EmptyCells { get; set; }

        Cell[,] m_GameMatrix;

        // C'tor
        public Board(int i_Size)
        {
            m_MatrixSize = i_Size;
            m_EmptyCells = m_MatrixSize * m_MatrixSize;
            InitGameMatrix();
        }

        // < ------------------ INIT METHODS ------------------ >

        public void InitGameMatrix()
        {

            m_GameMatrix = new Cell[m_MatrixSize, m_MatrixSize];
            initCellsMatrix();
        }

        public void initCellsMatrix()
        {
            m_EmptyCells = m_MatrixSize * m_MatrixSize;
            for (int i = 0; i < m_MatrixSize; i++)
            {
                for (int j = 0; j < m_MatrixSize; j++)
                {
                    m_GameMatrix[i, j] = Cell.Empty;
                }
            }
        }

        // < ------------------ GET METHODS ------------------ >

        public int GetMatsize()
        {
            return m_MatrixSize;
        }

        public Cell[,] GetMat()
        {
            return m_GameMatrix;
        }

        public int GetEmptyCells()
        {
            return m_EmptyCells;
        }

        // < ------------------ SET METHODS ------------------ >

        public void SetCell(int i_Row, int i_Col, Cell i_PlayerShape)
        {
            m_GameMatrix[i_Row, i_Col] = i_PlayerShape;
            m_EmptyCells--;
        }



        // < ------------------ CHECK METHODS ------------------ >

        public bool CheckUserCellChoiceRangeInput(int i_Input)
        {
            return (i_Input <= m_MatrixSize && i_Input > 0);
        }

        public bool CheckIfUserCellChoiceEmpty(int i_Row, int i_Col)
        {
            return m_GameMatrix[i_Row - 1, i_Col - 1] == Cell.Empty;
        }

        public bool CheckColWin(int i_Col, Cell i_PlayerShape)
        {
            bool v_ResFlag = true;


            for (int i = 0; i < m_MatrixSize; i++)
            {
                if (m_GameMatrix[i, i_Col] != i_PlayerShape)
                {
                    v_ResFlag = false;
                    break;
                }
            }

            return v_ResFlag;
        }

        public bool CheckRowWin(int i_Row, Cell i_PlayerShape)
        {
            bool v_ResFlag = true;

            for (int i = 0; i < m_MatrixSize; i++)
            {
                if (m_GameMatrix[i_Row, i] != i_PlayerShape)
                {
                    v_ResFlag = false;
                    break;
                }
            }

            return v_ResFlag;
        }

        public bool CheckDiagonalWin(Cell i_PlayerShape)
        {
            bool v_DiagonalFlag1 = true, v_DiagonalFlag2 = true;
            for (int i = 0; i < m_MatrixSize; i++)
            {
                if (m_GameMatrix[i, i] != i_PlayerShape)
                {
                    v_DiagonalFlag1 = false;
                    break;
                }
            }



            for (int i = 0; i < m_MatrixSize; i++)
            {
                if (m_GameMatrix[(m_MatrixSize - 1) - i, i] != i_PlayerShape)
                {
                    v_DiagonalFlag2 = false;
                    break;
                }
            }


            return (v_DiagonalFlag1 || v_DiagonalFlag2);
        }

        public bool IsBoardFull()
        {
            return GetEmptyCells() == 0;
        }

    }
}
