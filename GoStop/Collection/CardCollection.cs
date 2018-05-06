using System;
using System.Collections;
using System.Collections.Generic;
using GoStop.Card;

namespace GoStop.Collection
{
    /// <summary>
    /// collection of Hanafuda
    /// </summary>
    public class CardCollection : ICollection<Hanafuda>
    {
        protected List<Hanafuda> cards; //list of hanafuda class

        public int Count { get => cards.Count; }
        public bool IsReadOnly { get => false; }

        public CardCollection()
        {
            cards = new List<Hanafuda>();
        }

        public Hanafuda this[int index]
        {
            get => cards[index];
            set { cards[index] = value; }
        }

        public void Add(Hanafuda item)
        {
            if (!Contains(item))
                cards.Add(item);
            else
                new ArgumentException("Card is already contained");
        }

        /// <summary>
        /// overload Add() for Pi. there can be 2 Pi of each type
        /// </summary>
        public void Add(Pi item)
        {
            if (item is SsangPi) // Saang pi follows same rule as other: one per month if they exist
                Add((Hanafuda)item); // cast and call normal add
            // special rule applies for regular Pi
            // there can be only 2 Pi in each month
            int count = 0;
            foreach (Pi pi in cards)
                if (pi == item)
                    count++;
            if (count < 2) // if theres only one or no Pi
                cards.Add(item);
            else
                new ArgumentException("Pi is already contained");
        }

        public void Add(IEnumerable<Hanafuda> items)
        {
            var enumerator = items.GetEnumerator();
            if (enumerator == null || !(enumerator is CardEnumerator))
                new ArgumentException("Invalid enumerable");
            while (enumerator.MoveNext())
            {
                var card = enumerator.Current;
                if (!cards.Contains(card))
                    cards.Add(card);
            }
        }

        public virtual bool Remove(Hanafuda item)
        {
            if (Contains(item))
            {
                cards.Remove(item);
                return true;
            }
            return false;
        }

        public virtual Hanafuda Draw()
        {
            if (!this)
                return null;
            int idx = Count - 1;
            Hanafuda toReturn = cards[idx];
            cards.RemoveAt(idx);
            return toReturn;
        }

        public virtual Hanafuda Peek()
        {
            if (!this)
                return null;
            int idx = Count - 1;
            return cards[idx];
        }

        public void CopyTo(Hanafuda[] array, int arrayIndex)
        {

            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex + 1)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < cards.Count; i++)
            {
                array[i + arrayIndex] = cards[i];
            }
        }

        public void Clear()
        {
            if (cards != null && cards.Count > 0)
                cards.Clear();
        }

        public bool Contains(Hanafuda item)
        {
            foreach (Hanafuda card in cards)
                if (card == item)
                    return true;
            //if none was found
            return false;
        }

        public bool Empty()
        {
            if (Count > 0)
                return false;
            return true;
        }

        public static bool operator true(CardCollection hanafudas) { return !hanafudas.Empty(); }
        public static bool operator false(CardCollection hanafudas) { return hanafudas.Empty(); }
        public static bool operator !(CardCollection hanafudas) { return hanafudas.Empty(); }

        #region Event

        public event EventHandler<EventArgs> CollectionEmpty;
        public event EventHandler<EventArgs> CollectionChanged;

        protected virtual void OnCollectionEmpty(EventArgs e)
        {
            CollectionEmpty?.Invoke(this, e);
        }

        protected virtual void OnCollectionChanged(EventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Enumerator
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
            public bool IsEmpty { get => _cards.Empty(); }

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
        #endregion Enumerator
    }
}
