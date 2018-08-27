using System;
using System.Collections.Generic;

using Hwatu.Card;
using Hwatu.Collection;

using Hwatu.MonoGameComponents;

namespace Hwatu
{
    public class Board : IBoard
    {
        protected BoardManager _manager;
        
        
        public Board(BoardManager manager)
        {
            _manager = manager;
            _manager.HandEmpty += manager_HandEmpty;
        }

        #region Prepare Game

        #endregion

        #region Game Progression Method
        
        public virtual int CalculatePoint(IHanafudaPlayer owner, Hanafuda card)
        {
            return -1;
        }

        public virtual void CalculatePoint(IHanafudaPlayer owner, List<Hanafuda> cards)
        {

        }

        public virtual List<SpecialCards> GetSpecialCollection(IHanafudaPlayer player)
        {
            return null;
        }

        #endregion

        #region Deal Card
        /// <summary>
        /// Deal card to all waiting players
        /// Queueing them into the Game
        /// </summary>
        public virtual void DealCard()
        {
            for (int i = 0; i < 2; i++)
            {
                DealCardsOnField();
                foreach (IHanafudaPlayer player in _manager.TurnQueue)
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

        #region Manager EventHandler
        
        public event EventHandler<DealCardEventArgs> CardsDealt;
        public event EventHandler<DealCardEventArgs> CardsOnField;
        
        protected virtual void OnCardsDealt(DealCardEventArgs args)
        {
            CardsDealt?.Invoke(this, args);
        }

        protected virtual void OnCardsOnField(DealCardEventArgs args)
        {
            CardsOnField?.Invoke(this, args);
        }

        protected virtual void manager_HandEmpty()
        { }

        #endregion
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
