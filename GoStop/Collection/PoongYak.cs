using GoStop.Card;

namespace GoStop.Collection
{
    public class PoongYak : YakCollection
    {
        public PoongYak(Player owner) : base(owner)
        {
            cards.Add(new Pi(Month.October));
            cards.Add(new Pi(Month.October));
            cards.Add(new ChungDan(Month.October));
            cards.Add(new Yul(Month.October));
        }
    }
}
