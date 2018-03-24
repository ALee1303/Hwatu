using System;

namespace GoStop.Card
{
    enum CardType { Pi, Tti, Yul, Kwang }
    enum Month { January, February, March, April, June, July, August, September, October, November, December }
    abstract class Hanafuda : IEquatable<Hanafuda>
    {
        Month month;
        CardType type;

        protected Hanafuda(Month _month, CardType _type)
        {
            month = _month;
            type = _type;
        }

        public Month Month { get => month; }
        public CardType Type { get => type; }

        public bool Equals(Hanafuda other)
        {
            if (this.month == other.Month && this.type == other.Type)
                return true;
            else
                return false;
        }
    }
}
