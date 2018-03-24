using System;
using System.Collections;
using System.Collections.Generic;
using GoStop.Card;

namespace GoStop
{
    //collection of cards
    class CardCollection : ICollection<Hanafuda>
    {
        private List<Hanafuda> innerCol; //list of hanafuda class
        public int Count { get => innerCol.Count; }

        public bool IsReadOnly { get => false; }

        public CardCollection()
        {
            innerCol = new List<Hanafuda>();
        }

        public Hanafuda this[int index]
        {
            get => innerCol[index];
            set { innerCol[index] = value; }
        }

        public void Add(Hanafuda item)
        {
            if (!Contains(item))
                innerCol.Add(item);
            else
                new ArgumentException("Card is already contained");
        }

        public void Clear()
        {
            innerCol.Clear();
        }

        public bool Contains(Hanafuda item)
        {
            foreach (Hanafuda card in innerCol)
                if(card.Equals(item))
                    return true;
            //if none was found
            return false;
        }

        public void CopyTo(Hanafuda[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < innerCol.Count; i++)
            {
                array[i + arrayIndex] = innerCol[i];
            }
        }

        public bool Remove(Hanafuda item)
        {
            return innerCol.Remove(item);
        }

        public IEnumerator<Hanafuda> GetEnumerator()
        {
            return new CardEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CardEnumerator(this);
        }

        //Enumerator for ICollection
        internal class CardEnumerator : IEnumerator<Hanafuda>
        {
            CardCollection _cards;
            int curIdx;
            Hanafuda curCard;

            public CardEnumerator(CardCollection cards)
            {
                _cards = cards;
                curIdx = -1;
                curCard = default(Hanafuda);
            }

            public bool MoveNext()
            {
                if (++curIdx >= _cards.Count) return false;
                else
                {
                    curCard = _cards[curIdx];
                }
                return true;
            }

            public void Reset() { curIdx = -1; }

            void IDisposable.Dispose() { }

            public Hanafuda Current
            {
                get => curCard;
            }

            object IEnumerator.Current
            {
                get => Current;
            }
        }
    }
}
