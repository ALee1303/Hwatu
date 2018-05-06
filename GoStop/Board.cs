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
        protected Dictionary<IHanafudaPlayer, CollectedCards> collected;
        protected Dictionary<IHanafudaPlayer, int> scoreBoard;
        //players
        protected IHanafudaPlayer currentPlayer;
        protected List<IHanafudaPlayer> playerWaitList;
        protected Queue<IHanafudaPlayer> orderedPlayers;

        public Board()
        {
            deck = new DeckCollection();
            field = new FieldCollection();
            field.MultipleMatchFound += field_MultipleMatchFound;
            field.CardsPaired += field_CardsPaired;
            scoreBoard = new Dictionary<IHanafudaPlayer, int>();
            collected = new Dictionary<IHanafudaPlayer, CollectedCards>();
            orderedPlayers = new Queue<IHanafudaPlayer>();
        }

        public virtual void JoinGame(IHanafudaPlayer player)
        {
            if (!IsNewPlayer(player) || playerWaitList.Remove(player))
                new ArgumentException("Player Already Exist");
            playerWaitList.Add(player);
        }

        public virtual void StartGame()
        {
            foreach (IHanafudaPlayer player in playerWaitList)
                SubscribePlayer(player);
        }

        public virtual void SubscribePlayer(IHanafudaPlayer player)
        {
            if (!IsNewPlayer(player))
                return;
            //remove player from wait list if it exist
            playerWaitList.Remove(player);
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

        public virtual void UnsubscribePlayer(IHanafudaPlayer player)
        {
            if (IsNewPlayer(player))
                return; // if never subscribed
            if (currentPlayer == player)
                currentPlayer = null;
            else
            {
                Queue<IHanafudaPlayer> q = new Queue<IHanafudaPlayer>();
                while (orderedPlayers.Count > 0)
                {
                    IHanafudaPlayer temp = orderedPlayers.Dequeue();
                    if (temp == player)
                        continue;
                    q.Enqueue(temp);
                }
                orderedPlayers = q;
            }
            scoreBoard.Remove(player);
            collected.Remove(player);
            //event
            player.UnsubscribeSpecialEmptyEvent(collection_SpecialEmpty);
            var p = (Player)player;
            if (p != null)
            {
                p.CardPlayed -= player_CardPlayed;
                p.HandEmpty -= player_HandEmpty;
            }
        }

        protected virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
            var player = (Player)sender;
            if (player != currentPlayer)
                new ArgumentException("Player playing out of turn");

        }

        protected virtual void player_HandEmpty(object sender, EventArgs args)
        { }

        protected virtual void field_MultipleMatchFound(object sender, FieldEventArgs args)
        { }

        protected virtual void field_CardsPaired(object sender, FieldEventArgs args)
        { }

        protected virtual void collection_SpecialEmpty(object sender, EventArgs arg)
        { }

        protected bool IsNewPlayer(IHanafudaPlayer player)
        {
            return currentPlayer != player
                && !orderedPlayers.Contains(player);
        }
    }
}
