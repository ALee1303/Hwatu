using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    abstract class ChoDan : Tti
    {
        protected ChoDan(Month _month) : base(_month, TtiType.Cho)
        { }
    }
}
