using System;

namespace B20_Ex02_1
{
    public class AiEngine
    {
        private int[] m_Distances;
        private int m_size;
        public int[] Distances { get => m_Distances; }
        public AiEngine()
        {
            // do equlide distance between two most farest cells ->  sqrt((x2-x1)^2 + (y2-y1)^2)
            m_size = (int)Math.Sqrt(Math.Pow(5, 2) + Math.Pow(5, 2)) + 1 ;
            m_Distances = new int[m_size];
        }
        public void UpdateDistance(int i_Distance)
        {
            if ((i_Distance > 0) && (i_Distance < m_size))
            {
                Distances[i_Distance]++;
            }
        }
        public int CalculteDistanceForTwoCells(int i_RowFirstCell, int i_ColFirstCell, int i_RowSecondCell, int i_ColSecondCell)
        {
            return (int)(Math.Sqrt(Math.Pow(i_RowFirstCell - i_RowSecondCell, 2) + Math.Pow(i_ColFirstCell - i_ColSecondCell, 2)));
        }
    }
}
