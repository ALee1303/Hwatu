using System;
using System.Collections.Generic;

using GoStop.MonoGameComponents;
using GoStop.Collection;
using GoStop.Card;

namespace GoStop.Minhwatu
{
    public class MinhwatuBoard : Board
    {
        public MinhwatuBoard(BoardManager manager) : base(manager)
        {
        }

        public override List<SpecialCards> PrepareSpecialCollection(IHanafudaPlayer player)
        {
            List<SpecialCards> specials = new List<SpecialCards>
            {
                new ChoYak(player), new PoongYak(player), new BiYak(player),
                new ChoDanCollection(player), new ChungDanCollection(player), new HongDanCollection(player)
            };
            foreach (SpecialCards special in specials)
                special.CollectionEmpty += special_CollectionEmpty;
            return specials;
        }


        public override void CalculatePoint(IHanafudaPlayer owner, Hanafuda card)
        {
            CardType type = card.Type;
            int calculatedPnt = 0;
            if (type == CardType.Tti)
                calculatedPnt = 5;
            if (type == CardType.Yul)
                calculatedPnt = 10;
            if (type == CardType.Kwang)
                calculatedPnt = 20;
            scoreBoard[owner] += calculatedPnt;
        }


        protected override void PostGame()
        {

        }

        protected override void manager_HandEmpty()
        {
            RemovePlayer(CurrentPlayer);
        }

        protected override void special_CollectionEmpty(object sender, EventArgs args)
        {
            Type type = sender.GetType();
            SpecialEmptyEventArgs specialArg = (SpecialEmptyEventArgs)args;
            if (!(type.IsInstanceOfType(typeof(SpecialCards)) || specialArg == null))
                new ArgumentException("Not a Special Collection");
            IHanafudaPlayer owner = specialArg.Owner;
            int point = specialArg.Points;
            scoreBoard[owner] += point;
            specialPoints[owner] += point;
        }
    }
}
