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
    class PCShip : Ship
    {
        private Dictionary<Key, Action> keys;

        public PCShip()
            : this(Brushes.Red, Key.Left, Key.Right)
        {

        }

        public PCShip(Brush color, Key left, Key right)
            : base(color)
        {
            keys = new Dictionary<Key, Action>();
            keys.Add(left, RotateLeft);
            keys.Add(right, RotateRight);
            
            Start();
        }

        protected override void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(CheckKeyboard);
            }
            catch (Exception exc)
            {

            }
            base.timer_Elapsed(sender, e);
        }

        private void CheckKeyboard()
        {
            foreach (KeyValuePair<Key, Action> pair in keys)
            {
                if (Keyboard.IsKeyDown(pair.Key))
                    pair.Value();
            }
        }
    }
}
