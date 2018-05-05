using GoStop.Card;

namespace GoStop.Collection
{
    public class ChungDanCollection : DanCollection
    {
        public ChungDanCollection(Player owner) : base(owner)
        {
            cards.Add(new ChungDan(Month.June));
            cards.Add(new ChungDan(Month.September));
            cards.Add(new ChungDan(Month.October));
        }
    }
}
