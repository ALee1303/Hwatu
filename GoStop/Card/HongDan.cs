using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    abstract class HongDan : Tti
    {
        protected HongDan(Month _month) : base(_month, TtiType.Hong)
        { }
    }
}
