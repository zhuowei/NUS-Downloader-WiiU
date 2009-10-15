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
using wyDay.Controls;


namespace NUS_Downloader
{
    public partial class Form1 : Form
    {
        const string NUSURL = "http://nus.cdn.shop.wii.com/ccs/download/";
        const string DSiNUSURL = "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/";
        // TODO: Always remember to change version!
        string version = "v1.3 Beta";
        WebClient generalWC = new WebClient();
        static RijndaelManaged rijndaelCipher;
        static bool dsidecrypt = false;
        
        // Images do not compare unless globalized...
        Image green = Properties.Resources.bullet_green;
        Image orange = Properties.Resources.bullet_orange;
        Image redorb = Properties.Resources.bullet_red;
        Image redgreen = Properties.Resources.bullet_redgreen;
        Image redorange = Properties.Resources.bullet_redorange;

        // Certs storage
        byte[] cert_CA = new byte[0x400];
        byte[] cert_CACP = new byte[0x300];
        byte[] cert_CAXS = new byte[0x300];

        byte[] cert_CA_sha1 = new byte[20] {0x5B, 0x7D, 0x3E, 0xE2, 0x87, 0x06, 0xAD, 0x8D, 0xA2, 0xCB, 0xD5, 0xA6, 0xB7, 0x5C, 0x15, 0xD0, 0xF9, 0xB6, 0xF3, 0x18};
        byte[] cert_CACP_sha1 = new byte[20] {0x68, 0x24, 0xD6, 0xDA, 0x4C, 0x25, 0x18, 0x4F, 0x0D, 0x6D, 0xAF, 0x6E, 0xDB, 0x9C, 0x0F, 0xC5, 0x75, 0x22, 0xA4, 0x1C};
        byte[] cert_CAXS_sha1 = new byte[20] {0x09, 0x78, 0x70, 0x45, 0x03, 0x71, 0x21, 0x47, 0x78, 0x24, 0xBC, 0x6A, 0x3E, 0x5E, 0x07, 0x61, 0x56, 0x57, 0x3F, 0x8A};

        byte[] cert_total_sha1 = new byte[20] {0xAC, 0xE0, 0xF1, 0x5D, 0x2A, 0x85, 0x1C, 0x38, 0x3F, 0xE4, 0x65, 0x7A, 0xFC, 0x38, 0x40, 0xD6, 0xFF, 0xE3, 0x0A, 0xD0};

        string WAD_Saveas_Filename;

        // TODO: OOP scripting
        string script_filename;
        bool script_mode = false;

        // Proxy stuff...
        string proxy_url;
        string proxy_usr;
        string proxy_pwd;

        // Common Key hash
        byte[] wii_commonkey_sha1 = new byte[20] { 0xEB, 0xEA, 0xE6, 0xD2, 0x76, 0x2D, 0x4D, 0x3E, 0xA1, 0x60, 0xA6, 0xD8, 0x32, 0x7F, 0xAC, 0x9A, 0x25, 0xF8, 0x06, 0x2B };
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
        };

        public enum ContentTypes : int {
            Shared = 0x8001, Normal = 0x0001
        }

        // This is the standard entry to the GUI
        public Form1()
        {
            InitializeComponent();
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

               
            /*  CLI MODE DEPRECATED...
            // Vars
            bool startnow = false;
            bool endafter = false;

            // Fix'd
            localuse.Checked = false;

            // Switch through arguments
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-t":
                        if (args[i + 1].Length == 16)
                            titleidbox.Text = args[i + 1];
                        else
                        {
                            WriteStatus("Title ID: Your Doing It Wrong (c)");
                            WriteStatus("ex: -t 0000000100000002");
                        }
                        break;
                    case "-v":
                        titleversion.Text = args[i + 1];
                        break;
                    case "-s":
                        startnow = true;
                        break;
                    case "-close":
                        endafter = true;
                        break;
                    case "-d":
                        decryptbox.Checked = true;
                        break;
                    case "-ticket":
                        ignoreticket.Checked = true;
                        break;
                    case "-local":
                        localuse.Checked = true;
                        break;
                    case "-p":
                        packbox.Checked = true;
                        wadnamebox.Text = args[i + 1];
                        break;
                    case "-dsi":
                        radioButton2.Checked = true;
                        break;
                    default:
                        break;
                }
            }

            // Start doing stuff...
            if ((startnow) && (titleidbox.Text.Length != 0))
            {
                // Prevent mass deletion
                if ((titleidbox.Text == "") && (titleversion.Text == ""))
                {
                    WriteStatus("Please enter SOME info...");
                    return;
                }
                else
                {
                    if (!statusbox.Lines[0].StartsWith(" ---"))
                        statusbox.Text = " --- " + titleidbox.Text + " ---";
                }

                // Running Downloads in background so no form freezing
                NUSDownloader.RunWorkerAsync();
            }

            // Close if specified
            while (NUSDownloader.IsBusy)
            {
                Thread.Sleep(1000);
            }
            if ((NUSDownloader.IsBusy == false) && (endafter == true))
            {
                Application.Exit();
            } */
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "NUSD - " + version + " - WB3000";
            this.Size = this.MinimumSize;
        }

