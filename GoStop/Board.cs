using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using GoStop.Card;
using GoStop.Collection;
using System.Threading.Tasks;

namespace GoStop
{
    public class Board : IBoard
    {
        //cards
        protected DeckCollection deck;
        protected Dictionary<Month, CardCollection> field;
        protected Dictionary<IHanafudaPlayer, CollectedCards> collected;
        protected Dictionary<IHanafudaPlayer, int> scoreBoard;
        //players
        protected IHanafudaPlayer currentPlayer;
        protected List<IHanafudaPlayer> playerWaitList;
        protected Queue<IHanafudaPlayer> orderedPlayers;

        public int DeckCount { get => deck.Count; }

        public Board()
        {
            deck = new DeckCollection();
            field = new Dictionary<Month, CardCollection>();
            scoreBoard = new Dictionary<IHanafudaPlayer, int>();
            collected = new Dictionary<IHanafudaPlayer, CollectedCards>();
            orderedPlayers = new Queue<IHanafudaPlayer>();
        }

        // TODO:DealCard, PrepareGame, FinishGame, GameResult
        #region Protected Methods

        protected virtual void PrepareWaitingPlayers()
        {
            if (currentPlayer != null)
                new ArgumentException("Game in progress");
            foreach (IHanafudaPlayer player in playerWaitList)
                SubscribePlayer(player);
        }

        protected virtual void ResetBoard()
        {
            deck.Populate();
            field.Clear();
            collected = new Dictionary<IHanafudaPlayer, CollectedCards>();
            scoreBoard = new Dictionary<IHanafudaPlayer, int>();
            // add existing players
            foreach (IHanafudaPlayer player in orderedPlayers)
            {
                scoreBoard.Add(player, 0);
                collected.Add(player, new CollectedCards());
            }
            PrepareWaitingPlayers();
        }

        protected void DealCard()
        {

        }

        protected bool IsNewPlayer(IHanafudaPlayer player)
        {
            return currentPlayer != player
                && !orderedPlayers.Contains(player);
        }

        #endregion

        public virtual void StartGame()
        {

        }

        public virtual void EndGame()
        {

        }

        /// <summary>
        /// Add new player to waiting list
        /// </summary>
        /// <param name="player"></param>
        public virtual void AddPlayer(IHanafudaPlayer player)
        {
            if (!IsNewPlayer(player) || playerWaitList.Remove(player))
                new ArgumentException("Player Already Exist");
            playerWaitList.Add(player);
        }

        /// <summary>
        /// Remove player from the game
        /// </summary>
        /// <param name="player"></param>
        public virtual void RemovePlayer(IHanafudaPlayer player)
        {
            if (IsNewPlayer(player) || !playerWaitList.Remove(player))
                new ArgumentException("Player Already Exist");
            UnsubscribePlayer(player);
            playerWaitList.Remove(player);
        }

        #region Subscriber
        /// <summary>
        /// Subscribe waiting player to the game
        /// </summary>
        /// <param name="player"></param>
        protected virtual void SubscribePlayer(IHanafudaPlayer player)
        {
            if (!IsNewPlayer(player))
                return;
            if (currentPlayer != null)
                new ArgumentException("Game in progress");
            //remove player from wait list if it exist
            playerWaitList.Remove(player);
            //add to queue
            orderedPlayers.Enqueue(player);
            //add scoreBoard
            scoreBoard.Add(player, 0);
            collected.Add(player, new CollectedCards());
            //event
            ((Player)player).CardPlayed += player_CardPlayed;
            ((Player)player).HandEmpty += player_HandEmpty;
            player.SubscribeSpecialEmptyEvent(collection_SpecialEmpty);
            var p = (Player)player;
            if (p != null)
            {
                p.CardPlayed += player_CardPlayed;
                p.HandEmpty += player_HandEmpty;
            }
        }

        /// <summary>
        /// put player on the waiting list
        /// </summary>
        /// <param name="player"></param>
        protected virtual void UnsubscribePlayer(IHanafudaPlayer player)
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
            ((Player)player).CardPlayed -= player_CardPlayed;
            ((Player)player).HandEmpty -= player_HandEmpty;
            player.UnsubscribeSpecialEmptyEvent(collection_SpecialEmpty);
            var p = (Player)player;
            if (p != null)
            {
                p.CardPlayed -= player_CardPlayed;
                p.HandEmpty -= player_HandEmpty;
            }
            playerWaitList.Add(player);
        }

        #endregion

        #region Event

        public event EventHandler<FieldEventArgs> MultipleMatchFound;

        protected virtual void OnMultipleMatchFound(FieldEventArgs args)
        {
            MultipleMatchFound?.Invoke(this, args);
        }

        protected virtual void player_CardPlayed(object sender, CardPlayedEventArgs args)
        {
            var player = (Player)sender;
            if (player != currentPlayer)
                new ArgumentException("Player playing out of turn");
        }

        protected virtual void player_HandEmpty(object sender, EventArgs args)
        { }

        protected virtual void collection_SpecialEmpty(object sender, EventArgs arg)
        { }
        #endregion
    }
}
