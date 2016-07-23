﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class PathUtilityTest
    {
        private readonly string _sl = "/";

        private readonly string _bs
            = new string(Path.DirectorySeparatorChar, 1);

        [TestMethod]
        public void TestPathUtility_AppendBackslash()
        {
            const string source = "Some\\Path";
            string expected = source + _bs;
            string actual = PathUtility.AppendBackslash(source);
            Assert.AreEqual(expected, actual);

            actual = PathUtility.AppendBackslash(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPathUtility_ConvertSlashes()
        {
            string source = "Some" + _sl + "Path";
            string expected = "Some" + _bs + "Path";
            string actual = PathUtility.ConvertSlashes(source);
            Assert.AreEqual(expected, actual);

            source = "Some" + _bs + "Path";
            actual = PathUtility.ConvertSlashes(source);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPathUtility_MapPath()
        {
            string source = "Some" + _bs + "Path";
            string actual = PathUtility.MapPath(source);
            Assert.AreNotEqual(source, actual);
        }

        [TestMethod]
        public void TestPathUtility_StripExtension()
        {
            string source = "Some" + _bs + "Path" + _bs + "FileName.ext";
            string expected = "Some" + _bs + "Path" + _bs + "FileName";
            string actual = PathUtility.StripExtension(source);
            Assert.AreEqual(expected, actual);

            actual = PathUtility.StripExtension(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPathUtility_StripTrailingBackslash()
        {
            string source = "Some" + _bs + "Path" + _bs;
            string expected = "Some" + _bs + "Path";
            string actual = PathUtility.StripTrailingBackslash(source);
            Assert.AreEqual(expected, actual);

            actual = PathUtility.StripTrailingBackslash(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
