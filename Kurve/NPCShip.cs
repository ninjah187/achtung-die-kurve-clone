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
using System.Threading;

namespace Kurve
{
    class NPCShip : Ship
    {
        struct Ray
        {
            public Point location;
            public Vector power;
            public bool flag;
        }

        private Ray rayLeft;
        private Ray rayCenter;
        private Ray rayRight;

        private double moveCounter;
        private double moveLimit;
        private Action move;

        private const int rayCastDistance = 25; // * 3
        private const int rayCastAngle = 30;

        List<Ellipse> rayEllipse;

        public NPCShip()
            : this(Brushes.Red)
        {

        }

        public NPCShip(Brush color)
            : base(color)
        {
            rayLeft.location = new Point();
            rayCenter.location = new Point();
            rayRight.location = new Point();
            rayLeft.power = new Vector();
            rayCenter.power = new Vector();
            rayRight.power = new Vector();

            rayEllipse = new List<Ellipse>();
            for (int i = 0; i < rayCastDistance * 3; i++)
            {
                Ellipse e;
                rayEllipse.Add(e = new Ellipse()
                {
                    Width = 5,
                    Height = 5,
                    Fill = Brushes.Blue,
                });
                Engine.MainCanvas.Children.Add(e);
            }

            moveCounter = 0;
            moveLimit = 0;
            Start();
        }

