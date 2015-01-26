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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// TODO:
    /// - lepsza AI,
    /// - powerupy!,
    /// - menu, interfejs,
    /// - liczenie wyników,
    /// - multiplayer
    /// - W WPFie OBCZAJ DZIEDZICZENIE PO FrameworkElement, własne kontrolki, delegaty, zdarzenia
    public partial class MainWindow : Window
    {       
        // LISTA SHIPS JAKO STATIC W KLASIE SHIP, DZIĘKI CZEMU NIE BĘDZIE WYMAGANE TWORZENIE
        // LISTY PRZECHOWUJĄCEJ WSZYSTKIE STATKI GDZIEŚ ZEWNĄTRZ
        // KOD WYKRYWAJĄCY KOLIZJE JEST ZALEŻNY WŁAŚNIE OD TEJ LISTY
        // LUB TRZYMAĆ TAKĄ LISTĘ W ENGINIE

        public static List<Ship> ships = new List<Ship>();
        public static TextBox TxtBox;

        public MainWindow()
        {
            InitializeComponent();

            Engine e = new Engine(mainCanvas);
            MainWindow.TxtBox = txtBox;
            TxtBox.Visibility = Visibility.Hidden;

            //wyłącza anty-aliasing
            //RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);

            //Ship ship = new Ship();
            //Ship ship2 = new Ship(Brushes.Green, Key.A, Key.D);

            //ships.Add(new Ship());
            //ships.Add(new Ship(Brushes.Green, Key.A, Key.D));

            //ships.Add(new NPCShip(Brushes.Orange));
            //ships.Add(new NPCShip(Brushes.Purple));

            //PowerupManager pm = new PowerupManager();
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    foreach (Ship s in Engine.Ships)
                    {
                        s.Reset();
                    }
                    break;

                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}
