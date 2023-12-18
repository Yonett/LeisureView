using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeisureView
{
    public class Player
    {
        private string? name;
        public string Name
        {
            get
            {
                if (this.name == null)
                    return "Player";
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }
        public List<int> FirstRolles;
        public List<int> SecondRolles;
        public int score;
        public Player(string name)
        {
            this.Name = name;
            FirstRolles = new List<int>();
            SecondRolles = new List<int>();
            score = 1;
        }
    }
}
