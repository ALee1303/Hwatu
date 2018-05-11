using GoStop.Card;

namespace GoStop.Collection
{
    public class ChoYak : YakCollection
    {
        public ChoYak(IHanafudaPlayer owner) : base(owner)
        {
            cards.Add(new Pi(Month.May, 1));
            cards.Add(new Pi(Month.May, 2));
            cards.Add(new ChoDan(Month.May));
            cards.Add(new Yul(Month.May));
            cards.TrimExcess();
        }
    }
}
