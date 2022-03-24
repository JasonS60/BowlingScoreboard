using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreboard
{
    class Player
    {
        public List<Frame> frames = new List<Frame>();
        private char prevRoll = '0';
        private char prevRoll2 = '0';
        private string name = "";

        public Player(string name)
        {
            this.name = name;

            for(int i = 0; i < 10; i++)
            {
                Frame frame = new Frame();
                this.frames.Add(frame);
            }
        }

        public string getName()
        {
            return this.name;
        }

        public char getPrevRoll()
        {
            return this.prevRoll;
        }

        public void setPrevRoll(char prevRoll)
        {
            this.prevRoll = prevRoll;
        }

        public char getPrevRoll2()
        {
            return this.prevRoll2;
        }

        public void setPrevRoll2(char prevRoll2)
        {
            this.prevRoll2 = prevRoll2;
        }

        public int totalScore()
        {
            int score = 0;
            foreach(Frame f in frames)
            {
                score += f.getScore();
            }

            return score;
        }
    }
}
