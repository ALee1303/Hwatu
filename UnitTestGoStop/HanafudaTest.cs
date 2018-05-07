using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoStop;
using GoStop.Card;

namespace UnitTestGoStop
{
    [TestClass]
    public class HanafudaTest
    {
        Hanafuda hanafuda;

        [TestMethod]
        public void OwnerTest()
        {
            hanafuda = new Kwang(Month.April);
            //no owner initially
            Assert.AreEqual(null, hanafuda.Owner);
            //initialization
            IHanafudaPlayer player = new Player();
            bool isTriggered = false;
            IHanafudaPlayer argsPlayer = null;
            hanafuda.OwnerChanged += (s, e) => {
                isTriggered = true;
                argsPlayer = e.Owner; };
            hanafuda.Owner = player;
            Assert.AreEqual(true, isTriggered);
            Assert.AreEqual(player, hanafuda.Owner);
            Assert.AreEqual(player, argsPlayer);
        }

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
