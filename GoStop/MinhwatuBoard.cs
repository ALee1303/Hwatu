using System;
using System.Collections.Generic;

using GoStop.Card;
using GoStop.Collection;

namespace GoStop
{
    public class MinhwatuBoard : Board
    {
        private int finishedPlayers;
        private Dictionary<Type, Player> minhwatuPoints;

        public MinhwatuBoard() : base()
        {
            finishedPlayers = 0;
            minhwatuPoints = new Dictionary<Type, Player>(7);
            minhwatuPoints.Add(typeof(ChoYak), null);
            minhwatuPoints.Add(typeof(PoongYak), null);
            minhwatuPoints.Add(typeof(BiYak), null);
            minhwatuPoints.Add(typeof(ChoDanCollection), null);
            minhwatuPoints.Add(typeof(ChungDanCollection), null);
            minhwatuPoints.Add(typeof(HongDanCollection), null);
        }

        public override void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
            base.player_CardPlayed(sender, args);
        }

        public override void player_SpecialEmpty(object sender, EventArgs arg)
        {
            Type type = typeof(SpecialCards);
            SpecialEmptyEventArgs specialArg = (SpecialEmptyEventArgs)arg;
            if (!(type.IsInstanceOfType(sender) || specialArg == null))
                return;
            Player player = specialArg.Owner;
            scoreBoard[player] = specialArg.Points;
            minhwatuPoints[type] = specialArg.Owner;
        }

        public override void player_HandEmpty(object sender, HandEmptyEventArgs args)
        {
            base.player_HandEmpty(sender, args);
        }
    }
}
