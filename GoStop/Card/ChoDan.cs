using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    public class ChoDan : Tti
    {
        public ChoDan(Month _month) : base(_month, TtiType.Cho)
        {
            if (_month != Month.April || _month != Month.May || _month != Month.July)
                new ArgumentException("Invalid Card Type");
        }
    }
}
