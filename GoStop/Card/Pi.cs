using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    public class Pi : Hanafuda
    {
        private int piCount;
        public int PiCount { get => piCount; }
        public Pi(Month _month, int count) : base(_month, CardType.Pi)
        {
            if (_month == Month.December)
                new ArgumentException("Invalid Card Type");
            piCount = count;
        }
    }
}
