using System;

namespace Hwatu.Card
{
    public class ChoDan : Tti
    {
        public ChoDan(Month _month) : base(_month, TtiType.Cho)
        {
            if (_month != Month.April && _month != Month.May && _month != Month.July)
                new ArgumentException("Invalid Card Type");
        }
    }
}
