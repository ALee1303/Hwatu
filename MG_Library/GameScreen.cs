using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MG_Library
{
    //https://www.youtube.com/watch?v=CcPb0bKkpeg
    public abstract class GameScreen : DrawableGameComponent
    {
        protected Color backgroundColor;
        protected Sprite2D backgroundImage;

        public GameScreen(Game game) : base(game)
        {
            backgroundColor = Color.CornflowerBlue;
        }

        protected override void LoadContent()
        { }

        protected override void UnloadContent()
        { }

        public override void Update(GameTime gameTime)
        { }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
