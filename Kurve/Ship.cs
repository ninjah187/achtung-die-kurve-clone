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
using System.Timers;

namespace Kurve
{
    public struct BodyPiece
    {
        public Point location;
        public Ellipse ellipse;        

        public BodyPiece(Point _location, Ellipse _ellipse)
        {
            location = _location;
            ellipse = _ellipse;
        }
    }

    public abstract class Ship
    {
        protected static readonly Random random = new Random();
        private Timer timer;

        protected Point location;
        protected Vector velocity;
        protected double angle;
        private int gapCounter;

        protected Ellipse head;
        public List<BodyPiece> body; //public ?????????????!!!!!!!!!!!!! czemu nie protected???
        private Brush bodyBrush;        

        //private Dictionary<Key, Action> keys;

        public Point Location
        {
            get { return location; }
        }
        public Vector Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
       
        public Ship()
            : this(Brushes.Red)
        {
            
        }

        public Ship(Brush color)
        {
            timer = new Timer();
            timer.Interval = 10;
            timer.Elapsed += timer_Elapsed;

            location = new Point(random.Next(0, (int)Engine.MainCanvas.Width),
                                    random.Next(0, (int)Engine.MainCanvas.Height));
            velocity = new Vector(1, 0);
            angle = random.Next(0, 360);
            CalculateVelocity();
            gapCounter = 0;

            head = new Ellipse()
            {
                Width = 5,
                Height = 5,
                StrokeThickness = 0,
                Fill = Brushes.Yellow,
            };
            Engine.MainCanvas.Children.Add(head);

            body = new List<BodyPiece>();
            bodyBrush = color;

            /*keys = new Dictionary<Key, Action>();
            keys.Add(left, RotateLeft);
            keys.Add(right, RotateRight);*/

            Display();
            //Start();
        }

        public void Start()
        {
            timer.Start();
        }

        private bool drawFlag;

