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
    public class HanafudaController
    {
        private IInputHandler inputManager;

        public HanafudaController(GameServiceContainer services)
        {
            inputManager =services.GetService<IInputHandler>();
        }

        public DrawableCard GetMouseOverCard(List<DrawableCard> selecatables)
        {
            DrawableCard mouseOverCard = null;
            foreach (DrawableCard card in selecatables)
            {
                if (IsMouseOverCard(card))
                    mouseOverCard = card;
            }
            return mouseOverCard;
        }

        public bool IsMouseOverCard(DrawableCard drawable)
        {
            Rectangle bound = drawable.Bound;
            if (inputManager.IsMouseOver(bound))
                return true;
            return false;
        }

        public bool IsLeftMouseClicked()
        {
            return inputManager.MouseButtonClicked(MouseButton.Left);
        }
    }
}
