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
        private Queue<BackImage> backImages;
        private Sprite2D outline;
        private BoardManager manager;
        
        public Sprite2D Outline { get => outline; }

        public CardFactory(Game game) : base(game)
        {
            manager = Game.Services.GetService<BoardManager>();
            //setup gallery
            spriteGallery = new Dictionary<string, Sprite2D>();
            backImages = new Queue<BackImage>();
        }

        public DrawableCard ReturnPairedDrawable(Hanafuda card)
        {
            string idx = StringParseCardType(card.Month, card.Type);
            if (card is Pi)
                idx += ((Pi)card).PiCount;
            Sprite2D image = spriteGallery[idx];
            return new DrawableCard(Game, card, image);
        }
        public DrawableCard ReturnTurnedDrawable(Hanafuda card)
        {
            Sprite2D back = GetBackImage();
            return new DrawableCard(Game, card, back);
        }
        
        private string StringParseCardType(Month month, CardType type)
        {
            return String.Join("/", month, type);
        }

        public override void Initialize()
        {
            SetUpGallery();
            spriteGallery.Keys.ToList().ForEach(key => spriteGallery[key].Initialize());
        }

        #region Gallery SetUp

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
                    int slot = j + 1;
                    type = PossibleCard(month, slot);
                    path = StringParseCardType(month, type);
                    if (type == CardType.Pi)
                        path += slot.ToString();
                    spriteGallery.Add(path, new Sprite2D(Game, path));
                }
            }
        }

        private CardType PossibleCard(Month month, int slot)
        {
            CardType type = CardType.Pi;
            switch (month)
            {
                case Month.January:
                case Month.March:
                    if (slot == 1 || slot == 2)
                        type = CardType.Pi;
                    if (slot == 3)
                        type = CardType.Tti;
                    if (slot == 4)
                        type = CardType.Kwang;
                    break;
                case Month.February:
                case Month.April:
                case Month.May:
                case Month.June:
                case Month.July:
                case Month.September:
                case Month.October:
                    if (slot == 1 || slot == 2)
                        type = CardType.Pi;
                    if (slot == 3)
                        type = CardType.Tti;
                    if (slot == 4)
                        type = CardType.Yul;
                    break;
                case Month.August:
                    if (slot == 1 || slot == 2)
                        type = CardType.Pi;
                    if (slot == 3)
                        type = CardType.Yul;
                    if (slot == 4)
                        type = CardType.Kwang;
                    break;
                case Month.November:
                    if (slot == 1 || slot == 2)
                        type = CardType.Pi;
                    if (slot == 3)
                        type = CardType.SsangPi;
                    if (slot == 4)
                        type = CardType.Kwang;
                    break;
                case Month.December:
                    if (slot == 1)
                        type = CardType.SsangPi;
                    if (slot == 2)
                        type = CardType.Tti;
                    if (slot == 3)
                        type = CardType.Yul;
                    if (slot == 4)
                        type = CardType.Kwang;
                    break;
            }
            return type;
        }
        #endregion

        #region BackImage Setup

        private BackImage GetBackImage()
        {
            if (backImages.Count < 1)
                return new BackImage(Game);
            else
                return backImages.Dequeue();
        }

        #endregion
    }
}
