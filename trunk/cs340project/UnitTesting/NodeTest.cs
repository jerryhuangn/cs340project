using Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System;
namespace UnitTesting
{


    /// <summary>
    ///This is a test class for NodeTest and is intended
    ///to contain all NodeTest Unit Tests
    ///
    /// Author: Joel Day
    ///</summary>
    [TestClass()]
    public class NodeTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for InsertNode
        ///
        /// White-Box testing.  Test exhaustivly all the different insertions
        /// of nodes from every place in the HypeerWeb from 1-63 nodes.
        /// 
        /// All tests conclude with a comparison with the expected results.
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void InsertNodeTest()
        {
            for (uint size = 0; size < 63; size++)
            {
                TextReader tr = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("UnitTesting.TestNodeData." + (size + 1).ToString() + "Nodes.txt"));
                string expected = tr.ReadToEnd();

                for (uint i = 0; i <= size; i++)
                {
                    InsertNodeTest(size, i, expected);
                }
            }
        }


        /// <summary>
        /// Used to create the test documents
        /// </summary>
        public void InsertNodeTestCreate()
        {
            for (uint size = 0; size < 63; size++)
            {
                InsertNodeTestCreate(size);
            }
        }

        private void InsertNodeTestCreate(uint size)
        {
            //Refresh the whole hyperweb to size+1 nodes (including root)
            Node root = new Node(null);

            var curStat = root.CurrentState;
            for (uint j = 0; j < size; j++)
                root.InsertNode(new Node(null));

            //And add the node n+2, at insertion point i.
            root.InsertNode(new Node(null));

            string actual = root.DumpAllNodes();

            TextWriter tr = new StreamWriter(File.OpenWrite(@"C:\Users\joel.day3\Documents\Visual Studio 2008\Projects\school\cs340project\cs340project\UnitTesting\TestNodeData\" + (size + 1).ToString() + "Nodes.txt"));
            tr.Write(actual);
            tr.Close();
        }


        /// <summary>
        /// Insert test.
        /// 
        /// This is the main body of the Insert Test.
        /// </summary>
        /// <param name="size">The size of the web.</param>
        /// <param name="insertAt">The insertAt point.</param>
        /// <param name="expected">The expected output.</param>
        private static void InsertNodeTest(uint size, uint insertAt, string expected)
        {
            //Refresh the whole hyperweb to size+1 nodes (including root)
            Node root = new Node(null);

            var curStat = root.CurrentState;
            List<Node> AllNodes = new List<Node>(new Node[] { root });
            for (uint j = 0; j < size; j++) {
                Node n = new Node(null);
                root.InsertNode(n);
                AllNodes.Add(n);
            }

            //And add the node n+2, at insertion point i.
            Node n2 = new Node(null);
            AllNodes[(int)insertAt].InsertNode(n2);

            string actual = root.DumpAllNodes();
            if (expected != actual)
                Assert.Fail("Failed on size " + size + ", insertAt " + insertAt + ":\n\n" + expected + "\n\n" + actual);
        }


        /// <summary>
        /// Remove node test.
        /// </summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void RemoveNodeTest()
        {
            for (uint size = 1; size < 64; size++)
            {
                TextReader tr = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("UnitTesting.TestNodeData." + (size - 1) + "Nodes.txt"));
                string expected = tr.ReadToEnd();

                for (uint i = 0; i <= size; i++)
                {
                    RemoveNodeTest(size, i, (uint)(new Random().Next(0, (int)size)), expected);
                }
            }
        }

        private static void RemoveNodeTest(uint size, uint removeFrom, uint removeAt, string expected)
        {
            //Refresh the whole hyperweb to size+1 nodes (including root)
            Node root = new Node(null);
            List<Node> AllNodes = new List<Node>(new Node[] { root });
            for (uint j = 0; j < size; j++)
            {
                Node n2 = new Node(null);
                root.InsertNode(n2);
                AllNodes.Add(n2);
            }

            //Remove the node they wanted us to.
            var n = AllNodes[(int)removeFrom].RemoveById(removeAt);

            string actual = n.DumpAllNodes();
            if (expected != actual)
                Assert.Fail("Failed on size " + size + ", removeAt " + removeAt);
        }

        /// <summary>
        ///A test for ChildId.
        ///
        /// White-Box test for the ChildId attribute of a node
        /// Tests several nodes with specific Ids and compares their
        /// ChildId with an expected result.
        ///</summary>
        [TestMethod()]
        public void ChildIdTest()
        {
            // Target 1
            Node target = new Node(1406, null); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.ChildId(1406);
            Assert.AreEqual<uint>(actual, 3454);

            // Target 2         
            Node target2 = new Node(0, null); // TODO: Initialize to an appropriate value
            uint actual2;
            actual2 = target2.ChildId(0);
            Assert.AreEqual<uint>(actual2, 1);

            // Target 3         
            Node target3 = new Node(0, null); // TODO: Initialize to an appropriate value
            uint actual3;
            actual3 = target3.ChildId(1406);
            Assert.AreEqual<uint>(actual3, 2048);

            // Target 4         
            Node target4 = new Node(0, null); // TODO: Initialize to an appropriate value
            uint actual4;
            actual4 = target4.ChildId(1);
            Assert.AreEqual<uint>(actual4, 2);
        }

        /// <summary>
        ///A test for ParentId
        ///
        /// White-Box test for the ParentId attribute of a node
        /// Tests several nodes with specific Ids and compares their
        /// ParentId with an expected result.
        ///</summary>
        [TestMethod()]
        public void ParentIdTest()
        {
            // Target 1
            Node target = new Node(3454, null); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.ParentId;
            Assert.AreEqual<uint>(actual, 1406);

            // Target 2         
            Node target2 = new Node(1, null); // TODO: Initialize to an appropriate value
            uint actual2;
            actual2 = target2.ParentId;
            Assert.AreEqual<uint>(actual2, 0);

            // Target 3         
            Node target3 = new Node(2048, null); // TODO: Initialize to an appropriate value
            uint actual3;
            actual3 = target3.ParentId;
            Assert.AreEqual<uint>(actual3, 0);

            // Target 4         
            Node target4 = new Node(2, null); // TODO: Initialize to an appropriate value
            uint actual4;
            actual4 = target4.ParentId;
            Assert.AreEqual<uint>(actual4, 0);

        }

        /// <summary>
        ///A test for SurrogateId
        ///
        /// White-Box test for the SurrogateId attribute of a node
        /// Tests several nodes with specific Ids and compares their
        /// SurrogateId with an expected result.
        ///</summary>
        [TestMethod()]
        public void SurrogateIdTest()
        {
            // Target 1
            Node target = new Node(1406, null); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.SurrogateId(2450);
            Assert.AreEqual<uint>(actual, 3454);

            // Target 2         
            Node target2 = new Node(1, null); // TODO: Initialize to an appropriate value
            uint actual2;
            actual2 = target2.SurrogateId(2);
            Assert.AreEqual<uint>(actual2, 3);

            // Target 3         
            Node target3 = new Node(1, null); // TODO: Initialize to an appropriate value
            uint actual3;
            actual3 = target3.SurrogateId(1406);
            Assert.AreEqual<uint>(actual3, 1025);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            TextReader tr = new StreamReader(Assembly.GetExecutingAssembly()
     .GetManifestResourceStream("UnitTesting.TestNodeData.2Nodes.txt"));
            string expected = tr.ReadToEnd();

            Node root = new Node(null);
            List<Node> AllNodes = new List<Node>(new Node[] { root });
            for (uint j = 0; j < 3; j++)
            {
                Node n = new Node(null);
                root.InsertNode(n);
                AllNodes.Add(n);
            }

            //Remove the node they wanted us to.
            AllNodes[3].RemoveById(3);

            string actual = root.DumpAllNodes();
            if (expected != actual)
                Assert.Fail("Failed on size 4");
        }

        /// <summary>
        /// Test Broadcast and Broadcast with Acknowledgement Exhaustively
        /// </summary>
        [TestMethod()]
        public void TestSend()
        {
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    VisitTest((uint)i, (uint)j);
                }
            }
        }

        /// <summary>
        ///A test for Broadcast
        ///</summary>
        public void VisitTest(uint size, uint startnode)
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

            uint rand = (uint)(new Random(12123)).Next(0, (int)size - 1);

            AllNodes[(int)startnode].Send(v, rand);

            //Now make sure that all nodes have exactly one copy of that message.
            foreach (Node n in AllNodes)
            {
                if (n.Id == rand)
                {
                    List<string> Messages = (List<string>)n.Payload["Messages"];
                    Assert.AreEqual(1, Messages.Count);
                    Assert.AreEqual("First", Messages[0]);
                }
            }
        }

        /// <summary>
        /// Test Broadcast and Broadcast with Acknowledgement Exhaustively
        /// </summary>
        [TestMethod()]
        public void TestBroadcast()
        {
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    BroadcastTest((uint)i, (uint)j);
                    BroadcastWithAckTest((uint)i, (uint)j);
                }
            }
        }


        /// <summary>
        /// Tests the Broadcast method.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="startnode">The startnode.</param>
        public void BroadcastTest(uint size, uint startnode)
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
            AllNodes[(int)startnode].Broadcast(v);

            //Now make sure that all nodes have exactly one copy of that message.
            foreach (Node n in AllNodes)
            {
                List<string> Messages = (List<string>)n.Payload["Messages"];
                Assert.AreEqual(1, Messages.Count);
                Assert.AreEqual("First", Messages[0]);
            }
        }

        /// <summary>
        /// A test for Broadcast With Acknowledgement
        /// </summary>
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

    }
}
