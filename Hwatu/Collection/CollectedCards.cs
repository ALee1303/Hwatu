using System;

namespace Hwatu.Collection
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
