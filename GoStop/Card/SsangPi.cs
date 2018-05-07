using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    public class SsangPi : Pi
    {
        public SsangPi(Month _month) : base(_month, PiType.SsangPi)
        {
            if (_month != Month.November && _month != Month.December)
                new ArgumentException("Invalid Card Type");
        }
    }
}
