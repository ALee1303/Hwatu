using Hwatu.Card;

namespace Hwatu.Collection
{
    public class ChungDanCollection : DanCollection
    {
        public ChungDanCollection(IHanafudaPlayer owner) : base(owner)
        {
            cards.Add(new ChungDan(Month.June));
            cards.Add(new ChungDan(Month.September));
            cards.Add(new ChungDan(Month.October));
            cards.TrimExcess();
        }
    }
}
