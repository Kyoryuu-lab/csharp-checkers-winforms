using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers
{
    public class BoardPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public BoardPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }

    }
}