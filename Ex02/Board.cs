using System.Text;

namespace Ex02
{
    public struct Board
    {
        private const char k_LineSeparator = '=';
        private const char k_ColumnSeparator = '|';
        private const char k_Space = ' ';
        private readonly int r_Length;
        private readonly int r_Width;
        private readonly int r_WinnerNumber;
        private readonly bool[] r_IsFullColumns;
        private readonly Cell[,] r_CellsMatrix;
        private Player m_WinnerPlayer;

        struct Cell
        {
            private const string k_Space = " ";
            private Player m_CellOccupier;

            public Player CellOccupier
            {
                get { return m_CellOccupier; }
                set { m_CellOccupier = value; }
            }

            public bool IsNull()
            {
                return CellOccupier == null;
            }

            public override string ToString()
            {
                string cellString = CellOccupier == null ? k_Space : CellOccupier.ToString();

                return cellString;
            }
        }

        public Board(int i_Length, int i_Width, int i_WinnerNumber = 4)
        {
            r_Length = i_Length;
            r_Width = i_Width;
            r_CellsMatrix = new Cell[i_Length, i_Width];
            r_IsFullColumns = new bool[i_Width];
            r_WinnerNumber = i_WinnerNumber;
            m_WinnerPlayer = null;
        }

        public int Length
        {
            get{ return r_Length; }
        }

        public int Width
        {
            get { return r_Width; }
        }

        public Player WinnerPlayer
        {
            get { return m_WinnerPlayer; }
        }

        public bool TryToPlaceIntoCell(int i_ColumnNumStartFromOne, Player i_Player)
        {
            int columnNum = i_ColumnNumStartFromOne - 1;
            bool isInRange = columnNum >= 0 && columnNum < Width;
            bool columnIsFull = IsColumnFull(columnNum);
            bool isSuccessful = false;

            if (isInRange && !columnIsFull)
            {
                for (int rowNum = 0; rowNum < Length; rowNum++)
                {
                    Player cellOccupier = r_CellsMatrix[rowNum, columnNum].CellOccupier;
                    
                    if (cellOccupier == null)
                    {
                        if (rowNum == Length - 1)
                        {
                            r_IsFullColumns[columnNum] = true;
                        }

                        r_CellsMatrix[rowNum, columnNum].CellOccupier = i_Player;
                        isSuccessful = true;
                        checkIfWinnerMove(rowNum, columnNum, i_Player);
                        break;
                    }
                }
            }

            return isSuccessful;
        }

        public bool IsBoardFull()
        {
            int countFullColumns = 0;

            foreach (bool IsFullColumn in r_IsFullColumns)
            {
                if(IsFullColumn)
                {
                    countFullColumns++;
                }
            }

            return countFullColumns == Width;
        }

        public bool IsColumnFull(int i_ColumnNum)
        {
            bool columnIsFull = false;

            if (i_ColumnNum >= 0 && i_ColumnNum < Width)
            {
                columnIsFull = r_IsFullColumns[i_ColumnNum];
            }

            return columnIsFull;
        }

        private void checkIfWinnerMove(int i_RowNum, int i_ColumnNum, Player i_Player)
        {
            checkWinnerInRow(i_RowNum, i_ColumnNum, i_Player);
            checkWinnerInColumn(i_RowNum, i_ColumnNum, i_Player);
            checkWinnerInDiagonal(i_RowNum, i_ColumnNum, i_Player);
            checkWinnerInAntiDiagonal(i_RowNum, i_ColumnNum, i_Player);
        }

        private void checkWinnerInRow(int i_RowNum, int i_StartingColumnNum, Player i_Player)
        {
            int countEqualPlayers = 1;
            int columnNum = i_StartingColumnNum - 1;

            while (columnNum >= 0)
            {
                if(isNotSequence(i_RowNum, columnNum, i_Player))
                {
                    break;
                }

                countEqualPlayers++;
                columnNum--;
            }

            columnNum = i_StartingColumnNum + 1;
            while (columnNum < Width)
            {
                if(isNotSequence(i_RowNum, columnNum, i_Player))
                {
                    break;
                }

                countEqualPlayers++;
                columnNum++;
            }

            checkEqualPlayersSequence(countEqualPlayers, i_Player);
        }

