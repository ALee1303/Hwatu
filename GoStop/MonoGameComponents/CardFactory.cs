using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using MG_Library;
using GoStop.Card;
using GoStop.MonoGameComponents.Drawables;

namespace GoStop.MonoGameComponents
{
    public class CardFactory : GameComponent
    {
        private Dictionary<KeyValuePair<Month, CardType>, Sprite2D> spriteGallery;
        private BoardManager manager;

        public CardFactory(Game game) : base(game)
        {
            manager = Game.Services.GetService<BoardManager>();
        }

        public void LoadTestImage()
        {
        }

        public DrawableCard ReturnedPairedDrawable(Hanafuda card)
        {
            KeyValuePair<Month, CardType> idx =
                new KeyValuePair<Month, CardType>(card.Month, card.Type);
            Sprite2D image = spriteGallery[idx];
            return new DrawableCard(Game, card, image);
        }
    }
}
