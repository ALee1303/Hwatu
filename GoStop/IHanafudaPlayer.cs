using System;
using System.Collections.Generic;

using GoStop.Card;
using GoStop.MonoGameComponents;
using GoStop.MonoGameComponents.Drawables;

namespace GoStop
{
    public interface IHanafudaPlayer
    {
        void PlayCard(DrawableCard card);
        void PlayCard(List<DrawableCard> hand);
        void ChooseCard(List<DrawableCard> choice);
        void JoinBoard(BoardManager manager);
        void ExitBoard(BoardManager manager);
    }
}
