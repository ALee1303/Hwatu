using Hwatu.Card;
using Hwatu.Collection;
using System.Collections.Generic;

namespace Hwatu
{
    public interface IBoard
    {
        int CalculatePoint(IHanafudaPlayer owner, Hanafuda card);
        void CalculatePoint(IHanafudaPlayer owner, List<Hanafuda> cards);
        
        List<SpecialCards> GetSpecialCollection(IHanafudaPlayer player);
        void DealCard();
    }
}
