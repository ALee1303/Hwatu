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
        void CardWon(Stack<Hanafuda> wonCards);
    }
}
