using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurve
{
    public class SpeedDownPowerup : SpeedUpPowerup
    {
        public SpeedDownPowerup()
            : base()
        {
            ellipse.Fill = System.Windows.Media.Brushes.Blue;
        }

        public override void Activate(Ship s)
        {
            base.target = s;
            base.target.Velocity /= 2;
            base.timer.Start();
        }

        public override void Deactivate()
        {
            base.target.Velocity *= 2;
            base.target = null;
            base.timer.Stop();
        }
    }
}
