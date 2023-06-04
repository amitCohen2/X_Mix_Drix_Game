using System;
using Ex02;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class UserInterface 
    {
        Board m_Board;
        Player m_Player1, m_Player2;
        int m_MatrixSize;
        bool mv_IsPlayerTwoHuman;
        Player[] m_PlayersArr;
        GameLogic m_GameLogic;

        // C'tor
        public UserInterface()
        {
            m_GameLogic = new GameLogic();
        }

        // < ------------------ RUN & PLAY METHODS ------------------ >

        public void Run()
        {
            bool v_IsPlayerWantToExit = false;

            while (false == v_IsPlayerWantToExit)
            {
                bool v_Hard;
                PrintPreMenu();
                mv_IsPlayerTwoHuman = IsPlayerTwoHumanFlag(out v_Hard, ref v_IsPlayerWantToExit);

                // Choose 'Q' For Exit
                if (v_IsPlayerWantToExit)
                {
                    WantToExit();
                    break;
                }

                EnterPlayerName(1);
                string player1Name = GetInputFromUser(), player2Name;

                // Choose 'Q' For Exit
                if (CheckUserToExitWithQ(player1Name))
                {
                    WantToExit();
                    break;
                }

                m_Player1 = new Player(Cell.P1, player1Name, true, v_Hard);
                m_MatrixSize = GetBoardSizeFromUser(ref v_IsPlayerWantToExit);

                // Choose 'Q' For Exit
                if (false == v_IsPlayerWantToExit)
                {
                    m_Board = new Board(m_MatrixSize);
                    m_GameLogic.InitGameLogicBoard(m_Board);
                }
                else
                {
                    WantToExit();
                    break;
                }

                GetInputName2User(player1Name, out player2Name, ref v_IsPlayerWantToExit);

                
                if (v_IsPlayerWantToExit) 
                {
                    WantToExit();
                    break;
                }
                else if (mv_IsPlayerTwoHuman)
                {

                    m_Player2 = new Player(Cell.P2, player2Name, true, v_Hard);
                }
                else
                {
                    m_Player2 = new Player(Cell.P2, player2Name, false, v_Hard);

                }
                initPlayersArr();

                PlayGame(ref v_IsPlayerWantToExit);
            }
        }


        private void PlayGame(ref bool iov_IsPlayerWantToExit)
        {
            bool v_WantToExitWithQ = false;

            while (v_WantToExitWithQ == false && iov_IsPlayerWantToExit == false)
            {
                bool v_FullMatrix = false, v_WinGame = false;
                short playerNum = 1;
                m_Board.initCellsMatrix();
                PrintMatrixGame(m_Board.GetMat(), m_Board.GetMatsize());


                while (v_FullMatrix == false && v_WinGame == false)
                {

                    if (m_PlayersArr[playerNum - 1].m_IsHuman == true)
                    {
                        GetUserCellInput(ref v_WantToExitWithQ, ref v_FullMatrix, ref v_WinGame, playerNum);
                    }
                    else
                    {
                        GetComputerCellInput(ref v_WantToExitWithQ, ref v_WinGame, playerNum, m_PlayersArr[playerNum - 1].m_isDifficultyov_Hard);
                    }

                    if (v_WantToExitWithQ)
                    {
                        iov_IsPlayerWantToExit = true;
                        break;
                    }
                    else if (v_WinGame)
                    {
                        UpdateWin(playerNum, m_PlayersArr);
                        ItsAWin(playerNum);
                        break;
                    }
                    else
                    {
                        PrintMatrixGame(m_Board.GetMat(), m_Board.GetMatsize());
                    }

                    m_GameLogic.SwitchPlayer(ref playerNum);

                }

                if (v_WantToExitWithQ == true)
                {
                    WantToExit();
                    break;
                }
                else if (v_FullMatrix == true)
                {
                    ItsATie();
                    UpdateGameToTie(m_PlayersArr);
                }

                PrintCurrentPoints(m_PlayersArr);
                if (IsPlayerWantToExit(Console.ReadLine()))
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    break;
                }
            }

        }

        // < ------------------ INIT METHODS ------------------ >

        private void initPlayersArr()
        {
            m_PlayersArr = new Player[2] { m_Player1, m_Player2 };
        }

        // < ------------------ GET INPUT METHODS ------------------ >


        private void GetInputName2User(string i_Player1Name, out string i_Player2Name, ref bool rv_ExitFlag)
        {
            EnterPlayerName(2);
            i_Player2Name = GetInputFromUser();

            if (CheckUserToExitWithQ(i_Player2Name))
            {
                rv_ExitFlag = true;
            }

            while (IsSameName(i_Player1Name, i_Player2Name))
            {
                Console.WriteLine("You have the same name like 1 player.. Please Enter another name");
                EnterPlayerName(2);
                i_Player2Name = GetInputFromUser();
            }
        }

        private string GetInputFromUser()
        {
            return Console.ReadLine();
        }

        private int GetBoardSizeFromUser(ref bool rv_ExitFlag)
        {
            EnterMatrixSize();

            string inputMatrixSizeStr = GetInputFromUser();
            int inputMatrixSize = 0;

            if (false == CheckUserToExitWithQ(inputMatrixSizeStr))
            {
                bool v_CheckInputFlag = CheckMatrixSizeInput(inputMatrixSizeStr, out inputMatrixSize);

                while (v_CheckInputFlag == false)
                {
                    PrintErrorMatrixSize();
                    inputMatrixSizeStr = GetInputFromUser();
                    v_CheckInputFlag = CheckMatrixSizeInput(inputMatrixSizeStr, out inputMatrixSize);
                }
            }
            else
            {
                rv_ExitFlag = true;
            }
           
            return inputMatrixSize;
        }

        private Cell GetPlayerShape(short i_PlayerNum)
        {
            return (i_PlayerNum == 1) ? Cell.P1 : Cell.P2;
        }

        private void GetComputerCellInput(ref bool rv_FullMatrixFlag, ref bool rv_WinGameFlag, short i_PlayerNum, bool iv_Hard)
        {
            Random random = new Random();
            bool v_CorrectInput = false;
            int min = 0, max = m_Board.GetMatsize() + 1;
            int errorNum = 0;
            int BestRow, BestCol;
            string inputUserRowChoiceStr;
            string inputUserColChoiceStr;
            Cell playerShape = GetPlayerShape(i_PlayerNum);

            while (v_CorrectInput == false)
            {
                if (m_Board.GetEmptyCells() == 0)
                {
                    rv_FullMatrixFlag = true;
                    break;
                }
                if (iv_Hard)
                {
                    m_GameLogic.FindBestCell(m_Board.GetMat(), m_Board.GetMatsize(), playerShape, out BestRow, out BestCol);
                    inputUserRowChoiceStr = (BestRow + 1).ToString();
                    inputUserColChoiceStr = (BestCol + 1).ToString();
                }
                else
                {
                    // get row
                    inputUserRowChoiceStr = random.Next(min, max).ToString();

                    // get col
                    inputUserColChoiceStr = random.Next(min, max).ToString();
                }


                int userColChoice, userRowChoice;

                RoundInputCheck(inputUserRowChoiceStr, inputUserColChoiceStr, out userRowChoice, out userColChoice, ref errorNum);

                if (errorNum == 0)
                {
                    v_CorrectInput = true;
                    m_Board.SetCell(userRowChoice - 1, userColChoice - 1, playerShape);
                    if (m_GameLogic.CheckIfPlayerWin(userRowChoice - 1, userColChoice - 1, playerShape))
                    {
                        ItsAWin(i_PlayerNum);
                        rv_WinGameFlag = true;
                    }
                    Ex02.ConsoleUtils.Screen.Clear();
                    break;
                }

                //PrintInputCellError(errorNum);
            }
        }

        private bool IsPlayerWantToExit(string i_ExitStr)
        {
            bool IsWantToExit;
            while (i_ExitStr != "y" && i_ExitStr != "n")
            {
                WrongExitInput();
                i_ExitStr = GetInputFromUser();
            }

            if (i_ExitStr == "n")
            {
                IsWantToExit = true;
            }
            else
            {
                IsWantToExit = false;
            }

            return IsWantToExit;
        }

        private bool IsPlayerTwoHumanFlag(out bool ov_Hard, ref bool rv_ExitFlag)
        {
            string playerTwoStr = GetInputFromUser();
            int playerTwoFlag;
            bool v_IsPlayerTwoHuman;
            ov_Hard = false;
            bool v_CheckInputFlag = CheckPlayerTwoInput(playerTwoStr, out playerTwoFlag, ref rv_ExitFlag);

            while (false == v_CheckInputFlag && false == rv_ExitFlag)
            {
                Ex02.ConsoleUtils.Screen.Clear();
                WrongInput();

                PrintPreMenu();
                //v_CheckInputFlag = CheckPlayerTwoInput(playerTwoStr, out playerTwoFlag);
                playerTwoStr = GetInputFromUser();
                v_CheckInputFlag = CheckPlayerTwoInput(playerTwoStr, out playerTwoFlag, ref rv_ExitFlag);
            }

            v_IsPlayerTwoHuman = playerTwoFlag == 1 ? true : false;
            if (playerTwoFlag == 3) { ov_Hard = true; }
            return v_IsPlayerTwoHuman;

        }

        private void GetUserCellInput(ref bool rv_ExitFlag, ref bool rv_FullMatrixFlag, ref bool rv_WinGameFlag, short i_PlayerNum)
        {
            bool v_CorrectInput = false;
            int errorNum = 0;
            Cell playerShape = m_PlayersArr[i_PlayerNum - 1].m_PlayerShape; // To Change this logic

            while (v_CorrectInput == false)
            {
                if (m_GameLogic.CheckTie(ref rv_FullMatrixFlag)) // m_Board.IsBoardFull(ref rv_FullMatrixFlag)
                {
                    break;
                }

                // get row
                EnterRow(m_PlayersArr[i_PlayerNum - 1].m_Name, m_PlayersArr[i_PlayerNum - 1].m_PlayerShape);
                string inputUserRowChoiceStr = GetInputFromUser();
                if (CheckUserToExitWithQ(inputUserRowChoiceStr))
                {
                    rv_ExitFlag = true;
                    break;
                }

                // get col
                EnterCol(m_PlayersArr[i_PlayerNum - 1].m_Name, m_PlayersArr[i_PlayerNum - 1].m_PlayerShape);
                string inputUserColChoiceStr = GetInputFromUser();
                if (CheckUserToExitWithQ(inputUserColChoiceStr))
                {
                    rv_ExitFlag = true;
                    break;
                }

                int userColChoice, userRowChoice;

                RoundInputCheck(inputUserRowChoiceStr, inputUserColChoiceStr, out userRowChoice, out userColChoice, ref errorNum);

                if (errorNum == 0)
                {
                    v_CorrectInput = true;
                    m_Board.SetCell(userRowChoice - 1, userColChoice - 1, playerShape);
                    if (m_GameLogic.CheckIfPlayerWin(userRowChoice - 1, userColChoice - 1, playerShape))
                    {
                        rv_WinGameFlag = true;
                    }
                    Ex02.ConsoleUtils.Screen.Clear();
                    break;
                }

                PrintInputCellError(errorNum, m_MatrixSize);
            }

        }



        // < ------------------ CHECK INPUT METHODS ------------------ >

        private bool CheckMatrixSizeInput(string i_MatrixSizeStr, out int o_MatrixSize)
        {
            bool v_CheckInputResult = int.TryParse(i_MatrixSizeStr, out o_MatrixSize);

            if (v_CheckInputResult == true && (o_MatrixSize < 3 || o_MatrixSize > 9))
            {
                v_CheckInputResult = false;
            }

            return v_CheckInputResult;
        }


        private bool CheckPlayerTwoInput(string i_IntplayerTwoStr, out int o_PlayerTwoFlag, ref bool rv_ExitFlag)
        {
            if (CheckUserToExitWithQ(i_IntplayerTwoStr))
            {
                rv_ExitFlag = true;
            }

            bool v_CheckInputResult = int.TryParse(i_IntplayerTwoStr, out o_PlayerTwoFlag);

            if (v_CheckInputResult == true)
            {
                if (o_PlayerTwoFlag != 1 && o_PlayerTwoFlag != 2 && o_PlayerTwoFlag != 3)
                {

                    v_CheckInputResult = false;
                }
            }

            return v_CheckInputResult;
        }


        private bool CheckUserToExitWithQ(string i_Input)
        {
            return i_Input == "Q";
        }

        private void EnterMatrixSize()
        {
            Console.WriteLine("Please enter a matrix size between 3-9: ");
        }

        private bool CheckUserEnterNumber(string i_Input, out int o_ParseInput)
        {
            return int.TryParse(i_Input, out o_ParseInput);
        }

        private void RoundInputCheck(string i_InputUserRowChoiceStr, string i_InputUserColChoiceStr, out int o_UserRowChoice, out int o_UserColChoice, ref int or_ErrorNum)
        {
            if (CheckUserEnterNumber(i_InputUserRowChoiceStr, out o_UserRowChoice) == false || CheckUserEnterNumber(i_InputUserColChoiceStr, out o_UserColChoice) == false)
            {
                or_ErrorNum = 1;
            }
            else if (m_Board.CheckUserCellChoiceRangeInput(o_UserColChoice) == false || m_Board.CheckUserCellChoiceRangeInput(o_UserRowChoice) == false)
            {
                or_ErrorNum = 2;
            }
            else if (m_Board.CheckIfUserCellChoiceEmpty(o_UserRowChoice, o_UserColChoice) == false)
            {
                or_ErrorNum = 3;
            }
            else
            {
                or_ErrorNum = 0;
            }
            CheckUserEnterNumber(i_InputUserColChoiceStr, out o_UserColChoice);
        }

        private bool IsSameName(string player1Name, string player2Name)
        {
            return (player1Name == player2Name);
        }


        // < ------------------ PRINT METHODS ------------------ >

        private void WrongInput()
        {
            Console.WriteLine("Wrong input! Please try again.. ");
        }

        private void WrongExitInput()
        {
            Console.WriteLine("Invalid Input!");
            Console.WriteLine("Do you want to Play another game? " +
                "press 'y' for yes and 'n' for no");
        }

        private void PrintPreMenu()
        {
            Console.WriteLine(string.Format(@"Welcome to the best Game! X-Mix-Drix!!!
Please choose an option:
(1) User Player
(2) Computer Player
(3) Computer Player Hard
(Q) For Exit
You Choose: "));
        }

        private void PrintErrorMatrixSize()
        {
            Console.WriteLine("Wrong input! Please enter a matrix size between 3-9: ");
        }

        private void PrintMatrixGame(Cell[,] m_GameMatrix, int i_MatrixSize)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            int col = 0;
            for (int i = 0; i < i_MatrixSize * 2 + 1; i++)
            {
                for (int j = 0; j < i_MatrixSize; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            Console.Write(' ');
                        }
                        Console.Write(' ');
                        Console.Write(' ');
                        Console.Write(j + 1);
                        Console.Write(' ');

                    }
                    else if (i % 2 == 1)
                    {
                        if (j == 0)
                        {
                            Console.Write(col + 1);
                        }
                        Console.Write("| ");
                        switch (m_GameMatrix[col, j])
                        {
                            case Cell.P1:
                                Console.Write("X ");
                                break;
                            case Cell.P2:
                                Console.Write("O ");
                                break;
                            default:
                                Console.Write("  ");
                                break;
                        }
                    }
                    else
                    {
                        Console.Write("====");
                    }

                    if (j + 1 == i_MatrixSize)
                    {
                        if (i % 2 == 1)
                        {
                            Console.Write("| ");
                        }
                        else if (i != 0 && i % 2 == 0)
                        {
                            Console.Write("===");
                        }
                        Console.WriteLine();
                    }
                }

                if (i % 2 == 1)
                {
                    col++;
                }
            }
        }

        private void PrintCurrentPoints(Player[] i_PlayersArr)
        {

            Console.WriteLine(string.Format(@"current game status: 
{0} have {1} Points
{2} have {3} Points 
Do you want to Play another game? press 'y' for yes and 'n' for no."
, i_PlayersArr[0].m_Name, i_PlayersArr[0].m_Points, i_PlayersArr[1].m_Name, i_PlayersArr[1].m_Points));

        }

        private void PrintInputCellError(int i_ErrorNum, int i_MatrixSize)
        {
            switch (i_ErrorNum)
            {
                case 1:
                    Console.WriteLine(string.Format(@"Wrong Input! Please enter a number between 1 to {0}", i_MatrixSize));
                    break;
                case 2:
                    Console.WriteLine(string.Format(@"Wrong Input, Out Of Range! Please enter a number between 1 to {0}", i_MatrixSize));
                    break;
                case 3:
                    Console.WriteLine("Wrong Input, This Cell is not empty! Please try again.. ");
                    break;
            }
        }

        private void ItsAWin(short i_PlayerNum)
        {
            int playerIdx = (i_PlayerNum == 2) ? 0 : 1;

            char v_shape = 'X';
            if (m_PlayersArr[playerIdx].m_PlayerShape == Cell.P2)
            {
                v_shape = 'O';
            }
            Console.WriteLine(string.Format(@"Player {0} Win!", v_shape));
        }

        private void ItsATie()
        {
            Console.WriteLine("The Matrix is full, This is a Tie!");
        }

        private void WantToExit()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("Bye Bye..");
        }

        private void EnterCol(string PlayerName, Cell i_shape)
        {
            char shape = i_shape == Cell.P1 ? 'X' : 'O';
            Console.WriteLine($"Player {PlayerName} the {shape} please enter a matrix col: ");
        }

        private void EnterPlayerName(int i)
        {
            Console.WriteLine("Please enter player " + i + " name: ");
        }

        private void EnterRow(string PlayerName, Cell i_shape)
        {
            char shape = i_shape == Cell.P1 ? 'X' : 'O';
            Console.WriteLine($"Player {PlayerName} the {shape} please enter a matrix row: ");
        }


        // < ------------------ UPDATE METHODS ------------------ >

        private void UpdateWin(short i_PlayerNum, Player[] i_PlayersArr)
        {
            int playerIdx = (i_PlayerNum == 2) ? 0 : 1;
            i_PlayersArr[playerIdx].m_Points++;
        }

        private void UpdateGameToTie(Player[] i_PlayersArr)
        {
            i_PlayersArr[0].m_Points++;
            i_PlayersArr[1].m_Points++;
        }

    }

}
