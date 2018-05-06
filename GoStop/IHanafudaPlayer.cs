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
        void CardWon(List<Hanafuda> wonCards);
        void TakeTurn();
        void SubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler);
        void UnsubscribeSpecialEmptyEvent(EventHandler<EventArgs> handler);
    }
}
