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
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void InsertNodeTest()
        {
            for (uint size = 0; size < 63; size++)
            {
                TextReader tr = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("UnitTesting.TestNodeData." + (size+1).ToString() + "Nodes.txt"));
                string expected = tr.ReadToEnd();

                for (uint i = 0; i <= size; i++)
                {
                    InsertNodeTest(size, i, expected);
                }
            }
        }

        private static void InsertNodeTest(uint size, uint insertAt, string expected)
        {
            //Refresh the whole hyperweb to size+1 nodes (including root)
            Node.AllNodes.Clear();
            Node root = new Node();
            for (uint j = 0; j < size; j++)
                root.CreateNode();

            //And add the node n+2, at insertion point i.
            Node.AllNodes[insertAt].CreateNode();

            string actual = Node.DumpAllNodes();
            if (expected != actual)
                Assert.Fail("Failed on size " + size + ", insertAt " + insertAt);
        }


        /// <summary>
        ///A test for ChildId
        ///</summary>
        [TestMethod()]
        public void ChildIdTest()
        {
            Node target = new Node(1406); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.ChildId(1406);
            Assert.AreEqual<uint>(actual, 3454);
        }

        /// <summary>
        ///A test for ParentId
        ///</summary>
        [TestMethod()]
        public void ParentIdTest()
        {
            Node target = new Node(3454); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.ParentId;
            Assert.AreEqual<uint>(actual, 1406);
        }

        /// <summary>
        ///A test for SurrogateId
        ///</summary>
        [TestMethod()]
        public void SurrogateIdTest()
        {
            Node target = new Node(1406); // TODO: Initialize to an appropriate value
            uint actual;
            actual = target.SurrogateId(1406);
            Assert.AreEqual<uint>(actual, 3454);
        }
    }
}
