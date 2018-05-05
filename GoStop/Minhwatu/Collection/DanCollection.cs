using System.Collections.Generic;

namespace GoStop.Collection
{
    public abstract class DanCollection : SpecialCards
    {
        protected DanCollection(Player owner) : base(owner, 30)
        {
            cards = new List<Card.Hanafuda>(3);
        }
    }
}
