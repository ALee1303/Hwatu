using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    enum TtiType { Hong, Chung, Cho}
    abstract class Tti : Hanafuda
    {
        TtiType dan;

        protected Tti(Month _month, TtiType _dan) : base(_month, CardType.Tti)
        {
            dan = _dan;
        }
    }
}
