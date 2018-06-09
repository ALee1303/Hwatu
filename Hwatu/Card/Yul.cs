using System;

namespace Hwatu.Card
{
    public class Yul : Hanafuda
    {
        public Yul(Month _month) : base(_month, CardType.Yul)
        {
            if (_month == Month.January || _month == Month.March || _month == Month.November)
                new ArgumentException("Invalid Card Type");
        }
    }
}
