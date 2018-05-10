using System;
using System.Collections.Generic;

using GoStop.Card;
using GoStop.MonoGameComponents;
using GoStop.MonoGameComponents.Drawables;

namespace GoStop
{
    public interface IHanafudaPlayer
    {
        Collection.CardCollection Hand { get; }

        void PlayCard(List<DrawableCard> hand);
        void JoinBoard(BoardManager manager);
        void ExitBoard(BoardManager manager);
        void RenewHandAndSpecial();
        void PrepareSpecialCollection();
        void CardsCollected(List<Hanafuda> wonCards);
        void SubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler);
        void UnsubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler);
    }
}
