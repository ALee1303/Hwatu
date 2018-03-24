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
        PiType point;

        public int Point { get => (int)point; }

        public Pi(Month _month) : this(_month, PiType.pi)
        { }

        protected Pi(Month _month, PiType _point) : base(_month, CardType.Pi)
        {
            point = _point;
        }

        protected override bool isEqual(Hanafuda other)
        {
            if (base.isEqual(other) 
                && this.Point == ((Pi)other).Point)
                return true;
            return false;
        }
    }
}