        /// <summary>
        /// Checks certain file existances, etc.
        /// </summary>
        /// <returns></returns>
        private void BootChecks()
        {
            // Directory stuff
            string currentdir = Application.StartupPath;

            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar.ToString())) == false)
                currentdir += Path.DirectorySeparatorChar.ToString();

            // Check for Wii common key bin file...
            if (File.Exists(currentdir + "key.bin") == false)
            {
                WriteStatus("Common Key (key.bin) missing! Decryption disabled!");
                decryptbox.Visible = false;
            }
            else
            {
                WriteStatus("Common Key detected.");
                if ((Convert.ToBase64String(ComputeSHA(LoadCommonKey("key.bin")))) != (Convert.ToBase64String(wii_commonkey_sha1)))
                    WriteStatus(" - (PS: Your common key isn't hashing right!");
            }

            // Check for Wii KOR common key bin file...
            if (File.Exists(currentdir + "kkey.bin") == true)
            {
                WriteStatus("Korean Common Key detected.");
            }

            // Check for DSi common key bin file...
            if (File.Exists(currentdir + "dsikey.bin") == true)
            {
                WriteStatus("DSi Common Key detected.");
                dsidecrypt = true;
            }

            // Check for database.xml
            if (File.Exists(currentdir + "database.xml") == false)
            {
                WriteStatus("Database.xml not found. Title database not usable!");
                databaseButton.Visible = false;
                Extrasbtn.Size = new System.Drawing.Size(134, 20);
                Extrasbtn.Anchor = AnchorStyles.Right;
            }
            else
            {
                string version = GetDatabaseVersion("database.xml");
                WriteStatus("Database.xml detected.");
                WriteStatus(" - Version: " + version);

                // Load it up...
                ClearDatabaseStrip();
                FillDatabaseStrip();
                LoadRegionCodes();
                ShowInnerToolTips(false);
            }

            // Check for Proxy Settings file...
            if (File.Exists(currentdir + "proxy.txt") == true)
            {
                WriteStatus("Proxy settings detected.");
                string[] proxy_file = File.ReadAllLines(currentdir + "proxy.txt");
                proxy_url = proxy_file[0];
                if (proxy_file.Length > 1)
                {
                    proxy_usr = proxy_file[1];
                    SetAllEnabled(false);
                    ProxyVerifyBox.Visible = true; ProxyVerifyBox.Enabled = true;
                    ProxyPwdBox.Enabled = true; SaveProxyBtn.Enabled = true;
                    ProxyVerifyBox.Select();
                }
            }
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
                xDoc.Load(file);
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
                    WriteStatus("   Content " + (i + 1) + ": " + tmdcontents[i] + " (" + Convert.ToString(int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber)) + " bytes)");
                    byte[] hash = new byte[20];
                    for (int x = 0; x < 20; x++)
                    {
                        hash[x] = tmdhashes[(i * 20) + x];
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
            {
                sysversion += MakeProperLength(ConvertToHex(Convert.ToString(tmd[0x184 + i])));
            }
            sysversion = Convert.ToString(int.Parse(sysversion.Substring(14, 2), System.Globalization.NumberStyles.HexNumber));
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
            nbr_cont = (tmd[0x1DE] * 256) + tmd[0x1DF];
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
            bootidx = (tmd[0x1E0] * 256) + tmd[0x1E1];
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
            // If we've been passed an unhelpful initial length, just
            // use 32K.
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
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
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
                    contenthashes[(i * 20) + x] = tmdfile[startoffset + x];
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
                    contenttypes[i] = (int)ContentTypes.Shared;
                else
                    contenttypes[i] = (int)ContentTypes.Normal;
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
        {
            // Prevent mass deletion
            if ((titleidbox.Text == "") && (titleversion.Text == ""))
            {
                WriteStatus("Please enter SOME info...");
                return;
            }
            else if (!(script_mode))
            {
                try
                {
                    if (!statusbox.Lines[0].StartsWith(" ---"))
                        statusbox.Text = " --- " + titleidbox.Text + " ---";
                }
                catch // No lines present...
                {
                    statusbox.Text = " --- " + titleidbox.Text + " ---";
                }
            }
            else
                statusbox.Text += "\r\n --- " + titleidbox.Text + " ---";

            // Handle SaveAs here so it shows up properly...
            if (saveaswadbox.Checked)
            {
                SaveFileDialog wad_saveas = new SaveFileDialog();
                wad_saveas.Title = "Save WAD File...";
                wad_saveas.Filter = "WAD Files|*.wad|All Files|*.*";
                wad_saveas.AddExtension = true;
                DialogResult dres = wad_saveas.ShowDialog();
                if (dres != DialogResult.Cancel)
                    WAD_Saveas_Filename = wad_saveas.FileName;
            }
            else
                WAD_Saveas_Filename = "";

            // Running Downloads in background so no form freezing
            NUSDownloader.RunWorkerAsync();
        }

        private void NUSDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Preparations for Downloading
            Control.CheckForIllegalCrossThreadCalls = false;
            if (!(script_mode))
                WriteStatus("Starting NUS Download. Please be patient!");
            SetEnableforDownload(false);
            downloadstartbtn.Text = "Starting NUS Download!";

            // Current directory...
            string currentdir = Application.StartupPath;

            if (!(currentdir.EndsWith(Path.DirectorySeparatorChar.ToString())) || !(currentdir.EndsWith(Path.AltDirectorySeparatorChar.ToString())))
                currentdir += Path.DirectorySeparatorChar.ToString();

            // Prevent crossthread issues
            string titleid = titleidbox.Text;

            // Creates the directory 
            CreateTitleDirectory();

            // Wii / DSi
            bool wiimode = radioButton1.Checked;
            
            // Set UserAgent to Wii value
            generalWC.Headers.Add("User-Agent", "wii libnup/1.0");

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
                generalWC.Proxy = customproxy;
                WriteStatus("Custom proxy settings applied!");
            }
            else
            {
                generalWC.Proxy = WebRequest.GetSystemWebProxy();
                generalWC.UseDefaultCredentials = true;
            }

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = currentdir + titleid + Path.DirectorySeparatorChar.ToString();
            else
                titledirectory = currentdir + titleid + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString();

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

            // Download CETK after tmd...
            bool ticket_exists = true;
            try
            {
                DownloadNUSFile(titleid, "cetk", titledirectory, 0, wiimode);
            }
            catch (Exception ex)
            {
                if (ignoreticket.Checked == false)
                {
                    WriteStatus("Download Failed: cetk");
                    WriteStatus(" - Reason: " + ex.Message.ToString().Replace("The remote server returned an error: ", ""));
                    WriteStatus("You may be able to retrieve the contents by Ignoring the Ticket (Check below)");
                    SetEnableforDownload(true);
                    downloadstartbtn.Text = "Start NUS Download!";
                    dlprogress.Value = 0;
                    DeleteTitleDirectory();
                    return;
                }
                else
                {
                    WriteStatus("Ticket not found! Continuing, however WAD packing and decryption are not possible!");
                    packbox.Checked = false;
                    decryptbox.Checked = false;
                    ticket_exists = false;
                }
            }
            downloadstartbtn.Text = "Prerequisites: (2/2)";
            dlprogress.Value = 100;

            byte[] cetkbuf = new byte[0];
            byte[] titlekey = new byte[0];
            if (ticket_exists)
            {
                // Create ticket file holder
                cetkbuf = FileLocationToByteArray(titledirectory + Path.DirectorySeparatorChar.ToString() + @"cetk");

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
                        keyBytes = LoadCommonKey(Path.DirectorySeparatorChar.ToString() + @"kkey.bin");
                    }
                    else
                    {
                        WriteStatus("Key Type: Standard");
                        if (wiimode)
                            keyBytes = LoadCommonKey(Path.DirectorySeparatorChar.ToString() + @"key.bin");
                        else
                            keyBytes = LoadCommonKey(Path.DirectorySeparatorChar.ToString() + @"dsikey.bin");
                    }

                    initCrypt(iv, keyBytes);

                    WriteStatus("Title Key: " + DisplayBytes(Decrypt(titlekey), ""));
                    titlekey = Decrypt(titlekey);
                }
            }

            // Read the tmd as a stream...
            byte[] tmd = FileLocationToByteArray(titledirectory + tmdfull);

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
            /*if ((currentdir + titleid + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString()) != titledirectory)
 	        {
 	                Directory.Move(titledirectory, currentdir + titleid + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString());
 	                titledirectory = currentdir + titleid + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString();
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
            WriteStatus("Total Size: " + (long)totalcontentsize + " bytes");

            for (int i = 0; i < tmdcontents.Length; i++)
            {
                try
                {
                    // If it exists we leave it...
                    if ((localuse.Checked) && (File.Exists(titledirectory + tmdcontents[i])))
                    {
                        WriteStatus("Leaving local " + tmdcontents[i] + ".");
                    }
                    else
                    {
                        DownloadNUSFile(titleid, tmdcontents[i], titledirectory, int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber), wiimode);
                    }
                }
                catch (Exception ex)
                {
                    WriteStatus("Download Failed: " + tmdcontents[i]);
                    WriteStatus(" - Reason: " + ex.Message.ToString().Replace("The remote server returned an error: ", ""));
                    SetEnableforDownload(true);
                    downloadstartbtn.Text = "Start NUS Download!";
                    dlprogress.Value = 0;
                    DeleteTitleDirectory();
                    return;
                }

                // Progress reporting advances...
                downloadstartbtn.Text = "Content: (" + (i + 1) + Path.AltDirectorySeparatorChar.ToString() + contentstrnum + ")";
                currentcontentlocation += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);

                // Decrypt stuff...
                if (decryptbox.Checked == true)
                {
                    // Create content file holder
                    byte[] contbuf = FileLocationToByteArray(titledirectory + Path.DirectorySeparatorChar.ToString() + tmdcontents[i]);

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

                    FileStream decfs = new FileStream(titledirectory + Path.DirectorySeparatorChar.ToString() + tmdcontents[i] + ".app", FileMode.Create);
                    decfs.Write(Decrypt(contbuf), 0, int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber));
                    decfs.Close();
                    WriteStatus("  - Decrypted: " + tmdcontents[i] + ".app");

                    // Hash Check...
                    byte[] hash = new byte[20];
                    for (int x = 0; x < 20; x++)
                    {
                        hash[x] = tmdhashes[(i * 20) + x];
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

                dlprogress.Value = Convert.ToInt32(((currentcontentlocation / totalcontentsize) * 100));
            }

            WriteStatus("NUS Download Finished.");

            // Trucha signing...
            if ((truchabox.Checked == true) && (wiimode == true))
            {
                // Read information from TMD into signing GUI...
                requiredIOSbox.Text = Convert.ToString(tmd[0x18B]);
                tmdversiontrucha.Text = Convert.ToString((tmd[0x1DC]*256) + tmd[0x1DD]);
                newtitleidbox.Text = titleid;

                // Add contents to contentEdit...
                FillContentInfo(tmd);

                // Setup for NO IOS
                if (requiredIOSbox.Text == "0")
                    requiredIOSbox.Enabled = false;
                else
                    requiredIOSbox.Enabled = true;

                // Read information from TIK into signing GUI...
                // Titlekey
                for (int i = 0; i < 16; i++)
                {
                    titlekey[i] = cetkbuf[0x1BF + i];
                }
                //titlekeybox.Text = DisplayBytes(titlekey).Replace(" ", "");
                titlekeybox.Text = System.Text.Encoding.UTF7.GetString(titlekey);

                // IV (TITLEID+00000000s)
                byte[] iv = new byte[16];
                for (int i = 0; i < 8; i++)
                {
                    iv[i] = cetkbuf[0x1DC + i];
                }
                for (int i = 0; i < 8; i++)
                {
                    iv[i + 8] = 0x00;
                }
                titleIDIV.Text = DisplayBytes(iv, "");

                //DLC
                dlcamntbox.Text = Convert.ToString((cetkbuf[0x1E6]*256) + cetkbuf[0x1E7]);

                //keyindex
                if (cetkbuf[0x1F1] == 0x00)
                    ckeyindexcb.SelectedIndex = 0;
                else if (cetkbuf[0x1F1] == 0x01)
                    ckeyindexcb.SelectedIndex = 1;
                else
                    ckeyindexcb.SelectedIndex = 0;

                //time enabled
                if (cetkbuf[0x247] == 0x00)
                    timelimitenabledcb.SelectedIndex = 0;
                else if (cetkbuf[0x247] == 0x01)
                    timelimitenabledcb.SelectedIndex = 1;
                else
                    timelimitenabledcb.SelectedIndex = 0;

                //time in seconds
                byte[] timelimit = new byte[4];
                for (int i = 0; i < timelimit.Length; i++)
                {
                    timelimit[i] = cetkbuf[0x248 + 1];
                }
                timelimitsecs.Text = Convert.ToString(System.BitConverter.ToInt32(timelimit, 0));


                // Resize form to max to show trucha options...
                this.Size = this.MaximumSize;

                shamelessvariablelabel.Text = String.Format("{0},{1},{2}", titledirectory, tmdfull, contentstrnum); 

                // Loop until user is finished...
                while (this.Size == this.MaximumSize)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                /* Re-Gather information...
                byte[] tmdrefresh = FileLocationToByteArray(titledirectory + tmdfull);
                tmdcontents = GetContentNames(tmd, ContentCount(tmdrefresh));
                tmdsizes = GetContentSizes(tmd, ContentCount(tmdrefresh));
                tmdhashes = GetContentHashes(tmd, ContentCount(tmdrefresh));
                tmdindices = GetContentIndices(tmd, ContentCount(tmdrefresh)); */

                /*
                WriteStatus("Trucha Signing TMD...");
                Array.Resize(ref tmd, 484 + (Convert.ToInt32(contentstrnum) * 36));

                tmd = ZeroSignature(tmd);
                tmd = TruchaSign(tmd);

                FileStream testtmd = new FileStream(titledirectory + tmdfull, FileMode.Open);
                testtmd.Write(tmd, 0, tmd.Length);
                testtmd.Close();

                WriteStatus("Trucha Signing Ticket...");

                // Create ticket file holder
                FileStream cetkf = File.OpenRead(titledirectory + @"\cetk");
                byte[] cetkbuff = ReadFully(cetkf, 20);
                cetkf.Close();

                Array.Resize(ref cetkbuff, 0x2A4);

                cetkbuff = ZeroSignature(cetkbuff);
                cetkbuff = TruchaSign(cetkbuff);

                FileStream testtik = new FileStream(titledirectory + "cetk", FileMode.Open);
                testtik.Write(cetkbuff, 0, cetkbuff.Length);
                testtik.Close(); */
            }

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

        /// <summary>
        /// Creates the title directory.
        /// </summary>
        private void CreateTitleDirectory()
        {
            // Creates the directory for the downloaded title...
            string currentdir = Application.StartupPath;
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar.ToString())) == false)
                currentdir += Path.DirectorySeparatorChar.ToString();

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = Path.Combine(currentdir, titleidbox.Text + Path.DirectorySeparatorChar.ToString());
            else
                titledirectory = Path.Combine(currentdir, titleidbox.Text + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString());

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
            string currentdir = Application.StartupPath;
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar.ToString())) == false)
                currentdir += Path.DirectorySeparatorChar.ToString();

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = Path.Combine(currentdir, titleidbox.Text + Path.DirectorySeparatorChar.ToString());
            else
                titledirectory = Path.Combine(currentdir, titleidbox.Text + "v" + titleversion.Text + Path.DirectorySeparatorChar.ToString());

            if (Directory.Exists(titledirectory))
                Directory.Delete(titledirectory, true);

            //Directory.CreateDirectory(currentdir + titleidbox.Text);
        }

        /// <summary>
        /// Downloads the NUS file.
        /// </summary>
        /// <param name="titleid">The titleid.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="placementdir">The placementdir.</param>
        /// <param name="sizeinbytes">The sizeinbytes.</param>
        /// <param name="iswiititle">if set to <c>true</c> [iswiititle].</param>
        private void DownloadNUSFile(string titleid, string filename, string placementdir, int sizeinbytes, bool iswiititle)
        {
            // Create NUS URL...
            string nusfileurl;
            if (iswiititle)
                nusfileurl = NUSURL + titleid + Path.AltDirectorySeparatorChar.ToString() + filename;
            else
                nusfileurl = DSiNUSURL + titleid + Path.AltDirectorySeparatorChar.ToString() + filename;

            WriteStatus("Grabbing " + filename + "...");

            // State size of file...
            if (sizeinbytes != 0)
                statusbox.Text += " (" + Convert.ToString(sizeinbytes) + " bytes)";

            // Download NUS file...
            generalWC.DownloadFile(nusfileurl, placementdir + filename);
        }

        void StatusChange(string status)
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

            // Obtain Current Directory
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(Path.DirectorySeparatorChar.ToString())) || !(currentdir.EndsWith(Path.AltDirectorySeparatorChar.ToString())))
                currentdir += Path.DirectorySeparatorChar.ToString();

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
            packer.Ticket = FileLocationToByteArray(totaldirectory + Path.DirectorySeparatorChar.ToString() + @"cetk");
            packer.TMD = FileLocationToByteArray(totaldirectory + Path.DirectorySeparatorChar.ToString() + tmdfilename);
            
            // Get the TMD variables in here instead...
            int contentcount = ContentCount(packer.TMD);
            string[] contentnames = GetContentNames(packer.TMD, contentcount);
           
            packer.tmdnames = GetContentNames(packer.TMD, contentcount);
            packer.tmdsizes = GetContentSizes(packer.TMD, contentcount);

            if (wadnamebox.Text.Contains("[v]") == true)
                wadnamebox.Text = wadnamebox.Text.Replace("[v]", "v" + titleversion.Text);
            
            if (!(String.IsNullOrEmpty(WAD_Saveas_Filename)))
            {
                packer.FileName = System.IO.Path.GetFileName(WAD_Saveas_Filename);
                packer.Directory = WAD_Saveas_Filename.Replace(packer.FileName, "");
            }
            else
            {
                string wad_filename = totaldirectory + Path.DirectorySeparatorChar.ToString() + RemoveIllegalCharacters(wadnamebox.Text);
                packer.Directory = totaldirectory;
                packer.FileName = System.IO.Path.GetFileName(wad_filename);
            }
            
            // Gather contents...
            byte[][] contents_array = new byte[contentcount][];
            for (int a = 0; a < contentcount; a++)
            {
                contents_array[a] = FileLocationToByteArray(totaldirectory + contentnames[a]);
            }
            packer.Contents = contents_array;
           
            // Send operations over to the packer...
            packer.PackWAD();
            
            // Delete contents now...
            if (deletecontentsbox.Checked)
            {
                WriteStatus("Deleting contents...");
                File.Delete(totaldirectory + Path.DirectorySeparatorChar.ToString() + tmdfilename);
                File.Delete(totaldirectory + Path.DirectorySeparatorChar.ToString() + @"cetk");
                for (int a = 0; a < contentnames.Length; a++)
                    File.Delete(totaldirectory + Path.DirectorySeparatorChar.ToString() + contentnames[a]);
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

        /// <summary>
        /// Returns next 0x40 padded length.
        /// </summary>
        /// <param name="currentlength">The currentlength.</param>
        /// <returns></returns>
        private long ByteBoundary(int currentlength)
        {
            // Gets the next 0x40 offset.
            long thelength = currentlength - 1;
            long remainder = 1;

            while (remainder != 0)
            {
                thelength += 1;
                remainder = thelength % 0x40;
            }

            //WriteStatus("Initial Size: " + currentlength);
            //WriteStatus("0x40 Size: " + thelength);

            return (long)thelength;
        }

        /// <summary>
        /// Int -> Byte[] (OLD)
        /// </summary>
        /// <param name="inte">The int.</param>
        /// <param name="arraysize">The array length.</param>
        /// <returns></returns>
        private byte[] InttoByteArray(int inte, int arraysize)
        {
            // Take integer and make into byte array
            byte[] b = new byte[arraysize];
            b = BitConverter.GetBytes(inte);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);

            return b;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                // Cannot Pack WADs
                packbox.Checked = false;
                packbox.Enabled = false;

                // Can decrypt if key exists...lulz
                if (dsidecrypt == false)
                {
                    decryptbox.Checked = false;
                    decryptbox.Enabled = false;
                }

                wadnamebox.Enabled = false;
                wadnamebox.Text = "";

                // Cannot doit
                truchabox.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                // Can pack WADs
                // packbox.Checked = true;
                packbox.Enabled = true;
                decryptbox.Enabled = true;
                truchabox.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Display About Text...
            statusbox.Text = "";
            WriteStatus("NUS Downloader (NUSD)");
            WriteStatus("You are running version: " + version);
            WriteStatus("This application created by WB3000");
            WriteStatus("");
            string currentdir = Application.StartupPath;
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar.ToString())) == false)
                currentdir += Path.DirectorySeparatorChar.ToString();
            if (File.Exists(currentdir + "key.bin") == false)
                WriteStatus("Wii Decryption: Need (key.bin)");
            else
                WriteStatus("Wii Decryption: OK");

            if (File.Exists(currentdir + "kkey.bin") == false)
                WriteStatus("Wii Korea Decryption: Need (kkey.bin)");
            else
                WriteStatus("Wii Korea Decryption: OK");

            if (File.Exists(currentdir + "dsikey.bin") == false)
                WriteStatus("DSi Decryption: Need (dsikey.bin)");
            else
                WriteStatus("DSi Decryption: OK");

            if (File.Exists(currentdir + "database.xml") == false)
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
        
        private void packbox_CheckedChanged(object sender, EventArgs e)
        {
            if (packbox.Checked == true)
            {
                wadnamebox.Enabled = true;
                // Change WAD name if applicable
                UpdatePackedName();
            }
            else
            {
                wadnamebox.Enabled = false;
                wadnamebox.Text = "";
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
        static public byte[] ComputeSHA(byte[] data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            return sha.ComputeHash(data);
        }

        /// <summary>
        /// Loads the common key from disc.
        /// </summary>
        /// <param name="keyfile">The keyfile.</param>
        /// <returns></returns>
        public byte[] LoadCommonKey(string keyfile)
        {
            // Directory stuff
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(Path.DirectorySeparatorChar.ToString())) || !(currentdir.EndsWith(Path.AltDirectorySeparatorChar.ToString())))
                currentdir += Path.DirectorySeparatorChar.ToString();

            if (File.Exists(currentdir + keyfile) == true)
            {
                // Read common key byte[]
                return FileLocationToByteArray(currentdir + keyfile);
            }
            else
                return null;
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
        /// Fills the database strip.
        /// </summary>
        private void FillDatabaseStrip()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("database.xml");

            // Variables
            string[] XMLNodeTypes = new string[4] { "SYS", "IOS", "VC", "WW" };

            // Loop through XMLNodeTypes
            for (int i = 0; i < XMLNodeTypes.Length; i++)
            {
                XmlNodeList XMLSpecificNodeTypeList = xDoc.GetElementsByTagName(XMLNodeTypes[i]);

                for (int x = 0; x < XMLSpecificNodeTypeList.Count; x++)
                {
                    ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                    XmlAttributeCollection XMLAttributes = XMLSpecificNodeTypeList[x].Attributes;

                    string titleID = "";
                    string descname = "";
                    bool dangerous = false;
                    bool ticket = true;
                    
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
                            case "version":
                                string[] versions = ChildrenOfTheNode[z].InnerText.Split(',');
                                // Add to region things?
                                if (XMLToolStripItem.DropDownItems.Count > 0)
                                {
                                    for (int b = 0; b < XMLToolStripItem.DropDownItems.Count; b++)
                                    {
                                        if (ChildrenOfTheNode[z].InnerText != "")
                                        {
                                            ToolStripMenuItem regitem = (ToolStripMenuItem)XMLToolStripItem.DropDownItems[b];
                                            regitem.DropDownItems.Add("Latest Version");
                                            for (int y = 0; y < versions.Length; y++)
                                            {
                                                regitem.DropDownItems.Add("v" + versions[y]);
                                            }
                                            regitem.DropDownItemClicked += new ToolStripItemClickedEventHandler(deepitem_clicked);
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
                                        XMLToolStripItem.DropDownItems.Add(RegionFromIndex(Convert.ToInt32(regions[y]), xDoc));
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
                        
                        XMLToolStripItem.Text = String.Format("{0} - {1}", titleID, descname);
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
        void AddToolStripItemToStrip(int type, ToolStripMenuItem additionitem, XmlAttributeCollection attributes)
        { 
            // Deal with VC list depth...
            if (type == 2)
            {
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

        void deepitem_clicked(object sender, ToolStripItemClickedEventArgs e)
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
            string titlename = e.ClickedItem.OwnerItem.OwnerItem.Text.Substring(19, (e.ClickedItem.OwnerItem.OwnerItem.Text.Length - 19));
            statusbox.Text = " --- " + titlename + " ---";

            // Check if a ticket is present...
            if ((e.ClickedItem.OwnerItem.OwnerItem.Image) == (orange) || (e.ClickedItem.OwnerItem.OwnerItem.Image) == (redorange))
            {
                ignoreticket.Checked = true;
                WriteStatus("Note: This title has no ticket and cannot be packed/decrypted!");
                packbox.Checked = false;
                decryptbox.Checked = false;
            }
            else
            {
                ignoreticket.Checked = false;
            }

            // Change WAD name if packed is already checked...
            if (packbox.Checked)
            {
                OfficialWADNaming(titlename);
            }

            // Check for danger item
            if ((e.ClickedItem.OwnerItem.OwnerItem.Image) == (redgreen) || (e.ClickedItem.OwnerItem.OwnerItem.Image) == (redorange))
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
            if (titlename.Contains("IOS"))
                wadnamebox.Text = titlename + "-64-[v].wad";
            else if (titlename.Contains("System Menu"))
                wadnamebox.Text = "RVL-WiiSystemmenu-[v].wad";
            else
                wadnamebox.Text = titlename + "-NUS-[v].wad";
            if (titleversion.Text != "")
                wadnamebox.Text = wadnamebox.Text.Replace("[v]", "v" + titleversion.Text);
        }

        void wwitem_regionclicked(object sender, ToolStripItemClickedEventArgs e)
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
                ignoreticket.Checked = true;
                WriteStatus("Note: This title has no ticket and cannot be packed/decrypted!");
                packbox.Checked = false;
                decryptbox.Checked = false;
            }
            else
            {
                ignoreticket.Checked = false;
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

        void sysitem_versionclicked(object sender, ToolStripItemClickedEventArgs e)
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
                ignoreticket.Checked = true;
                WriteStatus("Note: This title has no ticket and cannot be packed/decrypted!");
                packbox.Checked = false;
                decryptbox.Checked = false;
            }
            else
            {
                ignoreticket.Checked = false;
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
        string RegionFromIndex(int index, XmlDocument databasexml)
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
        private string RemoveIllegalCharacters(string databasestr)
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
        /// Zeroes the signature in TMD/TIK.
        /// </summary>
        /// <param name="tmdortik">TMD/TIK</param>
        /// <returns>Zeroed TMD/TIK</returns>
        private byte[] ZeroSignature(byte[] tmdortik)
        { 
            // Write all 0x00 to signature...
            // Sig starts at 0x04 in both TMD/TIK
            for (int i = 0; i < 256; i++)
            {
                tmdortik[i + 4] = 0x00;
            }

            WriteStatus(" - Signature Emptied...");
            return tmdortik;
        }

        /// <summary>
        /// Trucha Signs a TMD/TIK
        /// </summary>
        /// <param name="tmdortik">The tmdortik.</param>
        /// <returns>Fake-signed byte[]</returns>
        private byte[] TruchaSign(byte[] tmdortik)
        {
            // Loop through 2 bytes worth of numbers until hash starts with 0x00...
            // Padding starts at 0x104 in both TMD/TIK, seems like a good place to me...

            byte[] payload = new byte[2];
            byte[] hashobject = new byte[tmdortik.Length - 0x104];

            for (int i = 0; i < 65535; i++)
            {
                payload = incrementAtIndex(payload, 1);

                tmdortik[0x104] = payload[0];
                tmdortik[0x105] = payload[1];

                for (int x = 0; x < (tmdortik.Length - 0x104); x++)
                {
                    hashobject[x] = tmdortik[0x104 + x];
                }

                if (ComputeSHA(hashobject)[0] == 0x00)
                {
                    WriteStatus(" - Successfully Trucha Signed.");
                    return tmdortik;
                }
            }

            WriteStatus(" - Sign FAIL!");
            return tmdortik;
        }

        /// <summary>
        /// Increments at an index.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        static public byte[] incrementAtIndex(byte[] array, int index)
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

        private void button6_Click(object sender, EventArgs e)
        {
            // Revert to TMD information...
            string[] fileinfo = shamelessvariablelabel.Text.Split(',');

            // Read the tmd as a stream...
            byte[] tmd = FileLocationToByteArray(fileinfo[0] + fileinfo[1]);

            // Read information from TMD into signing GUI...
            requiredIOSbox.Text = Convert.ToString(tmd[0x18B]);
            // Lulzy cheap way of getting version... *256
            tmdversiontrucha.Text = Convert.ToString(((tmd[0x1DC]*256) + tmd[0x1DD]));
            newtitleidbox.Text = titleidbox.Text;

            // Setup for NO IOS
            if (requiredIOSbox.Text == "0")
                requiredIOSbox.Enabled = false;
            else
                requiredIOSbox.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Revert to Ticket information...
            string[] fileinfo = shamelessvariablelabel.Text.Split(',');

            // Create ticket file holder
            byte[] cetkbuff = FileLocationToByteArray(fileinfo[0] + Path.DirectorySeparatorChar.ToString() + @"cetk");

            // Titlekey
            byte[] titlekey = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                titlekey[i] = cetkbuff[0x1BF + i];
            }
            titlekeybox.Text = System.Text.Encoding.UTF7.GetString(titlekey);

            // IV (TITLEID+00000000s)
            byte[] iv = new byte[16];
            for (int i = 0; i < 8; i++)
            {
                iv[i] = cetkbuff[0x1DC + i];
            }
            for (int i = 0; i < 8; i++)
            {
                iv[i + 8] = 0x00;
            }
            titleIDIV.Text = DisplayBytes(iv, "");

            //DLC
            dlcamntbox.Text = Convert.ToString((cetkbuff[0x1E6]*256) + cetkbuff[0x1E7]);

            //keyindex
            if (cetkbuff[0x1F1] == 0x00)
                ckeyindexcb.SelectedIndex = 0;
            else if (cetkbuff[0x1F1] == 0x01)
                ckeyindexcb.SelectedIndex = 1;
            else
                ckeyindexcb.SelectedIndex = 0;

            //time enabled
            if (cetkbuff[0x247] == 0x00)
                timelimitenabledcb.SelectedIndex = 0;
            else if (cetkbuff[0x247] == 0x01)
                timelimitenabledcb.SelectedIndex = 1;
            else
                timelimitenabledcb.SelectedIndex = 0;

            //time in seconds
            byte[] timelimit = new byte[4];
            for (int i = 0; i < timelimit.Length; i++)
            {
                timelimit[i] = cetkbuff[0x248 + i];
            }
            timelimitsecs.Text = Convert.ToString(System.BitConverter.ToInt32(timelimit, 0));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Write Trucha changes to TMD...
            WriteStatus("Trucha Signing TMD...");

            // Cheezy file info
            string[] fileinfo = shamelessvariablelabel.Text.Split(',');

            // Read the tmd as a stream...
            byte[] tmd = FileLocationToByteArray(fileinfo[0] + fileinfo[1]);

            // Resize to just TMD...
            Array.Resize(ref tmd, 484 + (Convert.ToInt32(fileinfo[2]) * 36));

            // Change Required IOS
            if (requiredIOSbox.Text != "0")
            {
                tmd[0x18B] = Convert.ToByte(requiredIOSbox.Text);
            }

            // Change Title Version
            byte[] version = new byte[2];
            version = InttoByteArray(Convert.ToInt32(tmdversiontrucha.Text), version.Length);
            tmd[0x1DC] = version[version.Length - 2];
            tmd[0x1DD] = version[version.Length - 1];

            // Change Title ID
            tmd[0x18C] = byte.Parse(newtitleidbox.Text.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            tmd[0x18D] = byte.Parse(newtitleidbox.Text.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            tmd[0x18E] = byte.Parse(newtitleidbox.Text.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            tmd[0x18F] = byte.Parse(newtitleidbox.Text.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            tmd[0x190] = byte.Parse(newtitleidbox.Text.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
            tmd[0x191] = byte.Parse(newtitleidbox.Text.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
            tmd[0x192] = byte.Parse(newtitleidbox.Text.Substring(12, 2), System.Globalization.NumberStyles.HexNumber);
            tmd[0x193] = byte.Parse(newtitleidbox.Text.Substring(14, 2), System.Globalization.NumberStyles.HexNumber);

            tmd = ZeroSignature(tmd);
            tmd = TruchaSign(tmd);

            FileStream testtmd = new FileStream(fileinfo[0] + fileinfo[1], FileMode.Open);
            testtmd.Write(tmd, 0, tmd.Length);
            testtmd.Close();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // Write Trucha changes to Ticket...
            WriteStatus("Trucha Signing Ticket...");

            // Cheezy file info
            string[] fileinfo = shamelessvariablelabel.Text.Split(',');

            // Create ticket file holder
            byte[] cetkbuff = FileLocationToByteArray(fileinfo[0] + Path.DirectorySeparatorChar.ToString() + @"cetk");

            // Resize Ticket to actual size.
            Array.Resize(ref cetkbuff, 0x2A4);

            // TODO: Title Key and IV changes!
            WriteStatus("Title Key / IV are not available to change in this release :(");

            // Write DLC Amount.
            byte[] dlcamount = new byte[2];
            dlcamount = InttoByteArray(Convert.ToInt32(dlcamntbox.Text), dlcamount.Length);
            cetkbuff[0x1E6] = dlcamount[dlcamount.Length - 2];
            cetkbuff[0x1E7] = dlcamount[dlcamount.Length - 1];

            // Common Key index.
            if (ckeyindexcb.SelectedIndex == 0)
                cetkbuff[0x1F1] = 0x00;
            else if (ckeyindexcb.SelectedIndex == 1)
                cetkbuff[0x1F1] = 0x01;
            else
                cetkbuff[0x1F1] = 0x00;

            // Time limit enable.
            if (timelimitenabledcb.SelectedIndex == 0)
                cetkbuff[0x247] = 0x00;
            else if (timelimitenabledcb.SelectedIndex == 1)
                cetkbuff[0x247] = 0x01;
            else
                cetkbuff[0x247] = 0x00;

            // The amount of time for the limit.
            byte[] limitseconds = new byte[4];
            limitseconds = InttoByteArray(Convert.ToInt32(timelimitsecs.Text), 4);
          
            for (int i = 0; i < 4; i++)
            {
                cetkbuff[0x248 + i] = limitseconds[i];
            }

            // Trucha (Fake) Sign
            cetkbuff = ZeroSignature(cetkbuff);
            cetkbuff = TruchaSign(cetkbuff);

            // Write changes to cetk.
            FileStream testtik = new FileStream(fileinfo[0] + "cetk", FileMode.Open);
            testtik.Write(cetkbuff, 0, cetkbuff.Length);
            testtik.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Proceed with process (BG worker waits for form to resize)
            WriteStatus("Trucha modifications complete.");
            this.Size = this.MinimumSize;
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
            // Disable things the user should not mess with during download...
            downloadstartbtn.Enabled = enabled;
            titleidbox.Enabled = enabled;
            titleversion.Enabled = enabled;
            Extrasbtn.Enabled = enabled;
            databaseButton.Enabled = enabled;
            packbox.Enabled = enabled;
            localuse.Enabled = enabled;
            ignoreticket.Enabled = enabled;
            truchabox.Enabled = enabled;
            decryptbox.Enabled = enabled;
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
                    ToolStripMenuItem menuitem = (ToolStripMenuItem)item;
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

            if ((titleidbox.Enabled == true) && (packbox.Checked == true))
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
            Ticket[1] = 0x01; Ticket[3] = 0x01;

            // Signature Issuer... (Root-CA00000001-XS00000003)
            byte[] SignatureIssuer = new byte[0x1A] { 0x52, 0x6F, 0x6F, 0x74, 0x2D, 0x43, 0x41, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x2D, 0x58, 0x53, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x33 };
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
            Ticket[0x1E4] = 0xFF; Ticket[0x1E5] = 0xFF;
            Ticket[0x1E6] = 0xFF; Ticket[0x1E7] = 0xFF;

            // Unknown 0x01...
            Ticket[0x221] = 0x01;

            // Misc FF...
            for (int e = 0; e < 0x20; e++)
            {
                Ticket[0x222 + e] = 0xFF;
            }

            return Ticket;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // Read Content info from TMD again (revert)
            string[] fileinfo = shamelessvariablelabel.Text.Split(',');

            // Read the tmd as a stream...
            byte[] tmd = FileLocationToByteArray(fileinfo[0] + fileinfo[1]);

            FillContentInfo(tmd);
        }

        /// <summary>
        /// Fills the content editor with info from TMD
        /// </summary>
        /// <param name="tmd">The TMD.</param>
        private void FillContentInfo(byte[] tmd)
        {
            // Clear anything existing...
            contentsEdit.Items.Clear();

            // # of Contents and BootIndex
            int nbr_cont = ContentCount(tmd);
            int boot_idx = GetBootIndex(tmd);

            string[] tmdcontents = GetContentNames(tmd, nbr_cont);
            byte[] tmdindices = GetContentIndices(tmd, nbr_cont);
            int[] tmdtypes = GetContentTypes(tmd, nbr_cont);

            // Loop and add contents to listbox...
            for (int a = 0; a < nbr_cont; a++)
            {
                contentsEdit.Items.Add(String.Format("[{0}] [{1}]", tmdindices[a], tmdcontents[a]));

                if (tmdtypes[a] == 0x8001)
                    contentsEdit.Items[a] += " [S]";
            }


            // Identify Boot Content...
            contentsEdit.Items[boot_idx] += " [BOOT]"; 

        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Move selected content upwards (down an index)...
            if (contentsEdit.SelectedIndex <= 0)
                return;

            int sel_index = contentsEdit.SelectedIndex;
            object sel_item = contentsEdit.SelectedItem;

            contentsEdit.Items.RemoveAt(sel_index);
            contentsEdit.Items.Insert(sel_index - 1, sel_item);

            string sel_itm = contentsEdit.Items[sel_index].ToString();
            string other_itm = contentsEdit.Items[sel_index - 1].ToString();

            sel_itm = sel_itm.Replace("[", ""); sel_itm = sel_itm.Replace("]", "");
            other_itm = other_itm.Replace("[", ""); other_itm = other_itm.Replace("]", "");

            string[] selarray = sel_itm.Split(' ');
            string[] otherary = other_itm.Split(' ');

            contentsEdit.Items[sel_index] = String.Format("[{0}]", sel_index);
            contentsEdit.Items[sel_index - 1] = String.Format("[{0}]", sel_index - 1);

            for (int a = 0; a < selarray.Length; a++)
            {
                if (a != 0)
                    contentsEdit.Items[sel_index] += String.Format(" [{0}]", selarray[a]);
            }

            for (int b = 0; b < otherary.Length; b++)
            {
                if (b != 0)
                    contentsEdit.Items[sel_index - 1] += String.Format(" [{0}]", otherary[b]);
            }
            /*int sel_idx = contentsEdit.SelectedIndex;
            string sel_item = contentsEdit.Items[sel_idx].ToString();
            string lower_item = contentsEdit.Items[sel_idx - 1].ToString();

            contentsEdit.Items[sel_idx] = String.Format("[{0}]{1}", sel_idx, lower_item.Substring(contentsEdit.SelectedItem.ToString().IndexOf(" ["), lower_item.Length - contentsEdit.SelectedItem.ToString().IndexOf(" [")));
            contentsEdit.Items[sel_idx - 1] = String.Format("[{0}]{1}", sel_idx - 1, sel_item.Substring(contentsEdit.SelectedItem.ToString().IndexOf(" ["), sel_item.Length - contentsEdit.SelectedItem.ToString().IndexOf(" [")));
            */
            contentsEdit.SelectedIndex = sel_index - 1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Move selected content down (up an index)...
            if (contentsEdit.SelectedIndex >= contentsEdit.Items.Count - 1)
                return;

            int sel_index = contentsEdit.SelectedIndex;
            object sel_item = contentsEdit.SelectedItem;

            contentsEdit.Items.RemoveAt(sel_index);
            contentsEdit.Items.Insert(sel_index + 1, sel_item);

            string sel_itm = contentsEdit.Items[sel_index].ToString();
            string other_itm = contentsEdit.Items[sel_index + 1].ToString();

            sel_itm = sel_itm.Replace("[", ""); sel_itm = sel_itm.Replace("]", "");
            other_itm = other_itm.Replace("[", ""); other_itm = other_itm.Replace("]", "");

            string[] selarray = sel_itm.Split(' ');
            string[] otherary = other_itm.Split(' ');

            contentsEdit.Items[sel_index] = String.Format("[{0}]", sel_index);
            contentsEdit.Items[sel_index + 1] = String.Format("[{0}]", sel_index + 1);

            for (int a = 0; a < selarray.Length; a++)
            {
                if (a != 0)
                    contentsEdit.Items[sel_index] += String.Format(" [{0}]", selarray[a]);
            }

            for (int b = 0; b < otherary.Length; b++)
            {
                if (b != 0)
                    contentsEdit.Items[sel_index + 1] += String.Format(" [{0}]", otherary[b]);
            }

            /*int sel_idx = contentsEdit.SelectedIndex;
            string sel_item = contentsEdit.Items[sel_idx].ToString();
            string upper_item = contentsEdit.Items[sel_idx + 1].ToString();

            contentsEdit.Items[sel_idx] = String.Format("[{0}]{1}", sel_idx, upper_item.Substring(contentsEdit.SelectedItem.ToString().IndexOf(" ["), upper_item.Length - contentsEdit.SelectedItem.ToString().IndexOf(" [")));
            contentsEdit.Items[sel_idx + 1] = String.Format("[{0}]{1}", sel_idx + 1, sel_item.Substring(contentsEdit.SelectedItem.ToString().IndexOf(" ["), sel_item.Length - contentsEdit.SelectedItem.ToString().IndexOf(" [")));
            */
            contentsEdit.SelectedIndex = sel_index + 1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Set a new boot index...

            // Handle help info first...
            if (Control.ModifierKeys == Keys.Shift)
            {
                WriteStatus("[HELP INFO] Select a content, and press this to make it the 'boot' content. The boot content is the" +
                    " DOL file which is ran when the channel is started.");
                return;
            }

            if (contentsEdit.SelectedIndex < 0)
                return;

            for (int a = 0; a < contentsEdit.Items.Count; a++)
            {
                if (contentsEdit.Items[a].ToString().Contains(" [BOOT]"))
                    contentsEdit.Items[a] = contentsEdit.Items[a].ToString().Substring(0, contentsEdit.Items[a].ToString().Length - 7);
            }

            contentsEdit.Items[contentsEdit.SelectedIndex] += " [BOOT]";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Add a file to the contents...

            // Handle help info first...
            if (Control.ModifierKeys == Keys.Shift)
            {
                WriteStatus("[HELP INFO] This button will allow you to add a content to the title. You can browse " +
                    "and select an .app file (a decrypted content) to insert into the contents list.");
                return;
            }

            OpenFileDialog opencont = new OpenFileDialog();
            opencont.Filter = "Decrypted Contents|*.app|All Files|*";
            opencont.Multiselect = false;
            opencont.Title = "Locate a Content";
            if (opencont.ShowDialog() != DialogResult.Cancel)
            {
                // OK WE MUST PREVENT:
                // - NON HEX NAMING
                // - FILE EXISTING WITH THE SAME NAME && THAT FILE != NEW FILE
                // - 


                if ((OnlyHexInString(opencont.SafeFileName.Substring(0,8)) == false))
                {
                    MessageBox.Show("Please locate/rename a file to be (8 HEX CHARACTERS) long + (.app) extention!", "Bad!", MessageBoxButtons.OK);
                    return;
                }

                for (int i = 0; i < contentsEdit.Items.Count; i++)
                {
                    if (contentsEdit.Items[i].ToString().Contains(opencont.SafeFileName))
                    {
                        MessageBox.Show("A file already exists in the title with that filename!", "Bad!", MessageBoxButtons.OK);
                        return;
                    }
                }

                // D: TODO?
                string[] fileinfo = shamelessvariablelabel.Text.Split(',');

                if (File.Exists(fileinfo[0] + opencont.SafeFileName) )
                {
                    MessageBox.Show("Rename the file you are adding, it already exists in the title directory!", "Bad!", MessageBoxButtons.OK);
                    return;
                }

                if (fileinfo[0] + opencont.SafeFileName != opencont.FileName)
                {
                    // Move the file into the directory...
                    File.Copy(opencont.FileName, fileinfo[0] + opencont.SafeFileName);
                }
                contentsEdit.Items.Add(String.Format("[{0}] [{1}]", contentsEdit.Items.Count, opencont.SafeFileName));
            }
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

        private void button10_Click(object sender, EventArgs e)
        {
            // Remove a content from the list...

            // Handle help info first...
            if (Control.ModifierKeys == Keys.Shift)
            {
                WriteStatus("[HELP INFO] This button will allow you to remove a content from the title. Be careful, " + 
                    "as most contents are necessary for proper channel usage!");
                return;
            }

            if ((contentsEdit.SelectedIndex < 0) || (contentsEdit.Items.Count <= 1))
                return;

            string[] fileinfo = shamelessvariablelabel.Text.Split(',');
            DialogResult question = MessageBox.Show("Delete the actual file as well?", "Delete content?", MessageBoxButtons.YesNoCancel);

            if (question == DialogResult.Yes)
                File.Delete(fileinfo[0] + contentsEdit.SelectedItem.ToString().Substring(contentsEdit.SelectedItem.ToString().IndexOf("] [") + 3, 8));

            if (question != DialogResult.Cancel)
                contentsEdit.Items.RemoveAt(contentsEdit.SelectedIndex);

            RecalculateIndices();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            UpdateTMDContents();
        }

        /// <summary>
        /// Updates the TMD contents.
        /// </summary>
        private void UpdateTMDContents()
        {
            // Write changes to TMD of contents...
            WriteStatus("Updating TMD with content information...");
            string[] fileinfo = shamelessvariablelabel.Text.Split(',');

            WriteStatus(" - Loading Title Key from Ticket...");
            byte[] ticket = FileLocationToByteArray(fileinfo[0] + "cetk");
            byte[] etitlekey = new byte[16];
            for (int a = 0; a < 16; a++)
            {
                etitlekey[a] = ticket[0x1BF + a];
            }
            // TODO: Add more key support
            byte[] commonkey = LoadCommonKey(Path.DirectorySeparatorChar.ToString() + @"key.bin");

            // IV (TITLEID00000000)
            byte[] iv = new byte[16];
            for (int b = 0; b < 8; b++)
            {
                iv[b] = ticket[0x1DC + b];
            }
            for (int c = 0; c < 8; c++)
            {
                iv[c + 8] = 0x00;
            }

            initCrypt(iv, commonkey);
            byte[] dtitlekey = Decrypt(etitlekey);

            // Holds all the content data...
            TitleContent[] contents = new TitleContent[contentsEdit.Items.Count];

            // Previous TMD for analysis
            byte[] tmd = FileLocationToByteArray(fileinfo[0] + fileinfo[1]);

            for (int c = 0; c < contentsEdit.Items.Count; c++)
            {
                string itemstr = contentsEdit.Items[c].ToString();
                contents[c] = new TitleContent();
                // Set boot index...
                if (itemstr.Contains(" [BOOT]"))
                {
                    tmd = SetBootIndex(tmd, c);
                }
                if (itemstr.Contains(".app"))
                {
                    // This is already decrypted, we're going to add it to the TMD...
                    string filename = itemstr.Substring(itemstr.IndexOf("] [") + 3, 12);
                    byte[] contentbytes = FileLocationToByteArray(fileinfo[0] + filename);
                    WriteStatus(filename + " is a decrypted file...");
                    WriteStatus(" - Encrypting " + filename + "...");

                    // Gather the contentID (crappy way to do it)...
                    contents[c].ContentID = new byte[4];
                    contents[c].ContentID[0] = byte.Parse(filename.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    contents[c].ContentID[1] = byte.Parse(filename.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    contents[c].ContentID[2] = byte.Parse(filename.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    contents[c].ContentID[3] = byte.Parse(filename.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                    // Grab SHA1/size of file
                    contents[c].SHAHash = new byte[20];
                    contents[c].SHAHash = ComputeSHA(contentbytes);

                    contents[c].Size = new byte[8];
                    // TODOCHECK THIS OVER
                    contents[c].Size = NewIntegertoByteArray(contentbytes.Length, 8);

                    contents[c].Index = new byte[2];
                    contents[c].Index = NewIntegertoByteArray(c, 2);

                    contents[c].Type = new byte[2];
                    contents[c].Type[1] = 0x01;
                    if (contentsEdit.Items[c].ToString().Contains(" [S]"))
                        contents[c].Type[0] = 0x80;
                    else
                        contents[c].Type[0] = 0x00;

                    // Pad to be 16 byte aligned
                    contentbytes = PadToMultipleOf(contentbytes, 16);

                    // Encrypt with correct index IV/titlekey
                    byte[] ivindex = new byte[16];
                    for (int d = 0; d < ivindex.Length; d++)
                    {
                        ivindex[d] = 0x00;
                    }
                    ivindex[0] = contents[c].Index[0];
                    ivindex[1] = contents[c].Index[1];

                    initCrypt(ivindex, dtitlekey);

                    FileStream encryptwrite = new FileStream(fileinfo[0] + filename.Substring(0, 8), FileMode.Create);
                    encryptwrite.Write(Encrypt(contentbytes), 0, contentbytes.Length);
                    encryptwrite.Close();

                    WriteStatus(" - " + filename.Substring(0, 8) + " written!");
                }
                else
                {
                    // An encrypted content...it was from the original TMD
                    string filename = itemstr.Substring(itemstr.IndexOf("] [") + 3, 8);
                    byte[] contentbytes = FileLocationToByteArray(fileinfo[0] + filename);
                    WriteStatus(filename + " is encrypted and from the original TMD...");
                    WriteStatus(" - Gathering " + filename + " information...");

                    // Grab previous values from TMD...
                    int nbr_cont = ContentCount(tmd);
                    string[] tmdoldcontents = GetContentNames(tmd, nbr_cont);
                    string[] tmdsizes = GetContentSizes(tmd, nbr_cont);
                    byte[] tmdhashes = GetContentHashes(tmd, nbr_cont);

                    int thiscontentidx = 0;

                    for (int f = 0; f < nbr_cont; f++)
                    {
                        if (tmdoldcontents[f] == filename)
                            thiscontentidx = f;
                    }

                    // if index has been changed...
                    if (thiscontentidx != c)
                    {
                        // We have to decrypt the content, and then encrypt to keep IV in line...
                        WriteStatus(" - Index altered. Must change IV...");
                        byte[] ivindex = new byte[16];
                        for (int d = 0; d < ivindex.Length; d++)
                        {
                            ivindex[d] = 0x00;
                        }
                        // TODO: Complete this...
                        ivindex[0] = 0x00;
                        ivindex[1] = (byte)thiscontentidx;

                        initCrypt(ivindex, dtitlekey);

                        byte[] hash = new byte[20];
                        for (int x = 0; x < 20; x++)
                        {
                            hash[x] = tmdhashes[(thiscontentidx * 20) + x];
                        }

                        byte[] decContent = Decrypt(contentbytes);
                        Array.Resize(ref decContent, int.Parse(tmdsizes[thiscontentidx], System.Globalization.NumberStyles.HexNumber));
                        contents[c].Size = NewIntegertoByteArray(decContent.Length, 8);
                        if ((Convert.ToBase64String(ComputeSHA(decContent))) == Convert.ToBase64String(hash))
                        {
                            WriteStatus(" - Hash Check: Content is Unchanged...");
                            contents[c].SHAHash = hash;
                            //WriteStatus("HASH: " + DisplayBytes(hash, ""));
                        }
                        else
                        {
                            WriteStatus(" - Hash Check: Content changed (did you add an encrypted file from another title?)...");
                            contents[c].SHAHash = ComputeSHA(decContent);
                        }

                        // Re-encrypt
                        byte[] newiv = new byte[16];
                        for (int g = 0; g < newiv.Length; g++)
                        {
                            newiv[g] = 0x00;
                        }
                        byte[] smallix = NewIntegertoByteArray(c, 2);
                        ivindex[0] = smallix[0];
                        ivindex[1] = smallix[1];

                        //WriteStatus(" - Old Index: " + thiscontentidx + "; New Index: " + c);

                        // Pad back to 0x16 alignment
                        //AlignByteArray(decContent, 0x16
                        decContent = PadToMultipleOf(decContent, 16);

                        initCrypt(newiv, dtitlekey);

                        byte[] encContent = Encrypt(decContent);

                        File.Delete(fileinfo[0] + filename.Substring(0, 8));

                        FileStream encryptwrite = new FileStream(fileinfo[0] + filename.Substring(0, 8), FileMode.OpenOrCreate);
                        encryptwrite.Write(encContent, 0, encContent.Length);
                        encryptwrite.Close();

                        WriteStatus(" - Encrypted Content Again!");
                    }
                    else
                    {
                        // Hopefully this content has not been touched...
                        WriteStatus(" - Content has not changed Index.");
                        byte[] hash = new byte[20];
                        for (int x = 0; x < 20; x++)
                        {
                            hash[x] = tmdhashes[(c * 20) + x];
                        }
                        contents[c].SHAHash = hash;

                        contents[c].Size = NewIntegertoByteArray(int.Parse(tmdsizes[c], System.Globalization.NumberStyles.HexNumber), 8);
                    }

                    contents[c].ContentID = new byte[4];
                    contents[c].ContentID[0] = byte.Parse(filename.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    contents[c].ContentID[1] = byte.Parse(filename.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    contents[c].ContentID[2] = byte.Parse(filename.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    contents[c].ContentID[3] = byte.Parse(filename.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

                    contents[c].Index = new byte[2];
                    contents[c].Index = NewIntegertoByteArray(c, 2);

                    contents[c].Type = new byte[2];
                    contents[c].Type[1] = 0x01;
                    if (contentsEdit.Items[c].ToString().Contains(" [S]"))
                        contents[c].Type[0] = 0x80;
                    else
                        contents[c].Type[0] = 0x00;
                }

            }

            // Collect everything into a single byte[]...
            byte[] contentSection = new byte[contents.Length * 36];
            for (int h = 0; h < contents.Length; h++)
            {
                for (int i = 0; i < contents[h].ContentID.Length; i++)
                {
                    contentSection[(h * 36) + i] = contents[h].ContentID[i];
                }

                for (int j = 0; j < contents[h].Index.Length; j++)
                {
                    contentSection[(h * 36) + (contents[h].ContentID.Length + j)] = contents[h].Index[j];
                }

                for (int k = 0; k < contents[h].Type.Length; k++)
                {
                    contentSection[(h * 36) + (contents[h].ContentID.Length + contents[h].Index.Length + k)] = contents[h].Type[k];
                }

                for (int l = 0; l < contents[h].Size.Length; l++)
                {
                    contentSection[(h * 36) + (contents[h].ContentID.Length + contents[h].Index.Length + contents[h].Type.Length + l)] = contents[h].Size[l];
                }

                for (int m = 0; m < contents[h].SHAHash.Length; m++)
                {
                    contentSection[(h * 36) + (contents[h].ContentID.Length + contents[h].Index.Length + contents[h].Type.Length + contents[h].Size.Length + m)] = contents[h].SHAHash[m];
                }
            }

            for (int n = 0; n < contentSection.Length; n++)
            {
                tmd[0x1E4 + n] = contentSection[n];
            }

            // Fakesign the TMD again...
            tmd = ZeroSignature(tmd);
            tmd = TruchaSign(tmd);

            // Write all this stuff to the TMD...
            FileStream testtmd = new FileStream(fileinfo[0] + fileinfo[1], FileMode.Open);
            testtmd.Write(tmd, 0, tmd.Length);
            testtmd.Close();
        }

        /// <summary>
        /// Pads to multiple of....
        /// </summary>
        /// <param name="src">The binary.</param>
        /// <param name="pad">The pad amount.</param>
        /// <returns>Padded byte[]</returns>
        private byte[] PadToMultipleOf(byte[] src, int pad)
        {
            int len = (src.Length + pad - 1) / pad * pad;
      
            Array.Resize(ref src, len);
            return src;
        }


        private void button17_Click(object sender, EventArgs e)
        {
            // Move groupbox to display title modder...
            if (button17.Text == "Modify Individual Contents...")
            {
                contentModBox.Location = new Point(278, 12);
                contentModBox.Visible = true;
                contentModBox.BringToFront();
                button17.Text = "Trucha Sign Title...";
            }
            else if (button17.Text == "Trucha Sign Title...")
            {
                //contentModBox.Location = new Point(300, 300);
                contentModBox.Visible = false;
                contentModBox.SendToBack();
                button17.Text = "Modify Individual Contents...";
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // Share/Unshare Contents in the list...

            // Handle help info first...
            if (Control.ModifierKeys == Keys.Shift)
            {
                WriteStatus("[HELP INFO] Toggles the shared state of the content.");
                return;
            }

            if (contentsEdit.SelectedIndex < 0)
                return;

            if (contentsEdit.Items[contentsEdit.SelectedIndex].ToString().Contains(" [S]"))
                contentsEdit.Items[contentsEdit.SelectedIndex] = contentsEdit.SelectedItem.ToString().Replace(" [S]", "");
            else
            {
                if (contentsEdit.Items[contentsEdit.SelectedIndex].ToString().Contains(" [BOOT]"))
                    contentsEdit.Items[contentsEdit.SelectedIndex] = contentsEdit.SelectedItem.ToString().Replace(" [BOOT]", "") + " [S] [BOOT]";
                else
                    contentsEdit.Items[contentsEdit.SelectedIndex] = contentsEdit.SelectedItem.ToString() + " [S]";
            }
        }

        /// <summary>
        /// Determines whether OS is win7.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if OS = win7; otherwise, <c>false</c>.
        /// </returns>
        private bool IsWin7()
        {
            return (Environment.OSVersion.VersionString.Contains("6.1") == true);
        }

        private byte[] NewIntegertoByteArray(int theInt, int arrayLen)
        {
            byte[] resultArray = new byte[arrayLen];

            for(int i = arrayLen - 1 ; i >= 0; i--) 
            {
                resultArray[i] = (byte)((theInt >> (8 * i)) & 0xFF);

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

        private void button16_Click(object sender, EventArgs e)
        {
            // add trucha bug to content...

            // Handle help info first...
            if (Control.ModifierKeys == Keys.Shift)
            {
                WriteStatus("[HELP INFO] Inserts the trucha bug into the selected content, if the bug was fixed previously.");
                return;
            }

            if (contentsEdit.SelectedIndex < 0)
                return;

            WriteStatus("Attempting to add 'bugs' back into content...");
            string[] fileinfo = shamelessvariablelabel.Text.Split(',');
            byte[] new_hash_check = new byte[] {0x20, 0x07, 0x4B, 0x0B};
            byte[] old_hash_check = new byte[] {0x20, 0x07, 0x23, 0xA2};

            // If decrypted...
            //   - check right away for bug...
            //   - add bug, then end...
            // if encrypted...
            //   - decrypt content...
            //   - find bug/patch...
            //   - recalculate hash...
            //   - write back into TMD...
            //   - trucha sign content...

            if (contentsEdit.Items[contentsEdit.SelectedIndex].ToString().Contains(".app"))
            {
                // Content is decrypted/new to the title...
                string filename = contentsEdit.Items[contentsEdit.SelectedIndex].ToString().Substring(contentsEdit.Items[contentsEdit.SelectedIndex].ToString().IndexOf("] [") + 3, 12);
                byte[] contentbt = FileLocationToByteArray(fileinfo[0] + filename);
                byte[] newvalues = new byte[4];
                newvalues[1] = 0x00;

                int[] oldresults = ByteArrayContainsByteArray(contentbt, old_hash_check);
                int[] newresults = ByteArrayContainsByteArray(contentbt, new_hash_check);

                if (oldresults[0] != 0)
                {
                    WriteStatus(String.Format(" - {0} Old-school ES Signing Fix(es) Found...", oldresults[0]));
                    for (int s = 1; s < oldresults.Length - 1; s++)
                    {
                        contentbt = PatchBinary(contentbt, oldresults[s], newvalues);
                        WriteStatus(String.Format(" - Bug restored at 0x{0}", int.Parse(oldresults[s].ToString(), System.Globalization.NumberStyles.HexNumber)));
                    }
                }
               
                if (newresults[0] != 0)
                {
                    WriteStatus(String.Format(" - {0} New-school ES Signing Fix(es) Found...", newresults[0]));
                    for (int s = 1; s < newresults.Length - 1; s++)
                    {
                        contentbt = PatchBinary(contentbt, newresults[s], newvalues);
                        WriteStatus(String.Format("  + Bug restored at 0x{0}.", int.Parse(newresults[s].ToString(), System.Globalization.NumberStyles.HexNumber)));
                    }
                }
            }
            else
            {
                WriteStatus(" - The file you selected was encrypted, attempting to decrypt and patch...");
                string filename = contentsEdit.Items[contentsEdit.SelectedIndex].ToString().Substring(contentsEdit.Items[contentsEdit.SelectedIndex].ToString().IndexOf("] [") + 3, 8);

                byte[] ticket = FileLocationToByteArray(fileinfo[0] + "cetk");
                byte[] tmd = FileLocationToByteArray(fileinfo[0] + fileinfo[1]);
                byte[] etitlekey = new byte[16];
                for (int a = 0; a < 16; a++)
                {
                    etitlekey[a] = ticket[0x1BF + a];
                }

                // TODO: Add more key support
                byte[] commonkey = LoadCommonKey(Path.DirectorySeparatorChar.ToString() + @"key.bin");

                // IV (TITLEID00000000)
                byte[] iv = new byte[16];
                for (int b = 0; b < 8; b++)
                {
                    iv[b] = ticket[0x1DC + b];
                }
                for (int c = 0; c < 8; c++)
                {
                    iv[c + 8] = 0x00;
                }

                initCrypt(iv, commonkey);
                byte[] dtitlekey = Decrypt(etitlekey);

                // Decrypt this content (determine index)
                string[] tmdcontents = GetContentNames(tmd, ContentCount(tmd));
                byte[] tmdindices = GetContentIndices(tmd, ContentCount(tmd));
                byte[] tmdhashes = GetContentHashes(tmd, ContentCount(tmd));
                string[] tmdsizes = GetContentSizes(tmd, ContentCount(tmd));

                iv = new byte[16];
                for (int f = 0; f < 16; f++)
                {
                    iv[f] = 0x00;
                }

                byte[] hash = new byte[20];
                for (int d = 0; d < tmdcontents.Length; d++)
                {
                    if (tmdcontents[d] == filename)
                    {
                        iv[0] = 0x00; // TODO: Add double index byte support
                        iv[1] = tmdindices[d];

                        for (int x = 0; x < 20; x++)
                        {
                            hash[x] = tmdhashes[(d*20) + x];
                        }
                    }
                }
                initCrypt(iv, commonkey);
                //DEBUG
                WriteStatus(DisplayBytes(iv, " "));

                byte[] decContent = Decrypt(FileLocationToByteArray(fileinfo[0] + filename));
                Array.Resize(ref decContent, int.Parse(tmdsizes[14], System.Globalization.NumberStyles.HexNumber));
          
                if ((Convert.ToBase64String(ComputeSHA(decContent))) == Convert.ToBase64String(hash))
                {
                    WriteStatus("  - Hash Check: Content is Unchanged...");
                }
                else
                {
                    WriteStatus("  - Hash Check: Content changed (did you add an encrypted file from another title?)...");
                    WriteStatus("  - Content Hash: " + DisplayBytes(ComputeSHA(decContent), ""));
                    WriteStatus("  - TMD Hash: " + DisplayBytes(hash, ""));
                    
                }

                if (File.Exists(fileinfo[0] + filename + ".app"))
                {
                    if (MessageBox.Show(fileinfo[0] + filename + ".app Exists! Delete the current file so we can move on?", "File Conflict", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        File.Delete(fileinfo[0] + filename + ".app");
                    else
                        return;
                }

                byte[] newvalues = new byte[4];
                newvalues[1] = 0x00;

                int[] oldresults = ByteArrayContainsByteArray(decContent, old_hash_check);
                int[] newresults = ByteArrayContainsByteArray(decContent, new_hash_check);

                if (oldresults[0] != 0)
                {
                    WriteStatus(String.Format(" - {0} Old-school ES Signing Fix(es) Found...", oldresults[0]));
                    for (int s = 1; s < oldresults.Length - 1; s++)
                    {
                        decContent = PatchBinary(decContent, oldresults[s], newvalues);
                        WriteStatus(String.Format(" - Bug restored at 0x{0}", int.Parse(oldresults[s].ToString(), System.Globalization.NumberStyles.HexNumber)));
                    }
                }

                if (newresults[0] != 0)
                {
                    WriteStatus(String.Format(" - {0} New-school ES Signing Fix(es) Found...", newresults[0]));
                    for (int s = 1; s < newresults.Length - 1; s++)
                    {
                        decContent = PatchBinary(decContent, newresults[s], newvalues);
                        WriteStatus(String.Format("  + Bug restored at 0x{0}.", int.Parse(newresults[s].ToString(), System.Globalization.NumberStyles.HexNumber)));
                    }
                }

                File.WriteAllBytes(fileinfo[0] + filename + ".app", decContent);

                contentsEdit.Items[contentsEdit.SelectedIndex] = contentsEdit.Items[contentsEdit.SelectedIndex].ToString().Replace(filename, filename + ".app");

                UpdateTMDContents();

                WriteStatus("Trucha signing complete!");
            }
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

        /// <summary>
        /// Patches the binary.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="newvalues">The newvalues.</param>
        /// <returns></returns>
        private byte[] PatchBinary(byte[] content, int offset, byte[] newvalues)
        {
            for (int a = 0; a < newvalues.Length; a++)
            {
                if (newvalues[a] >= 0)
                    content[offset + a] = newvalues[a];
            }
            return content;
        }

        /// <summary>
        /// Recalculates the indices.
        /// </summary>
        private void RecalculateIndices()
        {
            for (int a = 0; a < contentsEdit.Items.Count; a++)
            {
                string item = contentsEdit.Items[a].ToString();
                item = item.Replace("[", ""); item = item.Replace("]", "");
                string[] itemparts = item.Split(' ');
                contentsEdit.Items[a] = String.Format("[{0}]", a);
                for (int b = 0; b < itemparts.Length; b++)
                {
                    if (b != 0)
                        contentsEdit.Items[a] += String.Format(" [{0}]", itemparts[b]);
                }
            }
        }

        /// <summary>
        /// Retrieves the new database via WiiBrew.
        /// </summary>
        /// <returns>Database as a String</returns>
        private string RetrieveNewDatabase()
        {
            // Retrieve Wiibrew database page source code
            WebClient databasedl = new WebClient();
            statusbox.Refresh();

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
                databasedl.Proxy = customproxy;
                WriteStatus("  - Custom proxy settings applied!");
            }
            else
            {
                databasedl.Proxy = WebRequest.GetSystemWebProxy();
                databasedl.UseDefaultCredentials = true;
            }

            string wiibrewsource = databasedl.DownloadString("http://www.wiibrew.org/wiki/NUS_Downloader/database?cachesmash=" + System.DateTime.Now.ToString());
            statusbox.Refresh();

            // Strip out HTML
            wiibrewsource = Regex.Replace(wiibrewsource, @"<(.|\n)*?>", "");

            // Shrink to fix only the database
            string startofdatabase = "&lt;database v";
            string endofdatabase = "&lt;/database&gt;";
            wiibrewsource = wiibrewsource.Substring(wiibrewsource.IndexOf(startofdatabase), wiibrewsource.Length - wiibrewsource.IndexOf(startofdatabase));
            wiibrewsource = wiibrewsource.Substring(0, wiibrewsource.IndexOf(endofdatabase) + endofdatabase.Length);

            // Fix ", <, >, and spaces
            wiibrewsource = wiibrewsource.Replace("&lt;","<");
            wiibrewsource = wiibrewsource.Replace("&gt;",">");
            wiibrewsource = wiibrewsource.Replace("&quot;",'"'.ToString());
            wiibrewsource = wiibrewsource.Replace("&nbsp;"," "); // Shouldn't occur, but they happen...

            // Return parsed xml database...
            return wiibrewsource;
        }

        private void updateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusbox.Text = "";
            WriteStatus("Updating your database.xml from Wiibrew.org");

            string database = RetrieveNewDatabase();
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

            WriteStatus(" - Overwriting your current database.xml...");
            WriteStatus(" - The old database will become 'olddatabase.xml' in case the new one is faulty.");

            string olddatabase = File.ReadAllText("database.xml");
            File.WriteAllText("olddatabase.xml", olddatabase);
            File.Delete("database.xml");
            File.WriteAllText("database.xml", database);

            // Load it up...
            ClearDatabaseStrip();
            FillDatabaseStrip();
            LoadRegionCodes();
            ShowInnerToolTips(false);

            WriteStatus("Database successfully updated!");
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
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create("http://nus.shop.wii.com:80/nus/services/NetUpdateSOAP");
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
                System.Net.HttpWebResponse resp = (System.Net.HttpWebResponse)req.GetResponse();

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

            extrasStrip.Close();        

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
            "<Version>1.0</Version>\n<MessageId>" + messageID + "</MessageId>\n<DeviceId>" + deviceID + "</DeviceId>\n" +
            "<RegionId>" + RegionID + "</RegionId>\n<CountryCode>" + CountryCode + "</CountryCode>\n<TitleVersion>\n<TitleId>0000000100000001</TitleId>\n" +
            "<Version>2</Version>\n</TitleVersion>\n<TitleVersion>\n<TitleId>0000000100000002</TitleId>\n" +
            "<Version>33</Version>\n</TitleVersion>\n<TitleVersion>\n<TitleId>0000000100000009</TitleId>\n" +
            "<Version>516</Version>\n</TitleVersion>\n<Attribute>" + attr + "</Attribute>\n<AuditData></AuditData>\n" +
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

                if ((File.Exists("database.xml") == true) && ((!(String.IsNullOrEmpty(NameFromDatabase(TitleID))))))
                    statusbox.Text += String.Format(" [{0}]", NameFromDatabase(TitleID));

                script_text += String.Format("{0} {1}\n", TitleID, DisplayBytes(NewIntegertoByteArray(Convert.ToInt32(Version), 2), ""));
            }

            WriteStatus(" - Outputting results to NUS script...");

            // Current directory...
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(Path.DirectorySeparatorChar.ToString())) || !(currentdir.EndsWith(Path.AltDirectorySeparatorChar.ToString())))
                currentdir += Path.DirectorySeparatorChar.ToString();

            if (!(Directory.Exists(currentdir + "scripts")))
            {
                Directory.CreateDirectory(currentdir + "scripts");
                WriteStatus("  - Created 'scripts\' directory.");
            }
            string time = RemoveIllegalCharacters(DateTime.Now.ToShortTimeString());
            File.WriteAllText(String.Format(currentdir + "scripts\\{0}_Update_{1}_{2}_{3} {4}.nus", RegionID, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, time), script_text);
            WriteStatus(" - Script written!");
            WriteStatus(" - Run this script if you feel like downloading the update!");
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
            if ((!(tmdortik.Length < 0x400)) && ((Convert.ToBase64String(ComputeSHA(cert_CA)) != Convert.ToBase64String(cert_CA_sha1))))
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
            string[] XMLNodeTypes = new string[4] { "SYS", "IOS", "VC", "WW" };

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
                                else if ((ChildrenOfTheNode[z].InnerText.Substring(0, 14) + "XX") == (titleid.Substring(0, 14) + "XX") && (titleid.Substring(0, 14) != "00000001000000"))
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
            saveaswadbox.Enabled = packbox.Enabled;
            deletecontentsbox.Enabled = packbox.Enabled;
        }

        private void saveaswadbox_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.
            /*Rectangle rect = new Rectangle(0, 0, 16, 16);
            if (saveaswadbox.Checked)
                e.Graphics.DrawImageUnscaledAndClipped(green, rect);
            else
                e.Graphics.DrawImageUnscaled(orange, -7, -5); */
        }

        private void SaveProxyBtn_Click(object sender, EventArgs e)
        {
            // Current directory...
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(Path.DirectorySeparatorChar.ToString())) || !(currentdir.EndsWith(Path.AltDirectorySeparatorChar.ToString())))
                currentdir += Path.DirectorySeparatorChar.ToString();

            if ((String.IsNullOrEmpty(ProxyURL.Text)) && (String.IsNullOrEmpty(ProxyUser.Text)) && ((File.Exists(currentdir + "proxy.txt"))))
            {
                File.Delete(currentdir + "proxy.txt");
                proxyBox.Visible = false;
                proxy_usr = ""; proxy_url = ""; proxy_pwd = "";
                WriteStatus("Proxy settings deleted!");
                return;
            }
            else if ((String.IsNullOrEmpty(ProxyURL.Text)) && (String.IsNullOrEmpty(ProxyUser.Text)) && ((!(File.Exists(currentdir + "proxy.txt")))))
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
                File.WriteAllText(currentdir + "proxy.txt", proxy_file);
                WriteStatus("Proxy settings saved!");
            }

            proxyBox.Visible = false;

            SetAllEnabled(false);
            ProxyVerifyBox.Visible = true; ProxyVerifyBox.Enabled = true;
            ProxyPwdBox.Enabled = true; SaveProxyBtn.Enabled = true;
            ProxyVerifyBox.Select();
        }

        private void proxySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Current directory...
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(Path.DirectorySeparatorChar.ToString())) || !(currentdir.EndsWith(Path.AltDirectorySeparatorChar.ToString())))
                currentdir += Path.DirectorySeparatorChar.ToString();

            // Check for Proxy Settings file...
            if (File.Exists(currentdir + "proxy.txt") == true)
            {
                string[] proxy_file = File.ReadAllLines(currentdir + "proxy.txt");
               
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
                " If you have an alternate port for accessing your proxy, add ':' followed by the port." +
                " You will be prompted for your password each time you run NUSD, for privacy purposes.");
        }

        private void enableBETATruchaFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            truchabox.Visible = true;
        }

        private void loadNUSScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Current directory...
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(Path.DirectorySeparatorChar.ToString())) || !(currentdir.EndsWith(Path.AltDirectorySeparatorChar.ToString())))
                currentdir += Path.DirectorySeparatorChar.ToString();

            // Open a NUS script.
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "NUS Scripts|*.nus|All Files|*.*";
            if (Directory.Exists(currentdir + "scripts"))
                ofd.InitialDirectory = currentdir + "scripts";
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
            Control.CheckForIllegalCrossThreadCalls = false;
            script_mode = true;
            statusbox.Text = "";
            WriteStatus("Starting script download. Please be patient!");
            string[] NUS_Entries = File.ReadAllLines(script_filename);
            WriteStatus(String.Format(" - Script loaded ({0} Titles)", NUS_Entries.Length));

            for (int a = 0; a < NUS_Entries.Length; a++)
            {
                // Download the title
                WriteStatus(String.Format("===== Running Download ({0}/{1}) =====", a+1, NUS_Entries.Length));
                string[] title_info = NUS_Entries[a].Split(' ');
                // don't let the delete issue reappear...
                if (string.IsNullOrEmpty(title_info[0]))
                    break;
                titleidbox.Text = title_info[0];
                titleversion.Text = Convert.ToString(256*(byte.Parse(title_info[1].Substring(0, 2), System.Globalization.NumberStyles.HexNumber)));
                titleversion.Text = Convert.ToString(Convert.ToInt32(titleversion.Text) + byte.Parse(title_info[1].Substring(2, 2), System.Globalization.NumberStyles.HexNumber));

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


    }
}
