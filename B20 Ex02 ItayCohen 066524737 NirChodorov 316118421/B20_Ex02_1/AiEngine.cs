using System;
using System.Collections.Generic;
using System.Linq;

namespace B20_Ex02_1
{
    public class AiEngine
    {
        private static int m_ListUpdateIndex = 0;
        private List<CardOnBoard> m_PreviuosChoices;
        private int m_PreviousChoicesListDepth;
        private Random m_Random;
        private double m_UseListProbality;
        
        public List<CardOnBoard> PreviuosChoices { get => m_PreviuosChoices; }

        public class CardOnBoard
        {
            private int m_Row;
            private int m_Col;
            private Cell m_Cell;

            public CardOnBoard(int i_Row, int i_Col, Cell i_Cell)
            {
                m_Cell = i_Cell;
                m_Row = i_Row;
                m_Col = i_Col;
            }

            public Cell Cell { get => m_Cell; set => m_Cell = value; }

            public int Col { get => m_Col; set => m_Col = value; }

            public int Row { get => m_Row; set => m_Row = value; }
        }

        public AiEngine(int i_ChoicesListDepth = 5, double i_UseListProbality = 0.5)
        {
            m_PreviousChoicesListDepth = i_ChoicesListDepth;
            m_PreviuosChoices = new List<CardOnBoard>(m_PreviousChoicesListDepth);
            m_Random = new Random();
            m_UseListProbality = i_UseListProbality;
        }

        public int[] GetPick(int i_RowsLimit, int i_ColsLimit, CardOnBoard i_FirstPick = null)
        {
            int[] pickIndexes = new int[2];
            pickIndexes[0] = m_Random.Next(i_RowsLimit);
            pickIndexes[1] = m_Random.Next(i_ColsLimit);
            if (m_Random.NextDouble() > m_UseListProbality && m_PreviuosChoices.Any())
            {
                CardOnBoard matchingCard = null;
                if (i_FirstPick != null) 
                { 
                    matchingCard = tryFindPair(i_FirstPick);
                }
                else
                {
                    m_PreviuosChoices.ForEach(prevChoice => matchingCard = tryFindPair(prevChoice));
                }

                if (matchingCard != null)
                {
                    pickIndexes[0] = matchingCard.Row;
                    pickIndexes[1] = matchingCard.Col;
                }
            }

            return pickIndexes;
        }
        
        public void InsertPrevChoice(int i_Row, int i_Col, Cell i_Cell)
        {
            if(m_PreviuosChoices.Count > m_PreviousChoicesListDepth && m_PreviuosChoices[m_ListUpdateIndex % m_PreviousChoicesListDepth] != null)
            {
                UpdateAt(m_ListUpdateIndex % m_PreviousChoicesListDepth, i_Row, i_Col, i_Cell);
            }
            else
            {
                m_PreviuosChoices.Add(new CardOnBoard(i_Row, i_Col, i_Cell));
            }

            m_ListUpdateIndex++;
        }

        public void RemoveFromPrevChoices(Cell i_Cell)
        {
            m_PreviuosChoices.RemoveAll(card => card.Cell.Letter == i_Cell.Letter);
        }
        
        public void UpdateAt(int i_Index, int i_NewRow, int i_NewCol, Cell i_NewCell)
        {
            if (m_PreviuosChoices.Count == 0)
            {
                m_PreviuosChoices.Add(new CardOnBoard(i_NewRow, i_NewCol, i_NewCell));
            }
            else
            {
                m_PreviuosChoices[i_Index] = new CardOnBoard(i_NewRow, i_NewCol, i_NewCell);
            }
        }

        private CardOnBoard tryFindPair(CardOnBoard prevChoice)
        {
            return m_PreviuosChoices.FirstOrDefault(ch => ch.Cell.Letter == prevChoice.Cell.Letter && ch.Col != prevChoice.Col && ch.Row != prevChoice.Row);
        }
    }
}
