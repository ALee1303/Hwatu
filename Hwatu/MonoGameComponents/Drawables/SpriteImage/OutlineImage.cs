using MG_Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hwatu.MonoGameComponents.Drawables
{
    public class OutlineImage : Sprite2D
    {
        public OutlineImage(Game game) : base(game, null)
        { }

        protected override void LoadContent()
        {
            base.LoadContent();
            Texture = new Texture2D(Game.GraphicsDevice, 50, 81);
            Color[] data = new Color[50 * 81];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Yellow;
            Texture.SetData(data);
            Width = Texture.Width;
            Height = Texture.Height;
            Origin = new Vector2(Width / 2, Height / 2);
            Alpha = 0.5f;
        }
        public void Draw(Vector2 scale)
        {
            SpriteBatch.Draw(Texture, Position,
                SourceRect, Color.White * Alpha, Rotation,
                Origin, scale, SpriteEffects.None, 0.0f);
        }
    }
}
