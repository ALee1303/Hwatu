using System;
using System.Collections.Generic;
using GoStop.Card;

namespace GoStop.Collection
{
    public abstract class SpecialCards : CardCollection
    {
        private int _points;
        private Player _owner;

        protected SpecialCards(Player owner, int points)
        {
            this._owner = owner;
            this._points = points;
        }
        
        public void OnCardWon(List<Hanafuda> wonCards)
        {
            foreach(Hanafuda card in wonCards)
            {
                Remove(card);
                if (Empty())
                {
                    SpecialEmptyEventArgs arg = new SpecialEmptyEventArgs();
                    arg.Points = this._points;
                    arg.Owner = this._owner;
                    OnCollectionEmpty(arg);
                    break;
                }
            }
        }
    }

    public class SpecialEmptyEventArgs : EventArgs
    {
        public int Points { get; set; }
        public Player Owner { get; set; }
    }
}
