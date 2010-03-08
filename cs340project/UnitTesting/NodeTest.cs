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
        [TestMethod()]
        [DeploymentItem("Server.dll")]
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
            Node.AllNodes.Clear();
            Node root = new Node();

            var curStat = root.CurrentState;
            for (uint j = 0; j < size; j++)
                root.CreateNode();

            //And add the node n+2, at insertion point i.
            Node.AllNodes[0].CreateNode();

            string actual = Node.DumpAllNodes();

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
            Node.AllNodes.Clear();
            Node root = new Node();

            var curStat = root.CurrentState;
            for (uint j = 0; j < size; j++)
                root.CreateNode();

            //And add the node n+2, at insertion point i.
            Node.AllNodes[insertAt].CreateNode();

            string actual = Node.DumpAllNodes();
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
                    for (uint j = 0; j <= size; j++)
                        RemoveNodeTest(size, i, j, expected);
                }
            }
        }

        private static void RemoveNodeTest(uint size, uint removeFrom, uint removeAt, string expected)
        {
            //Refresh the whole hyperweb to size+1 nodes (including root)
            Node.AllNodes.Clear();
            Node root = new Node();
            for (uint j = 0; j < size; j++)
                root.CreateNode();

            //Remove the node they wanted us to.
            Node.AllNodes[removeFrom].Remove(removeAt);

            string actual = Node.DumpAllNodes();
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
            Node target = new Node(1406); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.ChildId(1406);
            Assert.AreEqual<uint>(actual, 3454);

            // Target 2         
            Node target2 = new Node(0); // TODO: Initialize to an appropriate value
            uint actual2;
            actual2 = target2.ChildId(0);
            Assert.AreEqual<uint>(actual2, 1);

            // Target 3         
            Node target3 = new Node(0); // TODO: Initialize to an appropriate value
            uint actual3;
            actual3 = target3.ChildId(1406);
            Assert.AreEqual<uint>(actual3, 2048);

            // Target 4         
            Node target4 = new Node(0); // TODO: Initialize to an appropriate value
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
            Node target = new Node(3454); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.ParentId;
            Assert.AreEqual<uint>(actual, 1406);

            // Target 2         
            Node target2 = new Node(1); // TODO: Initialize to an appropriate value
            uint actual2;
            actual2 = target2.ParentId;
            Assert.AreEqual<uint>(actual2, 0);

            // Target 3         
            Node target3 = new Node(2048); // TODO: Initialize to an appropriate value
            uint actual3;
            actual3 = target3.ParentId;
            Assert.AreEqual<uint>(actual3, 0);

            // Target 4         
            Node target4 = new Node(2); // TODO: Initialize to an appropriate value
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
            Node target = new Node(1406); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.SurrogateId(2450);
            Assert.AreEqual<uint>(actual, 3454);

            // Target 2         
            Node target2 = new Node(1); // TODO: Initialize to an appropriate value
            uint actual2;
            actual2 = target2.SurrogateId(2);
            Assert.AreEqual<uint>(actual2, 3);

            // Target 3         
            Node target3 = new Node(1); // TODO: Initialize to an appropriate value
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

            Node.AllNodes.Clear();
            Node root = new Node();
            for (uint j = 0; j < 3; j++)
                root.CreateNode();

            //Remove the node they wanted us to.
            Node.AllNodes[3].Remove(3);

            string actual = Node.DumpAllNodes();
            if (expected != actual)
                Assert.Fail("Failed on size 4");
        }
    }
}
