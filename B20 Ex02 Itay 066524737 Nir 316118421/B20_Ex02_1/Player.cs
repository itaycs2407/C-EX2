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
        public Player()
        {
            m_IsHuman = !true;
        }

        public int NumOfHits { get => m_NumOfHits; set => m_NumOfHits = value; }
        public bool IsHuman { get => m_IsHuman;  }
        public string Name { get => m_Name; }
        public int Id { get => m_Id; }

        public void MakeMove()
        {
            throw new NotImplementedException();
        }
    }
}
