﻿using System;
using System.IO;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Infrastructure.Sockets;

using Moq;

namespace UnitTests.ManagedIrbis.Infrastructure.Sockets
{
    [TestClass]
    public class TestingSocketTest
    {
        [NotNull]
        private Mock<IIrbisConnection> _GetMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();

            return result;
        }

        [TestMethod]
        public void TestingSocket_Construction_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            TestingSocket socket = new TestingSocket(connection);
            Assert.IsFalse(socket.RequireConnection);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestingSocket_AbortRequest_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            TestingSocket socket = new TestingSocket(connection);
            socket.AbortRequest();
        }

        [TestMethod]
        public void TestingSocket_ExecuteRequest_1()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            byte[][] expectedRequest = { new byte[0], new byte[0] };
            TestingSocket socket = new TestingSocket(connection)
            {
                Response = new byte[0],
                ExpectedRequest = expectedRequest
            };
            byte[][] request = { new byte[0], new byte[0] };
            byte[] expectedAnswer = new byte[0];
            byte[] actualAnswer = socket.ExecuteRequest(request);
            CollectionAssert.AreEqual(expectedAnswer, actualAnswer);
            CollectionAssert.AreEqual(request, socket.ActualRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestingSocket_ExecuteRequest_2()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            byte[][] expected = { new byte[] { 1 }, new byte[0] };
            TestingSocket socket = new TestingSocket(connection)
            {
                Response = new byte[] { 1, 2, 3 },
                ExpectedRequest = expected
            };
            byte[][] request = { new byte[] { 2 }, new byte[0] };
            socket.ExecuteRequest(request);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestingSocket_ExecuteRequest_3()
        {
            Mock<IIrbisConnection> mock = _GetMock();
            IIrbisConnection connection = mock.Object;
            byte[][] expected = { new byte[] { 1 }, new byte[0] };
            TestingSocket socket = new TestingSocket(connection)
            {
                ExpectedRequest = expected
            };
            byte[][] request = { new byte[] { 2 }, new byte[0] };
            socket.ExecuteRequest(request);
        }
    }
}
