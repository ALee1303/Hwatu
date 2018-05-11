using System.Collections.Generic;

namespace GoStop.Collection
{
    public abstract class DanCollection : SpecialCards
    {
        protected DanCollection(IHanafudaPlayer owner) : base(owner, 30)
        {
        }
    }
}
