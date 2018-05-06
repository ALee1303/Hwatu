using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Card
{
    public enum PiType { pi=1, SsangPi }

    public class Pi : Hanafuda
    {
        PiType piType;

        public Pi(Month _month) : this(_month, PiType.pi)
        {
            if (_month == Month.December)
                new ArgumentException("Invalid Card Type");
        }

        protected Pi(Month _month, PiType _piType) : base(_month, CardType.Pi)
        {
            piType = _piType;
        }

        protected override bool isEqual(Hanafuda other)
        {
            if (base.isEqual(other) 
                && this.piType == ((Pi)other).piType)
                return true;
            return false;
        }
    }
}
