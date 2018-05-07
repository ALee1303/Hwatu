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
    // TODO: separate to Interface
    public class CardFactory : GameComponent
    {
        private Dictionary<string, Sprite2D> spriteGallery;
        private Sprite2D backImage, outline;
        private BoardManager manager;

        public Sprite2D BackImage { get => backImage; }
        public Sprite2D Outline { get => outline; }

        public CardFactory(Game game) : base(game)
        {
            manager = Game.Services.GetService<BoardManager>();
            //setup gallery
            spriteGallery = new Dictionary<string, Sprite2D>();
            SetUpGallery();
        }

        public DrawableCard ReturnPairedDrawable(Hanafuda card)
        {
            string idx = StringParseCard(card.Month, card.Type);
            Sprite2D image = spriteGallery[idx];
            return new DrawableCard(Game, card, image);
        }
        
        private string StringParseCard(Month month, CardType type)
        {
            return String.Join("/", month, type);
        }

        private void SetUpGallery()
        {
            string path;
            Month month;
            CardType type;
            for (int i = 0; i < 12; i++)
            {
                month = (Month)i;
                for (int j = 0; j < 4; j++)
                {
                    type = (CardType)j;
                    if (!IsPossibleCard(month, type))
                        continue;
                    path = StringParseCard(month, type);
                    // in case of SsangPi
                    // TODO: change SSangPi logic later
                    if (month == Month.December &&
                        type == CardType.Pi)
                    {
                        spriteGallery.Add(path, new Sprite2D(Game, "December/SsangPi"));
                        continue;
                    }
                    spriteGallery.Add(path, new Sprite2D(Game, path));
                }
            }
        }

        private bool IsPossibleCard(Month month, CardType type)
        {
            switch (type)
            {
                case CardType.Tti:
                    if (month == Month.August ||
                        month == Month.November)
                        return false;
                    break;
                case CardType.Yul:
                    if (month == Month.January ||
                        month == Month.March ||
                        month == Month.November)
                        return false;
                    break;
                case CardType.Kwang:
                    if (month != Month.January ||
                        month != Month.March ||
                        month != Month.August ||
                        month != Month.November ||
                        month != Month.December)
                        return false;
                    break;
            }
            return true;
        }

    }
}
