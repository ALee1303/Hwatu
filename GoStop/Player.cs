using GoStop.Card;
using GoStop.Collection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoStop
{
    public class Player
    {
        private Board _board;
        private CardCollection hand;
        private List<SpecialCards> specials;

        public Player(Board board)
        {

        }

        #region Observers
        public virtual void CardWon(List<Hanafuda> won)
        {
            foreach (SpecialCards collection in specials)
                collection.OnCardWon(won);
        }
        #endregion

        public virtual Task TakeTurnAsync()
        {
            return null;
        }

        protected virtual void PlayCard(Hanafuda card)
        {
            if (card != null && hand.Remove(card))
            {
                CardPlayedEventArgs args = new CardPlayedEventArgs();
                args.Card = card;
                OnCardPlayed(args);
            }
            if (!hand)
                OnHandEmpty(null);
        }

        public event EventHandler<CardPlayedEventArgs> CardPlayed;
        public event EventHandler<EventArgs> HandEmpty;

        protected virtual void OnCardPlayed(CardPlayedEventArgs args)
        {
            if (args.Card != null)
                CardPlayed?.Invoke(this, args);
        }

        protected virtual void OnHandEmpty(EventArgs args)
        {
            HandEmpty?.Invoke(this, args);
        }

        /// <summary>
        /// Subscribe board to all the special collections
        /// </summary>
        /// <param name="handler"></param>
        public virtual void SubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler)
        {
            foreach (SpecialCards collection in specials)
                collection.CollectionEmpty += handler;
        }
        /// <summary>
        /// Unsubscribe board to all the special collections
        /// </summary>
        /// <param name="handler"></param>
        public virtual void UnsubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler)
        {
            foreach (SpecialCards collection in specials)
                collection.CollectionEmpty -= handler;
        }
    }

    public class CardPlayedEventArgs : EventArgs
    {
        public Hanafuda Card { get; set; }
    }
}
