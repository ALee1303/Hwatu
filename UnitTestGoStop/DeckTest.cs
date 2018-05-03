using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoStop.Collection;
using GoStop.Card;

namespace UnitTestGoStop
{
    [TestClass]
    public class DeckTest
    {
        GoStop.Collection.DeckCollection deck;

        [TestMethod]
        public void DeckShuffleTest()
        {
            deck = new DeckCollection();
            DeckCollection toCompare = new DeckCollection();
            Assert.AreNotEqual(deck, toCompare);
        }
    }
}
