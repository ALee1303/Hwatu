using System;
using System.Collections.Generic;
using GoStop.Card;
using GoStop.MonoGameComponents.Drawables;

namespace GoStop.Collection
{
    public abstract class SpecialCards : CardCollection
    {
        private int _points;
        private IHanafudaPlayer owner;

        protected SpecialCards(IHanafudaPlayer owner, int points) : base()
        {
            this._points = points;
            this.owner = owner;
        }

        public void OnCardsCollected(List<DrawableCard> wonCards)
        {
            foreach (DrawableCard card in wonCards)
            {
                OnCardCollected(card);
                // if last call emptied the list already
                if (Empty())
                    break;
            }
        }
        public void OnCardCollected(DrawableCard wonCard)
        {
            if (!Remove(wonCard.Card))
                return;
            SpecialChangedEventArgs changeArgs = new SpecialChangedEventArgs();
            changeArgs.Match = wonCard;
            OnCollectionChanged(changeArgs);
            if (Empty())
            {
                SpecialEmptyEventArgs emptyArgs = new SpecialEmptyEventArgs();
                emptyArgs.Points = this._points;
                emptyArgs.Owner = owner;
                OnCollectionEmpty(emptyArgs);
            }
        }

    }

    public class SpecialChangedEventArgs : EventArgs
    {
        public DrawableCard Match { get; set; }
    }

    public class SpecialEmptyEventArgs : EventArgs
    {
        public int Points { get; set; }
        public IHanafudaPlayer Owner { get; set; }
    }
}
