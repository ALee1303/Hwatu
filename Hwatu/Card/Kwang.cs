using System;

namespace Hwatu.Card
{
    public class Kwang : Hanafuda
    {
        public Kwang(Month _month) : base(_month, CardType.Kwang)
        {
            if (_month != Month.January && _month != Month.March && _month != Month.August
                && _month != Month.November && _month != Month.December)
                new ArgumentException("Invalid Card Type");
        }
    }
}
