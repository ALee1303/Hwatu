using System;
using System.Collections.Generic;

using GoStop.Card;
using GoStop.Collection;

namespace GoStop.Minhwatu
{
    public class MinhwatuBoard : Board
    {
        private Dictionary<Type, Player> minhwatuPoints;

        public MinhwatuBoard() : base()
        {
            minhwatuPoints = new Dictionary<Type, Player>(6);
            minhwatuPoints.Add(typeof(ChoYak), null);
            minhwatuPoints.Add(typeof(PoongYak), null);
            minhwatuPoints.Add(typeof(BiYak), null);
            minhwatuPoints.Add(typeof(ChoDanCollection), null);
            minhwatuPoints.Add(typeof(ChungDanCollection), null);
            minhwatuPoints.Add(typeof(HongDanCollection), null);
        }

        protected override void player_HandEmpty(object sender, EventArgs args)
        {
            var player = (Player)sender;
            if (player == null || player != currentPlayer)
                new ArgumentException("Invalid player HandEmpty");
            RemovePlayer(currentPlayer);
        }

        protected override void collection_SpecialEmpty(object sender, EventArgs arg)
        {
            Type type = typeof(SpecialCards);
            SpecialEmptyEventArgs specialArg = (SpecialEmptyEventArgs)arg;
            if (!(type.IsInstanceOfType(sender) || specialArg == null))
                return;
            Player player = specialArg.Owner;
            scoreBoard[player] = specialArg.Points;
            minhwatuPoints[sender.GetType()] = specialArg.Owner;
        }
    }
}
