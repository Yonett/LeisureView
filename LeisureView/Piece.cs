using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeisureView
{
    public class Piece
    {
        public int kind;
        public int w;
        public int h;
        public Piece(int kind, int w, int h)
        {
            this.kind = kind;
            this.w = w;
            this.h = h;
        }
    }
}
