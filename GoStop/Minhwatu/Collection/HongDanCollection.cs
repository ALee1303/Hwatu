using GoStop.Card;

namespace GoStop.Collection
{
    public class HongDanCollection : DanCollection
    {
        public HongDanCollection(Player owner) : base(owner)
        {
            cards.Add(new HongDan(Month.January));
            cards.Add(new HongDan(Month.February));
            cards.Add(new HongDan(Month.March));
            cards.TrimExcess();
        }
    }
}
