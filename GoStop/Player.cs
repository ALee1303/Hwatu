using GoStop.Card;
using GoStop.Collection;
using System;
using System.Collections.Generic;

using GoStop.MonoGameComponents;
using GoStop.MonoGameComponents.Drawables;

namespace GoStop
{
    public class Player : IHanafudaPlayer
    {
        private CardCollection hand;
        protected List<SpecialCards> specials;

        public Player()
        {
            hand = new CardCollection();
        }

        public CardCollection Hand { get => hand; }

        #region Interface Methods

        public virtual void PlayCard(List<DrawableCard> hand)
        { }

        protected virtual void PlayCard(DrawableCard card)
        {
            PlayerEventArgs args = new PlayerEventArgs();
            args.Card = card;
            //if (!hand)
            //    OnHandEmpty(EventArgs.Empty);
            OnCardPlayed(args);
        }


        public void JoinBoard(BoardManager manager)
        {
            manager.OnJoinBoard(this);
        }

        public void ExitBoard(BoardManager manager)
        {
            manager.OnExitBoard(this);
        }

        public void RenewHandAndSpecial()
        {
            hand.Clear();
            foreach (SpecialCards special in specials)
                special.Clear();
        }

        public virtual void PrepareSpecialCollection()
        { }

        public virtual void CardsCollected(List<Hanafuda> wonCards)
        {
            foreach (SpecialCards collection in specials)
                collection.OnCardsCollected(wonCards);
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

        #endregion



        #region Event

        public event EventHandler<PlayerEventArgs> CardPlayed;
        public event EventHandler<EventArgs> HandEmpty;
        public event EventHandler<PlayerEventArgs> MouseOverCard;

        protected virtual void OnMouseOver(PlayerEventArgs args)
        {
            MouseOverCard?.Invoke(this, args);
        }
        /// <summary>
        /// On Manager
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCardPlayed(PlayerEventArgs args)
        {
            CardPlayed?.Invoke(this, args);
        }

        /// <summary>
        /// On Board
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnHandEmpty(EventArgs args)
        {
            HandEmpty?.Invoke(this, args);
        }

        #endregion
    }

    public class PlayerEventArgs : EventArgs
    {
        public DrawableCard Card { get; set; }
    }
}
