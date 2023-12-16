using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeisureView
{
    public class Player
    {
        public List<int> FirstRolles;
        public List<int> SecondRolles;
        public int score;
        public Player()
        {
            FirstRolles = new List<int>();
            SecondRolles = new List<int>();
            score = 1;
        }
    }
}
