using NUS_Downloader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Xml;
using System;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace NUSDTEST
{
    
    
    /// <summary>
    ///This is a test class for Form1Test and is intended
    ///to contain all Form1Test Unit Tests
    ///</summary>
    [TestClass()]
    public class Form1Test
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
        ///A test for ZeroSignature
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void ZeroSignatureTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            byte[] tmdortik = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.ZeroSignature(tmdortik);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for wwitem_regionclicked
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void wwitem_regionclickedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            ToolStripItemClickedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.wwitem_regionclicked(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteStatus
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void WriteStatusTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            string Update = string.Empty; // TODO: Initialize to an appropriate value
            target.WriteStatus(Update);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for verifyMd5Hash
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void verifyMd5HashTest()
        {
            string input = string.Empty; // TODO: Initialize to an appropriate value
            string hash = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Form1_Accessor.verifyMd5Hash(input, hash);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UpdatePackedName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void UpdatePackedNameTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            target.UpdatePackedName();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for TruchaSign
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void TruchaSignTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            byte[] tmdortik = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.TruchaSign(tmdortik);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TrimLeadingZeros
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void TrimLeadingZerosTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            string num = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.TrimLeadingZeros(num);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for titleversion_TextChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void titleversion_TextChangedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.titleversion_TextChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for titleidbox_TextChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void titleidbox_TextChangedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.titleidbox_TextChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for sysitem_versionclicked
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void sysitem_versionclickedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            ToolStripItemClickedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.sysitem_versionclicked(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for StrToByteArray
        ///</summary>
        [TestMethod()]
        public void StrToByteArrayTest()
        {
            string str = string.Empty; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = Form1.StrToByteArray(str);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowInnerToolTips
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void ShowInnerToolTipsTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            bool enabled = false; // TODO: Initialize to an appropriate value
            target.ShowInnerToolTips(enabled);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SetEnableforDownload
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void SetEnableforDownloadTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            bool enabled = false; // TODO: Initialize to an appropriate value
            target.SetEnableforDownload(enabled);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SelectItemImage
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void SelectItemImageTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            bool ticket = false; // TODO: Initialize to an appropriate value
            bool danger = false; // TODO: Initialize to an appropriate value
            Image expected = null; // TODO: Initialize to an appropriate value
            Image actual;
            actual = target.SelectItemImage(ticket, danger);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RemoveIllegalCharacters
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void RemoveIllegalCharactersTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            string databasestr = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.RemoveIllegalCharacters(databasestr);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RegionFromIndex
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void RegionFromIndexTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            XmlDocument databasexml = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.RegionFromIndex(index, databasexml);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RegionCodesList_DropDownItemClicked
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void RegionCodesList_DropDownItemClickedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            ToolStripItemClickedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.RegionCodesList_DropDownItemClicked(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReadIDType
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void ReadIDTypeTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            string ttlid = string.Empty; // TODO: Initialize to an appropriate value
            target.ReadIDType(ttlid);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReadFully
        ///</summary>
        [TestMethod()]
        public void ReadFullyTest1()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            int initialLength = 0; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = Form1.ReadFully(stream, initialLength);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ReadFully
        ///</summary>
        [TestMethod()]
        public void ReadFullyTest()
        {
            Form1 target = new Form1(); // TODO: Initialize to an appropriate value
            Stream stream = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.ReadFully(stream);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for radioButton2_CheckedChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void radioButton2_CheckedChangedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.radioButton2_CheckedChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for radioButton1_CheckedChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void radioButton1_CheckedChangedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.radioButton1_CheckedChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for PackWAD
        ///</summary>
        [TestMethod()]
        public void PackWADTest()
        {
            Form1 target = new Form1(); // TODO: Initialize to an appropriate value
            string titleid = string.Empty; // TODO: Initialize to an appropriate value
            string tmdfilename = string.Empty; // TODO: Initialize to an appropriate value
            int contentcount = 0; // TODO: Initialize to an appropriate value
            string[] contentnames = null; // TODO: Initialize to an appropriate value
            string[] contentsizes = null; // TODO: Initialize to an appropriate value
            string totaldirectory = string.Empty; // TODO: Initialize to an appropriate value
            target.PackWAD(titleid, tmdfilename, contentcount, contentnames, contentsizes, totaldirectory);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for packbox_CheckedChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void packbox_CheckedChangedTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.packbox_CheckedChanged(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for NUSDownloader_DoWork
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void NUSDownloader_DoWorkTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            DoWorkEventArgs e = null; // TODO: Initialize to an appropriate value
            target.NUSDownloader_DoWork(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for MakeProperLength
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void MakeProperLengthTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            string hex = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.MakeProperLength(hex);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadRegionCodes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void LoadRegionCodesTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            target.LoadRegionCodes();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadCommonKey
        ///</summary>
        [TestMethod()]
        public void LoadCommonKeyTest()
        {
            Form1 target = new Form1(); // TODO: Initialize to an appropriate value
            string keyfile = string.Empty; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.LoadCommonKey(keyfile);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IOSNeededFromTMD
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void IOSNeededFromTMDTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            byte[] tmd = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.IOSNeededFromTMD(tmd);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for InttoByteArray
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void InttoByteArrayTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            int inte = 0; // TODO: Initialize to an appropriate value
            int arraysize = 0; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.InttoByteArray(inte, arraysize);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for InitializeComponent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void InitializeComponentTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            target.InitializeComponent();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for initCrypt
        ///</summary>
        [TestMethod()]
        public void initCryptTest()
        {
            Form1 target = new Form1(); // TODO: Initialize to an appropriate value
            byte[] iv = null; // TODO: Initialize to an appropriate value
            byte[] key = null; // TODO: Initialize to an appropriate value
            target.initCrypt(iv, key);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for incrementAtIndex
        ///</summary>
        [TestMethod()]
        public void incrementAtIndexTest()
        {
            byte[] array = null; // TODO: Initialize to an appropriate value
            int index = 0; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = Form1.incrementAtIndex(array, index);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for HandleMismatch
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void HandleMismatchTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            int contentsize = 0; // TODO: Initialize to an appropriate value
            int actualsize = 0; // TODO: Initialize to an appropriate value
            target.HandleMismatch(contentsize, actualsize);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for getMd5Hash
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void getMd5HashTest()
        {
            string input = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Form1_Accessor.getMd5Hash(input);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetContentSizes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void GetContentSizesTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            byte[] tmdfile = null; // TODO: Initialize to an appropriate value
            int length = 0; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetContentSizes(tmdfile, length);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetContentNames
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void GetContentNamesTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            byte[] tmdfile = null; // TODO: Initialize to an appropriate value
            int length = 0; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetContentNames(tmdfile, length);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetContentIndices
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void GetContentIndicesTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            byte[] tmdfile = null; // TODO: Initialize to an appropriate value
            int length = 0; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.GetContentIndices(tmdfile, length);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetContentHashes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void GetContentHashesTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            byte[] tmdfile = null; // TODO: Initialize to an appropriate value
            int length = 0; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.GetContentHashes(tmdfile, length);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getcerts_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void getcerts_ClickTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.getcerts_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Form1_Load
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void Form1_LoadTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.Form1_Load(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FillDatabaseStrip
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void FillDatabaseStripTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            target.FillDatabaseStrip();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for FileLocationToByteArray
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void FileLocationToByteArrayTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            string filename = string.Empty; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.FileLocationToByteArray(filename);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Encrypt
        ///</summary>
        [TestMethod()]
        public void EncryptTest()
        {
            Form1 target = new Form1(); // TODO: Initialize to an appropriate value
            byte[] plainBytes = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.Encrypt(plainBytes);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DownloadNUSFile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void DownloadNUSFileTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            string titleid = string.Empty; // TODO: Initialize to an appropriate value
            string filename = string.Empty; // TODO: Initialize to an appropriate value
            string placementdir = string.Empty; // TODO: Initialize to an appropriate value
            int sizeinbytes = 0; // TODO: Initialize to an appropriate value
            bool iswiititle = false; // TODO: Initialize to an appropriate value
            target.DownloadNUSFile(titleid, filename, placementdir, sizeinbytes, iswiititle);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void DisposeTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            bool disposing = false; // TODO: Initialize to an appropriate value
            target.Dispose(disposing);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DisplayBytes
        ///</summary>
        [TestMethod()]
        public void DisplayBytesTest()
        {
            Form1 target = new Form1(); // TODO: Initialize to an appropriate value
            byte[] bytes = null; // TODO: Initialize to an appropriate value
            string spacer = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.DisplayBytes(bytes, spacer);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DeleteTitleDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void DeleteTitleDirectoryTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            target.DeleteTitleDirectory();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Decrypt
        ///</summary>
        [TestMethod()]
        public void DecryptTest()
        {
            Form1 target = new Form1(); // TODO: Initialize to an appropriate value
            byte[] encryptedData = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.Decrypt(encryptedData);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateTitleDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void CreateTitleDirectoryTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            target.CreateTitleDirectory();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ConvertToHex
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void ConvertToHexTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            string decval = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ConvertToHex(decval);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ComputeSHA
        ///</summary>
        [TestMethod()]
        public void ComputeSHATest()
        {
            byte[] data = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = Form1.ComputeSHA(data);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ClearDatabaseStrip
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void ClearDatabaseStripTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            target.ClearDatabaseStrip();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ByteBoundary
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void ByteBoundaryTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            int currentlength = 0; // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            actual = target.ByteBoundary(currentlength);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for button7_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button7_ClickTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button7_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button6_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button6_ClickTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button6_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button5_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button5_ClickTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button5_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button4_Click_1
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button4_Click_1Test()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button4_Click_1(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button4_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button4_ClickTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button4_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button3_Click_1
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button3_Click_1Test()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button3_Click_1(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button3_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button3_ClickTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button3_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button2_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button2_ClickTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button2_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button1_Click_1
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button1_Click_1Test()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button1_Click_1(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for button1_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void button1_ClickTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.button1_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for BootChecks
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void BootChecksTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.BootChecks();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddToolStripItemToStrip
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NUS Downloader.exe")]
        public void AddToolStripItemToStripTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: Initialize to an appropriate value
            int type = 0; // TODO: Initialize to an appropriate value
            ToolStripMenuItem additionitem = null; // TODO: Initialize to an appropriate value
            XmlAttributeCollection attributes = null; // TODO: Initialize to an appropriate value
            target.AddToolStripItemToStrip(type, additionitem, attributes);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Form1 Constructor
        ///</summary>
        [TestMethod()]
        public void Form1ConstructorTest1()
        {
            Form1 target = new Form1();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Form1 Constructor
        ///</summary>
        [TestMethod()]
        public void Form1ConstructorTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            Form1 target = new Form1(args);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
