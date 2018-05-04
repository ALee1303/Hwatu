using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GoStop.Card;
using GoStop.Collection;

namespace GoStop
{
    public class Board
    {
        private DeckCollection deck;
        private List<Player> players;
        private Dictionary<Player, int> ScoreBoard;
        private Dictionary<Player, int> MinhwatuPoints;

        public Board()
        {
            deck = new DeckCollection();
            players = new List<Player>();
            ScoreBoard = new Dictionary<Player, int>();
            MinhwatuPoints = new Dictionary<Player, int>();
        }

        public virtual void player_SpecialEmpty(object sender, EventArgs arg)
        {
            SpecialEmptyEventArgs specialArg = (SpecialEmptyEventArgs)arg;
            if (specialArg == null)
                return;
            Player player = specialArg.Owner;
            ScoreBoard[player] = specialArg.Points;
            MinhwatuPoints[player] = specialArg.Points;
        }
    }
}
