using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoStop.Card;
using GoStop.Collection;

namespace UnitTestGoStop
{
    /// <summary>
    /// Summary description for SpecialTest
    /// </summary>
    [TestClass]
    public class SpecialTest
    {
        SpecialCards special;

        [TestMethod]
        public void SpecialEmptyTest()
        {
            special = new BiYak(null);
            int point = 0;
            special.CollectionEmpty += (s, e) => { point = ((SpecialEmptyEventArgs)e).Points; };
            special.OnCardWon(new List<Hanafuda> { new SsangPi(Month.December), new ChoDan(Month.December), new Yul(Month.December), new Kwang(Month.December) });
            Assert.AreEqual(20, point);
        }
    }
}
