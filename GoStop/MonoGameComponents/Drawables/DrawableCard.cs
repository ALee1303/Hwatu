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
        private Sprite2D cardImage;
        public Vector2 Position
        {
            get => position;
            set
            {
                if (position == value)
                    return;
                position = value;
                cardImage.Position = value;
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
                cardImage.Scale = value;
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
        }
    }
}
