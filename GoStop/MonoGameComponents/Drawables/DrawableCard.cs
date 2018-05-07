using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using MG_Library;
using GoStop.Card;
using Microsoft.Xna.Framework.Graphics;

namespace GoStop.MonoGameComponents.Drawables
{
    public class DrawableCard : DrawableGameComponent
    {
        private BoardManager _manager;
        private Hanafuda _card;
        private Vector2 position, scale;
        private Sprite2D _frontImage;
        public Sprite2D CurrentImage
        {
            get
            {
                if (Revealed)
                    return _frontImage;
                else
                    return _manager.BackImage;
            }
        }
        public Sprite2D Outline { get => _manager.Outline; }
        public bool Revealed { get => _card.Revealed; }
        public Vector2 Position
        {
            get => position;
            set
            {
                if (position == value)
                    return;
                position = value;
                _frontImage.Position = value;
            }
        }
        public Vector2 Scale
        {
            get => scale;
            set
            {
                if (scale == value)
                    return;
                scale = value;
                _frontImage.Scale = value;
            }
        }
        public Rectangle Bound { get => _frontImage.SourceRect; }
        public Hanafuda Card { get => _card; }
        
        public DrawableCard(Game game, Hanafuda card, Sprite2D image) : base(game)
        {
            _manager = Game.Services.GetService<BoardManager>();
            _card = card;
            //_card.OwnerChanged += card_OwnerChanged;
            //_card.RevealedChanged += card_RevealedChanged;
            _card.LocationChanged += card_LocationChanged;
            _frontImage = image;
        }

        #region Drawable Override

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _frontImage.Initialize();
        }
        
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
              
        #endregion

        public virtual void Draw()
        {
            if (Revealed)
                _frontImage.Draw();

        }

        /// <summary>
        /// Change card display side based on its status
        /// </summary>

        /// <summary>
        /// Change Hidden status based on MainPlayer
        /// </summary>
        private void CheckRevealedByOwner()
        {
            if (_card.Owner == _manager.MainPlayer)
                _card.Revealed = true;
        }

        #region Event

        // currently not used
        protected virtual void card_OwnerChanged(object sender, HanafudaEventArgs args)
        { }
        // currently not used
        protected virtual void card_RevealedChanged(object sender, HanafudaEventArgs args)
        {
            var card = (Hanafuda)sender;
            if (card != _card)
                new ArgumentException("Changed card does not belong to this object");
        }

        protected virtual void card_LocationChanged(object sender, HanafudaEventArgs args)
        {
            var card = (Hanafuda)sender;
            if (card != _card)
                new ArgumentException("Changed card does not belong to this object");
            if (args.Location == Location.Deck)
            {
                _manager.drawable_MovedToDeck(this, args);
                _card.Revealed = false;
                _card.Owner = null;
            }
            if (args.Location == Location.Hand)
            {
                _manager.drawable_MovedToHand(this, args);
                CheckRevealedByOwner();
            }
            else
            {
                _manager.drawable_MovedToField(this);
                _card.Revealed = true;
            }
            // TODO: change location logic
        }

        #endregion
    }
}
