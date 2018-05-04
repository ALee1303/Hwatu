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
        protected List<Player> finishedPlayers;
        protected Dictionary<Player, int> scoreBoard;
        protected Dictionary<Player, PlayerCollection> collected;

        public Board()
        {
            deck = new DeckCollection();
            players = new List<Player>();
            finishedPlayers = new List<Player>();
            scoreBoard = new Dictionary<Player, int>();
            collected = new Dictionary<Player, PlayerCollection>();
        }

        public virtual void RegisterPlayer(Player player)
        {
            if (players.Contains(player))
                return;
            players.Add(player);
            player.SubscribeSpecialEmptyEvent(collection_SpecialEmpty);
            scoreBoard.Add(player, 0);
            collected.Add(player, new PlayerCollection());
        }

        protected virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
            var player = (Player)sender;

        }

        protected virtual void player_HandEmpty(object sender, EventArgs args)
        {
            var player = (Player)sender;
            if (player == null || !players.Remove(player))
                return;
            finishedPlayers.Add(player);
            player.UnsubscribeSpecialEmptyEvent(collection_SpecialEmpty);
        }

        protected virtual void collection_SpecialEmpty(object sender, EventArgs arg)
        { }

    }
}
