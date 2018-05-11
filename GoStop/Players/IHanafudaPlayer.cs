using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GoStop.Card;
using GoStop.MonoGameComponents;
using GoStop.MonoGameComponents.Drawables;

namespace GoStop
{
    public interface IHanafudaPlayer
    {
        void PlayCard(List<DrawableCard> hand);
        Task<DrawableCard> ChooseCard(List<DrawableCard> choice);
        void JoinBoard(BoardManager manager);
        void ExitBoard(BoardManager manager);
    }
}
