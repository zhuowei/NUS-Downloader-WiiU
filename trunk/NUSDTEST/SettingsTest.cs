using NUS_Downloader.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace NUSDTEST
{
    
    
    /// <summary>
    ///This is a test class for SettingsTest and is intended
    ///to contain all SettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SettingsTest
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
        ///A test for Default
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void DefaultTest()
        {
            // Private Accessor for NUS_Downloader.Properties.Settings is not found. Please rebuild the containing project or run the Publicize.exe manually.
            Assert.Inconclusive("Private Accessor for NUS_Downloader.Properties.Settings is not found. Please rebu" +
                    "ild the containing project or run the Publicize.exe manually.");
        }

        /// <summary>
        ///A test for Settings Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void SettingsConstructorTest()
        {
            // Private Accessor for NUS_Downloader.Properties.Settings is not found. Please rebuild the containing project or run the Publicize.exe manually.
            Assert.Inconclusive("Private Accessor for NUS_Downloader.Properties.Settings is not found. Please rebu" +
                    "ild the containing project or run the Publicize.exe manually.");
        }
    }
}
