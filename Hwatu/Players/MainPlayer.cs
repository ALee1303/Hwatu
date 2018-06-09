using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Hwatu.MonoGameComponents;
using Hwatu.MonoGameComponents.Drawables;

namespace Hwatu
{
    public class MainPlayer : Player, IMainPlayer
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
        
        public MainPlayer(GameServiceContainer services) : base()
        {
            controller = new HanafudaController(services);
            outlinedCard = null;
        }
        
        public override void PlayCard(List<DrawableCard> hand)
        {
            Task playerTask = new Task(() => PlayCardTask(hand));
            playerTask.Start();
        }
        protected override void PlayCardTask(List<DrawableCard> hand)
        {
            DrawableCard selected = SelectCardLoop(hand);
            PlayCard(selected);
        }

        public override Task<DrawableCard> ChooseCard(List<DrawableCard> choices)
        {
            var chooseTask = Task<DrawableCard>.Factory.StartNew(() => SelectCardLoop(choices));
            chooseTask.Wait();
            return chooseTask;
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
