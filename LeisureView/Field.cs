using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LeisureView
{
    public class Field
    {
        public int size;
        public int bounds;
        public int amount;
        public Cell[] cells;

        public bool CanPlaceOnCellByEmptiness(int index)
        {
            return this.cells[index].Kind == 0;
        }

        public bool CanPlaceOnCellByNearCells(int kind, int index)
        {
            return
            (
                this.cells[index + 1].Kind == kind // Right cell
                ||
                this.cells[index - 1].Kind == kind // Left cell
                ||
                this.cells[index - this.size].Kind == kind // Top cell
                ||
                this.cells[index + this.size].Kind == kind // Bottom cell
            );
        }

        public bool CanPlacePiece(Piece piece, int index)
        {
            int i = index / this.size;
            int j = index % this.size;
            if (i > bounds || i < 1 || j > bounds || j < 1)
            {
                //Console.WriteLine("Trying to place piece on cell out of field");
                return false;
            }

            if (i + piece.h - 1 > bounds || j + piece.w - 1 > bounds)
            {
                //Console.WriteLine("Piece is too big to be placed on this cell");
                return false;
            }

            bool canPlace = true;

            for (int m = 0; m < piece.h; m++)
            {
                for (int n = 0; n < piece.w; n++)
                {
                    canPlace &= CanPlaceOnCellByEmptiness(index + m * this.size + n);
                    if (!canPlace)
                        return false;
                }
            }

            canPlace = false;

            for (int m = 0; m < piece.h; m++)
            {
                for (int n = 0; n < piece.w; n++)
                {
                    canPlace |= CanPlaceOnCellByNearCells(piece.kind, index + m * this.size + n);
                    if (canPlace)
                        break;
                }
                if (canPlace)
                    break;
            }

            if (!canPlace)
            {
                //Console.WriteLine("No similar cells nearby");
                return false;
            }

            return true;
        }

        public Field(int n)
        {
            this.size = n + 2;
            this.bounds = n;
            this.amount = this.size * this.size;
            this.cells = new Cell[this.amount];

            for (int i = 0; i < this.amount; i++)
                cells[i] = new Cell();

            cells[n + 3].Kind = 1;
            cells[this.amount - n - 4].Kind = 2;

            for (int i = 1; i <= n; i++)
                cells[i].Kind = 3;

            for (int i = 0; i < this.size; i++)
                cells[this.size * i].Kind = 3;

            cells[n + 1].Kind = 3;
            for (int i = 1; i <= this.size; i++)
                cells[this.size * i - 1].Kind = 3;

            for (int i = 0; i < n; i++)
                cells[amount - (n - i) - 1].Kind = 3;

        }
    }
}
