using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;

namespace B20_Ex02_1
{
    public class Logic
    {
        #region props
        // to be changed to 4!!!!!!!!!
        private const int MINIMUM_LENGTH_FOR_GRID = 1;
        private const int MAXIMUM_LENGTH_FOR_GRID = 6;
        private const int SAME_CARDS_COUNT = 2;
        private Random rnd = new Random();

        public Cell[,] GameGrid { get => m_GameGrid; set => m_GameGrid = value; }

        public int CurrentActivePlayerId { get => m_CurrentActivePlayerId; set => m_CurrentActivePlayerId = value; }

        public bool IsGameOver { get => m_IsGameOver; set => m_IsGameOver = value; }

        #endregion
        
        #region fields
        private List<Player> m_Players = null;
        private int m_CurrentActivePlayerId = 0;
        private bool m_IsGameOver = !true;
        private AiEngine m_AiEngine;
        private Cell[,] m_GameGrid;
        private List<char> m_OptionalCardsLetters;

        #endregion

        #region C'tors
        public Logic()
        {
            m_Players = new List<Player>();
            m_OptionalCardsLetters = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'V' };
        }
        #endregion
        #region PublicMethods

        public void ShuffleGrid()
        {
            int rowGeneratedIndex, colGeneratedIndex;
            for (int i = 0; i < (GetGridCols() * GetGridRows()) / SAME_CARDS_COUNT; i++)
            {
                for (int j = 0; j < SAME_CARDS_COUNT; j++)
                {
                    do
                    {
                        rowGeneratedIndex = rnd.Next(GetGridRows());
                        colGeneratedIndex = rnd.Next(GetGridCols());
                    } 
                    while (m_GameGrid[rowGeneratedIndex, colGeneratedIndex] != null);

                    m_GameGrid[rowGeneratedIndex, colGeneratedIndex] = new Cell(m_OptionalCardsLetters[i], !true);
                }
            }
        }

        public Player GetActivePlayer()
        {
            return m_Players.Find(ply => ply.Id == m_CurrentActivePlayerId);
        }

        public bool TryCreateGrid(int i_Rows, int i_Cols)
        {
            bool v_IsValid = checkLimits(i_Rows, MINIMUM_LENGTH_FOR_GRID, MAXIMUM_LENGTH_FOR_GRID) && checkLimits(i_Cols, MINIMUM_LENGTH_FOR_GRID, MAXIMUM_LENGTH_FOR_GRID) && ((i_Rows * i_Cols) % 2 == 0);
            if (v_IsValid)
            {
                GameGrid = new Cell[i_Rows, i_Cols];
                ShuffleGrid();
            }
            
            return v_IsValid;
        }

        public bool TryFlipCard(int i_Row, int i_Col)
        {
            bool v_FlipSuccess = !true;
            if (checkLimits(i_Row, 0, GetGridRows()) && checkLimits(i_Col, 0, GetGridCols()))
            {
                if(m_GameGrid[i_Row, i_Col].IsVisable == !true)
                {
                    setCellVisiballity(i_Row, i_Col, true);
                    v_FlipSuccess = true;
                }
            }

            return v_FlipSuccess;
        }

        public void AddNewPlayer(Player i_Player)
        {
            //CR :: added player to empry list
            if (m_Players == null)
            {
                m_Players = new List<Player>();
                m_Players.Add(i_Player);
            }
            else if (m_Players.Count < 2)
            {
                m_Players.Add(i_Player);
            }
        }

        public Player GetWinner()
        {
            int max = 0;
            Player winningPlayer = null;
            m_Players.ForEach(ply =>
            {
                if (ply.NumOfHits > max)
                {
                    max = ply.NumOfHits;
                    winningPlayer = ply;
                }
            });

            return winningPlayer;
        }

        public Player GetLoser()
        {
            int min = GetGridCols() * GetGridRows() / 2;
            Player losingPlayer = null;
            m_Players.ForEach(ply =>
            {
                if (ply.NumOfHits < min)
                {
                    min = ply.NumOfHits;
                    losingPlayer = ply;
                }
            });
            return losingPlayer;
        }

        public void MakeComputerAiMove(int i_FirstCardRow, int i_FirstCardCol, ref int i_SecondCardRow, ref int i_SecondCardCol)
        {
            //get the most popular distance
            int mostPopularDistance = m_AiEngine.Distances.ToList().IndexOf(m_AiEngine.Distances.Max());

            // try to find educated match for five times, if dont find any match, will try in the regular way
            int counterForEductedGuessTry = 5;
            bool V_EducatedGuess = !true;
            do
            {
                i_SecondCardRow = rnd.Next(0, GetGridRows());
                i_SecondCardCol = rnd.Next(0, GetGridCols());
                V_EducatedGuess = TryFlipCard(i_SecondCardRow, i_SecondCardCol);
                counterForEductedGuessTry--;
            } 
            while (counterForEductedGuessTry > 0 && !V_EducatedGuess);

            //count <=0 : try to find educated gess for five times and hasnt succed. try now in the naive way
            if (counterForEductedGuessTry <= 0 && !V_EducatedGuess)
            {
                MakeComputerMove(ref i_SecondCardRow, ref i_SecondCardCol);
            }
        }

