using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    abstract class ChungDan : Tti
    {
        protected ChungDan(Month _month) : base(_month, TtiType.Chung)
        { }
    }
}
