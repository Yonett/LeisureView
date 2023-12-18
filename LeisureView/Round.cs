using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeisureView
{
    public class Round
    {
        public int FirstMove_FirstDice;
        public int FirstMove_SecondDice;
        public int SecondMove_FirstDice;
        public int SecondMove_SecondDice;
        public Round()
        {
            FirstMove_FirstDice = 0;
            FirstMove_SecondDice = 0;
            SecondMove_FirstDice = 0;
            SecondMove_SecondDice = 0;
        }
    }
}
