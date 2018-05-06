using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Collection
{
    public class CollectedCards : CardCollection
    {
        public CollectedCards() : base()
        { }

        public EventHandler<CardCollectedEventArgs> CardCollected;

        protected virtual void OnCardCollected(CardCollectedEventArgs args)
        {
            CardCollected?.Invoke(this, args);
        }
    }

    public class CardCollectedEventArgs : EventArgs
    {

    }
}
