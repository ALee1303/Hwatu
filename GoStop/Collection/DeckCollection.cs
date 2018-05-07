using System;
using System.Collections.Generic;
using System.Linq;

using GoStop.Card;

namespace GoStop.Collection
{
    public class DeckCollection : CardCollection
    {
        private static DeckCollection instance;

        private Hanafuda[] reference;

        public static DeckCollection Instance
        {
            get
            {
                if (instance == null)
                    instance = new DeckCollection();
                return instance;
            }
        }

        private DeckCollection() : base()
        {
            Populate();
            cards.CopyTo(reference);
        }

        private void Populate()
        {
            this.cards = new List<Hanafuda>
            {
                new Pi(Month.January), new Pi(Month.January), new HongDan(Month.January), new Kwang(Month.January),
                new Pi(Month.February), new Pi(Month.February), new HongDan(Month.February), new Yul(Month.February),
                new Pi(Month.March), new Pi(Month.March), new HongDan(Month.March), new Kwang(Month.March),
                new Pi(Month.April), new Pi(Month.April), new ChoDan(Month.April), new Yul(Month.April),
                new Pi(Month.May), new Pi(Month.May), new ChoDan(Month.May), new Yul(Month.May),
                new Pi(Month.June), new Pi(Month.June), new ChungDan(Month.June), new Yul(Month.June),
                new Pi(Month.July), new Pi(Month.July), new ChoDan(Month.July), new Yul(Month.July),
                new Pi(Month.August), new Pi(Month.August), new Yul(Month.August), new Kwang(Month.August),
                new Pi(Month.September), new Pi(Month.September), new ChungDan(Month.September), new Yul(Month.September),
                new Pi(Month.October), new Pi(Month.October), new ChungDan(Month.October), new Yul(Month.October),
                new Pi(Month.November), new Pi(Month.November), new SsangPi(Month.November), new Kwang(Month.November),
                new SsangPi(Month.December), new ChoDan(Month.December), new Yul(Month.December), new Kwang(Month.December)
            };
            Shuffle();
        }
        
        private void Shuffle()
        {
            Random rnd = new Random();
            int toExc;
            Hanafuda temp;
            for (int i = this.Count()-1; i >= 0; i--)
            {
                toExc = rnd.Next(i);
                temp = this.cards[toExc];
                this.cards[toExc] = this.cards[i];
                this.cards[i] = temp;
            }
        }

        public void GatherCards()
        {
            this.cards = reference.ToList<Hanafuda>();
            foreach (Hanafuda card in cards)
                card.Location = Location.Deck;
            Shuffle();
        }
    }
}
