using cs340project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTesting
{
    
    
    /// <summary>
    ///This is a test class for ProxifierTest and is intended
    ///to contain all ProxifierTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProxifierTest
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
        ///A test for GetProxy
        ///</summary>
        public void GetProxyTestHelper<T>()
        {
            string server = string.Empty; // TODO: Initialize to an appropriate value
            int port = 0; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            string AppName = "Test";
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            actual = Proxifier.GetProxy<T>(server, port, AppName, id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetProxyTest()
        {
            GetProxyTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for CreateProxyCode
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void CreateProxyCodeTest()
        {
            Type original = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Proxifier_Accessor.CreateProxyCode(original);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateProxy
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void CreateProxyTest()
        {
            Type original = null; // TODO: Initialize to an appropriate value
            Type expected = null; // TODO: Initialize to an appropriate value
            Type actual;
            actual = Proxifier_Accessor.CreateProxy(original);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Proxifier Constructor
        ///</summary>
        [TestMethod()]
        public void ProxifierConstructorTest()
        {
            Proxifier target = new Proxifier();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
