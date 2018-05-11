using System;
using System.Collections.Generic;

using GoStop.MonoGameComponents;
using GoStop.Collection;

namespace GoStop.Minhwatu
{
    public class MinhwatuBoard : Board
    {
        private Dictionary<Type, Player> minhwatuPoints;

        public MinhwatuBoard(BoardManager manager) : base(manager)
        {
            minhwatuPoints = new Dictionary<Type, Player>(6);
            minhwatuPoints.Add(typeof(ChoYak), null);
            minhwatuPoints.Add(typeof(PoongYak), null);
            minhwatuPoints.Add(typeof(BiYak), null);
            minhwatuPoints.Add(typeof(ChoDanCollection), null);
            minhwatuPoints.Add(typeof(ChungDanCollection), null);
            minhwatuPoints.Add(typeof(HongDanCollection), null);
        }

        public override List<SpecialCards> PrepareSpecialCollection(IHanafudaPlayer player)
        {
            List<SpecialCards> specials = new List<SpecialCards>
            {
                new ChoYak(player), new PoongYak(player), new BiYak(player),
                new ChoDanCollection(player), new ChungDanCollection(player), new HongDanCollection(player)
            };
            foreach (SpecialCards special in specials)
                special.CollectionEmpty += collection_SpecialEmpty;
            return specials;
        }

        protected override void player_HandEmpty(object sender, EventArgs args)
        {
            var player = (Player)sender;
            if (player == null || player != currentPlayer)
                new ArgumentException("Invalid player HandEmpty");
            RemovePlayer(currentPlayer);
        }

        protected override void collection_SpecialEmpty(object sender, EventArgs args)
        {
            Type type = sender.GetType();
            SpecialEmptyEventArgs specialArg = (SpecialEmptyEventArgs)args;
            if (!(type.IsInstanceOfType(typeof(SpecialCards)) || specialArg == null))
                new ArgumentException("Not a Special Collection");
            _manager.UpdateScore(specialArg.Points, CurrentPlayer);
        }
    }
}