        protected override void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(Move);
            }
            catch (Exception exc)
            {

            }
            base.timer_Elapsed(sender, e);
        }

        private void RotateVector()
        {

        }

        private void Move()
        {
            rayLeft.location.X = rayCenter.location.X = rayRight.location.X = location.X;
            rayLeft.location.Y = rayCenter.location.Y = rayRight.location.Y = location.Y;

            rayCenter.power.X = velocity.X;
            rayCenter.power.Y = velocity.Y;

            rayCenter.power *= 3;

            double angleRadians = ((angle + rayCastAngle) * Math.PI) / 180;
            double sin = Math.Sin(angleRadians);
            double minSin = Math.Sin(((angle - rayCastAngle) * Math.PI) / 180);
            double minCos = Math.Cos(((angle - rayCastAngle) * Math.PI) / 180);
            double cos = Math.Cos(angleRadians);

            rayLeft.power.X = rayCenter.power.Length * minCos;
            rayLeft.power.Y = rayCenter.power.Length * minSin;
            rayRight.power.X = rayCenter.power.Length * cos;
            rayRight.power.Y = rayCenter.power.Length * sin;

            rayLeft.power *= 0.6;
            rayRight.power *= 0.6;
            //string msg = "";

            //z losowaniem nowego ruchu, po wykonaniu ruchu "mądrego" moveCounter = moveLimit = 0;
            
            for (int i = 0; i < rayCastDistance; i++)
            {
                //strzał promieniem
                rayLeft.location += rayLeft.power;
                rayCenter.location += rayCenter.power;
                rayRight.location += rayRight.power;

                Canvas.SetLeft(rayEllipse[i], rayLeft.location.X);
                Canvas.SetTop(rayEllipse[i], rayLeft.location.Y);
                Canvas.SetLeft(rayEllipse[i + rayCastDistance], rayCenter.location.X);
                Canvas.SetTop(rayEllipse[i + rayCastDistance], rayCenter.location.Y);
                Canvas.SetLeft(rayEllipse[i + 2 * rayCastDistance], rayRight.location.X);
                Canvas.SetTop(rayEllipse[i + 2 * rayCastDistance], rayRight.location.Y);

                //sprawdzanie ścian
                CheckEdges(ref rayLeft);
                CheckEdges(ref rayCenter);
                CheckEdges(ref rayRight);

                //sprawdzanie siebie
                if (!rayLeft.flag)
                    CheckSelf(ref rayLeft);
                if (!rayCenter.flag)
                    CheckSelf(ref rayCenter);
                if (!rayRight.flag)
                    CheckSelf(ref rayRight);

                //sprawdzanie innych
                if (!rayLeft.flag)
                    CheckOthers(ref rayLeft);
                if (!rayCenter.flag)
                    CheckOthers(ref rayCenter);
                if (!rayRight.flag)
                    CheckOthers(ref rayRight);

                //DecideMove() ?
            }
            //string flag1, flag2, flag3;
            //if (rayLeft.flag)
            //    flag1 = "true";
            //else flag1 = "false";
            //if (rayCenter.flag)
            //    flag2 = "true";
            //else flag2 = "false";
            //if (rayRight.flag)
            //    flag3 = "true";
            //else flag3 = "false";
            //string txt = "flag left: " + flag1 + "\nflag center: " + flag2 +
            //                "\nflag right: " + flag3;
            ////string txt = location.X + " " + location.Y;
            //MainWindow.TxtBox.Text = txt;

            DecideMove();

            rayLeft.flag = false;
            rayCenter.flag = false;
            rayRight.flag = false;

            //moveCounter = moveLimit = 0; //z tym lepiej czy bez tego ?!?!?!?!
            // ?!?!?!?!?!?!?!?!?!?!?!

            //wędrowanie
            //Wander();
        }

        private void CheckRay(object o)
        {
            Ray r = (Ray)o;
            CheckEdges(ref r);
            if (!r.flag)
                CheckSelf(ref r);
            if (!r.flag)
                CheckOthers(ref r);
        }

        private void CheckEdges(ref Ray r)
        {
            if (r.location.Y <= 0 ||
                r.location.Y >= Engine.MainCanvas.Height - 0 ||
                r.location.X <= 0 ||
                r.location.X >= Engine.MainCanvas.Width - 0)
            {
                r.flag = true;
                return;
            }            
        }

        private void CheckSelf(ref Ray r)
        {
            for (int j = 0; j < body.Count - 100; j++)
            {
                //distanceVector.X = r.location.X - body[j].location.X;
                //distanceVector.Y = r.location.Y - body[j].location.Y;
                //if (distanceVector.Length < head.Width)
                if(Math.Sqrt((r.location.X - body[j].location.X)
                    * (r.location.X - body[j].location.X)
                    + (r.location.Y - body[j].location.Y)
                    * (r.location.Y - body[j].location.Y)) < head.Width)
                {
                    r.flag = true;
                    return;
                }
            }            
        }

        private void CheckOthers(ref Ray r)
        {
            foreach (Point checkPoint in Engine.GetPointsToCheck(this, r.location))
            {
                if (Math.Sqrt((r.location.X - checkPoint.X)
                                * (r.location.X - checkPoint.X)
                                + (r.location.Y - checkPoint.Y)
                                * (r.location.Y - checkPoint.Y)) < head.Width)
                {
                    r.flag = true;
                    return;
                }
            }         
        }

        private void DecideMove()
        {
            if (!rayLeft.flag && !rayCenter.flag && !rayRight.flag)
            {
                Wander(); //Attack() ?
                return;
            }
            if (rayLeft.flag)
            {
                if (!rayCenter.flag && !rayRight.flag)
                {
                    RotateRight();
                    return;
                }
                if (rayCenter.flag && rayRight.flag)
                {
                    Wander();
                    return;
                    //jesteś w dupie
                }
                if (rayRight.flag)
                {
                    //prosto
                    return;
                }
                if (rayCenter.flag)
                {
                    RotateRight();
                    return;
                }                
            }
            if (rayCenter.flag)
            {
                if (rayRight.flag)
                {
                    RotateLeft();
                    return;
                }
                Wander();
                return;
            }
            if (rayRight.flag)
            {
                RotateLeft();
                return;
            }
        }

        private void Wander()
        {
            if (moveCounter == moveLimit)
            {
                moveCounter = 0;
                moveLimit = random.Next(20, 50);

                switch (random.Next(3))
                {
                    case 0:
                        move = RotateLeft;
                        break;
                    case 1:
                        move = RotateRight;
                        break;
                    case 2:
                        move = null;
                        break;
                }
            }
            else
            {
                if (move != null)
                    move();
                moveCounter++;
            }
        }

        //private void Move()
        //{
        //    //sprawdzanie czy trzeba odsunąć się od krawędzi
        //    if (location.Y < edgeCheckDistance)
        //    {
        //        if (velocity.X > 0)
        //            RotateRight();
        //        else RotateLeft();
        //        return;
        //    }
        //    if (location.Y > MainWindow.MainCanvas.Height - head.Height - edgeCheckDistance)
        //    {
        //        if (velocity.X > 0)
        //            RotateLeft();
        //        else RotateRight();
        //        return;
        //    }
        //    if (location.X < edgeCheckDistance)
        //    {
        //        /*if (velocity.Y > 0)
        //            RotateRight();
        //        else RotateLeft();
        //        return;*/
        //        if (velocity.Y > 0)
        //            RotateLeft();
        //        else RotateRight();
        //        return;
        //    }
        //    if (location.X > MainWindow.MainCanvas.Width - head.Width - edgeCheckDistance)
        //    {
        //        /*if (velocity.Y > 0)
        //            RotateLeft();
        //        else RotateRight();
        //        return;*/
        //        if (velocity.Y > 0)
        //            RotateRight();
        //        else RotateLeft();
        //        return;
        //    }

        //    //sprawdzanie czy trzeba się odsunąć od siebie
        //    Point bpLocation = new Point(0, 0);            
        //    //double bpDistance = 0;
        //    for (int i = 0; i < body.Count - 100; i++)
        //    {
        //        Vector distanceVector = new Vector(location.X - body[i].location.X,
        //                                        location.Y - body[i].location.Y);
        //        if (distanceVector.Length < 50)
        //        {
        //            bpLocation = body[i].location;
        //            //bpDistance = distanceVector.Length;
        //            break;
        //        }
        //    }
        //    //sprawdzanie czy trzeba się odsunąć od kogoś
        //    foreach (Ship s in MainWindow.ships)
        //    {
        //        if (s != this)
        //        {                    
        //            for (int i = 0; i < s.body.Count; i++)
        //            {
        //                Vector distanceVector = new Vector(location.X - s.body[i].location.X,
        //                                        location.Y - s.body[i].location.Y);
        //                if (distanceVector.Length < 50)
        //                {
        //                    bpLocation = s.body[i].location;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    if (bpLocation.X != 0 && bpLocation.Y != 0)
        //    {
        //        double rr, rl;
        //        rr = rl = 0;

        //        if (location.Y < bpLocation.Y)
        //        {
        //            if (velocity.X > 0)
        //                //RotateLeft();
        //                rl++;
        //            else rr++;//RotateRight();
        //            //return;
        //        }
        //        if (location.Y > bpLocation.Y)
        //        {
        //            if (velocity.X > 0)
        //                rr++;//RotateRight();
        //            else rl++;// RotateLeft();
        //            //return;
        //        }
        //        if (location.X < bpLocation.X)
        //        {
        //            if (velocity.Y > 0)
        //                rr++;//rl++;//RotateLeft();
        //            else rl++;// rr++;// RotateRight();
        //            //return;
        //        }
        //        if (location.X > bpLocation.X)
        //        {                    
        //            if (velocity.Y > 0)
        //                rl++;//rr++;//RotateRight();
        //            else rr++;// rl++;//RotateLeft();
        //            //return;  
        //        }

        //        if (rr >= rl)
        //            RotateRight();
        //        else RotateLeft();
        //        return;
        //    }

        //    if (moveCounter == moveLimit)
        //    {
        //        moveCounter = 0;
        //        moveLimit = random.Next(20, 50);

        //        switch (random.Next(3))
        //        {
        //            case 0:
        //                move = RotateLeft;
        //                break;
        //            case 1:
        //                move = RotateRight;
        //                break;
        //            case 2:
        //                move = null;
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        if (move != null)
        //            move();
        //        moveCounter++;
        //    }
        //}

        class FiniteStateMachine
        {
            
        }
    }
}
