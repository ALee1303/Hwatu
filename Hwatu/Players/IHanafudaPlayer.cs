using System.Collections.Generic;
using System.Threading.Tasks;
using Hwatu.Card;
using Hwatu.MonoGameComponents;
using Hwatu.MonoGameComponents.Drawables;

namespace Hwatu
{
    public interface IHanafudaPlayer
    {
        void PlayCard(List<DrawableCard> hand);
        Task<DrawableCard> ChooseCard(List<DrawableCard> choice);
        void JoinBoard(BoardManager manager);
        void ExitBoard(BoardManager manager);
    }
}
