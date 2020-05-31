using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_1 
{ 
    public class Player 
    {
        private int m_Id;
        private string m_Name;
        private int m_NumOfHits;
        private bool m_IsHuman = !true;

        public Player(int i_Id, string i_Name, bool i_IsHuman)
        {
            m_Id = i_Id;
            m_Name = i_Name;
            m_IsHuman = i_IsHuman;
            m_NumOfHits = 0;
        }

        public int NumOfHits { get => m_NumOfHits; set => m_NumOfHits = value; }

        public bool IsHuman { get => m_IsHuman;  }

        public string Name { get => m_Name; }

        public int Id { get => m_Id; }

    }
}
