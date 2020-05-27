using System;
using System.Collections.Generic;
using System.Linq;

namespace B20_Ex02_1
{
    public partial class AiEngine
    {
        private int[] m_Distances;
        private int m_size;
        private List<CardOnBoard> m_PreviuosChoices;
        private int m_PreviousChoicesListDepth;
        private Random m_Random;
        private static int m_ListUpdateIndex = 0;
        private double m_UseListProbality;

        public int[] Distances { get => m_Distances; }

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

        public int[] GetFirstPick(int i_RowsLimit, int i_ColsLimit)
        {
            int[] pickIndexes = new int[2];
            pickIndexes[0] = m_Random.Next(i_RowsLimit);
            pickIndexes[1] = m_Random.Next(i_ColsLimit);
            if (m_Random.NextDouble() > m_UseListProbality && m_PreviuosChoices.Any())
            {
                m_PreviuosChoices.ForEach(prevChoice =>
                {
                    var matchingCard = tryFindPair(prevChoice);
                    if (matchingCard != null)
                    {
                        pickIndexes[0] = prevChoice.Row;
                        pickIndexes[1] = prevChoice.Col;
                    }
                });
            }

            return pickIndexes;
        }

        public int[] GetSecondPick(int i_RowsLimit, int i_ColsLimit, CardOnBoard i_FirstPick)
        {
            int[] pickIndexes = new int[2];
            pickIndexes[0] = m_Random.Next(i_RowsLimit);
            pickIndexes[1] = m_Random.Next(i_ColsLimit);
            if (m_Random.NextDouble() > m_UseListProbality && m_PreviuosChoices.Any())
            {
                var matchingCard = tryFindPair(i_FirstPick);
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

        public void RemoveFromPrevChoices(int i_Row, int i_Col, Cell i_Cell)
        {
            m_PreviuosChoices.RemoveAll(card => card.Col == i_Row && card.Col == i_Col && card.Cell.Letter == i_Cell.Letter);
        }

        public void RemoveFromPrevChoices(int i_Index)
        {
            m_PreviuosChoices.RemoveAt(i_Index);
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

        //public AiEngine()
        //{
        //    // do equlide distance between two most farest cells ->  sqrt((x2-x1)^2 + (y2-y1)^2)
        //    m_size = (int)Math.Sqrt(Math.Pow(5, 2) + Math.Pow(5, 2)) + 1;
        //    m_Distances = new int[m_size];
        //}

        public void UpdateDistance(int i_Distance)
        {
            if ((i_Distance > 0) && (i_Distance < m_size))
            {
                Distances[i_Distance]++;
            }
        }

        public int CalculteDistanceForTwoCells(int i_RowFirstCell, int i_ColFirstCell, int i_RowSecondCell, int i_ColSecondCell)
        {
            return (int)Math.Sqrt(Math.Pow(i_RowFirstCell - i_RowSecondCell, 2) + Math.Pow(i_ColFirstCell - i_ColSecondCell, 2));
        }

    }
}
