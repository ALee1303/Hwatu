
using GoStop.Card;
using GoStop.Collection;
using System.Collections.Generic;

namespace GoStop
{
    public interface IBoard
    {
        IHanafudaPlayer CurrentPlayer { get; }
        int PlayingCount { get; set; }

        void StartGame();
        void EndGame();
        void ResetBoard();
        void CalculatePoint(Hanafuda card);
        void CalculatePoint(List<Hanafuda> cards);

        void EndTurn();

        List<SpecialCards> PrepareSpecialCollection(IHanafudaPlayer player);

        void SubscribePlayer(IHanafudaPlayer player);
        void UnsubscribePlayer(IHanafudaPlayer player);

        bool IsNewPlayer(IHanafudaPlayer player);
    }
}
