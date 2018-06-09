using System;
using System.Collections.Generic;

using Hwatu.MonoGameComponents;
using Hwatu.MonoGameComponents.Drawables;
using System.Threading.Tasks;

namespace Hwatu
{
    public class Player : IHanafudaPlayer
    {
        public Player()
        { }

        protected virtual void PlayCard(DrawableCard card)
        {
            PlayerEventArgs args = new PlayerEventArgs();
            args.Selected = card;

            OnCardPlayed(args);
        }
        #region Interface Methods

        public virtual void PlayCard(List<DrawableCard> hand)
        {
            Task playerTask = new Task(() => PlayCardTask(hand));
            playerTask.Start();
        }
        protected virtual void PlayCardTask(List<DrawableCard> hand)
        {
        }

        public virtual Task<DrawableCard> ChooseCard (List<DrawableCard> choice)
        {
            return null;
        }
        public void JoinBoard(BoardManager manager)
        {
            manager.OnJoinBoard(this);
        }

        public void ExitBoard(BoardManager manager)
        {
            manager.OnExitBoard(this);
        }

        #endregion
        
        #region Event

        public event EventHandler<PlayerEventArgs> CardPlayed;
        public event EventHandler<PlayerEventArgs> MouseOverCard;

        protected virtual void OnMouseOver(PlayerEventArgs args)
        {
            MouseOverCard?.Invoke(this, args);
        }
        /// <summary>
        /// On Manager
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCardPlayed(PlayerEventArgs args)
        {
            CardPlayed?.Invoke(this, args);
        }
        #endregion
    }

    public class PlayerEventArgs : EventArgs
    {
        public DrawableCard Selected { get; set; }
        public DrawableCard Unselected { get; set; }
    }
}
