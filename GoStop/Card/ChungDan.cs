using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    public class ChungDan : Tti
    {
        public ChungDan(Month _month) : base(_month, TtiType.Chung)
        {
            if (_month != Month.June && _month != Month.September && _month != Month.October)
                new ArgumentException("Invalid Card Type");
        }
    }
}
