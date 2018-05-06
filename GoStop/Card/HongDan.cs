using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    public class HongDan : Tti
    {
        public HongDan(Month _month) : base(_month, TtiType.Hong)
        {
            if (_month != Month.January || _month != Month.February || _month != Month.March)
                new ArgumentException("Invalid Card Type");
        }
    }
}
