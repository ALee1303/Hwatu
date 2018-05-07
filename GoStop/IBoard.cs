

namespace GoStop
{
    public interface IBoard
    {
        IHanafudaPlayer CurrentPlayer { get; }
        int PlayingCount { get; }
        Collection.DeckCollection Deck { get; }

        void StartGame();
        void EndGame();

        void SubscribePlayer(IHanafudaPlayer player);
        void UnsubscribePlayer(IHanafudaPlayer player);
    }
}
