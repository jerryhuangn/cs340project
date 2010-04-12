using Server;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    /// <summary>
    /// Summary description for TJTest
    /// </summary>
    [TestClass]
    public class TJTest
    {
        #region Default Stuff
        public TJTest()
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

        /// <summary>
        /// Test covers loop testing, path, relational, and internal boundary coverage for broadcast.
        /// </summary>
        [TestMethod()]
        public void TestBroadcast()
        {
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    BroadcastWithAckTest((uint)i, (uint)j);
                }
            }
        }
        public void BroadcastWithAckTest(uint size, uint startnode)
        {
            //First, create a hyperweb with 6 nodes in it.
            Node root = new Node(null);
            List<Node> AllNodes = new List<Node>(new Node[] { root });
            for (int i = 0; i < size; i++)
            {
                Node n = new Node(null);
                root.InsertNode(n);
                AllNodes.Add(n);
            }

            //Now create a message visitor and broadcast it.
            MessageVisitor v = new MessageVisitor("First");
            uint Retval = AllNodes[(int)startnode].BroadcastWithAck(v, 0);
            uint Expected = (uint)AllNodes.Count;

            //Now make sure that all nodes have exactly one copy of that message.
            foreach (Node n in AllNodes)
            {
                List<string> Messages = (List<string>)n.Payload["Messages"];
                Assert.AreEqual(1, Messages.Count);
                Assert.AreEqual("First", Messages[0]);
                Assert.AreEqual(Expected, Retval);
            }
        }
#endregion
        #region Black Box testing
        [TestMethod]
        public void EquivelancePartitioning()
        {
            //
            // http://www.softwaretestinghelp.com/what-is-boundary-value-analysis-and-equivalence-partitioning/
            //Equivalence class 1: valid values
            uint test1 = 4;
            uint expected1 = 3;
            uint test2 = 254;
            uint expected2 = 8;


            //Equivalence class 2: invalid low
            int test3 = -1;
            uint expected3=0;

            uint actual1 = Extensions.Dimension(test1);
            uint actual2 = Extensions.Dimension(test2);

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
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
