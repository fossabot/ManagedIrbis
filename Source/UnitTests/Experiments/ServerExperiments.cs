﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Server;

namespace UnitTests.Experiments
{
    [TestClass]
    public class ServerExperiments
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void RunEngine_1()
        {
            string serverRootPath = Irbis64RootPath;

            string fileName = Path.Combine(serverRootPath, "irbis_server.ini");
            IniFile simpleIni = new IniFile(fileName, IrbisEncoding.Ansi, false);
            ServerIniFile serverIni = new ServerIniFile(simpleIni);
            IrbisServerEngine engine = new IrbisServerEngine(serverIni, serverRootPath);
            Assert.IsNotNull(engine.IniFile);
            Assert.IsNotNull(engine.ClientIni);
            Assert.IsNotNull(engine.Contexts);
            Assert.IsNotNull(engine.DataPath);
            Assert.IsNotNull(engine.Listener);
            Assert.IsNotNull(engine.Mapper);
            Assert.IsNotNull(engine.StopSignal);
            Assert.IsNotNull(engine.SystemPath);
            Assert.IsNotNull(engine.Users);
            Assert.IsNotNull(engine.WorkDir);
            Assert.IsNotNull(engine.Workers);

            Assert.AreEqual(2, engine.Users.Length);
            Assert.AreEqual("librarian", engine.Users[0].Name);
            Assert.AreEqual("secret", engine.Users[0].Password);

            Task connecter = new Task(() =>
            {
                try
                {
                    Task.Delay(100).Wait();
                    IrbisConnection connection = new IrbisConnection
                    {
                        Host = "127.0.0.1",
                        Port = 6666,
                        Username = "librarian",
                        Password = "secret",
                        Workstation = IrbisWorkstation.Cataloger
                    };

                    connection.Connect();

                    Task.Delay(100).Wait();
                    connection.NoOp();

                    Task.Delay(100).Wait();
                    int maxMfn = connection.GetMaxMfn("IBIS");

                    Task.Delay(100).Wait();
                    MarcRecord record = connection.ReadRecord(1);

                    Task.Delay(100).Wait();
                    connection.Dispose();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }

                engine.StopSignal.Set();
            });
            connecter.Start();
            engine.MainLoop();
            Assert.AreEqual(0, engine.Contexts.Count);
        }
    }
}
