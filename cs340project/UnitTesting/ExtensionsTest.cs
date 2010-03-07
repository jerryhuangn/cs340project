using Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace UnitTesting
{
    
    
    /// <summary>
    ///This is a test class for ExtensionsTest and is intended
    ///to contain all ExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExtensionsTest
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
        ///A test for Distance
        ///</summary>
        [TestMethod()]
        public void DistanceTest()
        {
            uint numb = 297; // TODO: Initialize to an appropriate value
            uint otherNumb = 312; // TODO: Initialize to an appropriate value
            uint expected = 2; // TODO: Initialize to an appropriate value
            uint actual;
            actual = Extensions.Distance(numb, otherNumb);
            Assert.AreEqual(expected, actual);
       }

        /// <summary>
        /// white box testing for the extension method to
        /// calculate dimension
        /// </summary>
        [TestMethod()]
        public void DimensionTest()
        {
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
    }
}
