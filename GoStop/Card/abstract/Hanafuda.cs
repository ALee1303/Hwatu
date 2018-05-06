using System;

namespace GoStop.Card
{
    public enum Month { January, February, March, April, May, June, July, August, September, October, November, December }
    public enum CardType { Pi, Tti, Yul, Kwang }
    public enum CardState { Hidden, Revealed }

    // TODO: add IHanafuda interface
    public abstract class Hanafuda : IEquatable<Hanafuda>
    {
        #region Fields
        private Month month;
        private CardType type;
        private CardState state;
        private Player owner;
        #endregion

        protected Hanafuda(Month _month, CardType _type)
        {
            month = _month;
            type = _type;
            state = CardState.Hidden;
            owner = null;
        }

        #region Properties
        public Month Month { get => month; }
        public CardType Type { get => type; }
        public CardState State
        {
            get => state;

            set
            {
                if (state == value)
                    return;
                state = value;
                HanafudaEventArgs args = new HanafudaEventArgs();
                args.State = value;
                OnStateChanged(args);
            }
        }
        public Player Owner
        {
            get => owner;

            set
            {
                if (owner == value)
                    return;
                owner = value;
                HanafudaEventArgs args = new HanafudaEventArgs();
                args.Owner = value;
                OnOwnerChanged(args);
            }
        }
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

        #region Events
        public event EventHandler<HanafudaEventArgs> OwnerChanged;
        public event EventHandler<HanafudaEventArgs> StateChanged;

        protected virtual void OnOwnerChanged(HanafudaEventArgs args)
        {
            OwnerChanged?.Invoke(this, args);
        }
        
        protected virtual void OnStateChanged(HanafudaEventArgs args)
        {
            StateChanged?.Invoke(this, args);
        }
        #endregion
    }

    public class HanafudaEventArgs : EventArgs
    {
        public Player Owner { get; set; }
        public CardState State { get; set; }
    }
}
