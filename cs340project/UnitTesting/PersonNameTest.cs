using cs340project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTesting
{
    
    
    /// <summary>
    ///This is a test class for PersonNameTest and is intended
    ///to contain all PersonNameTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PersonNameTest
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
        ///A test for Nick
        ///</summary>
        [TestMethod()]
        [DeploymentItem("cs340project.exe")]
        public void NickTest()
        {
            PersonName target = new PersonName(null, null, null);
            PrivateObject param0 = new PrivateObject(target);
            string expected = "Nick";
            string actual;
            param0.SetProperty("Nick", expected);
            //target.Nick = expected;
            actual = target.Nick;
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Last
        ///</summary>
        [TestMethod()]
        [DeploymentItem("cs340project.exe")]
        public void LastTest()
        {
            PersonName target = new PersonName(null, null, null);
            PrivateObject param0 = new PrivateObject(target);
            string expected = "Last";
            string actual;
            param0.SetProperty("Last", expected);
            actual = target.Last;
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for First
        ///</summary>
        [TestMethod()]
        [DeploymentItem("cs340project.exe")]
        public void FirstTest()
        {
            PersonName target = new PersonName(null, null, null);
            PrivateObject param0 = new PrivateObject(target);
            string expected = "First"; 
            string actual;
            param0.SetProperty("First", expected);
            actual = target.First;
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PersonName Constructor
        ///</summary>
        [TestMethod()]
        public void PersonNameConstructorTest()
        {
            string first = "First";
            string firstActual;
            string last = "Last";
            string lastActual;
            string nick = "Nick";
            string nickActual;
            PersonName target = new PersonName(first, last, nick);
            firstActual = target.First;
            lastActual = target.Last;
            nickActual = target.Nick;
            Assert.AreEqual(first, firstActual);
            Assert.AreEqual(last, lastActual);
            Assert.AreEqual(nick, nickActual);
            //Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
