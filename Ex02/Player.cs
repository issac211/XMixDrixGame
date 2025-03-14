using System;

namespace Ex02
{
    public class Player
    {
        private readonly bool r_IsHuman;
        private readonly char r_PlayerShape;
        private readonly Random r_ComputerChoice;
        private int m_Score;

        public Player(char i_PlayerShape, bool i_IsHuman)
        {
            m_Score = 0;
            r_PlayerShape = i_PlayerShape;
            r_IsHuman = i_IsHuman;
            if (!i_IsHuman)
            {
                r_ComputerChoice = new Random();
            }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public char PlayerShape
        {
            get { return r_PlayerShape; }
        }

        public bool IsHuman
        {
            get { return r_IsHuman; }
        }

        public Random ComputerChoice
        {
            get { return r_ComputerChoice; }
        }

        public void ResetPoints()
        {
            Score = 0;
        }

        public override string ToString()
        {
            return PlayerShape.ToString();
        }
    }
}
