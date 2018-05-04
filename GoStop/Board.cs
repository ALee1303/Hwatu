using System;
using System.Collections.Generic;

using GoStop.Card;
using GoStop.Collection;

namespace GoStop
{
    public class Board
    {
        protected DeckCollection deck;
        protected List<Player> players;
        protected Dictionary<Player, int> scoreBoard;
        protected Dictionary<Player, PlayerCollection> collected;

        public Board()
        {
            deck = new DeckCollection();
            players = new List<Player>();
            scoreBoard = new Dictionary<Player, int>();
            collected = new Dictionary<Player, PlayerCollection>();
        }

        protected virtual void RegisterPlayer(Player player)
        {
            if (players.Contains(player))
                return;
            players.Add(player);
            scoreBoard.Add(player, 0);
            collected.Add(player, new PlayerCollection());
        }

        public virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
            var player = (Player)sender;

        }

        public virtual void player_SpecialEmpty(object sender, SpecialEmptyEventArgs arg)
        { }

        public virtual void player_HandEmpty(object sender, HandEmptyEventArgs args)
        { }
    }
}
