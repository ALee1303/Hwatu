using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStop.Collection
{
    public class PlayerCollection : CardCollection
    {
        public PlayerCollection()
        { }

        public EventHandler<CardCollectedEventArgs> CardCollected;

        protected virtual void OnCardCollected(CardCollectedEventArgs args)
        { }
    }

    public class CardCollectedEventArgs : EventArgs
    {

    }
}
