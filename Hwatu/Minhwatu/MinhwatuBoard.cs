using System.Collections.Generic;

using Hwatu.MonoGameComponents;
using Hwatu.Collection;
using Hwatu.Card;

namespace Hwatu.Minhwatu
{
    public class MinhwatuBoard : Board
    {
        public MinhwatuBoard(BoardManager manager) : base(manager)
        { }

        public override List<SpecialCards> PrepareSpecialCollection(IHanafudaPlayer player)
        {
            List<SpecialCards> specials = new List<SpecialCards>
            {
                new ChoYak(player), new PoongYak(player), new BiYak(player),
                new ChoDanCollection(player), new ChungDanCollection(player), new HongDanCollection(player)
            };
            return specials;
        }


        public override int CalculatePoint(IHanafudaPlayer owner, Hanafuda card)
        {
            CardType type = card.Type;
            int calculatedPnt = 0;
            if (type == CardType.Tti)
                calculatedPnt = 5;
            if (type == CardType.Yul)
                calculatedPnt = 10;
            if (type == CardType.Kwang)
                calculatedPnt = 20;
            return calculatedPnt;
        }

        public override void ResetBoard()
        {

        }

        protected override void PostGame()
        {

        }

        protected override void manager_HandEmpty()
        {
            RemovePlayer(CurrentPlayer);
        }
    }
}
