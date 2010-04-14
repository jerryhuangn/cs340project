using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using System.IO;
using System.Reflection;

namespace UnitTesting
{
    /// <summary>
    /// Summary description for JoelTest
    /// </summary>
    [TestClass]
    public class JoelTest
    {
        #region Default Stuff
        public JoelTest()
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
        [TestMethod]
        public void LoopTest()
        {
            /*
            
             * If a loop, definite or indefinite, is intended to iterate n times then the test 
             * plan should include the following seven considerations and possible faults.

That the loop might iterate zero times.
That the loop might iterate once
That the loop might iterate twice
That the loop might iterate several times
That the loop might iterate n - 1 times
That the loop might iterate n times
That the loop might iterate n + 1 times
That the loop might iterate infinite times
            
            */

            uint test1 = 4;
            uint expected1 = 3;
            uint test2 = 254;
            uint expected2 = 8;
            uint test3 = 1500;
            uint expected3 = 11;

            uint actual1 = Extensions.Dimension(test1);
            uint actual2 = Extensions.Dimension(test2);
            uint actual3 = Extensions.Dimension(test3);

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
            Assert.AreEqual(expected3, actual3);


        }

        [TestMethod]
        public void RelationalTest()
        {
            //
            // Test all forms of each if statement in the code
            //

            // Target 1
            Node target = new Node(0, null); // TODO: Initialize to an appropriate value
            
            Assert.AreEqual<NodeState>(target.CurrentState, NodeState.Largest);

            // Target 2         
            Node target2 = new Node(null); // TODO: Initialize to an appropriate value
            target.InsertNode(target2);
            Assert.AreEqual<NodeState>(target.CurrentState, NodeState.Inner);

            // Target 3         
            Node target3 = new Node(null); // TODO: Initialize to an appropriate value
            target.InsertNode(target3);
            Assert.AreEqual<NodeState>(target2.CurrentState, NodeState.Up);
            Assert.AreEqual<NodeState>(target3.CurrentState, NodeState.Down);

            // Target 4         
            Node target4 = new Node(null); // TODO: Initialize to an appropriate value
            target.InsertNode(target4);
            Assert.AreEqual<NodeState>(target3.CurrentState, NodeState.Edge);

        }

        [TestMethod]
        public void PathTest()
        {
            //
            // design tests to go through each path of the code (good coverage)
            //

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

        [TestMethod]
        public void InternalBoundaryTest()
        {
            //
            // Test all the extremes for the domain of the classes
            //

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
#endregion
        #region Black Box testing
        [TestMethod]
        public void EquivelancePartitioning()
        {
            //
            // http://www.softwaretestinghelp.com/what-is-boundary-value-analysis-and-equivalence-partitioning/
            //
            Random r = new Random(4356);
            Node root = new Node(null);
            List<Node> AllNodes = new List<Node>(new Node[] { root });
            for (uint j = 0; j < 16; j++)
            {
                Node n2 = new Node(null);
                root.InsertNode(n2);
                AllNodes.Add(n2);
            }

            var test = root.GetNode((uint) r.Next(0, 15));
            Assert.IsNotNull(test);

            test = root.GetNode((uint)r.Next(16, int.MaxValue));
            Assert.IsNull(test);
        }

        [TestMethod]
        public void BoundaryTest()
        {
            //
            // http://www.softwaretestinghelp.com/what-is-boundary-value-analysis-and-equivalence-partitioning/
            //

            Node root = new Node(null);
            List<Node> AllNodes = new List<Node>(new Node[] { root });
            for (uint j = 0; j < 16; j++)
            {
                Node n2 = new Node(null);
                root.InsertNode(n2);
                AllNodes.Add(n2);
            }

            var test = root.GetNode(15);
            Assert.IsNotNull(test);

            test = root.GetNode(0);
            Assert.IsNotNull(test);

            test = root.GetNode(17);
            Assert.IsNull(test);

            test = root.GetNode(1);
            Assert.IsNotNull(test);
        }
        #endregion
    }
}
