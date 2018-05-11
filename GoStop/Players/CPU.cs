using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoStop.MonoGameComponents.Drawables;

using GoStop.MonoGameComponents;
using GoStop.Card;

namespace GoStop
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
                Month bestMonth = maxSpanList[i].Card.Month;
                List<DrawableCard> bestSlot = _manager.Field[bestMonth];
                if (bestSlot.Count < 0)
                    continue;
                bestCandidate = maxSpanList[i];
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
