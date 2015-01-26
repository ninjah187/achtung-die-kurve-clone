using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Kurve
{
    public class PowerupManager
    {
        public static List<Powerup> Powerups = new List<Powerup>();
        public const double powerupRadius = 7.5;
        
        private Timer timer;
        private static readonly Random random = new Random();

        private double counter;
        private double stepLimit;        

        public PowerupManager()
        {
            timer = new Timer();
            timer.Interval = 5;
            timer.Elapsed += timer_Elapsed;

            counter = 0;
            stepLimit = random.Next(200, 1000);

            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (counter < stepLimit)
                counter++;
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    AddRandomPowerup();
                });
                counter = 0;
                stepLimit = random.Next(200, 1000);
                //ZDARZENIE NA Deactivate() usuwające powerupa z listy
            }
        }

        private void AddRandomPowerup()
        {
            switch (random.Next(2))
            {
                case 0:
                    Powerups.Add(new SpeedUpPowerup());
                    break;

                case 1:
                    Powerups.Add(new SpeedDownPowerup());
                    break;
            }
        }
    }
}
