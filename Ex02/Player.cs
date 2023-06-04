using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ex02
{
    public class Player 
    {
        // < ------------------ PROPERTIES ------------------ >
        internal Cell m_PlayerShape { get; set; }
        internal string m_Name { get; set; }
        internal bool m_IsHuman { get; set; }
        internal int m_Points { get; set; }
        internal bool m_isDifficultyov_Hard { get; set; }

        // C'tor
        public Player(Cell i_PlayerShape, string i_Name, bool i_IsHuman, bool i_isDifficultyov_Hard)
        {
            m_Name = i_Name;
            m_PlayerShape = i_PlayerShape;
            m_IsHuman = i_IsHuman;
            m_isDifficultyov_Hard = i_isDifficultyov_Hard;
        }
        
    }

}
