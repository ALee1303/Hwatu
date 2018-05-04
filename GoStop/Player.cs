using GoStop.Card;
using GoStop.Collection;
using System;
using System.Collections.Generic;

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
        
        protected virtual void SubscribeSpecialEmptyEvent()
        {
            foreach (SpecialCards collection in specials)
                collection.CollectionEmpty += _board.player_SpecialEmpty;
        }

        public virtual void PlayCard()
        {

        }

        public event EventHandler<CardPlayedEventArgs> CardPlayed;
        public event EventHandler<HandEmptyEventArgs> HandEmpty;

        protected virtual void OnCardPlayed(CardPlayedEventArgs args)
        {
            if (CardPlayed != null && args != null && args.Player == this)
                CardPlayed(this, args);
        }

        protected virtual void OnHandEmpty(HandEmptyEventArgs args)
        {
            if (HandEmpty != null && args != null && args.Player == this)
                HandEmpty(this, args);
        }

    }

    public class CardPlayedEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public Hanafuda Card { get; set; }
    }

    public class HandEmptyEventArgs : EventArgs
    {
        public Player Player { get; set; }
    }
}
