using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using GoStop.MonoGameComponents;
using GoStop.MonoGameComponents.Drawables;

namespace GoStop.Minhwatu
{
    public class MainMinhwatuPlayer : MinhwatuPlayer, IMainPlayer
    {
        private HanafudaController controller;
        public HanafudaController Controller { get => controller; }

        private DrawableCard outlinedCard;
        public DrawableCard OutlinedCard
        {
            get => outlinedCard;
            set
            {
                if (outlinedCard == value)
                    return;
                outlinedCard = value;
                PlayerEventArgs args = new PlayerEventArgs();
                args.Selected = outlinedCard;
                OnMouseOver(args);
            }
        }
        
        public MainMinhwatuPlayer(GameServiceContainer services) : base()
        {
            controller = new HanafudaController(services);
            outlinedCard = null;
        }
        
        public override void PlayCard(List<DrawableCard> hand)
        {
            Task playerTask = new Task(() => PlayCardTask(hand));
            playerTask.Start();
        }
        private void PlayCardTask(List<DrawableCard> hand)
        {
            DrawableCard selected = SelectCardLoop(hand);
            PlayCard(selected);
        }

        public override void ChooseCard(List<DrawableCard> choices)
        {
            Task chooseTask = new Task(() => ChooseCardTask(choices));
            chooseTask.Start();
        }

        private void ChooseCardTask(List<DrawableCard> choices)
        {
            DrawableCard selected = SelectCardLoop(choices);
            DrawableCard notSelected = choices.Find(x => x != selected);
            ChooseCard(selected, notSelected);
        }

        private DrawableCard SelectCardLoop(List<DrawableCard> cards)
        {
            DrawableCard selected = null;
            while (selected == null)
            {
                OutlinedCard = controller.GetMouseOverCard(cards);
                if (controller.IsLeftMouseClicked())
                    selected = OutlinedCard;
                
            }
            OutlinedCard = null;
            return selected;
        }


    }
}
