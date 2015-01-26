using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace Kurve
{
    public class Engine
    {
        public static Canvas MainCanvas { get; private set; }
        public static List<Ship> Ships = new List<Ship>();
        public static Map Map;

        public Engine(Canvas mainCanvas)
        {
            MainCanvas = mainCanvas;

            Map = new Map();

            Ships.Add(new PCShip(Brushes.Red, Key.Left, Key.Right));
            Ships.Add(new PCShip(Brushes.Green, Key.A, Key.D));

            /*Ships.Add(new NPCShip());
            Ships.Add(new NPCShip(Brushes.Green));
            Ships.Add(new NPCShip(Brushes.Purple));
            Ships.Add(new NPCShip(Brushes.Orange));*/
        }

        public Point GetPointsToCheck()
        {
            for (int i = 0; i < 100; i++)
            {
                return new Point();
            }
            return new Point();
        }

        public static IEnumerable<Point> GetPointsToCheck(Ship sender)
        {
            // Point sender, a nie Ship sender, bo w przypadku sprawdzania promieni, można
            // dostać złą listę punktów. Promień i głowa mogą znajdować się w innej komórce
            // mapy, przez co pewne punkty mogą zostać niesprawdzone.

            Cell checkCell = null;
            for (int i = 0; i < 100; i++)
            {
                int k = i / 10;
                int l = i % 10;

                if (Map.Cells[k, l].IsInTheCell(sender.Location))
                {
                    checkCell = Map.Cells[k, l];
                    break;
                }
            }


            for (int i = 0; i < Ships.Count; i++)
            {
                if (Ships[i] != sender)
                {
                    for (int j = 0; j < Ships[i].body.Count; j++)
                    {
                        BodyPiece bp = Ships[i].body[j];
                        if (checkCell.IsInTheCell(bp.location))
                            yield return bp.location;
                    }
                }
            }
            /*foreach (Ship s2 in Ships)
            {
                if (s2 != sender)
                {
                    for (int i = 0; i < s2.body.Count; i++)
                    {
                        BodyPiece bp = s2.body[i];
                        if (checkCell.IsInTheCell(bp.location))
                            yield return bp.location;
                    }
                }
            }*/
        }//YIELD RETURN!!!

        public static IEnumerable<Point> GetPointsToCheck(Ship sender, Point senderPoint)
        {
            //Metoda wykorzystywana w NPCShip.CheckCollisionsWithOthers()

            Cell checkCell = null;
            for (int i = 0; i < 100; i++)
            {
                int k = i / 10;
                int l = i % 10;

                if (Map.Cells[k, l].IsInTheCell(senderPoint))
                {
                    checkCell = Map.Cells[k, l];
                    break;
                }
            }

            for (int i = 0; i < Ships.Count; i++)
            {
                if (Ships[i] != sender)
                {
                    for (int j = 0; j < Ships[i].body.Count; j++)
                    {
                        BodyPiece bp = Ships[i].body[j];
                        if (checkCell.IsInTheCell(bp.location))
                            yield return bp.location;
                    }
                }
            }
        }
    }
}
