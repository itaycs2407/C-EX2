using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace B20_Ex02_1
{
    public class Logic
    {
        private List<Player> m_Players = null;
        private int m_CurrentActivePlayerId = 0;

        private Cell[,] m_GameGrid;
        public Cell[,] GameGrid { get => m_GameGrid; set => m_GameGrid = value; }
        public int CurrentActivePlayerId { get => m_CurrentActivePlayerId; set => m_CurrentActivePlayerId = value; }

        public Logic()
        {
        }


        // check how to define bool inside funcyion
        public bool TryCreateGrid(int i_Rows, int i_Cols)

        {
            bool isValid = checkLimits(i_Rows, 4, 6) && checkLimits(i_Cols, 4, 6) && ((i_Rows * i_Cols) % 2 == 0);
            if (isValid)
            {
                GameGrid = new Cell[i_Rows, i_Cols];
            }

            return isValid;
        }

        public bool TryFlipCard(int i_Row, int i_Col)
        {
            bool flipSuccess = !true;
            if (checkLimits(i_Row, 4, 6) && checkLimits(i_Col, 4, 6))
            {
                // row and col number are valid in matrix, need to check if the cell is hidden. true -> flip it
                if (!m_GameGrid[i_Row, i_Col].IsVisable)
                {
                    UpdateCellVisability(i_Row, i_Col);
                    flipSuccess = true;
                }
            }

            return flipSuccess;
        }
        private bool checkLimits(int i_Number, int i_Low, int i_High)
        {
            return (i_Number >= i_Low) && (i_Number <= i_High);
        }

        internal void AddNewPlayer(Player player) // WILL UPDATE
        {
            throw new NotImplementedException();
        }




        // todo logic return the active player
        public Player GetActivePlayer()
        {
            return m_Players.Find(ply => ply.Id == m_CurrentActivePlayerId);
        }


        private void updateActivePlayer()
        {
            int activePlayerIndex = m_Players.FindIndex(ply => ply.Id == CurrentActivePlayerId);
            CurrentActivePlayerId = (activePlayerIndex + 1 > m_Players.Count()) ? m_Players[0].Id : m_Players[activePlayerIndex + 1].Id;
            /* if pass chodorov CR, can be deleted.....
            if (activePlayerIndex + 1 > m_Players.Count())
            {
                CurrentActivePlayerId = m_Players[0].Id;
            }
            else
            {
                CurrentActivePlayerId = m_Players[activePlayerIndex + 1].Id;
            }
            */

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

        public void MakeComputerAiMove()
        {

        }

        public void MakeComputerMove(ref int i_Row, ref int i_Col)
        {
            Random rnd = new Random();
            do
            {
                i_Row = rnd.Next(0, GetGridRows());
                i_Col = rnd.Next(0, GetGridCols());
            } while (!TryFlipCard(i_Col, i_Col));
        }
     
        public int GetGridCols()

        {
            return m_GameGrid.GetLength(0);
        }

        public int GetGridRows()
        {
            return m_GameGrid.GetLength(1);
        }

        public bool IsGameOn()
        {
            int sumOfHits = 0;
            int numberOfTotalCells = m_GameGrid.GetLength(0) * m_GameGrid.GetLength(1);
            foreach(Player ply in m_Players){
                sumOfHits += ply.NumOfHits;
            }

            return !((sumOfHits * 2) == numberOfTotalCells);
        }
        private void UpdateCellVisability(int i_Row, int i_Col)
        {
            m_GameGrid[i_Row, i_Col].IsVisable = !m_GameGrid[i_Row, i_Col].IsVisable;
        }

        // update the cells property in which player find its match
        private void updatePlayerCellFinder(Player i_Ply, int i_Row, int i_Col)
        {

            m_GameGrid[i_Row, i_Col].PlayerId = i_Ply.Id;
        }


        // get player match cells. check the cells equaility. if true - > update the cell with the player id
        public bool TryUpdateForEquality( int i_RowFirstCell, int i_ColFirstCell, int i_RowSecondCell, int i_ColSecondCell )
        {
            // check if the cordinate are not eaqual
            bool isValid = !(i_RowFirstCell == i_RowSecondCell) && (i_ColSecondCell == i_RowSecondCell);
            // check valid cordinate
            isValid = isValid && (checkLimits(i_RowFirstCell, 4, 6) && checkLimits(i_ColFirstCell, 4, 6)) && (checkLimits(i_RowSecondCell, 4, 6) && checkLimits(i_ColSecondCell, 4, 6));
            if (isValid)
            {
                if ((m_GameGrid[i_RowFirstCell, i_ColFirstCell].Letter == m_GameGrid[i_RowSecondCell, i_ColSecondCell].Letter))
                {
                    Player currentPlayer = GetActivePlayer();

                   
                    // update cells match (which player discover this couple)
                    updatePlayerCellFinder(currentPlayer, i_RowFirstCell, i_ColFirstCell);
                    updatePlayerCellFinder(currentPlayer, i_RowSecondCell, i_ColSecondCell);

                    // update player hits
                    addHit(currentPlayer);
                }
                else
                {
                    // update cells visabillity
                    UpdateCellVisability(i_RowFirstCell, i_ColFirstCell);
                    UpdateCellVisability(i_RowSecondCell, i_ColSecondCell);

                    // give the turn to another player
                    updateActivePlayer();
                }
            }
            return isValid;
        }


        private void addHit(Player i_Ply)
        {
            m_Players.Find(ply => ply == i_Ply).NumOfHits++;
        }
    }
}
