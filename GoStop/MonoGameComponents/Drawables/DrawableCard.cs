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
        private Sprite2D cardImage;
        private OutlineImage specialIndicator;
        public Vector2 Position
        {
            get => cardImage.Position;
            set
            {
                if (cardImage.Position == value)
                    return;
                cardImage.Position = value;
            }
        }
        public Vector2 Scale
        {
            get => cardImage.Scale;
            set
            {
                if (cardImage.Scale == value)
                    return;
                cardImage.Scale = value;
            }
        }
        public Color Color
        {
            get => cardImage.Color;
            set
            {
                if (cardImage.Color == value)
                    return;
                cardImage.Color = value;
            }
        }
        public float Alpha
        {
            get => cardImage.Alpha;
            set
            {
                if (cardImage.Alpha == value)
                    return;
                cardImage.Alpha = value;
            }
        }

        public Rectangle Bound { get => cardImage.BoundingRect; }
        public Hanafuda Card { get => _card; }
        public string Path { get => cardImage.Path; }
        
        public DrawableCard(Game game, Hanafuda card, Sprite2D image) : base(game)
        {
            _manager = Game.Services.GetService<BoardManager>();
            _card = card;
            cardImage = image;
            specialIndicator = null;
        }


        /// <summary>
        /// retrieve current image and set it to null
        /// Or to provided parameter
        /// </summary>
        /// <param name="newImg"></param>
        /// <returns></returns>
        public Sprite2D RetrieveImage(Sprite2D newImg = null)
        {
            Sprite2D oldImg = cardImage;
            cardImage = newImg;
            return oldImg;
        }

        // Temporary
        // Called by event SpecialCards.CollectionChanged
        public void OnSpecialCollected()
        {
            specialIndicator = new OutlineImage(Game);
            specialIndicator.Initialize();
            specialIndicator.Position = this.Position;
            specialIndicator.Scale = this.Scale;
        }

        #region Drawable Override

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            cardImage.Initialize();
        }
              
        #endregion

        public virtual void Draw()
        {
            cardImage.Draw();
            if (specialIndicator != null)
                specialIndicator.Draw();
        }
    }
}
