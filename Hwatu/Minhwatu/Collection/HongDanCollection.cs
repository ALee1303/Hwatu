using Hwatu.Card;

namespace Hwatu.Collection
{
    public class HongDanCollection : DanCollection
    {
        public HongDanCollection(IHanafudaPlayer owner) : base(owner)
        {
            cards.Add(new HongDan(Month.January));
            cards.Add(new HongDan(Month.February));
            cards.Add(new HongDan(Month.March));
            cards.TrimExcess();
        }
    }
}
