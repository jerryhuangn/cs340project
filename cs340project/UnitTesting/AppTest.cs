﻿using cs340project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System;

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
            App_Accessor target = new App_Accessor("test");
            PrivateObject param0 = new PrivateObject(target);
            Nullable<int> id = new Nullable<int>(0); //TODO: initialize to proper value
            string name = null; //TODO: initialize to proper value
            object[] p = null; //TODO: initialize to proper value 
            App.Command cmd = new App.Command(id,name,p);
            object expected = null; //initialize to proper return value
            object actual;
            actual = target.RunCommand(cmd);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
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
            target.AddObject(o);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for App Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.dll")]
        public void AppConstructorTest()
        {
            string name = string.Empty; // TODO: Initialize to an appropriate value
            App_Accessor target = new App_Accessor(name);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}