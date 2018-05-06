using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using MG_Library;
using GoStop.Card;

namespace GoStop.GameComponent.Drawables
{
    public class DrawableCard : DrawableGameComponent
    {
        private Hanafuda _card;
        private Sprite2D _image;

        public DrawableCard(Game game, Hanafuda card, Sprite2D image) : base(game)
        {
            _card = card;
            _image = image;
        }

        public override void Initialize()
        {
            _image.Position
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
    }
}
