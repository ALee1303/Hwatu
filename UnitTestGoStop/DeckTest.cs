using Microsoft.VisualStudio.TestTools.UnitTesting;

using Hwatu.Collection;

namespace UnitTestGoStop
{
    [TestClass]
    public class DeckTest
    {
        CardCollection deck;

        [TestMethod]
        public void DeckShuffleTest()
        {
            deck = DeckCollection.Instance;
            CardCollection toCompare = DeckCollection.Reference;
            Assert.AreNotEqual(deck, toCompare);
        }
    }
}
