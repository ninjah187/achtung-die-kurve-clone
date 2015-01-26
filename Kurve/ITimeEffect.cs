using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Kurve
{
    public interface ITimeEffect : IEffect
    {
        void Deactivate();
    }
}
