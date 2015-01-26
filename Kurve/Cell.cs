using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kurve
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //System.Windows.Shapes.Rectangle rectangle;

        public Cell(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            /*rectangle = new System.Windows.Shapes.Rectangle()
            {
                Fill = System.Windows.Media.Brushes.Gray,
                Width = X,
                Height = Y,
            };
            Engine.MainCanvas.Children.Add(rectangle);
            System.Windows.Controls.Canvas.SetLeft(rectangle, X);
            System.Windows.Controls.Canvas.SetTop(rectangle, Y);
            System.Windows.Controls.Canvas.SetZIndex(rectangle, -2);*/
        }

        public bool IsInTheCell(Point p)
        {
            if (p.X >= X && p.X <= X + Width
                && p.Y >= Y && p.Y <= Y + Height)
            {
                return true;
            }
            else return false;
        }
    }
}
