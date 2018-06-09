using MG_Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hwatu.MonoGameComponents.Drawables
{
    public class BackImage : Sprite2D
    {

        public BackImage(Game game) : base(game, null)
        { }

        protected override void LoadContent()
        {
            base.LoadContent();
            Texture = new Texture2D(Game.GraphicsDevice, 50, 81);
            Color[] data = new Color[50 * 81];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            Texture.SetData(data);
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }
    }
}
