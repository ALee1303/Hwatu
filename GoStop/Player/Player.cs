using GoStop.Card;
using GoStop.Collection;
using System;
using System.Collections.Generic;

using GoStop.MonoGameComponents;
using GoStop.MonoGameComponents.Drawables;
using System.Threading.Tasks;

namespace GoStop
{
    public class Player : IHanafudaPlayer
    {
        public Player()
        { }

        public virtual void PlayCard(DrawableCard card)
        {
            PlayerEventArgs args = new PlayerEventArgs();
            args.Selected = card;

            OnCardPlayed(args);
        }
        #region Interface Methods

        public virtual void PlayCard(List<DrawableCard> hand)
        { }

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
