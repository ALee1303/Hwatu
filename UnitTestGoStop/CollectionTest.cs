﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoStop.Card;
using GoStop.Collection;
using GoStop;
using GoStop.MonoGameComponents;

namespace UnitTestGoStop
{
    [TestClass]
    public class CollectionTest
    {
        CardCollection collection;
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AddListTest()
        {
            collection = new CardCollection();
            List<Hanafuda> cards = new List<Hanafuda> { new Kwang(Month.April), new Kwang(Month.December) };
            collection.Add(cards);
            Assert.AreEqual(collection[0], cards[0]);
            Assert.AreEqual(collection[1], cards[1]);
        }

    }
}