        public bool TryQuitGame(string i_UserInput)
        {
            string userInputAfterTrim = i_UserInput.Trim(' ').ToUpper();
            m_IsGameOver = userInputAfterTrim.Contains('Q');
            return m_IsGameOver;
        }

        public void MakeComputerMove(ref int i_Row, ref int i_Col)
        {
            do
            {
                i_Row = rnd.Next(0, GetGridRows());
                i_Col = rnd.Next(0, GetGridCols());
            } 
            while (!TryFlipCard(i_Row, i_Col));
        }

        public int GetGridCols()
        {
            return m_GameGrid.GetLength(1);
        }

        public int GetGridRows()
        {
            return m_GameGrid.GetLength(0);
        }

        public bool IsGameOn()
        {
            bool v_FoundInvisible = !true;
            if (!IsGameOver)
            {
                for (int i = 0; i < GetGridRows() && !v_FoundInvisible; i++)
                {
                    for (int j = 0; j < GetGridCols() && !v_FoundInvisible; j++)
                    {
                        if (!m_GameGrid[i, j].IsVisable)
                        {
                            v_FoundInvisible = true;
                        }
                    }
                }
            }

            return v_FoundInvisible;
            /*
            //int sumOfHits = 0;
            //int numberOfTotalCells = m_GameGrid.GetLength(0) * m_GameGrid.GetLength(1);
            //foreach (Player ply in m_Players)
            //{
            //    sumOfHits += ply.NumOfHits;
            //}

            //return !((sumOfHits * 2) == numberOfTotalCells);
            */
        }

        // get player match cells. check the cells equaility. if true - > update the cell with the player id
        public bool TryUpdateForEquality(int i_RowFirstCell, int i_ColFirstCell, int i_RowSecondCell, int i_ColSecondCell)
        {
            // check if the cordinate are not eaqual
            bool v_IsValid = !((i_RowFirstCell == i_RowSecondCell) && (i_ColSecondCell == i_ColFirstCell));

            // check valid cordinate
            v_IsValid = v_IsValid && (checkLimits(i_RowFirstCell, 0, GetGridRows()) && checkLimits(i_ColFirstCell, 0, GetGridCols() ))
                && (checkLimits(i_RowSecondCell, 0, GetGridRows()) && checkLimits(i_ColSecondCell, 0, GetGridCols()));

            if (v_IsValid)
            {
                if (m_GameGrid[i_RowFirstCell, i_ColFirstCell].Letter == m_GameGrid[i_RowSecondCell, i_ColSecondCell].Letter)
                {
                    Player currentPlayer = GetActivePlayer();

                    // update cells match (which player discover this couple)
                    updatePlayerCellFinder(currentPlayer, i_RowFirstCell, i_ColFirstCell);
                    updatePlayerCellFinder(currentPlayer, i_RowSecondCell, i_ColSecondCell);

                    // update cells visabillity
                    setCellVisiballity(i_RowFirstCell, i_ColFirstCell, true);
                    setCellVisiballity(i_RowSecondCell, i_ColSecondCell, true);
                    
                    // update player hits
                    addHit(currentPlayer);

                    //CR :: ignoring AI - it has some bugs rn
                    
                    //update the distance for AI use
                    //int currentDistanceForTwoCells = m_AiEngine.CalculteDistanceForTwoCells(i_RowFirstCell, i_ColFirstCell, i_RowSecondCell, i_ColSecondCell);
                    //m_AiEngine.UpdateDistance(currentDistanceForTwoCells);
                }
                else
                {
                    // update cells visabillity
                    setCellVisiballity(i_RowFirstCell, i_ColFirstCell, !true);
                    setCellVisiballity(i_RowSecondCell, i_ColSecondCell, !true);

                    //CR :: maybe not the right param name but we still didnt update for equalit - therefore we shuold return false
                    v_IsValid = !true;

                    // give the turn to another player
                    updateActivePlayer();
                }
            }

            return v_IsValid;
        }

        #endregion

        #region PrivateMethods

        private bool checkLimits(int i_Number, int i_Low, int i_High)
        {
            return (i_Number >= i_Low) && (i_Number <= i_High);
        }

        private void updateActivePlayer()
        {
            int activePlayerIndex = m_Players.FindIndex(ply => ply.Id == CurrentActivePlayerId);
            CurrentActivePlayerId = (activePlayerIndex + 1 >= m_Players.Count()) ? m_Players[0].Id : m_Players[activePlayerIndex + 1].Id;
        }

        private void setCellVisiballity(int i_Row, int i_Col, bool i_isVisible)
        {
            if(checkLimits(i_Row, 0, GetGridRows() - 1) && checkLimits(i_Row, 0, GetGridCols() - 1))
            {
                m_GameGrid[i_Row, i_Col].IsVisable = i_isVisible;
            }
        }

        // update the cells property in which player find its match
        private void updatePlayerCellFinder(Player i_Ply, int i_Row, int i_Col)
        {
            m_GameGrid[i_Row, i_Col].PlayerId = i_Ply.Id;
        }

        private void addHit(Player i_Ply)
        {
            m_Players.FirstOrDefault(ply => ply.Id == i_Ply.Id).NumOfHits++;
        }

        #endregion
    }
}
