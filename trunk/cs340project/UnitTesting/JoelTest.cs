using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        }

        [TestMethod]
        public void RelationalTest()
        {
            //
            // Test all forms of each if statement in the code
            //
        }

        [TestMethod]
        public void PathTest()
        {
            //
            // design tests to go through each path of the code (good coverage)
            //
        }

        [TestMethod]
        public void InternalBoundaryTest()
        {
            //
            // Test all the extremes for the domain of the classes
            //
        }
#endregion
        #region Black Box testing
        [TestMethod]
        public void EquivelancePartitioning()
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
