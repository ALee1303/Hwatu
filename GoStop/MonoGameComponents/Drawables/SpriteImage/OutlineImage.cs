using MG_Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GoStop.MonoGameComponents.Drawables
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
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Alpha = 0.5f;
        }
    }
}
