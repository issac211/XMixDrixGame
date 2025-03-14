using System;
using Ex02.ConsoleUtils;

namespace Ex02
{
    public class XMixDrixUI
    {
        private const string k_ExitString = "Q";
        private const string k_AnotherRoundString = "Y";
        private const string k_HumanInputString = "H";
        private const string k_ComputerInputString = "C";
        private const string k_GoodbyeString = "Bye Bye";
        private readonly string r_NewLine = Environment.NewLine;
        private XMixDrixLogic m_XMixDrixLogic;

        public XMixDrixUI()
        {
            int[] boardSize = getBoardSize();
            bool isHuman = chooseHumanOrComputer();

            m_XMixDrixLogic = new XMixDrixLogic(boardSize[0], boardSize[1], isHuman);
        }

        public void PlayGame()
        {
            do
            {
                startGame();
                showScore();
            } while (checkIfAnotherRound());

            Console.WriteLine(k_GoodbyeString);
        }

        private bool checkIfAnotherRound()
        {
            string userChoice;
            const string continueOrFinishMassage =
@"Press {0} if you want to continue to another game, press any key to finish the game.
After your choice press enter.. ";

            Console.WriteLine(continueOrFinishMassage, k_AnotherRoundString);
            userChoice = Console.ReadLine().ToLower();

            return userChoice == k_AnotherRoundString.ToLower();
        }

        private void startGame()
        {
            bool isPlayerChoiceExit = false;
            bool playerChoiceIsOK = true;
            bool playerChoiceIsInt = true;
            bool isThereAWinner;
            bool isThereADraw;
            char currentPlayerShape;

            do
            {
                Screen.Clear();
                drawBoard();
                currentPlayerShape = m_XMixDrixLogic.CurrentPlayerShape;

                if (!playerChoiceIsOK)
                {
                    Console.WriteLine(
                        "Column is full or the number is out of range!!, Try again" + r_NewLine);
                    playerChoiceIsOK = true;
                }

                if (!playerChoiceIsInt)
                {
                    Console.WriteLine(
                            "Not good input, needs to be an integer" + r_NewLine);
                }

                if (m_XMixDrixLogic.CurrentPlayerIsHuman)
                {
                    string playerchoice;

                    Console.WriteLine("Where to put {0}? for exit press {1}", currentPlayerShape, k_ExitString);
                    playerchoice = Console.ReadLine();
                    playerChoiceIsInt = int.TryParse(playerchoice, out int colChoice);
                    if (playerchoice.ToLower() == k_ExitString.ToLower())
                    {
                        m_XMixDrixLogic.QuitGame();
                        isPlayerChoiceExit = true;
                    }
                    else if (playerChoiceIsInt)
                    {
                        playerChoiceIsOK = m_XMixDrixLogic.MakeMove(colChoice);
                    }
                }
                else
                {
                    playerChoiceIsOK = m_XMixDrixLogic.MakeMove();
                }

                isThereAWinner = m_XMixDrixLogic.IsWinner;
                isThereADraw = m_XMixDrixLogic.IsDraw;
            } while (!isPlayerChoiceExit && !isThereADraw && !isThereAWinner);

            if (isThereAWinner)
            {
                Screen.Clear();
                drawBoard();
                m_XMixDrixLogic.WinGame();
                Console.WriteLine("The winner is: '{0}'!!!{1}", currentPlayerShape, r_NewLine);
            }

            if (isThereADraw)
            {
                Screen.Clear();
                drawBoard();
                Console.WriteLine("There is a draw" + r_NewLine);
            }

            m_XMixDrixLogic.ResetGame();
        }

        private int[] getBoardSize()
        {
            int NumOfRowInBoard;
            int NumOfColInBoard;
            int[] boardSize;
            bool rowsNumIsInt;
            bool columnsNumIsInt;
            int minRows = XMixDrixLogic.MinRows;
            int maxRows = XMixDrixLogic.MaxRows;
            int minColumns = XMixDrixLogic.MinColumns;
            int maxColumns = XMixDrixLogic.MaxColumns;

            do
            {
                Console.WriteLine("Enter a number between {0}-{1} for rows in board:", minRows, maxRows);
                rowsNumIsInt = int.TryParse(Console.ReadLine(), out NumOfRowInBoard);
                Console.WriteLine("Enter a number between {0}-{1} for columns in board:", minColumns, maxColumns);
                columnsNumIsInt = int.TryParse(Console.ReadLine(), out NumOfColInBoard);
                Screen.Clear();
                if (!rowsNumIsInt || !columnsNumIsInt)
                {
                    Console.WriteLine("Not good input, rows and columns must be an integer value" + r_NewLine);
                }
                else if (!(NumOfRowInBoard <= maxRows && NumOfRowInBoard >= minRows
                            && NumOfColInBoard <= maxColumns && NumOfColInBoard >= minColumns))
                {
                    Console.WriteLine("Not good input, rows and columns Not in range" + r_NewLine);
                }
            } while (!(NumOfRowInBoard <= maxRows && NumOfRowInBoard >= minRows
                        && NumOfColInBoard <= maxColumns && NumOfColInBoard >= minColumns) || !rowsNumIsInt || !columnsNumIsInt);

            boardSize = new int[2] { NumOfRowInBoard, NumOfColInBoard };

            return boardSize;
        }


        private bool chooseHumanOrComputer()
        {
            bool humanOrComputerInputIsOk;
            string humanOrComputerInput;
            const string humanOrComputerMassage =
@"Enter:
    '{0}' - if you want to play with conputer
    '{1}' - if you want to play with another human";

            do
            {
                Console.WriteLine(humanOrComputerMassage, k_ComputerInputString, k_HumanInputString);
                humanOrComputerInput = Console.ReadLine();
                humanOrComputerInputIsOk = humanOrComputerInput.ToLower() == k_HumanInputString.ToLower()
                                            || humanOrComputerInput.ToLower() == k_ComputerInputString.ToLower();
                Screen.Clear();
                if (!humanOrComputerInputIsOk)
                {
                    Console.WriteLine("'{0}' Is not a good input", humanOrComputerInput);
                }
            } while (!humanOrComputerInputIsOk);

            bool isHuman = humanOrComputerInput.ToLower() == k_HumanInputString.ToLower();

            return isHuman;
        }

        private void showScore()
        {
            int[] playerScore = m_XMixDrixLogic.GetScores();

            Console.WriteLine("The score of player1 is: {0}", playerScore[0]);
            Console.WriteLine("The score of player2 is: {0}", playerScore[1]);
            Console.WriteLine(r_NewLine);
        }

        private void drawBoard()
        {
            Console.WriteLine(m_XMixDrixLogic.GameBoard);
        }
    }
}
