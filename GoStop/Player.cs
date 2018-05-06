using GoStop.Card;
using GoStop.Collection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoStop
{
    public class Player : IHanafudaPlayer
    {

        private IBoard _board;
        private CardCollection hand;
        private List<SpecialCards> specials;

        public Player(IBoard board)
        {
            _board = board;
            hand = new CardCollection();
        }

        #region Interface Methods

        public void JoinGame(IBoard board)
        {
            _board.SubscribePlayer(this);
        }

        public void ExitGame(IBoard board)
        {
            _board.UnsubscribePlayer(this);
        }

        public virtual void OnCardWon(List<Hanafuda> wonCards)
        {
            foreach (SpecialCards collection in specials)
                collection.OnCardWon(wonCards);
        }

        public void TakeTurn()
        { }

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

        #endregion

        protected virtual void PlayCard(Hanafuda card)
        {
            if (card == null || !hand.Remove(card))
                return;
            CardPlayedEventArgs args = new CardPlayedEventArgs();
            args.Card = card;
            if (!hand)
                OnHandEmpty(EventArgs.Empty);
            OnCardPlayed(args);
        }


        #region Event

        public event EventHandler<CardPlayedEventArgs> CardPlayed;
        public event EventHandler<EventArgs> HandEmpty;

        protected virtual void OnCardPlayed(CardPlayedEventArgs args)
        {
            CardPlayed?.Invoke(this, args);
        }

        protected virtual void OnHandEmpty(EventArgs args)
        {
            HandEmpty?.Invoke(this, args);
        }

        #endregion
    }

    public class CardPlayedEventArgs : EventArgs
    {
        public Hanafuda Card { get; set; }
    }
}
