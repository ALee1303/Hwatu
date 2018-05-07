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
        private int playingCount;
        //cards
        protected DeckCollection deck;
        protected Dictionary<Month, CardCollection> field;
        protected Dictionary<IHanafudaPlayer, CollectedCards> collected;
        protected Dictionary<IHanafudaPlayer, int> scoreBoard;
        //players
        protected IHanafudaPlayer currentPlayer;
        protected List<IHanafudaPlayer> playerWaitList;
        protected Queue<IHanafudaPlayer> orderedPlayers;

        public IHanafudaPlayer CurrentPlayer
        {
            get => currentPlayer;
            private set
            {
                if (currentPlayer == value)
                    return;
                currentPlayer = value;
                if (currentPlayer != null)
                    OnNewPlayerTurn();
            }
        }
        // Adjusted in subscribe and unsubscribe
        public int PlayingCount
        {
            get => playingCount;
            private set
            {
                if (playingCount == value)
                    return;
                playingCount = value;
                if (playingCount == 0)
                    OnAllPlayerRemoved();
            }
        }
        public DeckCollection Deck { get => deck; }

        public Board()
        {
            deck = DeckCollection.Instance;
            field = new Dictionary<Month, CardCollection>();
            scoreBoard = new Dictionary<IHanafudaPlayer, int>();
            collected = new Dictionary<IHanafudaPlayer, CollectedCards>();
            playerWaitList = new List<IHanafudaPlayer>();
            orderedPlayers = new Queue<IHanafudaPlayer>();
        }

        // TODO:DealCard, FinishGame, GameResult
        #region Protected Methods

        protected virtual void PrepareGame()
        {
            OrderWaitingPlayers();
            DealCard();
        }

        protected virtual void OrderWaitingPlayers()
        {
            if (currentPlayer != null || orderedPlayers.Count > 0)
                new ArgumentException("Game in progress");
            foreach (IHanafudaPlayer player in playerWaitList)
                AddPlayer(player);
        }

        protected virtual void FinishGame()
        {

        }

        protected virtual void ResetBoard()
        {
            deck.GatherCards();
            field.Clear();
            collected = new Dictionary<IHanafudaPlayer, CollectedCards>();
            scoreBoard = new Dictionary<IHanafudaPlayer, int>();
        }

        protected bool IsNewPlayer(IHanafudaPlayer player)
        {
            return currentPlayer != player
                && !orderedPlayers.Contains(player);
        }

        #endregion

        public virtual void StartGame()
        {
            PrepareGame();
            CurrentPlayer = orderedPlayers.Dequeue();
        }

        public virtual void EndGame()
        {
            //in case game ends early
            foreach (IHanafudaPlayer player in orderedPlayers)
            {
                player.RenewHandAndSpecial();
                RemovePlayer(player);
            }
            ResetBoard();
        }
        
        /// <summary>
        /// Deal card to all waiting players
        /// Queueing them into the Game
        /// </summary>
        protected virtual void DealCard()
        {
            for (int i = 0; i < 2; i++)
            {
                DealCardsOnField();
                foreach (IHanafudaPlayer player in orderedPlayers)
                    DealCard(player);
            }
        }

        /// <summary>
        /// Deal card to specific player of specific amount
        /// Overload used inside DealCard()
        /// </summary>
        /// <param name="player"></param>
        /// <param name="amount">2p = 5, 3p = 4 then 3</param>
        private void DealCard(IHanafudaPlayer player, int amount = 5)
        {
            IEnumerable<Hanafuda> draws = deck.DrawCard(amount);
            foreach (Hanafuda drawn in draws)
            {
                drawn.Owner = player;
                drawn.Location = Location.Hand;
            }
            player.Hand.Add(draws);
        }

        /// <summary>
        /// Spread cards on dictionary of cards based on their month
        /// </summary>
        /// <param name="amount">2p = 4, 3p = 3</param>
        protected void DealCardsOnField(int amount = 4)
        {
            IEnumerable<Hanafuda> draws = deck.DrawCard(amount);
            OrganizeField(draws);
        }

        protected void OrganizeField(IEnumerable<Hanafuda> draws)
        {
            foreach (Hanafuda drawn in draws)
            {
                drawn.Location = Location.Field;
                field[drawn.Month].Add(drawn);
            }
        }

        #region Subscriber

        /// <summary>
        /// Add new player to waiting list
        /// </summary>
        /// <param name="player"></param>
        public virtual void SubscribePlayer(IHanafudaPlayer player)
        {
            if (!IsNewPlayer(player) || playerWaitList.Remove(player))
                new ArgumentException("Can't Add: Player Already Exist");
            playerWaitList.Add(player);
        }

        /// <summary>
        /// Remove player from the board
        /// </summary>
        /// <param name="player"></param>
        public virtual void UnsubscribePlayer(IHanafudaPlayer player)
        {
            if (IsNewPlayer(player) || !playerWaitList.Remove(player))
                new ArgumentException("Can't Remove: Player Doesn't Exist");
            RemovePlayer(player);
            playerWaitList.Remove(player);
        }

        /// <summary>
        /// Subscribe waiting player to the game
        /// </summary>
        /// <param name="player"></param>
        protected virtual void AddPlayer(IHanafudaPlayer player)
        {
            if (!IsNewPlayer(player))
                return;
            if (currentPlayer != null)
                new ArgumentException("Can't Join: Game in progress");
            //remove player from wait list if it exist
            if (playerWaitList.Remove(player))
                new ArgumentException("Unverified player joining game");
            //add to queue
            orderedPlayers.Enqueue(player);
            //add scoreBoard
            scoreBoard.Add(player, 0);
            collected.Add(player, new CollectedCards());
            //event
            player.PrepareSpecialCollection();
            player.SubscribeSpecialEmptyEvent(collection_SpecialEmpty);
            var p = (Player)player;
            if (p != null)
            {
                p.CardPlayed += player_CardPlayed;
                p.HandEmpty += player_HandEmpty;
            }
            PlayingCount++;
        }

        /// <summary>
        /// put player back on the waiting list
        /// </summary>
        /// <param name="player"></param>
        protected virtual void RemovePlayer(IHanafudaPlayer player)
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
            playerWaitList.Add(player);
            PlayingCount--;
        }

        #endregion

        #region fields EventHandler

        public event EventHandler<MatchEventArgs> MultipleMatchFound;
        
        protected virtual void OnMultipleMatchFound(MatchEventArgs args)
        {
            MultipleMatchFound?.Invoke(this, args);
        }

        protected virtual void player_HandEmpty(object sender, EventArgs args)
        { }

        /// <summary>
        /// Not called on Clear()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        protected virtual void collection_SpecialEmpty(object sender, EventArgs arg)
        { }
        #endregion

        #region Notify Board of Property Change

        public event Action NewPlayerTurn;
        public event Action AllPlayerRemoved;

        protected virtual void OnNewPlayerTurn()
        { }

        protected virtual void OnAllPlayerRemoved()
        {

        }
        #endregion
    }

    public class MatchEventArgs : EventArgs
    {
        public List<Hanafuda> pairedCards { get; set; }
    }
}
