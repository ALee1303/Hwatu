using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hwatu.MonoGameComponents.Drawables;

using Hwatu.MonoGameComponents;
using Hwatu.Card;

namespace Hwatu
{
    public class CPU : Player
    {
        private BoardManager _manager;
        public CPU(BoardManager manager)
        {
            _manager = manager;

        }
        public override void PlayCard(List<DrawableCard> hand)
        {
            List<DrawableCard> maxSpanList = hand.OrderByDescending(card => card.Card.Month).ToList();
            DrawableCard bestCandidate = null;
            for (int i = 0; i < maxSpanList.Count; i++)
            {
                bestCandidate = maxSpanList[i];
                Month bestMonth = bestCandidate.Card.Month;
                List<DrawableCard> bestSlot = _manager.Field[bestMonth];
                if (bestSlot.Count == 0)
                    continue;
                break;
            }
            PlayCard(bestCandidate);
        }
        public override Task<DrawableCard> ChooseCard(List<DrawableCard> choice)
        {
            var chooseTask = Task<DrawableCard>.Factory.StartNew(( ) 
                => choice.OrderByDescending(card => card.Card.Type).First());
            chooseTask.Wait();
            return chooseTask;
        }
    }
}
