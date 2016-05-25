﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient;

namespace UnitTests.ManagedClient
{
    [TestClass]
    public class IrbisFormatTest
    {
        private void _TestFormat
            (
                string text,
                string expected
            )
        {
            string actual = IrbisFormat.PrepareFormat(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIrbisFormatPrepareFormat()
        {
            _TestFormat("", "");
            _TestFormat(" ", " ");
            _TestFormat("v100,/,v200", "v100,/,v200");
            _TestFormat("\tv100\r\n", " v100  ");
            _TestFormat
                (
                    "v100/*comment\r\nv200",
                    "v100  v200"
                );
        }
    }
}
