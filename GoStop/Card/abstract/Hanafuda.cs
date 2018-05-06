using System;

namespace GoStop.Card
{
    public enum Month
    {
        January, February, March, April,
        May, June, July, August, September,
        October, November, December
    }
    public enum CardType { Pi, Tti, Yul, Kwang }
    public enum Location { Deck, Field, Hand, Collected }

    // TODO: add IHanafuda interface
    public abstract class Hanafuda : IEquatable<Hanafuda>
    {
        #region Fields
        private Month month;
        private CardType type;
        private Location location;
        private bool hidden;
        private IHanafudaPlayer owner;
        #endregion

        protected Hanafuda(Month _month, CardType _type)
        {
            month = _month;
            type = _type;
            location = Location.Deck;
            owner = null;
        }

        #region Properties
        public Month Month { get => month; }
        public CardType Type { get => type; }
        public Location State
        {
            get => location;

            set
            {
                if (location == value)
                    return;
                location = value;
                HanafudaEventArgs args = InitializeArgs();
                OnLocationChanged(args);
            }
        }
        public bool Hidden
        {
            get => hidden;

            set
            {
                if (hidden == value)
                    return;
                hidden = value;
                HanafudaEventArgs args = InitializeArgs();
                OnHiddenChanged(args);
            }
        }
        public IHanafudaPlayer Owner
        {
            get => owner;

            set
            {
                if (owner == value)
                    return;
                owner = value;
                HanafudaEventArgs args = InitializeArgs();
                OnOwnerChanged(args);
            }
        }
        #endregion

        private HanafudaEventArgs InitializeArgs()
        {
            HanafudaEventArgs args = new HanafudaEventArgs();
            args.Owner = owner;
            args.Location = location;
            args.Hidden = hidden;
            return args;
        }

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
        public event EventHandler<HanafudaEventArgs> HiddenChanged;
        public event EventHandler<HanafudaEventArgs> LocationChanged;

        protected virtual void OnOwnerChanged(HanafudaEventArgs args)
        {
            OwnerChanged?.Invoke(this, args);
        }

        protected virtual void OnHiddenChanged(HanafudaEventArgs args)
        {
            HiddenChanged?.Invoke(this, args);
        }

        protected virtual void OnLocationChanged(HanafudaEventArgs args)
        {
            LocationChanged?.Invoke(this, args);
        }

        #endregion
    }

    public class HanafudaEventArgs : EventArgs
    {
        public IHanafudaPlayer Owner { get; set; }
        public bool Hidden { get; set; }
        public Location Location { get; set; }
    }
}
