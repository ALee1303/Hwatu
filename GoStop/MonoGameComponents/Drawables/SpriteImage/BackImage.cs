using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MG_Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GoStop.MonoGameComponents.Drawables
{
    public class BackImage : Sprite2D
    {

        public BackImage(Game game) : base(game, null)
        { }

        protected override void LoadContent()
        {
            Texture = new Texture2D(Game.GraphicsDevice, 50, 81);
            Texture.SetData(new[] { Color.Red });
            SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
