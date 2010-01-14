using cs340project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTesting
{
    
    
    /// <summary>
    ///This is a test class for SerializerTest and is intended
    ///to contain all SerializerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SerializerTest
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
        ///A test for XMLSerializeObject
        ///</summary>
        [TestMethod()]
        public void XMLSerializeObjectTest()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            object obj = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Serializer.XMLSerializeObject(stream, obj);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for XMLSerialize
        ///</summary>
        public void XMLSerializeTestHelper<T>()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            T obj = default(T); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Serializer.XMLSerialize<T>(stream, obj);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void XMLSerializeTest()
        {
            XMLSerializeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for BinarySerializeObject
        ///</summary>
        [TestMethod()]
        public void BinarySerializeObjectTest()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            object obj = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Serializer.BinarySerializeObject(stream, obj);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BinarySerialize
        ///</summary>
        public void BinarySerializeTestHelper<T>()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            T obj = default(T); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Serializer.BinarySerialize<T>(stream, obj);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void BinarySerializeTest()
        {
            BinarySerializeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Serializer Constructor
        ///</summary>
        [TestMethod()]
        public void SerializerConstructorTest()
        {
            Serializer target = new Serializer();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
