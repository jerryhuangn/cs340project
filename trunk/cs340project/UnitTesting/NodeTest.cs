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
            InsertNodeTest(new List<uint>(), 4);
        }

        void InsertNodeTest(List<uint> insertIndex, int maxNodes)
        {
            int alreadyExist = insertIndex.Count;
            if (alreadyExist >= maxNodes)
                return;

            if (alreadyExist == 0)
            {
                Node n = new Node();//Create a root (0) node.
            }

            for (uint i = 0; i <= alreadyExist; i++)
            {
                insertIndex.Add(i);
                InsertNodeTest(insertIndex, maxNodes);
                InsertNodeTestSingle(insertIndex.ToArray());
                insertIndex.RemoveAt(alreadyExist);
            }
        }

        void InsertNodeTestSingle(uint[] insertIndex)
        {
            Node.AllNodes.Clear();
            Node root = new Node();

            foreach (uint idx in insertIndex)
                Node.AllNodes[idx].CreateNode();

            TextReader tr = new StreamReader(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("UnitTesting.TestNodeData." + insertIndex.Length.ToString() + "Nodes.txt"));

            string expected = tr.ReadToEnd();
            string actual = Node.DumpAllNodes();

            Assert.AreEqual(expected, actual);
        }

    }
}
