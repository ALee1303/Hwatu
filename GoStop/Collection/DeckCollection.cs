using System;
using System.Collections.Generic;
using System.Linq;

using GoStop.Card;

namespace GoStop.Collection
{
    public class DeckCollection : CardCollection
    {
        private static DeckCollection instance;

        private static CardCollection reference;

        public static DeckCollection Instance
        {
            get
            {
                if (instance == null)
                    instance = new DeckCollection();
                return instance;
            }
        }
        public static CardCollection Reference { get => reference; }

        private DeckCollection() : base()
        {
            Populate();
            reference = new CardCollection();
            foreach (Hanafuda card in cards)
                reference.Add(card);
        }

        private void Populate()
        {
            this.cards = new List<Hanafuda>
            {
                new Pi(Month.January, 1), new Pi(Month.January, 2), new HongDan(Month.January), new Kwang(Month.January),
                new Pi(Month.February, 1), new Pi(Month.February,2), new HongDan(Month.February), new Yul(Month.February),
                new Pi(Month.March,1), new Pi(Month.March,2), new HongDan(Month.March), new Kwang(Month.March),
                new Pi(Month.April,1), new Pi(Month.April,2), new ChoDan(Month.April), new Yul(Month.April),
                new Pi(Month.May,1), new Pi(Month.May,2), new ChoDan(Month.May), new Yul(Month.May),
                new Pi(Month.June,1), new Pi(Month.June,2), new ChungDan(Month.June), new Yul(Month.June),
                new Pi(Month.July,1), new Pi(Month.July,2), new ChoDan(Month.July), new Yul(Month.July),
                new Pi(Month.August,1), new Pi(Month.August,2), new Yul(Month.August), new Kwang(Month.August),
                new Pi(Month.September,1), new Pi(Month.September,2), new ChungDan(Month.September), new Yul(Month.September),
                new Pi(Month.October,1), new Pi(Month.October,2), new ChungDan(Month.October), new Yul(Month.October),
                new Pi(Month.November,1), new Pi(Month.November,2), new SsangPi(Month.November), new Kwang(Month.November),
                new SsangPi(Month.December), new ChoDan(Month.December), new Yul(Month.December), new Kwang(Month.December)
            };
            Shuffle();
        }

        private void Shuffle()
        {
            Random rnd = new Random();
            int toExc;
            Hanafuda temp;
            for (int i = this.Count() - 1; i >= 0; i--)
            {
                toExc = rnd.Next(i);
                temp = this.cards[toExc];
                this.cards[toExc] = this.cards[i];
                this.cards[i] = temp;
            }
        }

        public void GatherCards()
        {
            foreach (Hanafuda card in reference)
            {
                cards.Add(card);
            }
            Shuffle();
        }
    }
}
