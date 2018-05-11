using System.Collections.Generic;

namespace GoStop.Collection
{
    public abstract class YakCollection : SpecialCards
    {
        protected YakCollection(IHanafudaPlayer owner): base(owner, 20)
        {
        }
    }
}
