

namespace Hwatu.Collection
{
    public abstract class YakCollection : SpecialCards
    {
        protected YakCollection(IHanafudaPlayer owner): base(owner, 20)
        {
        }
    }
}
