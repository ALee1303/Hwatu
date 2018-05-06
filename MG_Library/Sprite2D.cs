using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG_Library
{
    public class Sprite2D : DrawableGameComponent
    {
        public float Alpha, Rotation;
        public string Path;
        public Vector2 Position, Scale, Origin;
        public Rectangle SourceRect;
        public Texture2D Texture;

        private SpriteBatch _spriteBatch;

        public Sprite2D(Game game, string path) : base(game)
        {
            Path = path;
            Position = Origin = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = (SpriteBatch)
                Game.Services.GetService(typeof(SpriteBatch));
            Texture = Game.Content.Load<Texture2D>(Path);
            SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            UnloadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        { }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(Texture, Position + Origin,
                SourceRect, Color.White * Alpha, Rotation,
                Origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}