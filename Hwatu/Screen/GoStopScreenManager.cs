using MG_Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GoStop.Screen
{
    // https://www.youtube.com/watch?v=CcPb0bKkpeg
    public class GoStopScreenManager : ScreenManager2D<GoStopScreenManager>
    {
        public GoStopScreenManager()
        {
            //returns error when constructor is called after instantiation
            ValidateSingletonCreation();

        }

        public override void LoadContent(ContentManager Content, SpriteBatch spriteBatch)
        { }

        public override void UnloadContent()
        { }

        public override void Update(GameTime gameTime)
        { }

        public override void Draw(SpriteBatch spriteBatch)
        { }
    }
}