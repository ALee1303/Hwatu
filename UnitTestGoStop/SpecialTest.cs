using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoStop.Card;
using GoStop.Collection;
using GoStop.MonoGameComponents.Drawables;

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
            var collection = new Object();
            special.CollectionEmpty += (s, e) => { collection = s; point = ((SpecialEmptyEventArgs)e).Points; };
            Assert.AreEqual(20, point);
            Assert.IsInstanceOfType(collection, typeof(SpecialCards));
        }

        [TestMethod]
        public void ModTest()
        {
            int mod = 0 % 6;
            Assert.AreEqual(0, mod);
        }
    }
}
