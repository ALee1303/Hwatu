using GoStop.Card;

namespace GoStop.Collection
{
    public class PoongYak : YakCollection
    {
        public PoongYak(IHanafudaPlayer owner) : base(owner)
        {
            cards.Add(new Pi(Month.October, 1));
            cards.Add(new Pi(Month.October, 2));
            cards.Add(new ChungDan(Month.October));
            cards.Add(new Yul(Month.October));
            cards.TrimExcess();
        }
    }
}
