using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoStop.Card;

namespace GoStop.Collection
{
    public class FieldCollection : CardCollection
    {
        public FieldCollection() : base()
        { }



        public event EventHandler<FieldEventArgs> MatchFound;
        public event EventHandler<FieldEventArgs> CardsPaired;

        protected virtual void OnMatchFound(FieldEventArgs args)
        {
            MatchFound?.Invoke(this, args);
        }

        protected virtual void OnCardsPaired(FieldEventArgs args)
        {
            CardsPaired?.Invoke(this, args);
        }
    }

    public class FieldEventArgs : EventArgs
    {
        public List<Hanafuda> wonCards { get; set; }
    }
}
