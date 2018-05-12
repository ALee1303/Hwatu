
using GoStop.Card;
using GoStop.Collection;
using System.Collections.Generic;

using GoStop.MonoGameComponents.Drawables;

namespace GoStop
{
    public interface IBoard
    {
        IHanafudaPlayer CurrentPlayer { get; }
        int PlayingCount { get;}        

        void StartGame();
        void EndGame();
        void AddSpecialPoint(IHanafudaPlayer player, int point);
        int CalculatePoint(IHanafudaPlayer owner, Hanafuda card);
        void CalculatePoint(IHanafudaPlayer owner, List<Hanafuda> cards);

        void EndTurn();
        void ResetBoard();

        List<SpecialCards> PrepareSpecialCollection(IHanafudaPlayer player);

        void SubscribePlayer(IHanafudaPlayer player);
        void UnsubscribePlayer(IHanafudaPlayer player);

        bool IsNewPlayer(IHanafudaPlayer player);
    }
}
