using GoStop.Card;

namespace GoStop.Collection
{
    public class ChoYak : YakCollection
    {
        public ChoYak(Player owner) : base(owner)
        {
            cards.Add(new Pi(Month.May));
            cards.Add(new Pi(Month.May));
            cards.Add(new ChoDan(Month.May));
            cards.Add(new Yul(Month.May));
        }
    }
}
