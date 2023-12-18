using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeisureView
{
    public class Cell
    {
        private int kind = 0;

        public int Kind
        {
            get
            {
                return kind;
            }

            set
            {
                this.kind = value;
            }
        }

        public Cell() { }
    }
}
