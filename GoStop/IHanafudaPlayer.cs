using System;
using System.Collections.Generic;

using GoStop.Card;

namespace GoStop
{
    public interface IHanafudaPlayer
    {
        Collection.CardCollection Hand { get; }

        void TakeTurn();
        void JoinGame(IBoard board);
        void ExitGame(IBoard board);
        void RenewHandAndSpecial();
        void PrepareSpecialCollection();
        void CardsCollected(List<Hanafuda> wonCards);
        void SubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler);
        void UnsubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler);
    }
}
