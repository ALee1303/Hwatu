using System;

namespace GoStop.Card
{
    public enum CardType { Pi, Tti, Yul, Kwang }
    public enum Month { January, February, March, April, May, June, July, August, September, October, November, December }

    public abstract class Hanafuda : IEquatable<Hanafuda>
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

        //from interface
        public bool Equals(Hanafuda other)
        {
            return isEqual(other);
        }
        //obj override
        public override bool Equals(object obj)
        {
            var toCompare = obj as Hanafuda;

            if (obj == null)
                return false;

            return this.Equals(toCompare);
        }

        //Helper Function
        protected virtual bool isEqual(Hanafuda other)
        {
            if (this.month == other.Month && this.type == other.Type)
                return true;
            else
                return false;
        }

        public static bool operator ==(Hanafuda card1, Hanafuda card2)
        {
            return card1.Equals(card2);
        }

        public static bool operator !=(Hanafuda card1, Hanafuda card2)
        {
            return !card1.Equals(card2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