        protected virtual void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if(drawFlag = (!drawFlag))
                {
                    Application.Current.Dispatcher.Invoke(BuildBody);
                }
                //Application.Current.Dispatcher.Invoke(CheckKeyboard);
                /*Application.Current.Dispatcher.Invoke(*/Update();/*);*/
                Application.Current.Dispatcher.Invoke(CheckCollisions);
                Application.Current.Dispatcher.Invoke(CheckEdges);
                Application.Current.Dispatcher.Invoke(Display);
            }
            catch (Exception exc)
            {

            }
            //gapCounter++;
        }

        private void BuildBody()
        {
            if (gapCounter < 100)
            {                
                Ellipse temp = new Ellipse()
                {
                    Width = 5,
                    Height = 5,
                    StrokeThickness = 0,
                    Fill = bodyBrush,                    
                };
                Engine.MainCanvas.Children.Add(temp);
                Canvas.SetLeft(temp, location.X);
                Canvas.SetTop(temp, location.Y);
                Canvas.SetZIndex(temp, -1);
                gapCounter++;

                body.Add(new BodyPiece(location, temp));
            }
            if (gapCounter >= 100 && gapCounter < 110)
            {
                gapCounter++;
                return;
            }
            if (gapCounter == 110)
            {
                gapCounter = 0;
            }
        }      

        private void Update()
        {
            location += velocity;
        }

        private void CheckCollisions()
        {
            //sprawdzanie siebie
            for (int i = 0; i < body.Count - 100; i++)
            {
                if(Math.Sqrt((location.X - body[i].location.X)
                    * (location.X - body[i].location.X)
                    + (location.Y - body[i].location.Y)
                    * (location.Y - body[i].location.Y)) < head.Width)
                {
                    timer.Stop();
                    return;
                }
            }

            //sprawdzanie innych            
            //CheckCollisionsWithOthers(Engine.GetPointsToCheck(this));
            CheckCollisionsWithOthers();

            //for (int i = 0; i < Engine.Ships.Count; i++)
            //{
            //    Ship s = Engine.Ships[i];
            //    if (s != this)
            //    {
            //        for (int j = 0; j < s.body.Count; j++)
            //        {
            //            if (Math.Sqrt((location.X - s.body[j].location.X)
            //                * (location.X - s.body[j].location.X)
            //                + (location.Y - s.body[j].location.Y)
            //                * (location.Y - s.body[j].location.Y)) < head.Width)
            //            {
            //                timer.Stop();
            //                return;
            //            }
            //        }
            //    }
            //}
        }

        //private void CheckCollisionsWithOthers(List<Point> checkPoints)
        private void CheckCollisionsWithOthers()
        {
            foreach (Point checkPoint in Engine.GetPointsToCheck(this))
            {
                if (Math.Sqrt((location.X - checkPoint.X)
                            * (location.X - checkPoint.X)
                            + (location.Y - checkPoint.Y)
                            * (location.Y - checkPoint.Y)) < head.Width)
                {
                    timer.Stop();
                    return;
                }
            }
        }

        /*private void CheckCollisions()
        {
            double bodyPieceRadius = head.Width / 2;
            Point headCenter = new Point(location.X + bodyPieceRadius,
                                            location.Y + bodyPieceRadius);
            Point bodyPieceCenter = new Point();
            Vector distanceVector = new Vector();

            for (int i = 0; i < body.Count - 100; i++)
            {
                bodyPieceCenter.X = body[i].location.X + bodyPieceRadius;
                bodyPieceCenter.Y = body[i].location.Y + bodyPieceRadius;
                distanceVector.X = headCenter.X - bodyPieceCenter.X;
                distanceVector.Y = headCenter.Y - bodyPieceCenter.Y;
                if (distanceVector.Length < head.Width)
                {
                    timer.Stop();
                    return;
                }
            }

            foreach (Ship s in MainWindow.ships)
            {
                if (s != this)
                {
                    for (int i = 0; i < s.body.Count; i++)
                    {
                        bodyPieceCenter.X = s.body[i].location.X + bodyPieceRadius;
                        bodyPieceCenter.Y = s.body[i].location.Y + bodyPieceRadius;
                        distanceVector.X = headCenter.X - bodyPieceCenter.X;
                        distanceVector.Y = headCenter.Y - bodyPieceCenter.Y;
                        if (distanceVector.Length < head.Width)
                        {
                            timer.Stop();
                            return;
                        }
                    }
                }
            }

            Point powerupCenter = new Point();
            List<Powerup> toBeRemoved = new List<Powerup>();
            foreach (Powerup p in PowerupManager.Powerups)
            {
                powerupCenter.X = p.Location.X + PowerupManager.powerupRadius;
                powerupCenter.Y = p.Location.Y + PowerupManager.powerupRadius;
                distanceVector.X = headCenter.X - powerupCenter.X;
                distanceVector.Y = headCenter.Y - powerupCenter.Y;
                if (distanceVector.Length < bodyPieceRadius + PowerupManager.powerupRadius)
                {
                    p.Activate(this);
                    toBeRemoved.Add(p);
                }
            }
            foreach (Powerup p in toBeRemoved)
            {
                p.Remove();
            }
        }*/

        private void CheckEdges()
        {
            if (location.X < 0 ||
                location.X > Engine.MainCanvas.Width - head.Width ||
                location.Y < 0 ||
                location.Y > Engine.MainCanvas.Height - head.Height)
            {
                timer.Stop();
            }
        }

        protected void RotateLeft()
        {
            angle = (angle - 1.5) % 360;

            CalculateVelocity();
        }

        protected void RotateRight()
        {
            angle = (angle + 1.5) % 360;

            CalculateVelocity();
        }

        private void CalculateVelocity()
        {
            double angleRadians = (angle * Math.PI) / 180;
            double x = velocity.Length * Math.Cos(angleRadians);
            double y = velocity.Length * Math.Sin(angleRadians);

            velocity.X = x;
            velocity.Y = y;
        }

        private void Display()
        {
            Canvas.SetLeft(head, location.X);
            Canvas.SetTop(head, location.Y);            
        }

        public void Reset()
        {
            while (body.Count != 0)
            {
                Engine.MainCanvas.Children.Remove(body[body.Count - 1].ellipse);
                body.Remove(body[body.Count - 1]);
            }
            location = new Point(random.Next(0, (int)Engine.MainCanvas.Width),
                                    random.Next(0, (int)Engine.MainCanvas.Height));            
            angle = random.Next(0, 360);
            CalculateVelocity();
            gapCounter = 0;
            Display();

            Start();
        }
    }
}
