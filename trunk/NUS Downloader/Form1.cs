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


namespace NUS_Downloader
{
    public partial class Form1 : Form
    {
        const string NUSURL = "http://nus.cdn.shop.wii.com/ccs/download/";
        const string DSiNUSURL = "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/";
        // TODO: Always remember to change version!
        string version = "v1.2";
        WebClient generalWC = new WebClient();
        static RijndaelManaged rijndaelCipher;
        static bool dsidecrypt = false;
        const string certs_MD5 = "7677AD47BAA5D6E3E313E72661FBDC16";
        
        // Images do not compare unless globalized...
        Image green = Properties.Resources.bullet_green;
        Image orange = Properties.Resources.bullet_orange;
        Image redorb = Properties.Resources.bullet_red;
        Image redgreen = Properties.Resources.bullet_redgreen;
        Image redorange = Properties.Resources.bullet_redorange;

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
        };

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

            // certs.sys / key.bin
            if (BootChecks() == false)
                return;

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
                // Do nothing...
            }
            if ((NUSDownloader.IsBusy == false) && (endafter == true))
            {
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "NUSD - " + version + " - WB3000";
            this.Size = this.MinimumSize;
            
            
        }

        private bool BootChecks()
        {
            // Success?
            bool result = true;

            // Directory stuff
            string currentdir = Application.StartupPath;
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar)) == false)
                currentdir += Path.DirectorySeparatorChar;

            // Check for certs file
            if (File.Exists(currentdir + "cert.sys") == false)
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Enabled = false;
                }
                getcerts.Enabled = true;
                WriteStatus("You do not have a certs file. Press the button below to generate a cert file!");
                result = false;
            }
            else if (verifyMd5Hash(currentdir + "cert.sys", certs_MD5) == false)
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Enabled = false;
                }
                getcerts.Enabled = true;
                WriteStatus("Your certs file is corrupted/invalid. Press the button below to generate a cert file!");
                result = false;
            }
            else
            {
                getcerts.Visible = false;
                WriteStatus("Certs file is present and intact.");
            }

            // Check for Wii common key bin file...
            if (File.Exists(currentdir + "key.bin") == false)
            {
                WriteStatus("Common Key (key.bin) missing! Decryption disabled!");
                decryptbox.Visible = false;
            }
            else
            {
                WriteStatus("Common Key detected.");
            }

            // Check for Wii KOR common key bin file...
            if (File.Exists(currentdir + "kkey.bin") == false)
            {
                //WriteStatus("Korean Common Key (kkey.bin) missing! Decryption disabled!");
                //decryptbox.Visible = false;
            }
            else
            {
                WriteStatus("Korean Common Key detected.");
            }

            // Check for DSi common key bin file...
            if (File.Exists(currentdir + "dsikey.bin") == false)
            {
                // Do not pester about DSi key
            }
            else
            {
                WriteStatus("DSi Common Key detected.");
                dsidecrypt = true;
            }

            // Check for database.xml
            if (File.Exists(currentdir + "database.xml") == false)
            {
                WriteStatus("Database.xml not found. Title database not usable!");
                databaseButton.Visible = false;
                TMDButton.Size = new System.Drawing.Size(134, 20);
                TMDButton.Anchor = AnchorStyles.Right;
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

            return result;
        }

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
                        hash[x] = tmdhashes[(i*20)+x];
                    }
                    WriteStatus("  - Hash: " + DisplayBytes(hash, "").Substring(0, 8) + "...");
                    WriteStatus("  - Index: " + tmdindices[i]);
                    WriteStatus("  - Shared: " + (tmdtypes[i] == 0x8001));
                }
            }
        }

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

        private int ContentCount(byte[] tmd)
        { 
            // nbr_cont (0xDE) len=0x02
            int nbr_cont = 0;
            nbr_cont = (tmd[0x1DE] * 256) + tmd[0x1DF];
            return nbr_cont;
        }

        private int GetBootIndex(byte[] tmd)
        {
            // nbr_cont (0xE0) len=0x02
            int bootidx = 0;
            bootidx = (tmd[0x1E0] * 256) + tmd[0x1E1];
            return bootidx;
        }

        private byte[] SetBootIndex(byte[] tmd, int bootindex)
        {
            // nbr_cont (0xE0) len=0x02
            byte[] bootbytes = NewIntegertoByteArray(bootindex, 2);
            tmd[0x1E0] = bootbytes[0];
            tmd[0x1E1] = bootbytes[1];
            return tmd;
        }

        private void WriteStatus(string Update)
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

        private string MakeProperLength(string hex)
        {
            // If hex is like, 'A', makes it '0A', etc.
            if (hex.Length == 1)
                hex = "0" + hex;

            return hex;
        }

        private string ConvertToHex(string decval)
        {
            // Convert text string to unsigned integer
            int uiDecimal = System.Convert.ToInt32(decval); 
            return String.Format("{0:x2}", uiDecimal);
        }

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
            {
                WriteStatus("ID Type: System Title. BE CAREFUL!");
            }
            else if ((ttlid.Substring(0, 8) == "00010000") || (ttlid.Substring(0, 8) == "00010004"))
            {
                WriteStatus("ID Type: Disc-Based Game. Unlikely NUS Content!");
            }
            else if (ttlid.Substring(0, 8) == "00010001")
            {
                WriteStatus("ID Type: Downloaded Channel. Possible NUS Content.");
            }
            else if (ttlid.Substring(0, 8) == "00010002")
            {
                WriteStatus("ID Type: System Channel. BE CAREFUL!");
            }
            else if (ttlid.Substring(0, 8) == "00010004")
            {
                WriteStatus("ID Type: Game Channel. Unlikely NUS Content!");
            }
            else if (ttlid.Substring(0, 8) == "00010005")
            {
                WriteStatus("ID Type: Downloaded Game Content. Unlikely NUS Content!");
            }
            else if (ttlid.Substring(0, 8) == "00010008")
            {
                WriteStatus("ID Type: 'Hidden' Channel. Unlikely NUS Content!");
            }
            else
            {
                WriteStatus("ID Type: Unknown. Unlikely NUS Content!");
            }
        }

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

        // Returns array of shared/normal values for a tmd...
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
            else
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

            // Running Downloads in background so no form freezing
            NUSDownloader.RunWorkerAsync();
        }

        private void NUSDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Preparations for Downloading
            Control.CheckForIllegalCrossThreadCalls = false;
            WriteStatus("Starting NUS Download. Please be patient!");
            SetEnableforDownload(false);
            downloadstartbtn.Text = "Starting NUS Download!";

            // Current directory...
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(@"\")) || !(currentdir.EndsWith(@"/")))
                currentdir += @"\";

            // Prevent crossthread issues
            string titleid = titleidbox.Text;

            // Creates the directory 
            CreateTitleDirectory();

            // Wii / DSi
            bool wiimode = radioButton1.Checked;
            
            // Set UserAgent to Wii value
            generalWC.Headers.Add("User-Agent", "wii libnup/1.0");

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = currentdir + titleid + @"\";
            else
                titledirectory = currentdir + titleid + "v" + titleversion.Text + @"\";

            downloadstartbtn.Text = "Prerequisites: (0/2)";

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
                }
            }
            downloadstartbtn.Text = "Prerequisites: (2/2)";
            dlprogress.Value = 100;

            // Obtain TitleKey
            byte[] titlekey = new byte[16];
            if (decryptbox.Checked == true)
            { 
                // Create ticket file holder
                byte[] cetkbuf = FileLocationToByteArray(titledirectory + @"\cetk");

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
                    iv[i+8] = 0x00;
			    }

                // Standard/Korea Common Key
                byte[] keyBytes;
                if (cetkbuf[0x01F1] == 0x01)
                {
                    WriteStatus("Key Type: Korean");
                    keyBytes = LoadCommonKey(@"\kkey.bin");
                }
                else
                {
                    WriteStatus("Key Type: Standard");
                    if (wiimode)
                        keyBytes = LoadCommonKey(@"\key.bin");
                    else
                        keyBytes = LoadCommonKey(@"\dsikey.bin");
                }
           
                initCrypt(iv, keyBytes);

                WriteStatus("Title Key: " + DisplayBytes(Decrypt(titlekey), ""));
                titlekey = Decrypt(titlekey);
            }

            // Read the tmd as a stream...
            byte[] tmd = FileLocationToByteArray(titledirectory + tmdfull);

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

            // Renaming would be ideal, but gives too many errors...
            /*if ((currentdir + titleid + "v" + titleversion.Text + @"\") != titledirectory)
            {
                Directory.Move(titledirectory, currentdir + titleid + "v" + titleversion.Text + @"\");
                titledirectory = currentdir + titleid + "v" + titleversion.Text + @"\";
                DirectoryInfo di = new DirectoryInfo(titledirectory);
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
                downloadstartbtn.Text = "Content: (" + (i + 1) + @"/" + contentstrnum + ")";
                currentcontentlocation += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);

                // Decrypt stuff...
                if (decryptbox.Checked == true)
                {
                    // Create content file holder
                    byte[] contbuf = FileLocationToByteArray(titledirectory + @"\" + tmdcontents[i]);

                    // Create ticket file holder
                    byte[] cetkbuf = FileLocationToByteArray(titledirectory + @"\cetk");

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
                    FileStream decfs = new FileStream(titledirectory + @"\" + zeros + i.ToString("X2") + ".app", FileMode.Create);
                    decfs.Write(Decrypt(contbuf), 0, int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber));
                    decfs.Close();
                    WriteStatus("  - Decrypted: " + zeros + i.ToString("X2") + ".app"); */

                    FileStream decfs = new FileStream(titledirectory + @"\" + tmdcontents[i] + ".app", FileMode.Create);
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
                // Create ticket file holder
                byte[] cetkbuff = FileLocationToByteArray(titledirectory + @"\cetk");

                // Titlekey
                for (int i = 0; i < 16; i++)
                {
                    titlekey[i] = cetkbuff[0x1BF + i];
                }
                //titlekeybox.Text = DisplayBytes(titlekey).Replace(" ", "");
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
                    timelimit[i] = cetkbuff[0x248 + 1];
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

                // Re-Gather information...
                byte[] tmdrefresh = FileLocationToByteArray(titledirectory + tmdfull);
                tmdcontents = GetContentNames(tmd, ContentCount(tmdrefresh));
                tmdsizes = GetContentSizes(tmd, ContentCount(tmdrefresh));
                tmdhashes = GetContentHashes(tmd, ContentCount(tmdrefresh));
                tmdindices = GetContentIndices(tmd, ContentCount(tmdrefresh));

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
                PackWAD(titleid, tmdfull, tmdcontents.Length, tmdcontents, tmdsizes, titledirectory);
            }

            SetEnableforDownload(true);
            downloadstartbtn.Text = "Start NUS Download!";
            dlprogress.Value = 0;

        }

        private void CreateTitleDirectory()
        {
            // Creates the directory for the downloaded title...
            string currentdir = Application.StartupPath;
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar)) == false)
                currentdir += Path.DirectorySeparatorChar;

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = Path.Combine(currentdir, titleidbox.Text + Path.DirectorySeparatorChar);
            else
                titledirectory = Path.Combine(currentdir, titleidbox.Text + "v" + titleversion.Text + Path.DirectorySeparatorChar);

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

        private void DeleteTitleDirectory()
        {
            string currentdir = Application.StartupPath;
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar)) == false)
                currentdir += Path.DirectorySeparatorChar;

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = Path.Combine(currentdir, titleidbox.Text + Path.DirectorySeparatorChar);
            else
                titledirectory = Path.Combine(currentdir, titleidbox.Text + "v" + titleversion.Text + Path.DirectorySeparatorChar);

            if (Directory.Exists(titledirectory))
                Directory.Delete(titledirectory, true);

            //Directory.CreateDirectory(currentdir + titleidbox.Text);
        }

        private void DownloadNUSFile(string titleid, string filename, string placementdir, int sizeinbytes, bool iswiititle)
        {
            // Create NUS URL...
            string nusfileurl;
            if (iswiititle)
                nusfileurl = NUSURL + titleid + @"/" + filename;
            else
                nusfileurl = DSiNUSURL + titleid + @"/" + filename;

            WriteStatus("Grabbing " + filename + "...");

            // State size of file...
            if (sizeinbytes != 0)
                statusbox.Text += " (" + Convert.ToString(sizeinbytes) + " bytes)";

            // Download NUS file...
            generalWC.DownloadFile(nusfileurl, placementdir + filename);
        }

        public void PackWAD(string titleid, string tmdfilename, int contentcount, string[] contentnames, string[] contentsizes, string totaldirectory)
        {
            // Directory stuff
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(@"\")) || !(currentdir.EndsWith(@"/")))
                currentdir += @"\";

            // Create cert file holder
            byte[] certsbuf = FileLocationToByteArray(currentdir + @"\cert.sys");

            // Create ticket file holder
            byte[] cetkbuf = FileLocationToByteArray(totaldirectory + @"\cetk");

            // Create tmd file holder
            byte[] tmdbuf = FileLocationToByteArray(totaldirectory + @"\" + tmdfilename);

            if (wadnamebox.Text.Contains("[v]") == true)
                wadnamebox.Text = wadnamebox.Text.Replace("[v]", "v" + titleversion.Text);

            // Create wad file
            FileStream wadfs = new FileStream(totaldirectory + @"\" + RemoveIllegalCharacters(wadnamebox.Text), FileMode.Create);

            // Add wad stuffs
            WADHeader wad = new WADHeader();
            wad.HeaderSize = 0x20;
            wad.WadType = 0x49730000;
            wad.CertChainSize = 0xA00;

            // Write cert[] to 0x40.
            wadfs.Seek(0x40, SeekOrigin.Begin);
            wadfs.Write(certsbuf, 0, certsbuf.Length);
            WriteStatus("Cert wrote (0x" + Convert.ToString(64, 16) + ")");

            // Need 64 byte boundary...
            wadfs.Seek(2624, SeekOrigin.Begin);

            // Cert is 2560
            // Write ticket at this point...
            wad.TicketSize = 0x2A4;
            wadfs.Write(cetkbuf, 0, wad.TicketSize);
            WriteStatus("Ticket wrote (0x" + Convert.ToString((wadfs.Length - 0x2A4), 16) + ")");

            // Need 64 byte boundary...
            wadfs.Seek(ByteBoundary(Convert.ToInt32(wadfs.Length)), SeekOrigin.Begin);

            // Write TMD at this point...
            wad.TMDSize = 484 + (contentcount * 36);
            wadfs.Write(tmdbuf, 0, 484 + (contentcount * 36));
            WriteStatus("TMD wrote (0x" + Convert.ToString((wadfs.Length - (484 + (contentcount * 36))), 16) + ")");

            // Preliminary data size of wad file.
            wad.DataSize = 0;

            // Loop n Add contents
            for (int i = 0; i < contentcount; i++)
            {
                // Need 64 byte boundary...
                wadfs.Seek(ByteBoundary(Convert.ToInt32(wadfs.Length)), SeekOrigin.Begin);

                // Create content file holder
                byte[] contbuf = FileLocationToByteArray(totaldirectory + @"\" + contentnames[i]);

                wadfs.Write(contbuf, 0, contbuf.Length);

                WriteStatus(contentnames[i] + " wrote (0x" + Convert.ToString((wadfs.Length - contbuf.Length), 16) + ")");
                HandleMismatch(int.Parse(contentsizes[i], System.Globalization.NumberStyles.HexNumber), contbuf.Length);

                wad.DataSize += contbuf.Length;
            }

            // Seek the beginning of the WAD...
            wadfs.Seek(0, SeekOrigin.Begin);

            // Write initial part of header
            byte[] start = new byte[8] { 0x00, 0x00, 0x00, 0x20, 0x49, 0x73, 0x00, 0x00 };
            wadfs.Write(start, 0, start.Length);

            // Write CertChainLength
            wadfs.Seek(0x08, SeekOrigin.Begin);
            byte[] chainsize = InttoByteArray(wad.CertChainSize, 4);
            wadfs.Write(chainsize, 0, 4);

            // Write res
            byte[] reserved = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
            wadfs.Seek(0x0C, SeekOrigin.Begin);
            wadfs.Write(reserved, 0, 4);

            // Write ticketsize
            byte[] ticketsize = new byte[4] { 0x00, 0x00, 0x02, 0xA4 };
            wadfs.Seek(0x10, SeekOrigin.Begin);
            wadfs.Write(ticketsize, 0, 4);

            // Write tmdsize
            int strippedtmd = 484 + (contentcount * 36);
            byte[] tmdsize = InttoByteArray(strippedtmd, 4);
            wadfs.Seek(0x14, SeekOrigin.Begin);
            wadfs.Write(tmdsize, 0, 4);

            // Write data size
            wadfs.Seek(0x18, SeekOrigin.Begin);
            byte[] datasize = InttoByteArray(wad.DataSize, 4);
            wadfs.Write(datasize, 0, 4);

            // Finished.
            WriteStatus("WAD Created: " + wadnamebox.Text);

            // Close filesystem...
            wadfs.Close();
        }

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
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar)) == false)
                currentdir += Path.DirectorySeparatorChar;
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

            /* if (IsWin7())
                WriteStatus("Windows 7 Features: Enabled"); */
            
            WriteStatus("");
            WriteStatus("Special thanks to:");
            WriteStatus(" * Crediar for his wadmaker tool + source, and for the advice!");
            WriteStatus(" * SquidMan/Galaxy/comex/Xuzz for advice/sources.");
            WriteStatus(" * Pasta for database compilation assistance.");
            WriteStatus(" * #WiiDev for answering the tough questions.");
            WriteStatus(" * Anyone who helped beta test on GBATemp!");
            WriteStatus(" * Famfamfam for the Silk Icon Set.");
        }
        
        private void getcerts_Click(object sender, EventArgs e)
        {
            // Get a certs.sys from NUS...

            // Directory stuff
            string currentdir = Application.StartupPath;
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar)) == false)
                currentdir += Path.DirectorySeparatorChar;

            // Create certs file
            FileStream certsfs = new FileStream(currentdir + @"\cert.sys", FileMode.Create);

            // Getting it from SystemMenu 3.2
            DownloadNUSFile("0000000100000002", "tmd.289", currentdir + @"\", 0, true);
            DownloadNUSFile("0000000100000002", "cetk", currentdir + @"\", 0, true);

            // Create ticket file holder
            byte[] cetkbuf = FileLocationToByteArray(currentdir + "cetk");

            // Create tmd file holder
            byte[] tmdbuf = FileLocationToByteArray(currentdir + "tmd.289");

            // Write CA cert...
            certsfs.Seek(0, SeekOrigin.Begin);
            certsfs.Write(tmdbuf, 0x628, 0x400);
            WriteStatus("Added CA Cert!");

            // Write CACP cert...
            certsfs.Seek(0x400, SeekOrigin.Begin);
            certsfs.Write(tmdbuf, 0x328, 0x300);
            WriteStatus("Added CACP Cert!");

            // Write CAXS cert...
            certsfs.Seek(0x700, SeekOrigin.Begin);
            certsfs.Write(cetkbuf, 0x2A4, 0x300);
            WriteStatus("Added CAXS Cert!");
            certsfs.Close();

            // Hash check the cert.sys...
            if (verifyMd5Hash(currentdir + @"\cert.sys", certs_MD5) == true)
            {
                WriteStatus("Certs File Successfully Created!");
            }
            else
            {
                WriteStatus("Error in Creating Certs File!");
                WriteStatus("Please report this error if you are sure it is not your internet connection");
            }

            // Re-enable controls...
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Enabled = true;
            }
            getcerts.Visible = false;
            wadnamebox.Enabled = false;

            // Cleanup...
            File.Delete(currentdir + "cetk");
            File.Delete(currentdir + "tmd.289");
        }

        static string getMd5Hash(string input)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            FileStream fs = new FileStream(input, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(fs);
            fs.Close();
            fs.Dispose();
            foreach (byte hex in hash)
            {
                //Returns hash in lower case.
                sb.Append(hex.ToString("x2"));
            }
            return sb.ToString();
        }

        // Verify a hash against a string.
        static bool verifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = getMd5Hash(input);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public string DisplayBytes(byte[] bytes, string spacer)
        {
            string output = "";
            for (int i = 0; i < bytes.Length; ++i)
            {
                output += bytes[i].ToString("X2") + spacer;
            }
            return output;
        }

        static public byte[] ComputeSHA(byte[] data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            return sha.ComputeHash(data);
        }

        public byte[] LoadCommonKey(string keyfile)
        {
            // Directory stuff
            string currentdir = Application.StartupPath;
            if (!(currentdir.EndsWith(@"\")) || !(currentdir.EndsWith(@"/")))
                currentdir += @"\";

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
                if (titlename.Contains("IOS"))
                    wadnamebox.Text = titlename + "-64-[v].wad";
                else
                    wadnamebox.Text = titlename + "-NUS-[v].wad";
                if (titleversion.Text != "")
                    wadnamebox.Text = wadnamebox.Text.Replace("[v]", "v" + titleversion.Text);
            }

            // Check for danger item
            if ((e.ClickedItem.OwnerItem.OwnerItem.Image) == (redgreen) || (e.ClickedItem.OwnerItem.OwnerItem.Image) == (redorange))
            {
                WriteStatus("\r\n" + e.ClickedItem.OwnerItem.OwnerItem.ToolTipText);
            }
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

        void HandleMismatch(int contentsize, int actualsize)
        {
            if (contentsize != actualsize)
            {
                if ((contentsize - actualsize) > 16)
                {
                    statusbox.Text += " (BAD Mismatch) (Dif: " + (contentsize - actualsize);
                }
                else
                {
                    statusbox.Text += " (Safe Mismatch)";
                }
            }
        }

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
            byte[] cetkbuff = FileLocationToByteArray(fileinfo[0] + @"\cetk");

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
            byte[] cetkbuff = FileLocationToByteArray(fileinfo[0] + @"\cetk");

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

        // C# to convert a string to a byte array.
        public static byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
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

        private void SetEnableforDownload(bool enabled)
        {
            // Disable things the user should not mess with during download...
            downloadstartbtn.Enabled = enabled;
            titleidbox.Enabled = enabled;
            titleversion.Enabled = enabled;
            TMDButton.Enabled = enabled;
            databaseButton.Enabled = enabled;
            packbox.Enabled = enabled;
            localuse.Enabled = enabled;
            ignoreticket.Enabled = enabled;
            truchabox.Enabled = enabled;
            decryptbox.Enabled = enabled;
        }

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

        private byte[] FileLocationToByteArray(string filename)
        {
            FileStream fs = File.OpenRead(filename);
            byte[] filebytearray = ReadFully(fs, 460);
            fs.Close();
            return filebytearray;
        }

        private void UpdatePackedName()
        {
            // Change WAD name if applicable
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
            }
            wadnamebox.Text = RemoveIllegalCharacters(wadnamebox.Text);
        }

        // This is WIP code/theory...
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
            byte[] commonkey = LoadCommonKey(@"\key.bin");

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

        /* Pad Byte[] to specific alignment...
        private byte[] AlignByteArray(byte[] content, int alignto)
        {
            long thelength = content.Length - 1;
            long remainder = thelength % alignto;

            while (remainder != 0)
            {
                thelength += 1;
                remainder = thelength % alignto;
            }
            Array.Resize(ref content, (int)thelength);
            return content;
        } */

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
                byte[] commonkey = LoadCommonKey(@"\key.bin");

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

        private byte[] PatchBinary(byte[] content, int offset, byte[] newvalues)
        {
            for (int a = 0; a < newvalues.Length; a++)
            {
                if (newvalues[a] >= 0)
                    content[offset + a] = newvalues[a];
            }
            return content;
        }

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

        private string RetrieveNewDatabase()
        {
            // Retrieve Wiibrew database page source code
            WebClient databasedl = new WebClient();
            string wiibrewsource = databasedl.DownloadString("http://www.wiibrew.org/wiki/NUS_Downloader/database");

            // Strip out HTML
            wiibrewsource = Regex.Replace(wiibrewsource, @"<(.|\n)*?>", "");

            // Shrink to fix only the database
            string startofdatabase = "&lt;database v";
            string endofdatabase = "&lt;/database&gt;";
            wiibrewsource = wiibrewsource.Substring(wiibrewsource.IndexOf(startofdatabase), wiibrewsource.Length - wiibrewsource.IndexOf(startofdatabase));
            wiibrewsource = wiibrewsource.Substring(0, wiibrewsource.IndexOf(endofdatabase) + endofdatabase.Length);

            // Fix ", <, and >
            wiibrewsource = wiibrewsource.Replace("&lt;","<");
            wiibrewsource = wiibrewsource.Replace("&gt;",">");
            wiibrewsource = wiibrewsource.Replace("&quot;",'"'.ToString());
            wiibrewsource = wiibrewsource.Replace("&nbsp;", ""); // Shouldn't occur, but they happen...

            // Return parsed xml database...
            return wiibrewsource;
        }

        private void updateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusbox.Text = "";
            WriteStatus("Updating your database.xml from Wiibrew.org");
            WriteStatus(" - Database successfully parsed!");

            string database = RetrieveNewDatabase();
            string currentversion = GetDatabaseVersion("database.xml");
            string onlineversion = GetDatabaseVersion(database);
            WriteStatus("  - Current Database Version: " + currentversion);
            WriteStatus("  - Online Database Version: " + onlineversion);

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

            WriteStatus("Database successfully updated!");
        }
    }
}
