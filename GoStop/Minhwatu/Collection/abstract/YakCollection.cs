using System.Collections.Generic;

namespace GoStop.Collection
{
    public abstract class YakCollection : SpecialCards
    {
        protected YakCollection(Player owner): base(owner, 20)
        {
        }
    }
}
