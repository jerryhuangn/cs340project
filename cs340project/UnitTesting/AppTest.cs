using cs340project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System;
using System.Net;

namespace UnitTesting
{
    
    
    /// <summary>
    ///This is a test class for AppTest and is intended
    ///to contain all AppTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AppTest
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
        ///A test for RunCommand
        ///</summary>
        [TestMethod()]
        public void RunCommandTest()
        {
            try
            {
                App test1 = App.GetApp("test1");
                test1.Network.Listen(new IPEndPoint(IPAddress.Any, 10000));
                Person remote = Proxifier.GetProxy<Person>("127.0.0.1", 10000, "test1", 2);
                remote.Age = 15;
                remote.Name = new PersonName("Ben", "Dilts", "Beandog");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message.ToString());
            }
        }

        /// <summary>
        ///A test for Network_CommandReceived
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void Network_CommandReceivedTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            App_Accessor target = new App_Accessor(param0); // TODO: Initialize to an appropriate value
            TcpClient client = null; // TODO: Initialize to an appropriate value
            App.Command cmd = null; // TODO: Initialize to an appropriate value
            target.Network_CommandReceived(client, cmd);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetApp
        ///</summary>
        [TestMethod()]
        public void GetAppTest()
        {
            string name = string.Empty; // TODO: Initialize to an appropriate value
            App expected = null; // TODO: Initialize to an appropriate value
            App actual;
            actual = App.GetApp(name);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddObject
        ///</summary>
        [TestMethod()]
        public void AddObjectTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            App_Accessor target = new App_Accessor(param0); // TODO: Initialize to an appropriate value
            object o = null; // TODO: Initialize to an appropriate value
            target.AddObject(0, o);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for App Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void AppConstructorTest()
        {
            string name = "Test";
            try
            {
                App_Accessor target = new App_Accessor(name);
                target.AddObject(0, new Person());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message.ToString());   
            }
        }
    }
}
