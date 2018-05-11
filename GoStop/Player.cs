using GoStop.Card;
using GoStop.Collection;
using System;
using System.Collections.Generic;

using GoStop.MonoGameComponents;
using GoStop.MonoGameComponents.Drawables;

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
        protected virtual void ChooseCard(DrawableCard selected, DrawableCard unselected)
        {
            PlayerEventArgs args = new PlayerEventArgs();
            args.Selected = selected;
            args.Unselected = unselected;
            OnCardChoose(args);
        }

        #region Interface Methods

        public virtual void PlayCard(List<DrawableCard> hand)
        { }

        public virtual void ChooseCard(List<DrawableCard> choice)
        {
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
        public event EventHandler<PlayerEventArgs> CardChoose;
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
        protected virtual void OnCardChoose(PlayerEventArgs args)
        {
            CardChoose?.Invoke(this, args);
        }
        #endregion
    }

    public class PlayerEventArgs : EventArgs
    {
        public DrawableCard Selected { get; set; }
        public DrawableCard Unselected { get; set; }
    }
}
