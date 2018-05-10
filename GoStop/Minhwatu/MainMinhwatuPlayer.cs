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
                args.Card = outlinedCard;
                OnMouseOver(args);
            }
        }

        public void Update()
        {

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

        public void PlayCardTask(List<DrawableCard> hand)
        {
            DrawableCard selected = null;
            while (selected == null)
            {
                OutlinedCard = controller.GetMouseOverCard(hand);
                if (controller.IsLeftMouseClicked())
                    selected = OutlinedCard;
            }
            PlayCard(selected);
        }
        
    }
}
