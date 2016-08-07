﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class NonNullCollectionTest
    {
        [TestMethod]
        public void TestNonNullCollection_Constructor()
        {
            NonNullCollection<object> collection 
                = new NonNullCollection<object>();
            Assert.AreEqual(0, collection.Count);

            collection.Add(new object());
            Assert.AreEqual(1, collection.Count);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNonNullCollection_Exception()
        {
            NonNullCollection<object> collection
                = new NonNullCollection<object>();
            Assert.AreEqual(0, collection.Count);

            collection.Add(null);
        }
    }
}