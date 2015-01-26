using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Kurve
{
    public class SpeedUpPowerup : Powerup, ITimeEffect
    {
        protected Timer timer;
        private double counter;
        private double stepLimit;
        protected Ship target = null;

        public SpeedUpPowerup()
            : base()
        {
            ellipse.Fill = System.Windows.Media.Brushes.Red;

            timer = new Timer()
            {
                Interval = 5,
            };
            timer.Elapsed += timer_Elapsed;

            counter = 0;
            stepLimit = 400;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (counter < stepLimit)
                counter++;
            else
                Deactivate();
        }

        public override void Activate(Ship s)
        {
            target = s;
            target.Velocity *= 2;
            timer.Start();
            //MainWindow.MainCanvas.Children.Remove(base.ellipse);
            //PowerupManager.Powerups.Remove(this);
        }

        public virtual void Deactivate()
        {
            target.Velocity /= 2;
            target = null;
            timer.Stop();            
        }
    }
}
