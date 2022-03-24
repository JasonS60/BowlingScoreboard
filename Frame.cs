using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreboard
{
    class Frame
    {
        private char roll1;
        private char roll2;
        private int score;

        public Frame()
        {
            this.roll1 = '0';
            this.roll2 = '0';
            this.score = 0;
        }
        
        public char getRoll1()
        {
            return this.roll1;
        }

        public char getRoll2()
        {
            return this.roll2;
        }

        public char getRoll(bool isFirstRoll)
        {
            if (isFirstRoll)
                return this.roll1;
            else
                return this.roll2;
        }

        public void setRoll(bool isFirstRoll, char roll)
        {
            if (isFirstRoll)
                this.roll1 = roll;
            else
                this.roll2 = roll;
        }

        public void setScore(int score)
        {
            this.score = score;
        }

        public int getScore()
        {
            return this.score;
        }
    }
}
