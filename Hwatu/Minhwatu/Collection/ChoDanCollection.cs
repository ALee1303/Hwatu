using Hwatu.Card;

namespace Hwatu.Collection
{
    public class ChoDanCollection : DanCollection
    {
        public ChoDanCollection(IHanafudaPlayer owner) : base(owner)
        {
            cards.Add(new ChoDan(Month.April));
            cards.Add(new ChoDan(Month.May));
            cards.Add(new ChoDan(Month.July));
            cards.TrimExcess();
        }
    }
}
