using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoStop.Card;

namespace UnitTestGoStop
{
    [TestClass]
    public class EqualTest
    {
        Hanafuda hanafuda;

        [TestMethod]
        public void KwangEquals()
        {
            //initialization for test
            hanafuda = new Kwang(Month.April);
            Hanafuda toCompare = new Kwang(Month.April);
            //true test
            Assert.AreEqual(true, hanafuda == toCompare);
            //false test
            Assert.AreEqual(false, hanafuda != toCompare);
        }

        [TestMethod]
        public void TtiEquals()
        {
            hanafuda = new HongDan(Month.April);
            //same month wrong dan
            Hanafuda toCompare = new ChungDan(Month.April);
            Assert.AreEqual(false, hanafuda == toCompare);
            //same card
            toCompare = new HongDan(Month.April);
            Assert.AreEqual(true, hanafuda == toCompare);
        }

        [TestMethod]
        public void PiEquals()
        {
            hanafuda = new Pi(Month.April);
            Hanafuda toCompare = new SsangPi(Month.April);
            Assert.AreEqual(false, hanafuda == toCompare);
            toCompare = new Pi(Month.April);
            Assert.AreEqual(true, hanafuda == toCompare);
        }
    }
}
