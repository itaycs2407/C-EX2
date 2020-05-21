using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_1
{
    public class Cell
    {
        private char m_Letter;
        private bool m_IsVisable;
        private int m_PlayerId;


        public Cell(char i_CurrentLetter, bool i_IsVisible)
        {
            m_Letter = i_CurrentLetter;
            m_IsVisable = i_IsVisible;
        }

        public char Item { get => m_Letter; set => m_Letter = value; }

        public bool IsVisable { get => m_IsVisable; set => m_IsVisable = value; }

        public int PlayerId { get => m_PlayerId; set => m_PlayerId = value; }
    }
}
