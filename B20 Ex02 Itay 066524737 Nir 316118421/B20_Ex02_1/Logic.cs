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

        public Logic()
        {
           
        }
     

        internal bool TryCreateGrid(int i_NumInput)
        {
            throw new NotImplementedException();
        }

        
        internal void AddNewPlayer(Player player) // WILL UPDATE
        {
            throw new NotImplementedException();
        }

        internal int GetPlayerIdTurn()
        {
            throw new NotImplementedException();
        }

        internal Player GetPlayerType(int playerTurnId)
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

        internal bool CheckIsHit(List<int> playersCardsPicks)
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
        public void UpdateCellVisability(int i_Row, int i_Col)
        {
            m_Grid[i_Row, i_Col].IsVisable = !m_Grid[i_Row, i_Col].IsVisable;
        }
        private bool checkForEquaility(int i_RowFirstCell, int i_ColFirstCell, int i_RowSecondCell, int i_Coli_RowSecondCell )
        {
            return m_Grid[i_RowFirstCell, i_ColFirstCell].Letter == m_Grid[i_RowSecondCell, i_Coli_RowSecondCell].Letter;
        }
        public void AddHit(Player i_Ply)
        {
            m_Players.Find(ply => ply == i_Ply).NumOfHits++;
        }
    }
}
