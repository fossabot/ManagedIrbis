﻿using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftPowTest
    {
        private void _Execute
        (
            [NotNull] PftPow node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftPow_Construction_1()
        {
            PftPow node = new PftPow();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftPow_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Pow, 1, 1, "pow");
            PftPow node = new PftPow(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftPow_Execute_1()
        {
            PftPow node = new PftPow();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftPow_ToString_1()
        {
            PftPow node = new PftPow();
            Assert.AreEqual("pow(,)", node.ToString());
        }
    }
}