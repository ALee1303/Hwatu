using GoStop.Card;

namespace GoStop.Collection
{
    public class BiYak : YakCollection
    {
        public BiYak(Player owner) : base(owner)
        {
            cards.Add(new SsangPi(Month.December));
            cards.Add(new ChoDan(Month.December));
            cards.Add(new Yul(Month.December));
            cards.Add(new Kwang(Month.December));
            cards.TrimExcess();
        }
    }
}
