using System;
using System.Collections.Generic;

using Hwatu.Card;
using Hwatu.Collection;

using Hwatu.MonoGameComponents;

namespace Hwatu
{
    public class Board : IBoard
    {
        public int PlayingCount { get; protected set; }
        //cards
        private IHanafudaPlayer currentPlayer;
        protected List<IHanafudaPlayer> playerWaitList;
        protected Queue<IHanafudaPlayer> orderedPlayers;
        
        protected BoardManager _manager;

        public List<IHanafudaPlayer> PlayerWaitList
        {
            get => playerWaitList;
        }

        public IHanafudaPlayer CurrentPlayer
        {
            get => currentPlayer;
            protected set
            {
                if (currentPlayer == value)
                    return;
                currentPlayer = value;
                if (currentPlayer != null)
                    OnNewPlayerTurn();
            }
        }
        
        public Board(BoardManager manager)
        {
            currentPlayer = null;
            playerWaitList = new List<IHanafudaPlayer>();
            orderedPlayers = new Queue<IHanafudaPlayer>();
           

            _manager = manager;
            _manager.HandEmpty += manager_HandEmpty;
        }

        #region Prepare Game

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
            playerWaitList.Clear();
        }

        #endregion

        #region Game Progression Method

        public virtual void StartGame()
        {
            PrepareGame();
            CurrentPlayer = orderedPlayers.Dequeue();
        }
        

        public virtual void EndTurn()
        {
            if (PlayingCount <= 0)
                OnAllPlayerRemoved();
            if (CurrentPlayer != null)
                orderedPlayers.Enqueue(CurrentPlayer);
            CurrentPlayer = orderedPlayers.Dequeue();
        }

        // Caller: OnAllPlyaerRemoved()
        public virtual void EndGame()
        {
            //in case game ends early
            foreach (IHanafudaPlayer player in orderedPlayers)
            {
                RemovePlayer(player);
            }
            PostGame();
        }
        // Caller: EndGame
        protected virtual void PostGame()
        { }
        
        public virtual void ResetBoard()
        { }

        public virtual int CalculatePoint(IHanafudaPlayer owner, Hanafuda card)
        {
            return -1;
        }

        public virtual void CalculatePoint(IHanafudaPlayer owner, List<Hanafuda> cards)
        {

        }

        public virtual List<SpecialCards> PrepareSpecialCollection(IHanafudaPlayer player)
        {
            return null;
        }

        #endregion

        #region Deal Card
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
            IEnumerable<Hanafuda> draws = DeckCollection.Instance.DrawCard(amount);
            DealCardEventArgs args = new DealCardEventArgs();
            args.Player = player;
            args.Cards = draws;
            OnCardsDealt(args);
        }

        /// <summary>
        /// Spread cards on dictionary of cards based on their month
        /// </summary>
        /// <param name="amount">2p = 4, 3p = 3</param>
        protected void DealCardsOnField(int amount = 4)
        {
            IEnumerable<Hanafuda> draws = DeckCollection.Instance.DrawCard(amount);
            DealCardEventArgs args = new DealCardEventArgs();
            args.Cards = draws;
            // update board manager's field
            OnCardsOnField(args);
        }
        #endregion

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
            //add to queue
            orderedPlayers.Enqueue(player);
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
            //event
            var p = (Player)player;
            playerWaitList.Add(player);
            PlayingCount--;
        }

        #endregion

        #region Manager EventHandler

        
        public event Action NewPlayerTurn;
        public event Action AllPlayerRemoved;

        public event EventHandler<DealCardEventArgs> CardsDealt;
        public event EventHandler<DealCardEventArgs> CardsOnField;
        public event EventHandler<MultipleMatchEventArgs> MultipleMatch;
        
        protected virtual void OnNewPlayerTurn()
        {
            NewPlayerTurn?.Invoke();
        }

        protected virtual void OnAllPlayerRemoved()
        {
            EndGame();
            AllPlayerRemoved?.Invoke();
        }

        protected virtual void OnCardsDealt(DealCardEventArgs args)
        {
            CardsDealt?.Invoke(this, args);
        }

        protected virtual void OnCardsOnField(DealCardEventArgs args)
        {
            CardsOnField?.Invoke(this, args);
        }

        protected virtual void OnMultipleMatch(MultipleMatchEventArgs args)
        {
            MultipleMatch?.Invoke(this, args);
        }

        protected virtual void manager_HandEmpty()
        { }

        #endregion

        public bool IsNewPlayer(IHanafudaPlayer player)
        {
            return currentPlayer != player
                && !orderedPlayers.Contains(player);
        }
    }

    public class DealCardEventArgs : EventArgs
    {
        public IHanafudaPlayer Player { get; set; }
        public IEnumerable<Hanafuda> Cards { get; set; }
    }

    public class MultipleMatchEventArgs : EventArgs
    {
        public Month Month { get; set; }
    }
}
