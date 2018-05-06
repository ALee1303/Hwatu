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
        protected FieldCollection field;
        protected Dictionary<IHanafudaPlayer, CardCollection> collected;
        protected Dictionary<IHanafudaPlayer, int> scoreBoard;
        //players
        protected Player currentPlayer;
        protected List<IHanafudaPlayer> playerWaitedList;
        protected Queue<IHanafudaPlayer> orderedPlayers;

        public Board()
        {
            deck = new DeckCollection();
            field = new FieldCollection();
            field.MatchFound += field_MatchFound;
            field.CardsPaired += field_CardsPaired;
            scoreBoard = new Dictionary<IHanafudaPlayer, int>();
            collected = new Dictionary<IHanafudaPlayer, CardCollection>();
            currentPlayer = new Player(this);
            orderedPlayers = new Queue<IHanafudaPlayer>();
        }

        public virtual void RegisterPlayer(IHanafudaPlayer player)
        {
            if (!IsNewPlayer(player))
                return;
            playerWaitedList.Add(player);
            //add scoreBoard
            scoreBoard.Add(player, 0);
            collected.Add(player, new CollectedCards());
            //event
            player.SubscribeSpecialEmptyEvent(collection_SpecialEmpty);
            var p = (Player)player;
            if (p != null)
            {
                p.CardPlayed += player_CardPlayed;
                p.HandEmpty += player_HandEmpty;
            }
        }

        protected virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
            var player = (Player)sender;
            if (player == currentPlayer)
            { }

        }

        protected virtual void player_HandEmpty(object sender, EventArgs args)
        { }

        protected virtual void field_MatchFound(object sender, FieldEventArgs args)
        { }

        protected virtual void field_CardsPaired(object sender, FieldEventArgs args)
        { }

        protected virtual void collection_SpecialEmpty(object sender, EventArgs arg)
        { }

        protected bool IsNewPlayer(IHanafudaPlayer player)
        {
            return currentPlayer != player
                && !orderedPlayers.Contains(player)
                && !playerWaitedList.Contains(player);
        }
    }
}
