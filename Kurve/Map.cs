using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurve
{
    public class Map
    {
        public Cell[,] Cells { get; private set; }

        public Map()
        {
            Cells = new Cell[10, 10];

            int cellWidth = (int)Engine.MainCanvas.Width / 10;
            int cellHeight = (int)Engine.MainCanvas.Height / 10;
            int stepX = -1;
            int stepY = -1;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Cells[i, j] = new Cell(stepX, stepY, cellWidth, cellHeight);
                    stepX += cellWidth;
                }
                stepX = -1;
                stepY += cellHeight;
            }
        }
    }
}
