using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    abstract class Kwang : Hanafuda
    {
        protected Kwang(Month _month) : base(_month, CardType.Kwang)
        { }
    }
}
