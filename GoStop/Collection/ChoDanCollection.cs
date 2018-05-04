using GoStop.Card;

namespace GoStop.Collection
{
    public class ChoDanCollection : DanCollection
    {
        public ChoDanCollection(Player owner) : base(owner)
        {
            cards.Add(new ChoDan(Month.April));
            cards.Add(new ChoDan(Month.May));
            cards.Add(new ChoDan(Month.July));
        }
    }
}
