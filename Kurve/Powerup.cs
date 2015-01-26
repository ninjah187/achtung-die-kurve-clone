using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kurve
{
    public abstract class Powerup : IEffect
    {
        private static readonly Random random = new Random();

        private Point location;
        public Point Location
        {
            get { return location; }
        }

        protected Ellipse ellipse;

        public Powerup()
        {
            ellipse = new Ellipse()
            {
                Width = 15,
                Height = 15,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
            };
            Engine.MainCanvas.Children.Add(ellipse);

            location = new Point(random.Next(0, (int)(Engine.MainCanvas.Width - ellipse.Width)),
                                    random.Next(0, (int)(Engine.MainCanvas.Height - ellipse.Height)));

            Display();
        }

        public abstract void Activate(Ship s);

        private void Display()
        {
            Canvas.SetLeft(ellipse, location.X);
            Canvas.SetTop(ellipse, location.Y);
        }

        public void Remove()
        {
            Engine.MainCanvas.Children.Remove(this.ellipse);
            PowerupManager.Powerups.Remove(this);
        }
    }
}
