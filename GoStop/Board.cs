using System;
using System.Collections.Generic;

using GoStop.Card;
using GoStop.Collection;

namespace GoStop
{
    public class Board
    {
        //cards
        protected DeckCollection deck;
        protected CardCollection field;
        protected Dictionary<Player, PlayerCollection> collected;
        protected Dictionary<Player, int> scoreBoard;
        //players
        protected Player currentPlayer;
        protected Queue<Player> orderedPlayers;
        protected List<Player> finishedPlayers;

        public Board()
        {
            deck = new DeckCollection();
            orderedPlayers = new Queue<Player>();
            finishedPlayers = new List<Player>();
            scoreBoard = new Dictionary<Player, int>();
            collected = new Dictionary<Player, PlayerCollection>();
        }

        public virtual void RegisterPlayer(Player player)
        {
            if (orderedPlayers.Contains(player))
                return;
            orderedPlayers.Enqueue(player);
            player.SubscribeSpecialEmptyEvent(collection_SpecialEmpty);
            scoreBoard.Add(player, 0);
            collected.Add(player, new PlayerCollection());
        }

        protected virtual void CheckAvailableCard(Hanafuda card)
        {

        }

        protected virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
            var player = (Player)sender;
            if (player == currentPlayer)

        }

        protected virtual void player_HandEmpty(object sender, EventArgs args)
        {
            var player = (Player)sender;
            if (player == null || player != currentPlayer)
                return;
            finishedPlayers.Add(currentPlayer);
            player.UnsubscribeSpecialEmptyEvent(collection_SpecialEmpty);
        }

        protected virtual void collection_SpecialEmpty(object sender, EventArgs arg)
        { }

    }
}
