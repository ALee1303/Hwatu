using System;

namespace GoStop.Card
{
    public enum Month
    {
        January, February, March, April,
        May, June, July, August, September,
        October, November, December
    }
    public enum CardType { Pi, SsangPi, Tti, Yul, Kwang }

    // TODO: add IHanafuda interface
    public abstract class Hanafuda : IEquatable<Hanafuda>
    {
        #region Fields
        private Month month;
        private CardType type;
        #endregion

        protected Hanafuda(Month _month, CardType _type)
        {
            month = _month;
            type = _type;
        }

        #region Properties
        public Month Month { get => month; }
        public CardType Type { get => type; }
        #endregion

        #region IEquatable
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
        #endregion

    }
}
