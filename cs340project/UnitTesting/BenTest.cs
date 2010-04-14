using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cs340project;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnitTesting
{
    /// <summary>
    /// Summary description for BenTest
    /// </summary>
    [TestClass]
    public class BenTest
    {
        #region Default Stuff
        public BenTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
        #endregion
        #region White Box testing

        List<string> ExpectedMessages = null;

        [TestMethod]
        public void LoopTest()
        {
            /*
             * This tests the ability of the NetworkHub to receive 0, 1, 2, or more
             * messages at once, looking at the following loop:
             * 
             * while ((o = CheckForMessage(client, IP)) != null)
             *      commands.Add(o);
             * 
             * We have that loop run 0 times by sending part of a message.
             * We then have it run 1 time by sending the rest of that message.
             * We then have it run 2 times by sending two messages at once.
             * We then have it run 5 times by sending five messages at once.
            */

            NetworkHub n1 = new NetworkHub();
            n1.Listen(new IPEndPoint(IPAddress.Any, 14325));
            n1.MessageReceived += new NetworkHub.NetworkHubMessageEvent(n1_MessageReceived);
            ExpectedMessages = new List<string>(new string[] { "1", "2", "3", "4", "5", "6", "7", "8" });

            //Get our connection and stream for sending data
            NetworkHub n2 = new NetworkHub();
            TcpClient client = n2.Connect("127.0.0.1", 14325);
            MemoryStream bytes, toSend;
            Stream s = client.GetStream();
            BinaryWriter bw = new BinaryWriter(s);

            bytes = new MemoryStream();
            new BinaryFormatter().Serialize(bytes, "1");
            //Send only the header, not the data.
            bw.Write((int)bytes.Length);
            Thread.Sleep(500);

            //Now finish sending the data.
            bytes.Seek(0, SeekOrigin.Begin);
            bw.Write(new BinaryReader(bytes).ReadBytes((int)bytes.Length));


            //Now send a batch of 2.
            toSend = new MemoryStream();
            bytes = new MemoryStream();
            new BinaryFormatter().Serialize(bytes, "2");
            new BinaryWriter(toSend).Write((int)bytes.Length);
            bytes.Seek(0, SeekOrigin.Begin);
            new BinaryWriter(toSend).Write(new BinaryReader(bytes).ReadBytes((int)bytes.Length));
            bytes = new MemoryStream();
            new BinaryFormatter().Serialize(bytes, "3");
            new BinaryWriter(toSend).Write((int)bytes.Length);
            bytes.Seek(0, SeekOrigin.Begin);
            new BinaryWriter(toSend).Write(new BinaryReader(bytes).ReadBytes((int)bytes.Length));

            toSend.Seek(0, SeekOrigin.Begin);
            byte[] data = new BinaryReader(toSend).ReadBytes((int)toSend.Length);
            bw.Write(data);

            Thread.Sleep(1000);
            Assert.AreEqual(5, ExpectedMessages.Count);

            //Now send a batch of five.
            toSend = new MemoryStream();
            for (int i = 4; i <= 8; i++)
            {
                bytes = new MemoryStream();
                new BinaryFormatter().Serialize(bytes, i.ToString());
                new BinaryWriter(toSend).Write((int)bytes.Length);
                bytes.Seek(0, SeekOrigin.Begin);
                new BinaryWriter(toSend).Write(new BinaryReader(bytes).ReadBytes((int)bytes.Length));
            }
            
            toSend.Seek(0, SeekOrigin.Begin);
            data = new BinaryReader(toSend).ReadBytes((int)toSend.Length);
            bw.Write(data);

            Thread.Sleep(1000);
            Assert.AreEqual(0, ExpectedMessages.Count);
        }

        void n1_MessageReceived(TcpClient client, string msg)
        {
            Assert.AreEqual(msg, ExpectedMessages[0]);
            ExpectedMessages.RemoveAt(0);
        }

        [TestMethod]
        public void RelationalTest()
        {
            /*
             * Full branch testing of NetworkHub.ObjectReadyToRead.
             * 
             * We test when there are less than 4 bytes to read (returns null).
             * We then test when the object is not fully available (returns null).
             * And then when the object is fully available (returns the object's size).
             */

            NetworkHub n = new NetworkHub();
            n.clientMemoryStreams[""] = new MemoryStream();

            MemoryStream junk = new MemoryStream();
            new BinaryWriter(junk).Write("Hello");
            junk.Seek(0, SeekOrigin.Begin);
            byte[] data = new BinaryReader(junk).ReadBytes((int)junk.Length);

            //While it's empty:
            Assert.IsNull(n.ObjectReadyToRead(""));

            //Put in a length.
            new BinaryWriter(n.clientMemoryStreams[""]).Write((int)data.Length);
            n.clientMemoryStreams[""].Seek(0, SeekOrigin.Begin);
            Assert.IsNull(n.ObjectReadyToRead(""));

            //Put in the actual object:
            new BinaryWriter(n.clientMemoryStreams[""]).Write(data);
            n.clientMemoryStreams[""].Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(n.ObjectReadyToRead(""), data.Length);
        }

        #endregion

        #region Black Box testing
        [TestMethod]
        public void EquivalancePartitioning()
        {
            //
            // http://www.softwaretestinghelp.com/what-is-boundary-value-analysis-and-equivalence-partitioning/
            //
        }

        [TestMethod]
        public void BoundaryTest()
        {
            //
            // http://www.softwaretestinghelp.com/what-is-boundary-value-analysis-and-equivalence-partitioning/
            //
        }
        #endregion
    }
}
