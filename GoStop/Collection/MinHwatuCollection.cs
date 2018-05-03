using System;
using System.Collections.Generic;
using GoStop.Card;

namespace GoStop.Collection
{
    public abstract class MinhwatuCollection : CardCollection
    {
        private int points;
        private Player owner;

        private void owner_CardWon(object sender)
        {

        }
    }

    public class MinhwatuCollectionEmptyEventArgs : EventArgs
    {
        public int Points { get; set; }
        public Player Owner { get; set; }
    }
}
