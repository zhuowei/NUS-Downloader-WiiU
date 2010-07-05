///////////////////////////////////////
// NUS Downloader: Form1.cs          //
// $Rev::                          $ //
// $Author::                       $ //
// $Date::                         $ //
///////////////////////////////////////

///////////////////////////////////////
// Copyright (C) 2010
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>
///////////////////////////////////////


using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Xml;
using System.Drawing;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.Diagnostics;


namespace NUS_Downloader
{
    public partial class Form1 : Form
    {
        private const string WII_NUS_URL = "http://nus.cdn.shop.wii.com/ccs/download/";
        private const string DSI_NUS_URL = "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/";

        private readonly string CURRENT_DIR = Directory.GetCurrentDirectory();

        // TODO: Always remember to change version!
        private string version = "v2.0 Beta";
        private WebClient generalWC = new WebClient();
        private static RijndaelManaged rijndaelCipher;
        private static bool dsidecrypt = false;

        // Cross-thread Windows Formsing
        private delegate void AddToolStripItemToStripCallback(
            int type, ToolStripMenuItem additionitem, XmlAttributeCollection attributes);
        private delegate void WriteStatusCallback(string Update);
        private delegate void BootChecksCallback();
        private delegate void SetEnableForDownloadCallback(bool enabled);
        private delegate void SetTextThreadSafeCallback(System.Windows.Forms.Control what, string setto);

        // Images do not compare unless globalized...
        private Image green = Properties.Resources.bullet_green;
        private Image orange = Properties.Resources.bullet_orange;
        private Image redorb = Properties.Resources.bullet_red;
        private Image redgreen = Properties.Resources.bullet_redgreen;
        private Image redorange = Properties.Resources.bullet_redorange;

        // Certs storage
        private byte[] cert_CA = new byte[0x400];
        private byte[] cert_CACP = new byte[0x300];
        private byte[] cert_CAXS = new byte[0x300];

        private byte[] cert_CA_sha1 = new byte[20]
                                          {
                                              0x5B, 0x7D, 0x3E, 0xE2, 0x87, 0x06, 0xAD, 0x8D, 0xA2, 0xCB, 0xD5, 0xA6, 0xB7,
                                              0x5C, 0x15, 0xD0, 0xF9, 0xB6, 0xF3, 0x18
                                          };

        private byte[] cert_CACP_sha1 = new byte[20]
                                            {
                                                0x68, 0x24, 0xD6, 0xDA, 0x4C, 0x25, 0x18, 0x4F, 0x0D, 0x6D, 0xAF, 0x6E,
                                                0xDB, 0x9C, 0x0F, 0xC5, 0x75, 0x22, 0xA4, 0x1C
                                            };

        private byte[] cert_CAXS_sha1 = new byte[20]
                                            {
                                                0x09, 0x78, 0x70, 0x45, 0x03, 0x71, 0x21, 0x47, 0x78, 0x24, 0xBC, 0x6A,
                                                0x3E, 0x5E, 0x07, 0x61, 0x56, 0x57, 0x3F, 0x8A
                                            };

        private byte[] cert_total_sha1 = new byte[20]
                                             {
                                                 0xAC, 0xE0, 0xF1, 0x5D, 0x2A, 0x85, 0x1C, 0x38, 0x3F, 0xE4, 0x65, 0x7A,
                                                 0xFC, 0x38, 0x40, 0xD6, 0xFF, 0xE3, 0x0A, 0xD0
                                             };

        private string WAD_Saveas_Filename;

        // TODO: OOP scripting
        private string script_filename;
        private bool script_mode = false;
        private string[] nusentries;

        // Proxy stuff...
        private string proxy_url;
        private string proxy_usr;
        private string proxy_pwd;

        // Database thread
        private BackgroundWorker fds;

        // Scripts Thread
        private BackgroundWorker scriptsWorker;

        // Common Key hash
        private byte[] wii_commonkey_sha1 = new byte[20]
                                                {
                                                    0xEB, 0xEA, 0xE6, 0xD2, 0x76, 0x2D, 0x4D, 0x3E, 0xA1, 0x60, 0xA6, 0xD8,
                                                    0x32, 0x7F, 0xAC, 0x9A, 0x25, 0xF8, 0x06, 0x2B
                                                };

        private byte[] wii_commonkey_sha1_asstring = new byte[20]
                                                         {
                                                             0x56, 0xdd, 0x4e, 0xb3, 0x59, 0x75, 0xc2, 0xfd, 0x5a, 0xe8,
                                                             0xba, 0x8c, 0x7d, 0x89, 0x9a, 0xc5, 0xe6, 0x17, 0x54, 0x19
                                                         };

        /*
        public struct WADHeader
        {
            public int HeaderSize;
            public int WadType;
            public int CertChainSize;
            public int Reserved;
            public int TicketSize;
            public int TMDSize;
            public int DataSize;
            public int FooterSize;
        };*/

        public struct TitleContent
        {
            public byte[] ContentID;
            public byte[] Index;
            public byte[] Type;
            public byte[] Size;
            public byte[] SHAHash;
        } ;

        public enum ContentTypes : int
        {
            Shared = 0x8001,
            Normal = 0x0001
        }

        // This is the standard entry to the GUI
        public Form1()
        {
            InitializeComponent();
            KoreaMassUpdate.DropDownItemClicked += new ToolStripItemClickedEventHandler(upditem_itemclicked);
            NTSCMassUpdate.DropDownItemClicked += new ToolStripItemClickedEventHandler(upditem_itemclicked);
            PALMassUpdate.DropDownItemClicked += new ToolStripItemClickedEventHandler(upditem_itemclicked);

            // Database BGLoader
            this.fds = new BackgroundWorker();
            this.fds.DoWork += new DoWorkEventHandler(DoAllDatabaseyStuff);
            this.fds.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoAllDatabaseyStuff_Completed);
            this.fds.ProgressChanged += new ProgressChangedEventHandler(DoAllDatabaseyStuff_ProgressChanged);
            this.fds.WorkerReportsProgress = true;

            // Scripts BGLoader
            this.scriptsWorker = new BackgroundWorker();
            this.scriptsWorker.DoWork += new DoWorkEventHandler(OrganizeScripts);
            this.scriptsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(scriptsWorker_RunWorkerCompleted);
            RunScriptOrganizer();

            BootChecks();
        }

        // CLI Mode
        public Form1(string[] args)
        {
            InitializeComponent();
            Application.DoEvents();

            BootChecks();

            // Fix proxy entry.
            if (!(String.IsNullOrEmpty(proxy_url)))
                while (String.IsNullOrEmpty(proxy_pwd))
                    Thread.Sleep(1000);

            if ((args.Length == 1) && (File.Exists(args[0])))
            {
                script_filename = args[0];
                BackgroundWorker scripter = new BackgroundWorker();
                scripter.DoWork += new DoWorkEventHandler(RunScript);
                scripter.RunWorkerAsync();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("NUSD - {0}", version); ;
            this.Size = this.MinimumSize;
            consoleCBox.SelectedIndex = 0;
        }

        private bool NUSDFileExists(string filename)
        {
            return File.Exists(Path.Combine(CURRENT_DIR, filename));
        }

        /// <summary>
        /// Checks certain file existances, etc.
        /// </summary>
        /// <returns></returns>
        private void BootChecks()
        {
            //Check if correct thread...
            if (this.InvokeRequired)
            {
                Debug.WriteLine("InvokeRequired...");
                BootChecksCallback bcc = new BootChecksCallback(BootChecks);
                this.Invoke(bcc);
                return;
            }

            // Check for Wii common key bin file...
            if (NUSDFileExists("key.bin") == false)
            {
                WriteStatus("Common Key (key.bin) missing! Decryption disabled!");
                WriteStatus(" - Try: Extras -> Retrieve Key -> Common Key");
                decryptbox.Visible = false;
            }
            else
            {
                WriteStatus("Common Key detected.");
                if ((Convert.ToBase64String(ComputeSHA(LoadCommonKey("key.bin")))) !=
                    (Convert.ToBase64String(wii_commonkey_sha1)))
                {
                    // Hmm, seems to be a bad hash
                    // Let's check if it matches the hex string version...
                    if ((Convert.ToBase64String(ComputeSHA(LoadCommonKey("key.bin")))) !=
                        (Convert.ToBase64String(wii_commonkey_sha1_asstring)))
                        WriteStatus(" - (PS: Your common key isn't hashing right!)");
                    else
                    {
                        WriteStatus(" - Converting your key.bin file to the correct format...");
                        TextReader ckreader = new StreamReader(Path.Combine(CURRENT_DIR, "key.bin"));
                        String ckashex = ckreader.ReadLine();
                        ckreader.Close();
                        File.Delete(Path.Combine(CURRENT_DIR, "key.bin"));
                        WriteCommonKey("key.bin", HexStringToByteArray(ckashex));
                    }
                }
            }

            // Check for Wii KOR common key bin file...
            if (NUSDFileExists("kkey.bin") == true)
            {
                WriteStatus("Korean Common Key detected.");
            }

            // Check for DSi common key bin file...
            if (NUSDFileExists("dsikey.bin") == true)
            {
                WriteStatus("DSi Common Key detected.");
                dsidecrypt = true;
            }

            // Check for database.xml
            if (NUSDFileExists("database.xml") == false)
            {
                WriteStatus("Database.xml not found. Title database not usable!");
                /*databaseButton.Click -= new System.EventHandler(this.button4_Click);
                databaseButton.Click += new System.EventHandler(this.updateDatabaseToolStripMenuItem_Click);
                databaseButton.Text = "Download DB"; */
                DatabaseEnabled(false);
                updateDatabaseToolStripMenuItem.Enabled = true;
                updateDatabaseToolStripMenuItem.Text = "Download Database";
            }
            else
            {
                string version = GetDatabaseVersion("database.xml");
                WriteStatus("Database.xml detected.");
                WriteStatus(" - Version: " + version);
                updateDatabaseToolStripMenuItem.Text = "Update Database";
                databaseButton.Enabled = false;
                databaseButton.Text = "DB Loading";
                // Load it up...
                this.fds.RunWorkerAsync();
            }

            // Check for Proxy Settings file...
            if (NUSDFileExists("proxy.txt") == true)
            {
                WriteStatus("Proxy settings detected.");
                string[] proxy_file = File.ReadAllLines(Path.Combine(CURRENT_DIR, "proxy.txt"));
                proxy_url = proxy_file[0];

                // If proxy\nuser\npassword
                if (proxy_file.Length > 2)
                {
                    proxy_usr = proxy_file[1];
                    proxy_pwd = proxy_file[2];
                }
                else if (proxy_file.Length > 1)
                {
                    proxy_usr = proxy_file[1];
                    SetAllEnabled(false);
                    ProxyVerifyBox.Visible = true;
                    ProxyVerifyBox.Enabled = true;
                    ProxyPwdBox.Enabled = true;
                    SaveProxyBtn.Enabled = true;
                    ProxyVerifyBox.Select();
                }
            }
        }

        private void DoAllDatabaseyStuff(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ClearDatabaseStrip();
            FillDatabaseStrip(worker);
            LoadRegionCodes();
            ShowInnerToolTips(false);
        }

        private void DoAllDatabaseyStuff_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.databaseButton.Enabled = true;
            this.databaseButton.Text = "Database...";
            if (this.KoreaMassUpdate.HasDropDownItems || this.PALMassUpdate.HasDropDownItems || this.NTSCMassUpdate.HasDropDownItems)
            {
                this.scriptsbutton.Enabled = true;
            }
        }

