using System;

namespace Hwatu.Card
{
    public class HongDan : Tti
    {
        public HongDan(Month _month) : base(_month, TtiType.Hong)
        {
            if (_month != Month.January && _month != Month.February && _month != Month.March)
                new ArgumentException("Invalid Card Type");
        }
    }
}
