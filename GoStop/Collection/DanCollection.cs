using System.Collections.Generic;

namespace GoStop.Collection
{
    public abstract class DanCollection : SpecialCards
    {
        protected DanCollection(Player owner) : base(owner,10)
        {
            cards = new List<Card.Hanafuda>(3);
        }
    }
}