        private void checkWinnerInColumn(int i_StartingRowNum, int i_ColumnNum, Player i_Player)
        {
            int countEqualPlayers = 1;
            int rowNum = i_StartingRowNum - 1;

            while (rowNum >= 0)
            {
                if(isNotSequence(rowNum, i_ColumnNum, i_Player))
                {
                    break;
                }

                countEqualPlayers++;
                rowNum--;
            }

            rowNum = i_StartingRowNum + 1;
            while (rowNum < Length)
            {
                if(isNotSequence(rowNum, i_ColumnNum, i_Player))
                {
                    break;
                }

                countEqualPlayers++;
                rowNum++;
            }

            checkEqualPlayersSequence(countEqualPlayers, i_Player);
        }

        private void checkWinnerInDiagonal(int i_StartingRowNum, int i_StartingColumnNum, Player i_Player)
        {
            int countEqualPlayers = 1;
            int rowNum = i_StartingRowNum + 1;
            int colNum = i_StartingColumnNum + 1;

            while (rowNum < Length && colNum < Width)
            {
                if(isNotSequence(rowNum, colNum, i_Player))
                {
                    break;
                }

                countEqualPlayers++;
                rowNum++;
                colNum++;
            }
            
            rowNum = i_StartingRowNum - 1;
            colNum = i_StartingColumnNum - 1;
            while (rowNum >= 0 && colNum >= 0)
            {
                if(isNotSequence(rowNum, colNum, i_Player))
                {
                    break;
                }

                countEqualPlayers++;
                rowNum--;
                colNum--;
            }

            checkEqualPlayersSequence(countEqualPlayers, i_Player);
        }

        private void checkWinnerInAntiDiagonal(int i_StartingRowNum, int i_StartingColumnNum, Player i_Player)
        {
            int countEqualPlayers = 1;
            int rowNum = i_StartingRowNum - 1;
            int colNum = i_StartingColumnNum + 1;

            while (rowNum >= 0 && colNum < Width)
            {
                if (isNotSequence(rowNum, colNum, i_Player))
                {
                    break;
                }

                countEqualPlayers++;
                rowNum--;
                colNum++;
            }

            rowNum = i_StartingRowNum + 1;
            colNum = i_StartingColumnNum - 1;
            while (rowNum < Length && colNum >= 0)
            {
                if(isNotSequence(rowNum, colNum, i_Player))
                {
                    break;
                }

                countEqualPlayers++;
                rowNum++;
                colNum--;
            }

            checkEqualPlayersSequence(countEqualPlayers, i_Player);
        }

        private bool isNotSequence(int i_RowNum, int i_ColumnNum, Player i_Player)
        {
            return r_CellsMatrix[i_RowNum, i_ColumnNum].CellOccupier != i_Player
                    || r_CellsMatrix[i_RowNum, i_ColumnNum].IsNull();
        }

        private void checkEqualPlayersSequence(int countEqualPlayersSequence, Player i_Player)
        {
            if(countEqualPlayersSequence >= r_WinnerNumber)
            {
                m_WinnerPlayer = i_Player;
            }
        }

        private string getRowString(int i_RowNum)
        {
            StringBuilder rowBuilder = new StringBuilder();

            for (int columnNum = 0; columnNum < Width; columnNum++)
            {
                Cell cell = r_CellsMatrix[i_RowNum, columnNum];

                rowBuilder.Append(k_ColumnSeparator);
                rowBuilder.Append(k_Space);
                rowBuilder.Append(cell);
                rowBuilder.Append(k_Space);
            }

            rowBuilder.Append(k_ColumnSeparator);

            return rowBuilder.ToString();
        }

        public override string ToString()
        {
            StringBuilder boardBuilder = new StringBuilder();

            boardBuilder.Append(k_Space, 2);
            for (int columnNum = 1; columnNum <= Width; columnNum++)
            {
                boardBuilder.Append(columnNum);
                boardBuilder.Append(k_Space, 3);
            }

            boardBuilder.Append(k_Space, 2);
            boardBuilder.AppendLine();
            for (int rowNum = Length - 1; rowNum >= 0; rowNum--)
            {
                string row = getRowString(rowNum);

                boardBuilder.AppendLine(row);
                boardBuilder.Append(k_LineSeparator, (r_Width * 4) + 1);
                boardBuilder.AppendLine();
            }

            return boardBuilder.ToString();
        }
    }
}
