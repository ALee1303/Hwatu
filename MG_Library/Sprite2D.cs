using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MG_Library
{
    public class Sprite2D : DrawableGameComponent
    {
        public Texture2D Texture;
        public float Alpha, Rotation;
        public string Path;
        public Vector2 Position, Scale, Origin;
        public Rectangle SourceRect { get => Texture.Bounds; }
        public Rectangle BoundingRect
        {
            get
            {
                return new Rectangle((int)Position.X - (int)Origin.X,
                    (int)Position.Y - (int)Origin.Y, Texture.Width, Texture.Height);
            }
        }

        private SpriteBatch _spriteBatch;

        public Sprite2D(Game game, string path) : base(game)
        {
            Path = path;
            Position = Origin = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = (SpriteBatch)
                Game.Services.GetService(typeof(SpriteBatch));
            if (Path == null)
                return;
            Texture = Game.Content.Load<Texture2D>(Path);
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        { }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(Texture, Position,
                SourceRect, Color.White * Alpha, Rotation,
                Origin, Scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Overload used when not added to Game.Components
        /// </summary>
        public virtual void Draw()
        {
            _spriteBatch.Draw(Texture, Position,
                SourceRect, Color.White * Alpha, Rotation,
                Origin, Scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Overload for drawing on specified SpriteBatch
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,
                SourceRect, Color.White * Alpha, Rotation,
                Origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}