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

        //overload for Pi
        //there can be 2 Pi of each type
        public void Add(Pi item)
        {
            if (item is SsangPi) // Saang pi follows same rule as other: one per month
                Add((Hanafuda)item); // call normal add

            if (!Contains(item)) // there can be only 2 Pi in each month
                innerCol.Add(item);
            else
                new ArgumentException("Card is already contained");
        }

        public bool Contains(Hanafuda item)
        {
            foreach (Hanafuda card in innerCol)
                if (card == item)
                    return true;
            //if none was found
            return false;
        }

        //overload for Pi
        public bool Contains(Pi item)
        {
            int count = 0;
            foreach (Pi pi in innerCol) //check how many Pi there is
                if (pi == item)
                    count++;
            if (count >= 2) // there can be only 2 Pi in each month
                return true;
            else
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
            if (Contains(item))
            {
                innerCol.Remove(item);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            innerCol.Clear();
        }

        // The generic enumerator obtained from IEnumerator<>
        // by GetEnumerator can also be used with the non-generic IEnumerator.
        public IEnumerator<Hanafuda> GetEnumerator()
        {
            return new CardEnumerator(this);
        }
        // To avoid a naming conflict, the non-generic IEnumerable method
        // is explicitly implemented.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CardEnumerator(this);
        }

        //Enumerator for iterating ICollection
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
