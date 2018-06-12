using Hwatu.Card;
using Hwatu.Collection;
using System.Collections.Generic;

namespace Hwatu
{
    public interface IBoard
    {
        List<IHanafudaPlayer> PlayerWaitList { get; }
        IHanafudaPlayer CurrentPlayer { get; }
        int PlayingCount { get;}        

        void StartGame();
        void EndGame();
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
