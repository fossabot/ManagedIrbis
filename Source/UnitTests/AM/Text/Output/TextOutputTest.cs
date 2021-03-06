﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Output;

namespace UnitTests.AM.Text.Output
{
    [TestClass]
    public class TextOutputTest
    {
        [TestMethod]
        public void TestTextOutput()
        {
            const string expected = "Quick brown fox";

            TextOutput output = new TextOutput();
            output.Write(expected);

            string actual = output.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
