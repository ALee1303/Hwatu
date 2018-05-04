using GoStop.Card;
using GoStop.Collection;
using System;
using System.Collections.Generic;

namespace GoStop
{
    public class Player
    {
        private Board _board;
        private CardCollection hand, collected;

        private List<SpecialCards> specials;

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

    }

}
