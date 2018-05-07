using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using MG_Library;
using GoStop.Card;

namespace GoStop.MonoGameComponents.Drawables
{
    public class DrawableCard : DrawableGameComponent
    {
        private Hanafuda _card;
        private Sprite2D _image;

        public DrawableCard(Game game, Hanafuda card, Sprite2D image) : base(game)
        {
            _card = card;
            //_card.OwnerChanged += card_OwnerChanged;
            _card.RevealedChanged += card_RevealedChanged;
            _card.LocationChanged += card_LocationChanged;
            _image = image;
        }

        #region Drawable Override

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void OnDrawOrderChanged(object sender, EventArgs args)
        {
            base.OnDrawOrderChanged(sender, args);
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            base.OnVisibleChanged(sender, args);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        /// <summary>
        /// Change card display side based on its status
        /// </summary>
        private void FlipCard()
        {

        }

        private void PlaceCardInDeck()
        {

        }

        /// <summary>
        /// Change Hidden status based on MainPlayer
        /// </summary>
        private void CheckRevealedByOwner()
        {

        }

        #region Event

        protected virtual void card_OwnerChanged(object sender, HanafudaEventArgs args)
        { }
        
        protected virtual void card_RevealedChanged(object sender, HanafudaEventArgs args)
        {
            var card = (Hanafuda)sender;
            if (card != _card)
                new ArgumentException("Changed card does not belong to this object");
            FlipCard();
        }

        protected virtual void card_LocationChanged(object sender, HanafudaEventArgs args)
        {
            var card = (Hanafuda)sender;
            if (card != _card)
                new ArgumentException("Changed card does not belong to this object");
            if (args.Location == Location.Deck)
            {
                _card.Revealed = false;
                _card.Owner = null;
                PlaceCardInDeck();
            }
            if (args.Location == Location.Hand)
                CheckRevealedByOwner();
            else
                _card.Revealed = true;
            // TODO: change location logic
        }

        #endregion
    }
}
