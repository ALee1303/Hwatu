

namespace GoStop
{
    public interface IBoard
    {
        IHanafudaPlayer CurrentPlayer { get; }
        int PlayingCount { get; }

        void StartGame();
        void EndGame();

        void SubscribePlayer(IHanafudaPlayer player);
        void UnsubscribePlayer(IHanafudaPlayer player);

        bool IsNewPlayer(IHanafudaPlayer player);
    }
}
