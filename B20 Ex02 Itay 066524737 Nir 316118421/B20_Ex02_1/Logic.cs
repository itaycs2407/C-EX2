using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_1
{
    public class Logic
    {
        private List<Player> m_Players = null;

        public Cell[,] m_Grid { get => m_Grid; set => m_Grid = value; }

        public Logic(Cell [,] i_Grid , List<Player> i_Players)
        {
            m_Grid = i_Grid;
            m_Players = i_Players;
        }
      
      private void Initilize()
        {
        }

        private bool checkForEquaility(Cell i_First, Cell i_Second)
        {
            return i_First.Item == i_Second.Item;
        }

        internal char GetColsLength()
        {
            throw new NotImplementedException();
        }

        internal bool IsValidGrid(int i_NumInput)
        {
            throw new NotImplementedException();
        }

        internal bool IsValidPlayersCount(int playersCountFromInput)
        {
            throw new NotImplementedException();
        }

        
        internal void AddNewPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        internal int GetIdPlayerTurn()
        {
            throw new NotImplementedException();
        }

        internal Player GetPlayer(int playerTurnId)
        {
            throw new NotImplementedException();
        }

        internal void FlipTurn(int playerTurnId)
        {
            throw new NotImplementedException();
        }

        internal int GetSameCardsCount()
        {
            throw new NotImplementedException();
        }

        internal char GetRowsLength()
        {
            throw new NotImplementedException();
        }

        internal bool CheckIsHit(List<int> playersCardsPicks)
        {
            throw new NotImplementedException();
        }

        internal object GetCardChar(int i, int j)
        {
            throw new NotImplementedException();
        }

        internal object GetCardValue(int i, int j)
        {
            throw new NotImplementedException();
        }
        public bool IsGameOn()
        {
            int sumOfHits = 0;
            int numberOfTotalCells = m_Grid.GetLength(0) * m_Grid.GetLength(1);
            foreach(Player ply in m_Players){
                sumOfHits += ply.NumOfHits;
            }

            return !((sumOfHits * 2) == numberOfTotalCells);
        }
        public void UpdateCell(int i_Row, int i_Col)
        {
            m_Grid[i_Row, i_Col].IsVisable = !m_Grid[i_Row, i_Col].IsVisable;
        }
        private bool checkForEquaility(int i_RowFirstCell, int i_ColFirstCell, int i_RowSecondCell, int i_Coli_RowSecondCell )
        {
            return m_Grid[i_RowFirstCell, i_ColFirstCell].Item == m_Grid[i_RowSecondCell, i_Coli_RowSecondCell].Item;
        }
        public void AddHit(Player i_Ply)
        {
            m_Players.Find(ply => ply == i_Ply).NumOfHits++;
        }
    }
}