        private void DoAllDatabaseyStuff_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.databaseButton.Text = "DB: " + e.ProgressPercentage + "%";
        }

        private void RunScriptOrganizer()
        {
            this.scriptsWorker.RunWorkerAsync();
        }

        private void SetAllEnabled(bool enabled)
        {
            for (int a = 0; a < this.Controls.Count; a++)
            {
                try
                {
                    this.Controls[a].Enabled = enabled;
                }
                catch
                {
                    // ...
                }
            }
        }

        /// <summary>
        /// Gets the database version.
        /// </summary>
        /// <param name="file">The database file.</param>
        /// <returns></returns>
        private string GetDatabaseVersion(string file)
        {
            // Read version of Database.xml
            XmlDocument xDoc = new XmlDocument();
            if (file.Contains("<"))
                xDoc.LoadXml(file);
            else
            {
                if (File.Exists(file))
                {
                    xDoc.Load(file);
                }
                else
                {
                    return "None Found";
                }
            }
            XmlNodeList DatabaseList = xDoc.GetElementsByTagName("database");
            XmlAttributeCollection Attributes = DatabaseList[0].Attributes;
            return Attributes[0].Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Show extras menu
            extrasStrip.Show(Extrasbtn, 2, 2);
        }

        /// <summary>
        /// Loads the title info from TMD.
        /// </summary>
        private void LoadTitleFromTMD()
        {
            // Show dialog for opening TMD file...
            OpenFileDialog opentmd = new OpenFileDialog();
            opentmd.Filter = "TMD Files|tmd";
            opentmd.Title = "Open TMD";
            if (opentmd.ShowDialog() != DialogResult.Cancel)
            {
                // Read the tmd as a stream...
                byte[] tmd = FileLocationToByteArray(opentmd.FileName);
                WriteStatus("TMD Loaded (" + tmd.Length + " bytes)");

                // Read ID...
                for (int x = 396; x < 404; x++)
                {
                    titleidbox.Text += MakeProperLength(ConvertToHex(Convert.ToString(tmd[x])));
                }
                WriteStatus("Title ID: " + titleidbox.Text);

                // Show TitleID Type/likelyhood of NUS existance...
                ReadIDType(titleidbox.Text);

                // Read Title Version...
                string tmdversion = "";
                for (int x = 476; x < 478; x++)
                {
                    tmdversion += MakeProperLength(ConvertToHex(Convert.ToString(tmd[x])));
                }
                titleversion.Text = Convert.ToString(int.Parse(tmdversion, System.Globalization.NumberStyles.HexNumber));

                // Read System Version (Needed IOS)
                string sysversion = IOSNeededFromTMD(tmd);
                if (sysversion != "0")
                    WriteStatus("Requires: IOS" + sysversion);

                // Read Content #...
                int nbr_cont = ContentCount(tmd);
                /*string contentstrnum = "";
                for (int x = 478; x < 480; x++)
                {
                    contentstrnum += TrimLeadingZeros(Convert.ToString(tmd[x]));
                }*/
                WriteStatus("Content Count: " + nbr_cont);

                string[] tmdcontents = GetContentNames(tmd, nbr_cont);
                string[] tmdsizes = GetContentSizes(tmd, nbr_cont);
                byte[] tmdhashes = GetContentHashes(tmd, nbr_cont);
                byte[] tmdindices = GetContentIndices(tmd, nbr_cont);
                int[] tmdtypes = GetContentTypes(tmd, nbr_cont);

                // Loop through each content and display name, hash, index
                for (int i = 0; i < nbr_cont; i++)
                {
                    WriteStatus("   Content " + (i + 1) + ": " + tmdcontents[i] + " (" +
                                Convert.ToString(int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber)) +
                                " bytes)");
                    byte[] hash = new byte[20];
                    for (int x = 0; x < 20; x++)
                    {
                        hash[x] = tmdhashes[(i*20) + x];
                    }
                    WriteStatus("  - Hash: " + DisplayBytes(hash, "").Substring(0, 8) + "...");
                    WriteStatus("  - Index: " + tmdindices[i]);
                    WriteStatus("  - Shared: " + (tmdtypes[i] == 0x8001));
                }
            }
        }

        /// <summary>
        /// Returns needed IOS from TMD.
        /// </summary>
        /// <param name="tmd">The TMD.</param>
        /// <returns></returns>
        private string IOSNeededFromTMD(byte[] tmd)
        {
            string sysversion = "";
            for (int i = 0; i < 8; i++)
                sysversion += MakeProperLength(ConvertToHex(Convert.ToString(tmd[0x184 + i])));
            sysversion =
                Convert.ToString(int.Parse(sysversion.Substring(14, 2), System.Globalization.NumberStyles.HexNumber));
            return sysversion;
        }

        /// <summary>
        /// Returns content count of TMD
        /// </summary>
        /// <param name="tmd">The TMD.</param>
        /// <returns>int Count of Contents</returns>
        private int ContentCount(byte[] tmd)
        {
            // nbr_cont (0xDE) len=0x02
            int nbr_cont = 0;
            nbr_cont = (tmd[0x1DE]*256) + tmd[0x1DF];
            return nbr_cont;
        }

        /// <summary>
        /// Gets a TMD Boot Index
        /// </summary>
        /// <param name="tmd">The TMD.</param>
        /// <returns>int BootIndex</returns>
        private int GetBootIndex(byte[] tmd)
        {
            // nbr_cont (0xE0) len=0x02
            int bootidx = 0;
            bootidx = (tmd[0x1E0]*256) + tmd[0x1E1];
            return bootidx;
        }

        /// <summary>
        /// Sets the Boot index of a TMD.
        /// </summary>
        /// <param name="tmd">The TMD.</param>
        /// <param name="bootindex">Index to set it too</param>
        /// <returns>Edited TMD</returns>
        private byte[] SetBootIndex(byte[] tmd, int bootindex)
        {
            // nbr_cont (0xE0) len=0x02
            byte[] bootbytes = NewIntegertoByteArray(bootindex, 2);
            tmd[0x1E0] = bootbytes[0];
            tmd[0x1E1] = bootbytes[1];
            return tmd;
        }

        /// <summary>
        /// Writes the status to the statusbox.
        /// </summary>
        /// <param name="Update">The update.</param>
        public void WriteStatus(string Update)
        {
            // Check if thread-safe
            if (this.InvokeRequired)
            {
                Debug.WriteLine("InvokeRequired...");
                WriteStatusCallback wsc = new WriteStatusCallback(WriteStatus);
                this.Invoke(wsc, new object[] {Update});
                return;
            }
            // Small function for writing text to the statusbox...
            if (statusbox.Text == "")
                statusbox.Text = Update;
            else
                statusbox.Text += "\r\n" + Update;

            // Scroll to end of text box.
            statusbox.SelectionStart = statusbox.TextLength;
            statusbox.ScrollToCaret();
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length*2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte) nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        /// <summary>
        /// Makes a hex string the correct length.
        /// </summary>
        /// <param name="hex">The hex.</param>
        /// <returns></returns>
        private string MakeProperLength(string hex)
        {
            // If hex is like, 'A', makes it '0A', etc.
            if (hex.Length == 1)
                hex = "0" + hex;

            return hex;
        }

        /// <summary>
        /// Converts to hex.
        /// </summary>
        /// <param name="decval">The string.</param>
        /// <returns>hex string</returns>
        private string ConvertToHex(string decval)
        {
            // Convert text string to unsigned integer
            int uiDecimal = System.Convert.ToInt32(decval);
            return String.Format("{0:x2}", uiDecimal);
        }

        /// <summary>
        /// Reads the type of the Title ID.
        /// </summary>
        /// <param name="ttlid">The TitleID.</param>
        private void ReadIDType(string ttlid)
        {
            /*  Wiibrew TitleID Info...
                # 3 00000001: Essential system titles
                # 4 00010000 and 00010004 : Disc-based games
                # 5 00010001: Downloaded channels

                    * 5.1 000010001-Cxxx : Commodore 64 Games
                    * 5.2 000010001-Exxx : NeoGeo Games
                    * 5.3 000010001-Fxxx : NES Games
                    * 5.4 000010001-Hxxx : Channels
                    * 5.5 000010001-Jxxx : SNES Games
                    * 5.6 000010001-Nxxx : Nintendo 64 Games
                    * 5.7 000010001-Wxxx : WiiWare

                # 6 00010002: System channels
                # 7 00010004: Game channels and games that use them
                # 8 00010005: Downloaded Game Content
                # 9 00010008: "Hidden" channels
             */

            if (ttlid.Substring(0, 8) == "00000001")
                WriteStatus("ID Type: System Title. BE CAREFUL!");
            else if ((ttlid.Substring(0, 8) == "00010000") || (ttlid.Substring(0, 8) == "00010004"))
                WriteStatus("ID Type: Disc-Based Game. Unlikely NUS Content!");
            else if (ttlid.Substring(0, 8) == "00010001")
                WriteStatus("ID Type: Downloaded Channel. Possible NUS Content.");
            else if (ttlid.Substring(0, 8) == "00010002")
                WriteStatus("ID Type: System Channel. BE CAREFUL!");
            else if (ttlid.Substring(0, 8) == "00010004")
                WriteStatus("ID Type: Game Channel. Unlikely NUS Content!");
            else if (ttlid.Substring(0, 8) == "00010005")
                WriteStatus("ID Type: Downloaded Game Content. Unlikely NUS Content!");
            else if (ttlid.Substring(0, 8) == "00010008")
                WriteStatus("ID Type: 'Hidden' Channel. Unlikely NUS Content!");
            else
                WriteStatus("ID Type: Unknown. Unlikely NUS Content!");
        }

        /// <summary>
        /// Trims the leading zeros of a string.
        /// </summary>
        /// <param name="num">The string with leading zeros.</param>
        /// <returns>no-0-string</returns>
        private string TrimLeadingZeros(string num)
        {
            int startindex = 0;
            for (int i = 0; i < num.Length; i++)
            {
                if ((num[i] == 0) || (num[i] == '0'))
                    startindex += 1;
                else
                    break;
            }

            return num.Substring(startindex, (num.Length - startindex));
        }

        /// <summary>
        /// Gets the content names in a TMD.
        /// </summary>
        /// <param name="tmdfile">The TMD.</param>
        /// <param name="length">The TMD contentcount.</param>
        /// <returns>Array of Content names</returns>
        private string[] GetContentNames(byte[] tmdfile, int length)
        {
            string[] contentnames = new string[length];
            int startoffset = 484;

            for (int i = 0; i < length; i++)
            {
                contentnames[i] = MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 1]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 2]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 3])));
                startoffset += 36;
            }

            return contentnames;
        }

        /// <summary>
        /// Gets the content sizes in a TMD.
        /// </summary>
        /// <param name="tmdfile">The TMD.</param>
        /// <param name="length">The TMD contentcount.</param>
        /// <returns></returns>
        private string[] GetContentSizes(byte[] tmdfile, int length)
        {
            string[] contentsizes = new string[length];
            int startoffset = 492;

            for (int i = 0; i < length; i++)
            {
                contentsizes[i] = MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 1]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 2]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 3]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 4]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 5]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 6]))) +
                                  MakeProperLength(ConvertToHex(Convert.ToString(tmdfile[startoffset + 7])));
                contentsizes[i] = TrimLeadingZeros(contentsizes[i]);
                /*contentsizes[i] = Convert.ToString(tmdfile[startoffset]) +
                    Convert.ToString(tmdfile[startoffset + 1]) +
                    Convert.ToString(tmdfile[startoffset + 2]) +
                    Convert.ToString(tmdfile[startoffset + 3]) +
                    Convert.ToString(tmdfile[startoffset + 4]) +
                    Convert.ToString(tmdfile[startoffset + 5]) +
                    Convert.ToString(tmdfile[startoffset + 6]) +
                    Convert.ToString(tmdfile[startoffset + 7]);
                contentsizes[i] = TrimLeadingZeros(contentsizes[i]);  */
                startoffset += 36;
            }

            return contentsizes;
        }

        /// <summary>
        /// Gets the content hashes.
        /// </summary>
        /// <param name="tmdfile">The tmd.</param>
        /// <param name="length">The content_count.</param>
        /// <returns></returns>
        private byte[] GetContentHashes(byte[] tmdfile, int length)
        {
            byte[] contenthashes = new byte[length*20];
            int startoffset = 500;

            for (int i = 0; i < length; i++)
            {
                for (int x = 0; x < 20; x++)
                {
                    contenthashes[(i*20) + x] = tmdfile[startoffset + x];
                }
                startoffset += 36;
            }
            return contenthashes;
        }

        /// <summary>
        /// Gets the content types.
        /// </summary>
        /// <param name="tmdfile">The tmd.</param>
        /// <param name="length">The content_count.</param>
        /// <returns></returns>
        private int[] GetContentTypes(byte[] tmdfile, int length)
        {
            int[] contenttypes = new int[length];
            int startoffset = 0x1EA;

            for (int i = 0; i < length; i++)
            {
                if (tmdfile[startoffset] == 0x80)
                    contenttypes[i] = (int) ContentTypes.Shared;
                else
                    contenttypes[i] = (int) ContentTypes.Normal;
                startoffset += 36;
            }

            return contenttypes;
        }

        /// <summary>
        /// Gets the content indices.
        /// </summary>
        /// <param name="tmdfile">The tmd.</param>
        /// <param name="length">The contentcount.</param>
        /// <returns></returns>
        private byte[] GetContentIndices(byte[] tmdfile, int length)
        {
            byte[] contentindices = new byte[length];
            int startoffset = 0x1E9;

            for (int i = 0; i < length; i++)
            {
                contentindices[i] = tmdfile[startoffset];
                startoffset += 36;
            }

            return contentindices;
        }

        private void button3_Click(object sender, EventArgs e)
        {            if (titleidbox.Text == String.Empty)
            {
                // Prevent mass deletion and fail
                WriteStatus("Please enter a Title ID!");
                return;
            }
            else if (!(packbox.Checked) && !(decryptbox.Checked) && !(keepenccontents.Checked))
            {
                // Prevent pointless running by n00bs.
                WriteStatus("Running with your current settings will produce no output!");
                WriteStatus(" - To amend this, look below and check an output type.");
                return;
            }
            else if (!(script_mode))
            {
                try
                {
                    if (!statusbox.Lines[0].StartsWith(" ---"))
                        SetTextThreadSafe(statusbox, " --- " + titleidbox.Text + " ---");
                }
                catch // No lines present...
                {
                    SetTextThreadSafe(statusbox, " --- " + titleidbox.Text + " ---");
                }
            }
            else
                SetTextThreadSafe(statusbox, statusbox.Text + "\r\n --- " + titleidbox.Text + " ---");
           

            // Running Downloads in background so no form freezing
            NUSDownloader.RunWorkerAsync();
        }

        private void SetTextThreadSafe(System.Windows.Forms.Control what, string setto)
        {
            if (this.InvokeRequired)
            {
                SetTextThreadSafeCallback sttscb = new SetTextThreadSafeCallback(SetTextThreadSafe);
                this.Invoke(sttscb, new object[] { what, setto });
                return;
            }
            what.Text = setto;
        }

        private void NUSDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Preparations for Downloading
            Control.CheckForIllegalCrossThreadCalls = false; // this function would need major rewriting to get rid of this...
            if (!(script_mode))
                WriteStatus("Starting NUS Download. Please be patient!");
            SetEnableforDownload(false);

            downloadstartbtn.Text = "Starting NUS Download!";

            // Creates the directory 
            if (!script_mode)
                CreateTitleDirectory();

            // Wii / DSi
            bool wiimode = (consoleCBox.SelectedIndex == 0);

            string titleid = titleidbox.Text;

            // Set UserAgent to Wii value
            generalWC.Headers.Add("User-Agent", "wii libnup/1.0");

            // Proxy
            generalWC = ConfigureWithProxy(generalWC);

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = Path.Combine(CURRENT_DIR, titleid);
            else
                titledirectory = Path.Combine(CURRENT_DIR, (titleid + "v" + titleversion.Text));

            if (script_mode)
                titledirectory = Path.Combine(CURRENT_DIR, "output_" + Path.GetFileNameWithoutExtension(script_filename));

            downloadstartbtn.Text = "Prerequisites: (0/2)";

            // Windows 7?
            if (IsWin7())
            {
                // Windows 7 Taskbar progress can be used.
                dlprogress.ShowInTaskbar = true;
            }

            // Download TMD before the rest...
            string tmdfull = "tmd";
            if (titleversion.Text != "")
                tmdfull += "." + titleversion.Text;
            try
            {
                DownloadNUSFile(titleid, tmdfull, titledirectory, 0, wiimode);
            }
            catch (Exception ex)
            {
                WriteStatus("Download Failed: " + tmdfull);
                WriteStatus(" - Reason: " + ex.Message.ToString().Replace("The remote server returned an error: ", ""));
                SetEnableforDownload(true);
                downloadstartbtn.Text = "Start NUS Download!";
                dlprogress.Value = 0;
                DeleteTitleDirectory();
                return;
            }
            downloadstartbtn.Text = "Prerequisites: (1/2)";
            dlprogress.Value = 50;

            if (script_mode)
            {
                packbox.Checked = true;
                packbox_CheckedChanged("scripted", new EventArgs());
                keepenccontents.Checked = false;
                wadnamebox.Enabled = false;
            }

            // Download CETK after tmd...
            bool ticket_exists = true;
            try
            {
                DownloadNUSFile(titleid, "cetk", titledirectory, 0, wiimode);
            }
            catch (Exception ex)
            {
                //WriteStatus("Download Failed: cetk");
                /*WriteStatus("You may be able to retrieve the contents by Ignoring the Ticket (Check below)");
                SetEnableforDownload(true);
                downloadstartbtn.Text = "Start NUS Download!";
                dlprogress.Value = 0;
                DeleteTitleDirectory();
                return;*/
                
                WriteStatus("Ticket not found! Continuing, however WAD packing and decryption are not possible!");
                WriteStatus(" - Reason: " +
                            ex.Message.ToString().Replace("The remote server returned an error: ", ""));

                packbox.Checked = false;
                decryptbox.Checked = false;
                WAD_Saveas_Filename = String.Empty;
                ticket_exists = false;
            }
            downloadstartbtn.Text = "Prerequisites: (2/2)";
            dlprogress.Value = 100;

            byte[] cetkbuf = new byte[0];
            byte[] titlekey = new byte[0];
            if (ticket_exists)
            {
                // Create ticket file holder
                cetkbuf = FileLocationToByteArray(Path.Combine(titledirectory, "cetk"));

                // Obtain TitleKey
                titlekey = new byte[16];
                if (decryptbox.Checked == true)
                {
                    // Load TitleKey into it's byte[]
                    // It is currently encrypted...
                    for (int i = 0; i < 16; i++)
                    {
                        titlekey[i] = cetkbuf[0x1BF + i];
                    }

                    // IV (TITLEID+0000s)
                    byte[] iv = new byte[16];
                    for (int i = 0; i < 8; i++)
                    {
                        iv[i] = cetkbuf[0x1DC + i];
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        iv[i + 8] = 0x00;
                    }

                    // Standard/Korea Common Key
                    byte[] keyBytes;
                    if (cetkbuf[0x01F1] == 0x01)
                    {
                        WriteStatus("Key Type: Korean");
                        keyBytes = LoadCommonKey("kkey.bin");
                    }
                    else
                    {
                        WriteStatus("Key Type: Standard");
                        if (wiimode)
                            keyBytes = LoadCommonKey("key.bin");
                        else
                            keyBytes = LoadCommonKey("dsikey.bin");
                    }

                    initCrypt(iv, keyBytes);

                    WriteStatus("Title Key: " + DisplayBytes(Decrypt(titlekey), ""));
                    titlekey = Decrypt(titlekey);
                }
            }

            // Read the tmd as a stream...
            byte[] tmd = FileLocationToByteArray(Path.Combine(titledirectory, tmdfull));

            if (ticket_exists == true)
            {
                // Locate Certs **************************************
                if (!(CertsValid()))
                {
                    WriteStatus("Searching for certs...");
                    ScanForCerts(tmd);
                    ScanForCerts(cetkbuf);
                }
                else
                    WriteStatus("Using cached certs...");
                // /Locate Cert **************************************
            }

            // Read Title Version...
            string tmdversion = "";
            for (int x = 476; x < 478; x++)
            {
                tmdversion += MakeProperLength(ConvertToHex(Convert.ToString(tmd[x])));
            }
            titleversion.Text = Convert.ToString(int.Parse(tmdversion, System.Globalization.NumberStyles.HexNumber));

            //Read System Version (Needed IOS)
            string sysversion = IOSNeededFromTMD(tmd);
            if (sysversion != "0")
                WriteStatus("Requires: IOS" + sysversion);

            // Renaming would be ideal, but gives too many permission errors...
            /*if ((CURRENT_DIR + titleid + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString()) != titledirectory)
 	        {
 	                Directory.Move(titledirectory, CURRENT_DIR + titleid + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString());
 	                titledirectory = CURRENT_DIR + titleid + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString();
            } */

            // Read Content #...
            string contentstrnum = "";
            for (int x = 478; x < 480; x++)
            {
                contentstrnum += TrimLeadingZeros(Convert.ToString(tmd[x]));
            }
            WriteStatus("Content #: " + contentstrnum);
            downloadstartbtn.Text = "Content: (0/" + contentstrnum + ")";
            dlprogress.Value = 0;

            // Gather information...
            string[] tmdcontents = GetContentNames(tmd, Convert.ToInt32(contentstrnum));
            string[] tmdsizes = GetContentSizes(tmd, Convert.ToInt32(contentstrnum));
            byte[] tmdhashes = GetContentHashes(tmd, Convert.ToInt32(contentstrnum));
            byte[] tmdindices = GetContentIndices(tmd, Convert.ToInt32(contentstrnum));

            // Progress bar total size tally info...
            float totalcontentsize = 0;
            float currentcontentlocation = 0;
            for (int i = 0; i < tmdsizes.Length; i++)
            {
                totalcontentsize += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);
            }
            WriteStatus("Total Size: " + (long) totalcontentsize + " bytes");

            for (int i = 0; i < tmdcontents.Length; i++)
            {
                try
                {
                    // If it exists we leave it...
                    if ((localuse.Checked) && (File.Exists(Path.Combine(titledirectory, tmdcontents[i]))))
                    {
                        WriteStatus("Leaving local " + tmdcontents[i] + ".");
                    }
                    else
                    {
                        DownloadNUSFile(titleid, tmdcontents[i], titledirectory,
                                        int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber), wiimode);
                    }
                }
                catch (Exception ex)
                {
                    WriteStatus("Download Failed: " + tmdcontents[i]);
                    WriteStatus(" - Reason: " +
                                ex.Message.ToString().Replace("The remote server returned an error: ", ""));
                    SetEnableforDownload(true);
                    downloadstartbtn.Text = "Start NUS Download!";
                    dlprogress.Value = 0;
                    DeleteTitleDirectory();
                    return;
                }

                // Progress reporting advances...
                downloadstartbtn.Text = String.Format("Content: ({0} / {1})", (i + 1), contentstrnum);
                currentcontentlocation += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);

                // Decrypt stuff...
                if (decryptbox.Checked == true)
                {
                    // Create content file holder
                    byte[] contbuf =
                        FileLocationToByteArray(Path.Combine(titledirectory, tmdcontents[i]));

                    // IV (00+IDX+more000)
                    byte[] iv = new byte[16];
                    for (int x = 0; x < 16; x++)
                    {
                        iv[x] = 0x00;
                    }
                    iv[1] = tmdindices[i];

                    initCrypt(iv, titlekey);

                    /* Create decrypted file
                    string zeros = "000000";
                    FileStream decfs = new FileStream(titledirectory + Path.DirectorySeparatorChar.ToString() + zeros + i.ToString("X2") + ".app", FileMode.Create);
                    decfs.Write(Decrypt(contbuf), 0, int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber));
                    decfs.Close();
                    WriteStatus("  - Decrypted: " + zeros + i.ToString("X2") + ".app"); */

                    FileStream decfs =
                        new FileStream(
                            Path.Combine(titledirectory, (tmdcontents[i] + ".app")),
                            FileMode.Create);
                    decfs.Write(Decrypt(contbuf), 0, int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber));
                    decfs.Close();
                    WriteStatus("  - Decrypted: " + tmdcontents[i] + ".app");

                    // Hash Check...
                    byte[] hash = new byte[20];
                    for (int x = 0; x < 20; x++)
                    {
                        hash[x] = tmdhashes[(i*20) + x];
                    }
                    byte[] deccont = Decrypt(contbuf);
                    Array.Resize(ref deccont, int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber));
                    if ((Convert.ToBase64String(ComputeSHA(deccont))) == Convert.ToBase64String(hash))
                    {
                        WriteStatus("  - Hash Check: Pass");
                    }
                    else
                    {
                        WriteStatus("  - Hash Check: Fail");
                        WriteStatus("    - True Hash: " + DisplayBytes(hash, ""));
                        WriteStatus("    - You Have: " + DisplayBytes(ComputeSHA(Decrypt(contbuf)), ""));
                    }
                }

                dlprogress.Value = Convert.ToInt32(((currentcontentlocation/totalcontentsize)*100));
            }

            WriteStatus("NUS Download Finished.");

            if ((packbox.Checked == true) && (wiimode == true))
            {
                PackWAD(titleid, tmdfull, titledirectory);
            }

            SetEnableforDownload(true);
            downloadstartbtn.Text = "Start NUS Download!";
            dlprogress.Value = 0;

            if (IsWin7())
                dlprogress.ShowInTaskbar = false;

            if (script_mode)
                statusbox.Text = "";
        }

        private void NUSDownloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WAD_Saveas_Filename = String.Empty;
        } 

        /// <summary>
        /// Creates the title directory.
        /// </summary>
        private void CreateTitleDirectory()
        {
            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = Path.Combine(CURRENT_DIR, titleidbox.Text);
            else
                titledirectory = Path.Combine(CURRENT_DIR, (titleidbox.Text + "v" + titleversion.Text));

            // Keep local directory if present and checked out...
            if ((localuse.Checked) && (Directory.Exists(titledirectory)))
            {
                //WriteStatus("Using Local Files");
            }
            else
            {
                if (Directory.Exists(titledirectory))
                    Directory.Delete(titledirectory, true);

                Directory.CreateDirectory(titledirectory);
            }
        }

        /// <summary>
        /// Deletes the title directory.
        /// </summary>
        private void DeleteTitleDirectory()
        {
            if (script_mode)
                return;
            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = Path.Combine(CURRENT_DIR, titleidbox.Text);
            else
                titledirectory = Path.Combine(CURRENT_DIR, (titleidbox.Text + "v" + titleversion.Text));

            if (Directory.Exists(titledirectory))
                Directory.Delete(titledirectory, true);
        }

        /// <summary>
        /// Downloads the NUS file.
        /// </summary>
        /// <param name="titleid">The titleid.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="placementdir">The placementdir.</param>
        /// <param name="sizeinbytes">The sizeinbytes.</param>
        /// <param name="iswiititle">if set to <c>true</c> [iswiititle].</param>
        private void DownloadNUSFile(string titleid, string filename, string placementdir, int sizeinbytes,
                                     bool iswiititle)
        {
            // Create NUS URL...
            string nusfileurl;
            if (iswiititle)
                nusfileurl = CombinePaths(WII_NUS_URL, titleid, filename);
            else
                nusfileurl = CombinePaths(DSI_NUS_URL, titleid, filename);

            WriteStatus("Grabbing " + filename + "...");

            // State size of file...
            if (sizeinbytes != 0)
                statusbox.Text += " (" + Convert.ToString(sizeinbytes) + " bytes)";

            // Download NUS file...
            generalWC.DownloadFile(nusfileurl, Path.Combine(placementdir, filename));
        }

        private void StatusChange(string status)
        {
            WriteStatus(status);
        }

        /// <summary>
        /// Packs the WAD.
        /// </summary>
        /// <param name="titleid">The titleid.</param>
        /// <param name="tmdfilename">The tmdfilename.</param>
        /// <param name="totaldirectory">The working directory.</param>
        public void PackWAD(string titleid, string tmdfilename, string totaldirectory)
        {
            WriteStatus("Beginning WAD Pack...");

            // Create instance of WAD Packing class
            WADPacker packer = new WADPacker();
            packer.StatusChanged += WriteStatus;

            // Mash together certs into one array.
            byte[] certsbuf = new byte[0xA00];
            if (!(CertsValid()))
            {
                WriteStatus("Error: NUSD could not locate cached certs!");
                return;
            }
            for (int c = 0; c < cert_CA.Length; c++)
                certsbuf[c] = cert_CA[c];
            for (int c = 0; c < cert_CACP.Length; c++)
                certsbuf[c + 0x400] = cert_CACP[c];
            for (int c = 0; c < cert_CAXS.Length; c++)
                certsbuf[c + 0x700] = cert_CAXS[c];
            if (!(TotalCertValid(certsbuf)))
            {
                WriteStatus("Error: Cert array did not hash properly!");
                return;
            }
            packer.Certs = certsbuf;

            // Read TMD/TIK into Packer.
            packer.Ticket = FileLocationToByteArray(Path.Combine(totaldirectory, "cetk"));
            packer.TMD = FileLocationToByteArray(Path.Combine(totaldirectory, tmdfilename));

            // Get the TMD variables in here instead...
            int contentcount = ContentCount(packer.TMD);
            string[] contentnames = GetContentNames(packer.TMD, contentcount);

            packer.tmdnames = GetContentNames(packer.TMD, contentcount);
            packer.tmdsizes = GetContentSizes(packer.TMD, contentcount);

            if (script_mode)
                UpdatePackedName();

            if (wadnamebox.Text.Contains("[v]") == true)
                wadnamebox.Text = wadnamebox.Text.Replace("[v]", "v" + titleversion.Text);

            if (!(String.IsNullOrEmpty(WAD_Saveas_Filename)))
            {
                packer.FileName = System.IO.Path.GetFileName(WAD_Saveas_Filename);
                packer.Directory = WAD_Saveas_Filename.Replace(packer.FileName, "");
            }
            else
            {
                string wad_filename = Path.Combine(totaldirectory, RemoveIllegalCharacters(wadnamebox.Text));
                packer.Directory = totaldirectory;
                packer.FileName = System.IO.Path.GetFileName(wad_filename);
            }

            // Gather contents...
            byte[][] contents_array = new byte[contentcount][];
            for (int a = 0; a < contentcount; a++)
            {
                contents_array[a] = FileLocationToByteArray(Path.Combine(totaldirectory, contentnames[a]));
            }
            packer.Contents = contents_array;

            // Send operations over to the packer...
            packer.PackWAD();

            // Delete contents now...
            if (keepenccontents.Checked == false)
            {
                WriteStatus("Deleting contents...");
                File.Delete(Path.Combine(totaldirectory, tmdfilename));
                File.Delete(Path.Combine(totaldirectory, "cetk"));
                for (int a = 0; a < contentnames.Length; a++)
                    File.Delete(Path.Combine(totaldirectory, contentnames[a]));
                WriteStatus(" - Contents have been deleted.");
                string[] leftovers = Directory.GetFiles(totaldirectory);
                if (leftovers.Length <= 0)
                {
                    WriteStatus(" - Title directory was empty; Deleted.");
                    Directory.Delete(totaldirectory);
                }
                WriteStatus("All deletion completed.");
            }
        }

        private void consoleCBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (consoleCBox.SelectedIndex == 0)
            {
                // Can pack WADs / Decrypt
                packbox.Enabled = true;
                decryptbox.Enabled = true;
            }
            if (consoleCBox.SelectedIndex == 1)
            {
                // Cannot Pack WADs
                packbox.Checked = false;
                packbox.Enabled = false;

                // Can decrypt if dsikey exists...
                if (dsidecrypt == false)
                {
                    decryptbox.Checked = false;
                    decryptbox.Enabled = false;
                }

                wadnamebox.Enabled = false;
                wadnamebox.Text = "";
            }
        }

        private void packbox_CheckedChanged(object sender, EventArgs e)
        {
            if (packbox.Checked == true)
            {
                wadnamebox.Enabled = true;
                saveaswadbtn.Enabled = true;
                // Change WAD name if applicable
                UpdatePackedName();
            }
            else
            {
                wadnamebox.Enabled = false;
                saveaswadbtn.Enabled = false;
                wadnamebox.Text = String.Empty;
            }
        }

        private void titleidbox_TextChanged(object sender, EventArgs e)
        {
            UpdatePackedName();
        }

        private void titleversion_TextChanged(object sender, EventArgs e)
        {
            UpdatePackedName();
        }

        /// <summary>
        /// Inits the crypto stuffz.
        /// </summary>
        /// <param name="iv">The iv.</param>
        /// <param name="key">The key.</param>
        public void initCrypt(byte[] iv, byte[] key)
        {
            rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.None;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = iv;
        }

        /// <summary>
        /// Encrypts the specified plain bytes.
        /// </summary>
        /// <param name="plainBytes">The plain bytes.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] plainBytes)
        {
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream(plainBytes))
            {
                using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Read))
                {
                    return ReadFully(cs);
                }
            }
        }

        /// <summary>
        /// Decrypts the specified encrypted data.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] encryptedData)
        {
            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Read))
                {
                    return ReadFully(cs);
                }
            }
        }

        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// Displays the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="spacer">What separates the bytes</param>
        /// <returns></returns>
        public string DisplayBytes(byte[] bytes, string spacer)
        {
            string output = "";
            for (int i = 0; i < bytes.Length; ++i)
            {
                output += bytes[i].ToString("X2") + spacer;
            }
            return output;
        }

        /// <summary>
        /// Computes the SHA-1 Hash.
        /// </summary>
        /// <param name="data">A byte[].</param>
        /// <returns></returns>
        public static byte[] ComputeSHA(byte[] data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            return sha.ComputeHash(data);
        }

        /// <summary>
        /// Loads the common key from disc.
        /// </summary>
        /// <param name="keyfile">The keyfile filename.</param>
        /// <returns></returns>
        public byte[] LoadCommonKey(string keyfile)
        {
            if (File.Exists(Path.Combine(CURRENT_DIR, keyfile)) == true)
            {
                // Read common key byte[]
                return FileLocationToByteArray(Path.Combine(CURRENT_DIR, keyfile));
            }
            else
                return null;
        }

        /// <summary>
        /// Writes/overwrites the common key onto disc.
        /// </summary>
        /// <param name="keyfile">The keyfile filename.</param>
        /// <param name="commonkey">The byte array of the common key.</param>
        /// <returns></returns>
        public bool WriteCommonKey(string keyfile, byte[] commonkey)
        {
            if (File.Exists(Path.Combine(CURRENT_DIR, keyfile)) == true)
            {
                WriteStatus(String.Format("Overwriting old {0}...", keyfile));
            }
            try
            {
                FileStream fs = File.OpenWrite(Path.Combine(CURRENT_DIR, keyfile));
                fs.Write(commonkey, 0, commonkey.Length);
                fs.Close();
                WriteStatus(String.Format("{0} written - Reloading...", keyfile));
                return true;
            }
            catch (IOException e)
            {
                WriteStatus(String.Format("Error: Couldn't write {0}: {1}", keyfile, e.Message));
            }
            return false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Open Database button menu...
            databaseStrip.Show(databaseButton, 2, 2);
        }

        /// <summary>
        /// Clears the database strip.
        /// </summary>
        private void ClearDatabaseStrip()
        {
            SystemMenuList.DropDownItems.Clear();
            IOSMenuList.DropDownItems.Clear();
            WiiWareMenuList.DropDownItems.Clear();

            // VC Games Sections...
            C64MenuList.DropDownItems.Clear();
            NeoGeoMenuList.DropDownItems.Clear();
            NESMenuList.DropDownItems.Clear();
            SNESMenuList.DropDownItems.Clear();
            N64MenuList.DropDownItems.Clear();
            TurboGrafx16MenuList.DropDownItems.Clear();
            TurboGrafxCDMenuList.DropDownItems.Clear();
            MSXMenuList.DropDownItems.Clear();
            SegaMSMenuList.DropDownItems.Clear();
            GenesisMenuList.DropDownItems.Clear();
            VCArcadeMenuList.DropDownItems.Clear();
        }

        /// <summary>
        /// Fills the database strip with the local database.xml file.
        /// </summary>
        private void FillDatabaseStrip(BackgroundWorker worker)
        {
            // Load database.xml into memorystream to perhaps reduce disk reads?
            string databasestr = File.ReadAllText(Path.Combine(CURRENT_DIR, "database.xml"));
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] databasebytes = encoding.GetBytes(databasestr);

            // Load the memory stream
            MemoryStream databaseStream = new MemoryStream(databasebytes);
            databaseStream.Seek(0, SeekOrigin.Begin);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(databaseStream);

            // Variables
            string[] XMLNodeTypes = new string[5] {"SYS", "IOS", "VC", "WW", "UPD"};

            int totalLength = xDoc.SelectNodes("/database/*").Count;
            int rnt = 0;
            // Loop through XMLNodeTypes
            for (int i = 0; i < XMLNodeTypes.Length; i++)
            {
                XmlNodeList XMLSpecificNodeTypeList = xDoc.GetElementsByTagName(XMLNodeTypes[i]);

                for (int x = 0; x < XMLSpecificNodeTypeList.Count; x++)
                {
                    ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                    XmlAttributeCollection XMLAttributes = XMLSpecificNodeTypeList[x].Attributes;

                    string titleID = "";
                    string updateScript;
                    string descname = "";
                    bool dangerous = false;
                    bool ticket = true;

                    // Okay, so now report the progress...
                    rnt = rnt + 1;
                    float currentProgress = ((float) rnt/(float) totalLength)*(float) 100;
                    if (Convert.ToInt16(Math.Round(currentProgress))%10 == 0)
                        worker.ReportProgress(Convert.ToInt16(Math.Round(currentProgress)));

                    // Lol.
                    XmlNodeList ChildrenOfTheNode = XMLSpecificNodeTypeList[x].ChildNodes;

                    for (int z = 0; z < ChildrenOfTheNode.Count; z++)
                    {
                        switch (ChildrenOfTheNode[z].Name)
                        {
                            case "name":
                                descname = ChildrenOfTheNode[z].InnerText;
                                break;
                            case "titleID":
                                titleID = ChildrenOfTheNode[z].InnerText;
                                break;
                            case "titleIDs":
                                updateScript = ChildrenOfTheNode[z].InnerText;
                                XMLToolStripItem.AccessibleDescription = updateScript;
                                    // TODO: Find somewhere better to put this. AND FAST.
                                break;
                            case "version":
                                string[] versions = ChildrenOfTheNode[z].InnerText.Split(',');
                                // Add to region things?
                                if (XMLToolStripItem.DropDownItems.Count > 0)
                                {
                                    for (int b = 0; b < XMLToolStripItem.DropDownItems.Count; b++)
                                    {
                                        if (ChildrenOfTheNode[z].InnerText != "")
                                        {
                                            ToolStripMenuItem regitem =
                                                (ToolStripMenuItem) XMLToolStripItem.DropDownItems[b];
                                            regitem.DropDownItems.Add("Latest Version");
                                            for (int y = 0; y < versions.Length; y++)
                                            {
                                                regitem.DropDownItems.Add("v" + versions[y]);
                                            }
                                            regitem.DropDownItemClicked +=
                                                new ToolStripItemClickedEventHandler(deepitem_clicked);
                                        }
                                    }
                                }
                                else
                                {
                                    XMLToolStripItem.DropDownItems.Add("Latest Version");
                                    if (ChildrenOfTheNode[z].InnerText != "")
                                    {
                                        for (int y = 0; y < versions.Length; y++)
                                        {
                                            XMLToolStripItem.DropDownItems.Add("v" + versions[y]);
                                        }
                                    }
                                }
                                break;
                            case "region":
                                string[] regions = ChildrenOfTheNode[z].InnerText.Split(',');
                                if (ChildrenOfTheNode[z].InnerText != "")
                                {
                                    for (int y = 0; y < regions.Length; y++)
                                    {
                                        XMLToolStripItem.DropDownItems.Add(RegionFromIndex(Convert.ToInt32(regions[y]),
                                                                                           xDoc));
                                    }
                                }
                                break;
                            default:
                                break;
                            case "ticket":
                                ticket = Convert.ToBoolean(ChildrenOfTheNode[z].InnerText);
                                break;
                            case "danger":
                                dangerous = true;
                                XMLToolStripItem.ToolTipText = ChildrenOfTheNode[z].InnerText;
                                break;
                        }
                        XMLToolStripItem.Image = SelectItemImage(ticket, dangerous);

                        if (titleID != "")
                        {
                            XMLToolStripItem.Text = String.Format("{0} - {1}", titleID, descname);
                        }
                        else
                        {
                            XMLToolStripItem.Text = descname;
                        }
                    }
                    AddToolStripItemToStrip(i, XMLToolStripItem, XMLAttributes);
                }
            }
        }

        /// <summary>
        /// Adds the tool strip item to strip.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="additionitem">The additionitem.</param>
        /// <param name="attributes">The attributes.</param>
        private void AddToolStripItemToStrip(int type, ToolStripMenuItem additionitem, XmlAttributeCollection attributes)
        {
            Debug.WriteLine(String.Format("Adding item (Type: {0})...", type));
            // Check if thread-safe
            if (this.InvokeRequired)
            {
                Debug.WriteLine("InvokeRequired...");
                AddToolStripItemToStripCallback atsitsc = new AddToolStripItemToStripCallback(AddToolStripItemToStrip);
                this.Invoke(atsitsc, new object[] {type, additionitem, attributes});
                return;
            }
            // Deal with VC list depth...
            if (type == 2)
            {
                Debug.WriteLine("Adding:");
                Debug.WriteLine(additionitem);
                switch (attributes[0].Value)
                {
                    case "C64":
                        C64MenuList.DropDownItems.Add(additionitem);
                        break;
                    case "NEO":
                        NeoGeoMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "NES":
                        NESMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "SNES":
                        SNESMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "N64":
                        N64MenuList.DropDownItems.Add(additionitem);
                        break;
                    case "TG16":
                        TurboGrafx16MenuList.DropDownItems.Add(additionitem);
                        break;
                    case "TGCD":
                        TurboGrafxCDMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "MSX":
                        MSXMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "SMS":
                        SegaMSMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "GEN":
                        GenesisMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "ARC":
                        VCArcadeMenuList.DropDownItems.Add(additionitem);
                        break;
                    default:
                        break;
                }
                additionitem.DropDownItemClicked += new ToolStripItemClickedEventHandler(wwitem_regionclicked);
            }
            else if (type == 4)
            {
                // I am a brand new combine harvester
                //MassUpdateList.DropDownItems.Add(additionitem);
                switch (attributes[0].Value)
                {
                    case "KOR":
                        KoreaMassUpdate.DropDownItems.Add(additionitem);
                        KoreaMassUpdate.Enabled = true;
                        break;
                    case "PAL":
                        PALMassUpdate.DropDownItems.Add(additionitem);
                        PALMassUpdate.Enabled = true;
                        break;
                    case "NTSC":
                        NTSCMassUpdate.DropDownItems.Add(additionitem);
                        NTSCMassUpdate.Enabled = true;
                        break;
                    default:
                        Debug.WriteLine("Oops - database error");
                        return;
                }
            }
            else
            {
                // Add SYS, IOS, WW items
                // I thought using index would work in .Items, but I 
                // guess this switch will have to do...
                switch (type)
                {
                    case 0:
                        SystemMenuList.DropDownItems.Add(additionitem);
                        break;
                    case 1:
                        IOSMenuList.DropDownItems.Add(additionitem);
                        break;
                    case 3:
                        WiiWareMenuList.DropDownItems.Add(additionitem);
                        break;
                }
                additionitem.DropDownItemClicked += new ToolStripItemClickedEventHandler(sysitem_versionclicked);
            }
        }

        private void deepitem_clicked(object sender, ToolStripItemClickedEventArgs e)
        {
            titleidbox.Text = e.ClickedItem.OwnerItem.OwnerItem.Text.Substring(0, 16);
            titleidbox.Text = titleidbox.Text.Replace("XX", e.ClickedItem.OwnerItem.Text.Substring(0, 2));

            if (e.ClickedItem.Text != "Latest Version")
            {
                if (e.ClickedItem.Text.Contains("v"))
                {
                    if (e.ClickedItem.Text.Contains(" "))
                        titleversion.Text = e.ClickedItem.Text.Substring(1, e.ClickedItem.Text.IndexOf(' ') - 1);
                    else
                        titleversion.Text = e.ClickedItem.Text.Substring(1, e.ClickedItem.Text.Length - 1);
                }
            }
            else
            {
                titleversion.Text = "";
            }

            // Prepare StatusBox...
            string titlename = e.ClickedItem.OwnerItem.OwnerItem.Text.Substring(19,
                                                                                (e.ClickedItem.OwnerItem.OwnerItem.Text.
                                                                                     Length - 19));
            statusbox.Text = " --- " + titlename + " ---";

            // Check if a ticket is present...
            if ((e.ClickedItem.OwnerItem.OwnerItem.Image) == (orange) ||
                (e.ClickedItem.OwnerItem.OwnerItem.Image) == (redorange))
            {
                //ignoreticket.Checked = true;
                WriteStatus("Note: This title has no ticket and cannot be packed/decrypted!");
                packbox.Checked = false;
                decryptbox.Checked = false;
            }

            // Change WAD name if packed is already checked...
            if (packbox.Checked)
            {
                OfficialWADNaming(titlename);
            }

            // Check for danger item
            if ((e.ClickedItem.OwnerItem.OwnerItem.Image) == (redgreen) ||
                (e.ClickedItem.OwnerItem.OwnerItem.Image) == (redorange))
            {
                WriteStatus("\r\n" + e.ClickedItem.OwnerItem.OwnerItem.ToolTipText);
            }
        }

        /// <summary>
        /// Mods WAD names to be official.
        /// </summary>
        /// <param name="titlename">The titlename.</param>
        public void OfficialWADNaming(string titlename)
        {
            if (titlename == "MIOS")
                wadnamebox.Text = "RVL-mios-[v].wad";
            else if (titlename.Contains("IOS"))
                wadnamebox.Text = titlename + "-64-[v].wad";
            else if (titlename.Contains("System Menu"))
                wadnamebox.Text = "RVL-WiiSystemmenu-[v].wad";
            else if (titlename.Contains("System Menu"))
                wadnamebox.Text = "RVL-WiiSystemmenu-[v].wad";
            else if (titlename == "BC")
                wadnamebox.Text = "RVL-bc-[v].wad";
            else if (titlename.Contains("Mii Channel"))
                wadnamebox.Text = "RVL-NigaoeNR-[v].wad";
            else if (titlename.Contains("Shopping Channel"))
                wadnamebox.Text = "RVL-Shopping-[v].wad";
            else if (titlename.Contains("Weather Channel"))
                wadnamebox.Text = "RVL-Weather-[v].wad";
            else
                wadnamebox.Text = titlename + "-NUS-[v].wad";

            if (titleversion.Text != "")
                wadnamebox.Text = wadnamebox.Text.Replace("[v]", "v" + titleversion.Text);
        }

        private void wwitem_regionclicked(object sender, ToolStripItemClickedEventArgs e)
        {
            titleidbox.Text = e.ClickedItem.OwnerItem.Text.Substring(0, 16);
            titleversion.Text = "";
            titleidbox.Text = titleidbox.Text.Replace("XX", e.ClickedItem.Text.Substring(0, 2));

            // Prepare StatusBox...
            string titlename = e.ClickedItem.OwnerItem.Text.Substring(19, (e.ClickedItem.OwnerItem.Text.Length - 19));
            statusbox.Text = " --- " + titlename + " ---";

            // Check if a ticket is present...
            if ((e.ClickedItem.OwnerItem.Image) == (orange) || (e.ClickedItem.OwnerItem.Image) == (redorange))
            {
                //ignoreticket.Checked = true;
                WriteStatus("Note: This title has no ticket and cannot be packed/decrypted!");
                packbox.Checked = false;
                decryptbox.Checked = false;
            }

            // Change WAD name if packed is already checked...
            if (packbox.Checked)
            {
                OfficialWADNaming(titlename);
            }

            // Check for danger item
            if ((e.ClickedItem.OwnerItem.Image) == (redgreen) || (e.ClickedItem.OwnerItem.Image) == (redorange))
            {
                WriteStatus("\r\n" + e.ClickedItem.OwnerItem.ToolTipText);
            }
        }

        private void upditem_itemclicked(object sender, ToolStripItemClickedEventArgs e)
        {
            WriteStatus("Preparing to run download script...");
            script_mode = true;
            SetTextThreadSafe(statusbox, "");
            WriteStatus("Starting script download. Please be patient!");
            string[] NUS_Entries = e.ClickedItem.AccessibleDescription.Split('\n');
                // TODO: Find somewhere better to put this. AND FAST!
            for (int i = 0; i < NUS_Entries.Length; i++)
            {
                WriteStatus(NUS_Entries[i]);
            }
            script_filename = "\000";
            nusentries = NUS_Entries;
            BackgroundWorker scripter = new BackgroundWorker();
            scripter.DoWork += new DoWorkEventHandler(RunScript);
            scripter.RunWorkerAsync();
        }


        private void sysitem_versionclicked(object sender, ToolStripItemClickedEventArgs e)
        {
            titleidbox.Text = e.ClickedItem.OwnerItem.Text.Substring(0, 16);

            if (e.ClickedItem.Text != "Latest Version")
            {
                if (e.ClickedItem.Text.Contains("v"))
                {
                    if (e.ClickedItem.Text.Contains(" "))
                        titleversion.Text = e.ClickedItem.Text.Substring(1, e.ClickedItem.Text.IndexOf(' ') - 1);
                    else
                        titleversion.Text = e.ClickedItem.Text.Substring(1, e.ClickedItem.Text.Length - 1);
                }
                else
                {
                    // Apparently it's a region code..
                    titleidbox.Text = titleidbox.Text.Replace("XX", e.ClickedItem.Text.Substring(0, 2));
                    titleversion.Text = "";
                }
            }
            else
            {
                titleversion.Text = "";
            }

            // Prepare StatusBox...
            string titlename = e.ClickedItem.OwnerItem.Text.Substring(19, (e.ClickedItem.OwnerItem.Text.Length - 19));
            statusbox.Text = " --- " + titlename + " ---";

            if ((e.ClickedItem.OwnerItem.Image) == (orange) || (e.ClickedItem.OwnerItem.Image) == (redorange))
            {
                //ignoreticket.Checked = true;
                WriteStatus("Note: This title has no ticket and cannot be packed/decrypted!");
                packbox.Checked = false;
                decryptbox.Checked = false;
            }

            // Change WAD name if packed is already checked...
            if (packbox.Checked)
            {
                if (titlename.Contains("IOS"))
                    wadnamebox.Text = titlename + "-64-[v].wad";
                else
                    wadnamebox.Text = titlename + "-NUS-[v].wad";
                if (titleversion.Text != "")
                    wadnamebox.Text = wadnamebox.Text.Replace("[v]", "v" + titleversion.Text);
            }

            // Check for danger item
            if ((e.ClickedItem.OwnerItem.Image) == (redgreen) || (e.ClickedItem.OwnerItem.Image) == (redorange))
            {
                WriteStatus("\n" + e.ClickedItem.OwnerItem.ToolTipText);
            }
        }

        /// <summary>
        /// Gathers the region based on index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="databasexml">XmlDocument with database inside</param>
        /// <returns>Region desc</returns>
        private string RegionFromIndex(int index, XmlDocument databasexml)
        {
            /* Typical Region XML
             * <REGIONS>
		            <region index="0">41 (All/System)</region>
		            <region index=1>44 (German)</region>
		            <region index=2>45 (USA/NTSC)</region>
		            <region index=3>46 (French)</region>
		            <region index=4>4A (Japan)</region>
		            <region index=5>4B (Korea)</region>
		            <region index=6>4C (Japanese Import to Europe/Australia/PAL)</region>
		            <region index=7>4D (American Import to Europe/Australia/PAL)</region>
		            <region index=8>4E (Japanese Import to USA/NTSC)</region>
		            <region index=9>50 (Europe/PAL)</region>
		            <region index=10>51 (Korea w/ Japanese Language)</region>
		            <region index=11>54 (Korea w/ English Language)</region>
		            <region index=12>58 (Some Homebrew)</region>
	           </REGIONS>
            */

            XmlNodeList XMLRegionList = databasexml.GetElementsByTagName("REGIONS");
            XmlNodeList ChildrenOfTheNode = XMLRegionList[0].ChildNodes;

            // For each child node (region node)
            for (int z = 0; z < ChildrenOfTheNode.Count; z++)
            {
                // Gather attributes (index='x')
                XmlAttributeCollection XMLAttributes = ChildrenOfTheNode[z].Attributes;

                // Return value of node if index matches
                if (Convert.ToInt32(XMLAttributes[0].Value) == index)
                    return ChildrenOfTheNode[z].InnerText;
            }

            return "XX (Error)";
        }

        /// <summary>
        /// Loads the region codes.
        /// </summary>
        private void LoadRegionCodes()
        {
            // TODO: make this check InvokeRequired...
            if (this.InvokeRequired)
            {
                Debug.Write("TOLDYOUSO!");
            }
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("database.xml");

            XmlNodeList XMLRegionList = xDoc.GetElementsByTagName("REGIONS");
            XmlNodeList ChildrenOfTheNode = XMLRegionList[0].ChildNodes;

            // For each child node (region node)
            for (int z = 0; z < ChildrenOfTheNode.Count; z++)
            {
                RegionCodesList.DropDownItems.Add(ChildrenOfTheNode[z].InnerText);
            }
        }

        private void RegionCodesList_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (titleidbox.Text.Length == 16)
                titleidbox.Text = titleidbox.Text.Substring(0, 14) + e.ClickedItem.Text.Substring(0, 2);
        }

        /// <summary>
        /// Removes the illegal characters.
        /// </summary>
        /// <param name="databasestr">removes the illegal chars</param>
        /// <returns>legal string</returns>
        private static string RemoveIllegalCharacters(string databasestr)
        {
            // Database strings must contain filename-legal characters.
            foreach (char illegalchar in System.IO.Path.GetInvalidFileNameChars())
            {
                if (databasestr.Contains(illegalchar.ToString()))
                    databasestr = databasestr.Replace(illegalchar, '-');
            }
            return databasestr;
        }

        /// <summary>
        /// Increments at an index.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static byte[] incrementAtIndex(byte[] array, int index)
        {
            if (array[index] == byte.MaxValue)
            {
                array[index] = 0;
                if (index > 0)
                    incrementAtIndex(array, index - 1);
            }
            else
            {
                array[index]++;
            }

            return array;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Clear Statusbox.text
            statusbox.Text = "";
        }

        /// <summary>
        /// Makes everything disabled/enabled.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        private void SetEnableforDownload(bool enabled)
        {
            if (this.InvokeRequired)
            {
                SetEnableForDownloadCallback sefdcb = new SetEnableForDownloadCallback(SetEnableforDownload);
                this.Invoke(sefdcb, new object[] { enabled });
                return;
            }
            // Disable things the user should not mess with during download...
            downloadstartbtn.Enabled = enabled;
            titleidbox.Enabled = enabled;
            titleversion.Enabled = enabled;
            Extrasbtn.Enabled = enabled;
            databaseButton.Enabled = enabled;
            packbox.Enabled = enabled;
            localuse.Enabled = enabled;
            saveaswadbtn.Enabled = enabled;
            decryptbox.Enabled = enabled;
            keepenccontents.Enabled = enabled;
            scriptsbutton.Enabled = enabled;
            consoleCBox.Enabled = enabled;
        }

        /// <summary>
        /// Makes tooltips disappear in the database, as many contain danger tag info.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        private void ShowInnerToolTips(bool enabled)
        {
            // Force tooltips to GTFO in sub menus...
            foreach (ToolStripItem item in databaseStrip.Items)
            {
                try
                {
                    ToolStripMenuItem menuitem = (ToolStripMenuItem) item;
                    menuitem.DropDown.ShowItemToolTips = false;
                }
                catch (Exception)
                {
                    // Do nothing, some objects will not cast.
                }
            }
        }

        /// <summary>
        /// Selects the database item image.
        /// </summary>
        /// <param name="ticket">if set to <c>true</c> [ticket].</param>
        /// <param name="danger">if set to <c>true</c> [danger].</param>
        /// <returns>Correct Image</returns>
        private System.Drawing.Image SelectItemImage(bool ticket, bool danger)
        {
            // All is good, go green...
            if ((ticket) && (!danger))
                return green;

            // There's no ticket, but danger is clear...
            if ((!ticket) && (!danger))
                return orange;

            // DANGER WILL ROBINSON...
            if ((ticket) && (danger))
                return redgreen;

            // Double bad...
            if ((!ticket) && (danger))
                return redorange;

            return null;
        }

        /// <summary>
        /// Loads a file into a byte[]
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>byte[] of file contents</returns>
        private byte[] FileLocationToByteArray(string filename)
        {
            FileStream fs = File.OpenRead(filename);
            byte[] filebytearray = ReadFully(fs, 460);
            fs.Close();
            return filebytearray;
        }

        /// <summary>
        /// Updates the name of the packed WAD in the textbox.
        /// </summary>
        private void UpdatePackedName()
        {
            // Change WAD name if applicable

            string title_name = null;

            if ((titleidbox.Enabled == true || script_mode == true) && (packbox.Checked == true))
            {
                if (titleversion.Text != "")
                {
                    wadnamebox.Text = titleidbox.Text + "-NUS-v" + titleversion.Text + ".wad";
                }
                else
                {
                    wadnamebox.Text = titleidbox.Text + "-NUS-[v]" + titleversion.Text + ".wad";
                }

                if ((File.Exists("database.xml") == true) && (titleidbox.Text.Length == 16))
                    title_name = NameFromDatabase(titleidbox.Text);

                if (title_name != null)
                {
                    wadnamebox.Text = wadnamebox.Text.Replace(titleidbox.Text, title_name);
                    OfficialWADNaming(title_name);
                }
            }
            wadnamebox.Text = RemoveIllegalCharacters(wadnamebox.Text);
        }

        /// <summary>
        /// Generates a ticket from TitleKey/ID
        /// </summary>
        /// <param name="EncTitleKey">The enc title key.</param>
        /// <param name="TitleID">The title ID.</param>
        /// <returns>New Ticket</returns>
        private byte[] GenerateTicket(byte[] EncTitleKey, byte[] TitleID)
        {
            byte[] Ticket = new byte[0x2A4];

            // RSA Signature Heading...
            Ticket[1] = 0x01;
            Ticket[3] = 0x01;

            // Signature Issuer... (Root-CA00000001-XS00000003)
            byte[] SignatureIssuer = new byte[0x1A]
                                         {
                                             0x52, 0x6F, 0x6F, 0x74, 0x2D, 0x43, 0x41, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,
                                             0x30, 0x31, 0x2D, 0x58, 0x53, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x33
                                         };
            for (int a = 0; a < 0x40; a++)
            {
                Ticket[0x140 + a] = SignatureIssuer[a];
            }

            // Encrypted TitleKey...
            for (int b = 0; b < 0x10; b++)
            {
                Ticket[0x1BF + b] = EncTitleKey[b];
            }

            // Ticket ID...
            for (int c = 0; c < 0x08; c++)
            {
                Ticket[0x1D0 + c] = 0x49;
            }

            // Title ID...
            for (int d = 0; d < 0x08; d++)
            {
                Ticket[0x1DC + d] = TitleID[d];
            }

            // Misc FF...
            Ticket[0x1E4] = 0xFF;
            Ticket[0x1E5] = 0xFF;
            Ticket[0x1E6] = 0xFF;
            Ticket[0x1E7] = 0xFF;

            // Unknown 0x01...
            Ticket[0x221] = 0x01;

            // Misc FF...
            for (int e = 0; e < 0x20; e++)
            {
                Ticket[0x222 + e] = 0xFF;
            }

            return Ticket;
        }

        /// <summary>
        /// Checks for a hex string.
        /// </summary>
        /// <param name="test">The test string</param>
        /// <returns>Whether string is hex or not.</returns>
        public bool OnlyHexInString(string test)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        /// <summary>
        /// Pads to multiple of....
        /// </summary>
        /// <param name="src">The binary.</param>
        /// <param name="pad">The pad amount.</param>
        /// <returns>Padded byte[]</returns>
        private static byte[] PadToMultipleOf(byte[] src, int pad)
        {
            int len = (src.Length + pad - 1)/pad*pad;

            Array.Resize(ref src, len);
            return src;
        }

        /// <summary>
        /// Determines whether OS is win7.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if OS = win7; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsWin7()
        {
            return (Environment.OSVersion.VersionString.Contains("6.1") == true);
        }

        private byte[] NewIntegertoByteArray(int theInt, int arrayLen)
        {
            byte[] resultArray = new byte[arrayLen];

            for (int i = arrayLen - 1; i >= 0; i--)
            {
                resultArray[i] = (byte) ((theInt >> (8*i)) & 0xFF);
            }
            Array.Reverse(resultArray);

            // Fix duplication, rewrite extra to 0x00;
            if (arrayLen > 4)
            {
                for (int i = 0; i < (arrayLen - 4); i++)
                {
                    resultArray[i] = 0x00;
                }
            }
            return resultArray;
        }


        /// <summary>
        /// Does byte[] contain byte[]?
        /// </summary>
        /// <param name="bigboy">The large byte[].</param>
        /// <param name="littleman">Small byte[] which may be in large one.</param>
        /// <returns>messed up int[] with offsets.</returns>
        private int[] ByteArrayContainsByteArray(byte[] bigboy, byte[] littleman)
        {
            // bigboy.Contains(littleman);
            // returns offset          { cnt , ofst };
            int[] offset = new int[5];
            for (int a = 0; a < (bigboy.Length - littleman.Length); a++)
            {
                int matches = 0;
                for (int b = 0; b < littleman.Length; b++)
                {
                    if (bigboy[a + b] == littleman[b])
                        matches += 1;
                }
                if (matches == littleman.Length)
                {
                    offset[offset[0] + 1] = a;
                    offset[0] += 1;
                }
            }

            return offset;
        }

        private WebClient ConfigureWithProxy(WebClient client)
        {
            // Proxy
            if (!(String.IsNullOrEmpty(proxy_url)))
            {
                WebProxy customproxy = new WebProxy();
                customproxy.Address = new Uri(proxy_url);
                if (String.IsNullOrEmpty(proxy_usr))
                    customproxy.UseDefaultCredentials = true;
                else
                {
                    NetworkCredential cred = new NetworkCredential();
                    cred.UserName = proxy_usr;

                    if (!(String.IsNullOrEmpty(proxy_pwd)))
                        cred.Password = proxy_pwd;

                    customproxy.Credentials = cred;
                }
                client.Proxy = customproxy;
                WriteStatus("  - Custom proxy settings applied!");
            }
            else
            {
                client.Proxy = WebRequest.GetSystemWebProxy();
                client.UseDefaultCredentials = true;
            }
            return client;
        }

        /// <summary>
        /// Retrieves the new database via WiiBrew.
        /// </summary>
        /// <returns>Database as a String</returns>
        private void RetrieveNewDatabase(object sender, DoWorkEventArgs e)
        {
            // Retrieve Wiibrew database page source code
            WebClient databasedl = new WebClient();

            // Proxy
            databasedl = ConfigureWithProxy(databasedl);

            string wiibrewsource =
                databasedl.DownloadString("http://www.wiibrew.org/wiki/NUS_Downloader/database?cachesmash=" +
                                          System.DateTime.Now.ToString());

            // Strip out HTML
            wiibrewsource = Regex.Replace(wiibrewsource, @"<(.|\n)*?>", "");

            // Shrink to fix only the database
            string startofdatabase = "&lt;database v";
            string endofdatabase = "&lt;/database&gt;";
            wiibrewsource = wiibrewsource.Substring(wiibrewsource.IndexOf(startofdatabase),
                                                    wiibrewsource.Length - wiibrewsource.IndexOf(startofdatabase));
            wiibrewsource = wiibrewsource.Substring(0, wiibrewsource.IndexOf(endofdatabase) + endofdatabase.Length);

            // Fix ", <, >, and spaces
            wiibrewsource = wiibrewsource.Replace("&lt;", "<");
            wiibrewsource = wiibrewsource.Replace("&gt;", ">");
            wiibrewsource = wiibrewsource.Replace("&quot;", '"'.ToString());
            wiibrewsource = wiibrewsource.Replace("&nbsp;", " "); // Shouldn't occur, but they happen...

            // Return parsed xml database...
            e.Result = wiibrewsource;
        }

        private void RetrieveNewDatabase_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            string database = e.Result.ToString();
            string currentversion = GetDatabaseVersion("database.xml");
            string onlineversion = GetDatabaseVersion(database);
            WriteStatus(" - Database successfully parsed!");
            WriteStatus("   - Current Database Version: " + currentversion);
            WriteStatus("   - Online Database Version: " + onlineversion);

            if (currentversion == onlineversion)
            {
                WriteStatus(" - You have the latest database version!");
                return;
            }

            bool isCreation = false;
            if (File.Exists("database.xml"))
            {
                WriteStatus(" - Overwriting your current database.xml...");
                WriteStatus(" - The old database will become 'olddatabase.xml' in case the new one is faulty.");

                string olddatabase = File.ReadAllText("database.xml");
                File.WriteAllText("olddatabase.xml", olddatabase);
                File.Delete("database.xml");
                File.WriteAllText("database.xml", database);
            }
            else
            {
                WriteStatus(" - database.xml has been created.");
                File.WriteAllText("database.xml", database);
                isCreation = true;
            }

            // Load it up...
            this.fds.RunWorkerAsync();

            if (isCreation)
            {
                WriteStatus("Database successfully created!");
                databaseButton.Visible = true;
                databaseButton.Enabled = false;
                updateDatabaseToolStripMenuItem.Text = "Download Database";
            }
            else
            {
                WriteStatus("Database successfully updated!");
            }
        }

        private void updateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusbox.Text = "";
            WriteStatus("Updating your database.xml from Wiibrew.org");

            BackgroundWorker dbFetcher = new BackgroundWorker();
            dbFetcher.DoWork += new DoWorkEventHandler(RetrieveNewDatabase);
            dbFetcher.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RetrieveNewDatabase_Completed);
            dbFetcher.RunWorkerAsync();
        }

        private void loadInfoFromTMDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Extras menu -> Load TMD...
            LoadTitleFromTMD();
        }

        /// <summary>
        /// Sends the SOAP request to NUS.
        /// </summary>
        /// <param name="soap_xml">The Request</param>
        /// <returns></returns>
        public string SendSOAPRequest(string soap_xml)
        {
            System.Net.HttpWebRequest req =
                (System.Net.HttpWebRequest)
                System.Net.HttpWebRequest.Create("http://nus.shop.wii.com:80/nus/services/NetUpdateSOAP");
            req.Method = "POST";
            req.UserAgent = "wii libnup/1.0";
            req.Headers.Add("SOAPAction", '"' + "urn:nus.wsapi.broadon.com/" + '"');

            // Proxy
            if (!(String.IsNullOrEmpty(proxy_url)))
            {
                WebProxy customproxy = new WebProxy();
                customproxy.Address = new Uri(proxy_url);
                if (String.IsNullOrEmpty(proxy_usr))
                    customproxy.UseDefaultCredentials = true;
                else
                {
                    NetworkCredential cred = new NetworkCredential();
                    cred.UserName = proxy_usr;

                    if (!(String.IsNullOrEmpty(proxy_pwd)))
                        cred.Password = proxy_pwd;

                    customproxy.Credentials = cred;
                }
                req.Proxy = customproxy;
                WriteStatus("  - Custom proxy settings applied!");
            }
            else
            {
                req.Proxy = WebRequest.GetSystemWebProxy();
                req.UseDefaultCredentials = true;
            }

            Stream writeStream = req.GetRequestStream();

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] bytes = encoding.GetBytes(soap_xml);
            req.ContentType = "text/xml; charset=utf-8";
            //req.ContentLength = bytes.Length;
            writeStream.Write(bytes, 0, bytes.Length);
            writeStream.Close();
            Application.DoEvents();
            try
            {
                string result;
                System.Net.HttpWebResponse resp = (System.Net.HttpWebResponse) req.GetResponse();

                using (Stream responseStream = resp.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
                req.Abort();
                Application.DoEvents();
                return result;
            }
            catch (Exception ex)
            {
                req.Abort();

                return ex.Message.ToString();
            }
        }

        private void emulateUpdate_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Begin Wii System Update
            statusbox.Text = "";
            WriteStatus("Starting Wii System Update...");

            scriptsStrip.Close();

            string deviceID = "4362227770";
            string messageID = "13198105123219138";
            string attr = "2";

            string RegionID = e.ClickedItem.Text.Substring(0, 3);
            if (RegionID == "JAP") // Japan fix, only region not w/ 1st 3 letters same as ID.
                RegionID = "JPN";

            string CountryCode = RegionID.Substring(0, 2);

            /* [14:26] <Galaxy|> RegionID: USA, Country: US; 
            RegionID: JPN, Country: JP; 
            RegionID: EUR, Country: EU; 
            RegionID: KOR, Country: KO; */

            string soap_req = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"" +
                              " xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"" +
                              " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\n" +
                              "<soapenv:Body>\n<GetSystemUpdateRequest xmlns=\"urn:nus.wsapi.broadon.com\">\n" +
                              "<Version>1.0</Version>\n<MessageId>" + messageID + "</MessageId>\n<DeviceId>" + deviceID +
                              "</DeviceId>\n" +
                              "<RegionId>" + RegionID + "</RegionId>\n<CountryCode>" + CountryCode +
                              "</CountryCode>\n<TitleVersion>\n<TitleId>0000000100000001</TitleId>\n" +
                              "<Version>2</Version>\n</TitleVersion>\n<TitleVersion>\n<TitleId>0000000100000002</TitleId>\n" +
                              "<Version>33</Version>\n</TitleVersion>\n<TitleVersion>\n<TitleId>0000000100000009</TitleId>\n" +
                              "<Version>516</Version>\n</TitleVersion>\n<Attribute>" + attr +
                              "</Attribute>\n<AuditData></AuditData>\n" +
                              "</GetSystemUpdateRequest>\n</soapenv:Body>\n</soapenv:Envelope>";

            WriteStatus(" - Sending SOAP Request to NUS...");
            WriteStatus("   - Region: " + RegionID);
            string update_xml = SendSOAPRequest(soap_req);
            if (update_xml != null)
                WriteStatus("   - Recieved Update Info!");
            else
            {
                WriteStatus("   - Fail.");
                return;
            }
            WriteStatus("   - Title information:");

            string script_text = "";

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(update_xml);
            XmlNodeList TitleList = xDoc.GetElementsByTagName("TitleVersion");
            for (int a = 0; a < TitleList.Count; a++)
            {
                XmlNodeList TitleInfo = TitleList[a].ChildNodes;
                string TitleID = "";
                string Version = "";

                for (int b = 0; b < TitleInfo.Count; b++)
                {
                    switch (TitleInfo[b].Name)
                    {
                        case "TitleId":
                            TitleID = TitleInfo[b].InnerText;
                            break;
                        case "Version":
                            Version = TitleInfo[b].InnerText;
                            break;
                        default:
                            break;
                    }
                }
                WriteStatus(String.Format("    - {0} [v{1}]", TitleID, Version));

                if ((NUSDFileExists("database.xml") == true) && ((!(String.IsNullOrEmpty(NameFromDatabase(TitleID))))))
                    statusbox.Text += String.Format(" [{0}]", NameFromDatabase(TitleID));

                script_text += String.Format("{0} {1}\n", TitleID,
                                             DisplayBytes(NewIntegertoByteArray(Convert.ToInt32(Version), 2), ""));
            }

            WriteStatus(" - Outputting results to NUS script...");

            if (!(Directory.Exists(Path.Combine(CURRENT_DIR, "scripts"))))
            {
                Directory.CreateDirectory(Path.Combine(CURRENT_DIR, "scripts"));
                WriteStatus("  - Created 'scripts\' directory.");
            }
            string time = RemoveIllegalCharacters(DateTime.Now.ToShortTimeString());
            File.WriteAllText(
                String.Format(Path.Combine(CURRENT_DIR, "scripts\\{0}_Update_{1}_{2}_{3} at {4}.nus"), RegionID,
                              DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, time), script_text);
            WriteStatus(" - Script written!");
            scriptsLocalMenuEntry.Enabled = false;
            this.scriptsWorker.RunWorkerAsync();
            
            WriteStatus(" - Run this script if you feel like downloading the update!");
            // TODO: run the script...
        }

        /// <summary>
        /// Scans for certs in TMD/TIK.
        /// </summary>
        /// <param name="tmdortik">The tmdortik.</param>
        private void ScanForCerts(byte[] tmdortik)
        {
            // For some reason a few 00s are cut off, so pad it up to be safe.
            tmdortik = PadToMultipleOf(tmdortik, 16);

            // Search for cert_CACP
            if (!(tmdortik.Length < 0x300))
                for (int a = 0; a < (tmdortik.Length - 0x300); a++)
                {
                    byte[] chunk = new byte[0x300];
                    for (int b = 0; b < 0x300; b++)
                    {
                        chunk[b] = tmdortik[a + b];
                    }
                    if (Convert.ToBase64String(ComputeSHA(chunk)) == Convert.ToBase64String(cert_CACP_sha1))
                    {
                        cert_CACP = chunk;
                        WriteStatus(" - Cert CA-CP Located!");
                        break;
                    }
                }

            // Search for cert_CAXS
            if (!(tmdortik.Length < 0x300))
                for (int a = 0; a < (tmdortik.Length - 0x300); a++)
                {
                    byte[] chunk = new byte[0x300];
                    for (int b = 0; b < 0x300; b++)
                    {
                        chunk[b] = tmdortik[a + b];
                    }
                    if (Convert.ToBase64String(ComputeSHA(chunk)) == Convert.ToBase64String(cert_CAXS_sha1))
                    {
                        cert_CAXS = chunk;
                        WriteStatus(" - Cert CA-XS Located!");
                        break;
                    }
                }

            // Search for cert_CA
            if ((!(tmdortik.Length < 0x400)) &&
                ((Convert.ToBase64String(ComputeSHA(cert_CA)) != Convert.ToBase64String(cert_CA_sha1))))
            {
                for (int a = 0; a < (tmdortik.Length - 0x400); a++)
                {
                    byte[] chunk = new byte[0x400];
                    for (int b = 0; b < 0x400; b++)
                    {
                        chunk[b] = tmdortik[a + b];
                    }
                    if (Convert.ToBase64String(ComputeSHA(chunk)) == Convert.ToBase64String(cert_CA_sha1))
                    {
                        cert_CA = chunk;
                        WriteStatus(" - Cert CA Located!");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the certs are obtained.
        /// </summary>
        /// <returns></returns>
        private bool CertsValid()
        {
            if (Convert.ToBase64String(ComputeSHA(cert_CA)) != Convert.ToBase64String(cert_CA_sha1))
                return false;
            if (Convert.ToBase64String(ComputeSHA(cert_CACP)) != Convert.ToBase64String(cert_CACP_sha1))
                return false;
            if (Convert.ToBase64String(ComputeSHA(cert_CAXS)) != Convert.ToBase64String(cert_CAXS_sha1))
                return false;
            return true;
        }

        /// <summary>
        /// Checks the whole cert file for validity.
        /// </summary>
        /// <param name="cert_sys">The cert_sys.</param>
        /// <returns>Valid Cert state.</returns>
        private bool TotalCertValid(byte[] cert_sys)
        {
            if (Convert.ToBase64String(ComputeSHA(cert_sys)) != Convert.ToBase64String(cert_total_sha1))
                return false;
            return true;
        }

        /// <summary>
        /// Looks for a title's name by TitleID in Database.
        /// </summary>
        /// <param name="titleid">The titleid.</param>
        /// <returns>Existing name; else null</returns>
        private string NameFromDatabase(string titleid)
        {
            // DANGER: BAD h4x HERE!!
            // Fix MIOS/BC naming
            if (titleid == "0000000100000101")
                return "MIOS";
            else if (titleid == "0000000100000100")
                return "BC";

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("database.xml");

            // Variables
            string[] XMLNodeTypes = new string[4] {"SYS", "IOS", "VC", "WW"};

            // Loop through XMLNodeTypes
            for (int i = 0; i < XMLNodeTypes.Length; i++) // FOR THE FOUR TYPES OF NODES
            {
                XmlNodeList XMLSpecificNodeTypeList = xDoc.GetElementsByTagName(XMLNodeTypes[i]);

                for (int x = 0; x < XMLSpecificNodeTypeList.Count; x++) // FOR EACH ITEM IN THE LIST OF A NODE TYPE
                {
                    bool found_it = false;

                    // Lol.
                    XmlNodeList ChildrenOfTheNode = XMLSpecificNodeTypeList[x].ChildNodes;

                    for (int z = 0; z < ChildrenOfTheNode.Count; z++) // FOR EACH CHILD NODE
                    {
                        switch (ChildrenOfTheNode[z].Name)
                        {
                            case "titleID":
                                if (ChildrenOfTheNode[z].InnerText == titleid)
                                    found_it = true;
                                else if ((ChildrenOfTheNode[z].InnerText.Substring(0, 14) + "XX") ==
                                         (titleid.Substring(0, 14) + "XX") &&
                                         (titleid.Substring(0, 14) != "00000001000000"))
                                    found_it = true;
                                else
                                    found_it = false;
                                break;
                            default:
                                break;
                        }
                    }

                    if (found_it)
                    {
                        for (int z = 0; z < ChildrenOfTheNode.Count; z++) // FOR EACH CHILD NODE
                        {
                            switch (ChildrenOfTheNode[z].Name)
                            {
                                case "name":
                                    return ChildrenOfTheNode[z].InnerText;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void packbox_EnabledChanged(object sender, EventArgs e)
        {
            saveaswadbtn.Enabled = packbox.Enabled;
            //deletecontentsbox.Enabled = packbox.Enabled;
        }

        private void SaveProxyBtn_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(ProxyURL.Text)) && (String.IsNullOrEmpty(ProxyUser.Text)) &&
                ((File.Exists(Path.Combine(CURRENT_DIR, "proxy.txt")))))
            {
                File.Delete(Path.Combine(CURRENT_DIR, "proxy.txt"));
                proxyBox.Visible = false;
                proxy_usr = "";
                proxy_url = "";
                proxy_pwd = "";
                WriteStatus("Proxy settings deleted!");
                return;
            }
            else if ((String.IsNullOrEmpty(ProxyURL.Text)) && (String.IsNullOrEmpty(ProxyUser.Text)) &&
                     ((!(File.Exists(Path.Combine(CURRENT_DIR, "proxy.txt"))))))
            {
                proxyBox.Visible = false;
                WriteStatus("No proxy settings saved!");
                return;
            }

            string proxy_file = "";

            if (!(String.IsNullOrEmpty(ProxyURL.Text)))
            {
                proxy_file += ProxyURL.Text + "\n";
                proxy_url = ProxyURL.Text;
            }

            if (!(String.IsNullOrEmpty(ProxyUser.Text)))
            {
                proxy_file += ProxyUser.Text;
                proxy_usr = ProxyUser.Text;
            }

            if (!(String.IsNullOrEmpty(proxy_file)))
            {
                File.WriteAllText(Path.Combine(CURRENT_DIR, "proxy.txt"), proxy_file);
                WriteStatus("Proxy settings saved!");
            }

            proxyBox.Visible = false;

            SetAllEnabled(false);
            ProxyVerifyBox.Visible = true;
            ProxyVerifyBox.Enabled = true;
            ProxyPwdBox.Enabled = true;
            SaveProxyBtn.Enabled = true;
            ProxyVerifyBox.Select();
        }

        private void proxySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check for Proxy Settings file...
            if (File.Exists(Path.Combine(CURRENT_DIR, "proxy.txt")) == true)
            {
                string[] proxy_file = File.ReadAllLines(Path.Combine(CURRENT_DIR, "proxy.txt"));

                ProxyURL.Text = proxy_file[0];
                if (proxy_file.Length > 1)
                {
                    ProxyUser.Text = proxy_file[1];
                }
            }

            proxyBox.Visible = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            proxy_pwd = ProxyPwdBox.Text;
            ProxyVerifyBox.Visible = false;
            SetAllEnabled(true);
        }

        private void ProxyPwdBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                button18_Click("LOLWUT", EventArgs.Empty);
        }

        private void ProxyAssistBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you are behind a proxy, set these settings to get through to NUS." +
                            " If you have an alternate port for accessing your proxy, add ':' followed by the port.");
        }

        private void loadNUSScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open a NUS script.
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "NUS Scripts|*.nus|All Files|*.*";
            if (Directory.Exists(Path.Combine(CURRENT_DIR, "scripts")))
                ofd.InitialDirectory = Path.Combine(CURRENT_DIR, "scripts");
            ofd.Title = "Load a NUS/Wiimpersonator script.";

            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                script_filename = ofd.FileName;
                BackgroundWorker scripter = new BackgroundWorker();
                scripter.DoWork += new DoWorkEventHandler(RunScript);
                scripter.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Runs a NUS script (BG).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void RunScript(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            script_mode = true;
            SetTextThreadSafe(statusbox, "");
            WriteStatus("Starting script download. Please be patient!");
            if (!File.Exists(Path.Combine(CURRENT_DIR, "output_" + Path.GetFileNameWithoutExtension(script_filename))))
                Directory.CreateDirectory(Path.Combine(CURRENT_DIR, "output_" + Path.GetFileNameWithoutExtension(script_filename)));
            string[] NUS_Entries;
            if (script_filename != "\000")
            {
                NUS_Entries = File.ReadAllLines(script_filename);
            }
            else
            {
                NUS_Entries = nusentries;
            }
            WriteStatus(String.Format(" - Script loaded ({0} Titles)", NUS_Entries.Length));

            for (int a = 0; a < NUS_Entries.Length; a++)
            {
                // Download the title
                WriteStatus(String.Format("===== Running Download ({0}/{1}) =====", a + 1, NUS_Entries.Length));
                string[] title_info = NUS_Entries[a].Split(' ');
                // don't let the delete issue reappear...
                if (string.IsNullOrEmpty(title_info[0]))
                    break;
                SetTextThreadSafe(titleidbox, title_info[0]);
                SetTextThreadSafe(titleversion,
                    Convert.ToString(256*
                                     (byte.Parse(title_info[1].Substring(0, 2),
                                                 System.Globalization.NumberStyles.HexNumber))));
                SetTextThreadSafe(titleversion,
                    Convert.ToString(Convert.ToInt32(titleversion.Text) +
                                     byte.Parse(title_info[1].Substring(2, 2),
                                                System.Globalization.NumberStyles.HexNumber)));

                button3_Click("Scripter", EventArgs.Empty);

                Thread.Sleep(1000);

                while (NUSDownloader.IsBusy)
                {
                    Thread.Sleep(1000);
                }
            }
            script_mode = false;
            WriteStatus("Script completed!");
        }

        public static string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder();
            string HexAlphabet = "0123456789ABCDEF";

            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int) (B >> 4)]);
                Result.Append(HexAlphabet[(int) (B & 0xF)]);
            }

            return Result.ToString();
        }

        public static byte[] HexStringToByteArray(string Hex)
        {
            byte[] Bytes = new byte[Hex.Length/2];
            int[] HexValue = new int[]
                                 {
                                     0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x0B, 0x0C, 0x0D,
                                     0x0E, 0x0F
                                 };

            for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
            {
                Bytes[x] = (byte) (HexValue[Char.ToUpper(Hex[i + 0]) - '0'] << 4 |
                                   HexValue[Char.ToUpper(Hex[i + 1]) - '0']);
            }

            return Bytes;
        }

        private void commonKeykeybinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackgroundWorker keyFetcher = new BackgroundWorker();
            keyFetcher.DoWork += new DoWorkEventHandler(RetrieveCommonKey);
            keyFetcher.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CommonKey_Retrieved);
            keyFetcher.RunWorkerAsync("key.bin");
        }

        private void koreanKeykkeybinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackgroundWorker keyFetcher = new BackgroundWorker();
            keyFetcher.DoWork += new DoWorkEventHandler(RetrieveCommonKey);
            keyFetcher.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CommonKey_Retrieved);
            keyFetcher.RunWorkerAsync("kkey.bin");
        }

        void CommonKey_Retrieved(object sender, RunWorkerCompletedEventArgs e)
        {
            BootChecks();
        }

        void RetrieveCommonKey(object sender, DoWorkEventArgs e)
        {
            WriteStatus(String.Format("Retrieving Key ({0})...", e.Argument.ToString()));

            // Begin the epic grab for freedom
            WebClient keyclient = new WebClient();

            // Proxy
            keyclient = ConfigureWithProxy(keyclient);

            string htmlwithkey;
            if (e.Argument.ToString() == "key.bin")
            {
                htmlwithkey = keyclient.DownloadString("http://hackmii.com/2008/04/keys-keys-keys/");

                // Find our start point
                string startofcommonkey = "Common key (";
                htmlwithkey = htmlwithkey.Substring(
                    htmlwithkey.IndexOf(startofcommonkey) + startofcommonkey.Length, 32);
                WriteStatus(" - Got the Common Key as: ");
                WriteStatus("   " + htmlwithkey);
                byte[] commonkey = HexStringToByteArray(htmlwithkey);
                WriteCommonKey("key.bin", commonkey);
            }
            else if (e.Argument.ToString() == "kkey.bin")
            {
                htmlwithkey = keyclient.DownloadString("http://hackmii.com/2008/09/korean-wii/");

                // Find our start point
                string startofcommonkey = "those.</p>";
                htmlwithkey = htmlwithkey.Substring(
                    htmlwithkey.IndexOf(startofcommonkey) + startofcommonkey.Length + 6, 47);
                htmlwithkey = htmlwithkey.Replace(" ", "");
                WriteStatus(" - Got the Korean Key as: ");
                WriteStatus("   " + htmlwithkey);
                byte[] commonkey = HexStringToByteArray(htmlwithkey);
                WriteCommonKey("kkey.bin", commonkey);
            }
        }

        string CombinePaths(params string[] parts)
        {
            string result = String.Empty;
            foreach (string s in parts)
            {
                result = Path.Combine(result, s);
            }
            return result;
        }


        private void scriptsbutton_Click(object sender, EventArgs e)
        {
            // Show scripts menu
            scriptsStrip.Show(scriptsbutton, 2, 2);
        }

        private void DatabaseEnabled(bool enabled)
        {
            for (int a = 0; a < databaseStrip.Items.Count; a++)
            {
                databaseStrip.Items[a].Enabled = false;
            }
        }

        void scriptsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            scriptsLocalMenuEntry.Enabled = true;
        }

        void OrganizeScripts(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();

            if (Directory.Exists(Path.Combine(CURRENT_DIR, "scripts")) == false)
            {
                WriteStatus("Scripts directory not found...");
                WriteStatus("- Creating it.");
                Directory.CreateDirectory(Path.Combine(CURRENT_DIR, "scripts"));
            }

            // Clear any entries from previous runthrough
            if (scriptsLocalMenuEntry.DropDownItems.Count > 0)
            {
                // TODO: i suppose this is bad amiright
                Control.CheckForIllegalCrossThreadCalls = false;
                scriptsLocalMenuEntry.DropDownItems.Clear();
            }
            

            // Add directories w/ scripts in \scripts\
            foreach (string directory in Directory.GetDirectories(Path.Combine(CURRENT_DIR, "scripts"), "*", SearchOption.TopDirectoryOnly))
            {
                if (Directory.GetFiles(directory, "*.nus", SearchOption.TopDirectoryOnly).Length > 0)
                {
                    DirectoryInfo dinfo = new DirectoryInfo(directory);
                    ToolStripMenuItem folder_item = new ToolStripMenuItem();
                    folder_item.Text = dinfo.Name + Path.DirectorySeparatorChar;
                    folder_item.Image = Properties.Resources.folder_table;


                    foreach (string nusscript in Directory.GetFiles(directory, "*.nus", SearchOption.TopDirectoryOnly))
                    {
                        FileInfo finfo = new FileInfo(nusscript);
                        ToolStripMenuItem nus_script_item = new ToolStripMenuItem();
                        nus_script_item.Text = finfo.Name;
                        nus_script_item.Image = Properties.Resources.script_go;
                        folder_item.DropDownItems.Add(nus_script_item);

                            nus_script_item.Click += new EventHandler(nus_script_item_Click);
                        }

                        scriptsLocalMenuEntry.DropDownItems.Add(folder_item);
                    }
            }

            // Add scripts in \scripts\
            foreach (string nusscript in Directory.GetFiles(Path.Combine(CURRENT_DIR, "scripts"), "*.nus", SearchOption.TopDirectoryOnly))
            {
                FileInfo finfo = new FileInfo(nusscript);
                ToolStripMenuItem nus_script_item = new ToolStripMenuItem();
                nus_script_item.Text = finfo.Name;
                nus_script_item.Image = Properties.Resources.script_go;
                scriptsLocalMenuEntry.DropDownItems.Add(nus_script_item);

                nus_script_item.Click += new EventHandler(nus_script_item_Click);
            }
        }

        private void aboutNUSDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display About Text...
            statusbox.Text = "";
            WriteStatus("NUS Downloader (NUSD)");
            WriteStatus("You are running version: " + version);
            WriteStatus("This application created by WB3000");
            WriteStatus("Various sections contributed by lukegb");
            WriteStatus("");

            if (NUSDFileExists("key.bin") == false)
                WriteStatus("Wii Decryption: Need (key.bin)");
            else
                WriteStatus("Wii Decryption: OK");

            if (NUSDFileExists("kkey.bin") == false)
                WriteStatus("Wii Korea Decryption: Need (kkey.bin)");
            else
                WriteStatus("Wii Korea Decryption: OK");

            if (NUSDFileExists("dsikey.bin") == false)
                WriteStatus("DSi Decryption: Need (dsikey.bin)");
            else
                WriteStatus("DSi Decryption: OK");

            if (NUSDFileExists("database.xml") == false)
                WriteStatus("Database: Need (database.xml)");
            else
                WriteStatus("Database: OK");

            if (IsWin7())
                WriteStatus("Windows 7 Features: Enabled");

            WriteStatus("");
            WriteStatus("Special thanks to:");
            WriteStatus(" * Crediar for his wadmaker tool + source, and for the advice!");
            WriteStatus(" * SquidMan/Galaxy/comex/Xuzz for advice/sources.");
            WriteStatus(" * Pasta for database compilation assistance.");
            WriteStatus(" * #WiiDev for answering the tough questions.");
            WriteStatus(" * Anyone who helped beta test on GBATemp!");
            WriteStatus(" * Famfamfam for the Silk Icon Set.");
            WriteStatus(" * Wyatt O'Day for the Windows7ProgressBar Control.");
            WriteStatus(" * Napo7 for testing proxy usage.");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveProxyPwdPermanentBtn.Enabled = checkBox1.Checked;
        }

        private void SaveProxyPwdPermanentBtn_Click(object sender, EventArgs e)
        {
            proxy_pwd = ProxyPwdBox.Text;

            string proxy_file = File.ReadAllText(Path.Combine(CURRENT_DIR, "proxy.txt"));

            proxy_file += String.Format("\n{0}", proxy_pwd);

            File.WriteAllText(Path.Combine(CURRENT_DIR, "proxy.txt"), proxy_file);

            ProxyVerifyBox.Visible = false;
            SetAllEnabled(true);
            WriteStatus("To delete all traces of proxy settings, delete the proxy.txt file!");
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            // expand clear button
            button3.Location = new Point(194, 363);
            button3.Size = new System.Drawing.Size(68, 21);
            button3.Text = "Clear";
            button3.ImageAlign = ContentAlignment.MiddleLeft;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            // shrink clear button
            button3.Location = new Point(239, 363);
            button3.Size = new System.Drawing.Size(23, 21);
            button3.Text = String.Empty;
            button3.ImageAlign = ContentAlignment.MiddleCenter;
        }

        private void saveaswadbtn_MouseEnter(object sender, EventArgs e)
        {
            saveaswadbtn.Location = new Point(190, 433);
            saveaswadbtn.Size = new Size(72, 22);
            saveaswadbtn.Text = "Save As";
            saveaswadbtn.ImageAlign = ContentAlignment.MiddleLeft;
        }

        private void saveaswadbtn_MouseLeave(object sender, EventArgs e)
        {
            saveaswadbtn.Location = new Point(230, 433);
            saveaswadbtn.Size = new Size(32, 22);
            saveaswadbtn.Text = String.Empty;
            saveaswadbtn.ImageAlign = ContentAlignment.MiddleCenter;
        }

        void nus_script_item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            string folderpath = "";
            if (!tsmi.OwnerItem.Equals(this.scriptsLocalMenuEntry))
            {
                folderpath = Path.Combine(tsmi.OwnerItem.Text, folderpath);
            }
            folderpath = Path.Combine(this.CURRENT_DIR, Path.Combine("scripts", Path.Combine(folderpath, tsmi.Text)));
            script_filename = folderpath;
            BackgroundWorker scripter = new BackgroundWorker();
            scripter.DoWork += new DoWorkEventHandler(RunScript);
            scripter.RunWorkerAsync();
        }

        private void saveaswadbtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog wad_saveas = new SaveFileDialog();
            wad_saveas.Title = "Save WAD File...";
            wad_saveas.Filter = "WAD Files|*.wad|All Files|*.*";
            wad_saveas.AddExtension = true;
            DialogResult dres = wad_saveas.ShowDialog();
            if (dres != DialogResult.Cancel)
                WAD_Saveas_Filename = wad_saveas.FileName;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // This prevents errors when exiting before the database is parsed.
            // This is also probably not the best way to accomplish this...
            Environment.Exit(0);
        }
    }
}