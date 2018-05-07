using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoStop.Collection;
using GoStop.Card;

namespace UnitTestGoStop
{
    [TestClass]
    public class DeckTest
    {
        GoStop.Collection.CardCollection deck;

        [TestMethod]
        public void DeckShuffleTest()
        {
            deck = DeckCollection.Instance;
            CardCollection toCompare = DeckCollection.Reference;
            Assert.AreNotEqual(deck, toCompare);
        }
    }
}
