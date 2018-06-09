using System;

namespace Hwatu.Card
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

        protected override bool isEqual(Hanafuda other)
        {
            if (base.isEqual(other)
                && this.piCount == ((Pi)other).piCount)
                return true;
            return false;
        }
    }
}
