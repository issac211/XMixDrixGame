using System;

namespace Ex02
{
    public struct XMixDrixLogic
    {
        private const char k_PlayerOneShape = 'X';
        private const char k_PlayerTwoShape = 'O';
        private const int k_MinRows = 4;
        private const int k_MaxRows = 8;
        private const int k_MinColumns = 4;
        private const int k_MaxColumns = 8;
        private Board m_Board;
        private Player[] m_Players;
        private int m_CurrentPlayerIndex;

        public string GameBoard
        {
            get { return m_Board.ToString(); }
        }

        public char CurrentPlayerShape
        {
            get { return m_Players[m_CurrentPlayerIndex].PlayerShape; }
        }

        public bool CurrentPlayerIsHuman
        {
            get { return m_Players[m_CurrentPlayerIndex].IsHuman; }
        }

        public static int MinRows
        {
            get { return k_MinRows; }
        }

        public static int MaxRows
        {
            get { return k_MaxRows; }
        }

        public static int MinColumns
        {
            get { return k_MinColumns; }
        }

        public static int MaxColumns
        {
            get { return k_MaxColumns; }
        }

        public bool IsWinner
        {
            get
            {
                return m_Board.WinnerPlayer != null;
            }
        }

        public bool IsDraw
        {
            get
            {
                return m_Board.IsBoardFull() && !IsWinner;
            }
        }

        private Player getCurrentPlayer()
        {
            return m_Players[m_CurrentPlayerIndex];
        }

        public XMixDrixLogic(int i_NumOfRowInBoard, int i_NumOfColInBoard, bool i_IsHuman)
        {
            m_CurrentPlayerIndex = 0;
            m_Board = new Board(i_NumOfRowInBoard, i_NumOfColInBoard);
            m_Players = new Player[2] { new Player(k_PlayerOneShape, true), new Player(k_PlayerTwoShape, i_IsHuman) };
        }

        public bool MakeMove(int i_ColChoice = 0)
        {
            bool colChoiceIsOK = false;
            Player currentPlayer = getCurrentPlayer();
            
            if(!IsWinner || !IsDraw)
            {
                if (currentPlayer.IsHuman)
                {
                    colChoiceIsOK = m_Board.TryToPlaceIntoCell(i_ColChoice, currentPlayer);
                }
                else
                {
                    Random computerChoice = currentPlayer.ComputerChoice;
                    colChoiceIsOK = m_Board.TryToPlaceIntoCell(computerChoice.Next(1, m_Board.Width + 1), currentPlayer);
                }      
            }

            if (colChoiceIsOK && !IsWinner)
            {
                changeCurrentPlayer();
            }

            return colChoiceIsOK;
        }

        public int[] GetScores()
        {
            int[] playerScore = new int[] { m_Players[0].Score, m_Players[1].Score };

            return playerScore;
        }

        public void QuitGame()
        {
            Player currentPlayer;

            changeCurrentPlayer();
            currentPlayer = getCurrentPlayer();
            currentPlayer.Score++;
        }

        public void WinGame()
        {
            Player currentPlayer = getCurrentPlayer();

            currentPlayer.Score++;
        }

        public void ResetGame()
        {
            m_CurrentPlayerIndex = 0;
            m_Board = new Board(m_Board.Length, m_Board.Width);
        }

        private void changeCurrentPlayer()
        {
            m_CurrentPlayerIndex = m_CurrentPlayerIndex == 0 ? 1 : 0;
        }
    }
}
