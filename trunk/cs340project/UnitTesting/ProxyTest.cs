using cs340project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;

namespace UnitTesting
{
    
    
    /// <summary>
    ///This is a test class for ProxyTest and is intended
    ///to contain all ProxyTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProxyTest
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
        ///A test for Network_ResponseReceived
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void Network_ResponseReceivedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Proxy_Accessor target = new Proxy_Accessor(param0); // TODO: Initialize to an appropriate value
            TcpClient client = null; // TODO: Initialize to an appropriate value
            App.Response cmd = null; // TODO: Initialize to an appropriate value
            target.Network_ResponseReceived(client, cmd);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for InvokeAsync
        ///</summary>
        [TestMethod()]
        public void InvokeAsyncTest()
        {
            string server = string.Empty; // TODO: Initialize to an appropriate value
            int port = 0; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            string AppName = "Test";
            Proxy target = new Proxy(server, port, AppName, id); // TODO: Initialize to an appropriate value
            string method = string.Empty; // TODO: Initialize to an appropriate value
            object[] parameters = null; // TODO: Initialize to an appropriate value
            App.Command expected = null; // TODO: Initialize to an appropriate value
            App.Command actual;
            actual = target.InvokeAsync(method, parameters);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Invoke
        ///</summary>
        [TestMethod()]
        public void InvokeTest()
        {
            string server = string.Empty; // TODO: Initialize to an appropriate value
            int port = 0; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            string AppName = "Test";
            Proxy target = new Proxy(server, port, AppName, id); // TODO: Initialize to an appropriate value
            string method = string.Empty; // TODO: Initialize to an appropriate value
            object[] parameters = null; // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.Invoke(method, parameters);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Proxy Constructor
        ///</summary>
        [TestMethod()]
        public void ProxyConstructorTest()
        {
            string server = string.Empty; // TODO: Initialize to an appropriate value
            int port = 0; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            string AppName = "Test";
            Proxy target = new Proxy(server, port, AppName, id);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
