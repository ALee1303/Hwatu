using System;

namespace Hwatu.Card
{
    public class SsangPi : Hanafuda
    {
        public SsangPi(Month _month) : base(_month, CardType.SsangPi)
        {
            if (_month != Month.November && _month != Month.December)
                new ArgumentException("Invalid Card Type");
        }
    }
}
