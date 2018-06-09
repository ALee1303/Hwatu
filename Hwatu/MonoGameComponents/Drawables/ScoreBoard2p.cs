using System.Linq;

using MG_Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hwatu.MonoGameComponents.Drawables
{
    // TODO: Generelize
    public class ScoreBoard2p : Sprite2D
    {
        SpriteFont font;
        Texture2D playerBackground, enemyBackground;
        RenderTarget2D renderTarget;

        BoardManager _manager;
        IHanafudaPlayer mainPlayer;
        IHanafudaPlayer cpuPlayer;
        Vector2 fontOrigin;
        Vector2 fontOffset;

        public ScoreBoard2p(BoardManager manager, Game game) : base(game, null)
        {
            _manager = manager;
            mainPlayer = _manager.MainPlayer;
            cpuPlayer = _manager.ScoreBoard.Keys.Where(player => player != mainPlayer).First();
            fontOrigin = new Vector2(20);
            fontOffset = new Vector2(20, 0);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load Font
            font = Game.Content.Load<SpriteFont>("ScoreFont");
            //dimension for individual square
            Vector2 dimension = new Vector2(40, 40);
            Vector2 offset = new Vector2(40, 0);
            // Initialize Render Target
            renderTarget = new RenderTarget2D(Game.GraphicsDevice, 80, 40);
            // Set dimension for background
            playerBackground = new Texture2D(Game.GraphicsDevice,
                (int)dimension.X, (int)dimension.Y);
            enemyBackground = new Texture2D(Game.GraphicsDevice,
                (int)dimension.X, (int)dimension.Y);
            // Set Color for background
            Color[] data = new Color[(int)dimension.X * (int)dimension.Y];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Blue;
            playerBackground.SetData(data);
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            enemyBackground.SetData(data);

            Game.GraphicsDevice.SetRenderTarget(renderTarget);
            Game.GraphicsDevice.Clear(Color.Transparent);
            SpriteBatch.Begin();

            
            SpriteBatch.Draw(playerBackground, Vector2.Zero, Color.White);
            SpriteBatch.Draw(enemyBackground, offset, Color.White);
            SpriteBatch.End();
            // Texture & Sprite2D field Setup
            Texture = renderTarget;
            Game.GraphicsDevice.SetRenderTarget(null);

            Origin = new Vector2(40, 20);
            Position = new Vector2(Game.GraphicsDevice.Viewport.Width - Origin.X - 10, Game.GraphicsDevice.Viewport.Height / 2);

        }

        public override void Draw()
        {
            SpriteBatch.Draw(renderTarget, Position,
                SourceRect, Color.White * Alpha, Rotation,
                Origin, Scale, SpriteEffects.None, 0.0f);
            SpriteBatch.DrawString(font, _manager.ScoreBoard[mainPlayer].ToString(), Position - fontOffset,
                Color.White * Alpha, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            SpriteBatch.DrawString(font, _manager.ScoreBoard[cpuPlayer].ToString(), Position + fontOffset,
                Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
