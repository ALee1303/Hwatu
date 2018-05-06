using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoStop.Card;

namespace GoStop
{
    public interface IHanafudaPlayer
    {
        void TakeTurn();
        void JoinGame(IBoard board);
        void ExitGame(IBoard board);
        void CardWon(List<Hanafuda> wonCards);
        void SubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler);
        void UnsubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler);
    }
}
