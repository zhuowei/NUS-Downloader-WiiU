using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Xml;

namespace NUS_Downloader
{
    public partial class Form1 : Form
    {
        const string NUSURL = "http://nus.cdn.shop.wii.com/ccs/download/";
        const string DSiNUSURL = "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/";
        string version = "v1.2";
        WebClient generalWC = new WebClient();
        static RijndaelManaged rijndaelCipher;
        static bool dsidecrypt = false;

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

        const string certs_MD5 = "7677AD47BAA5D6E3E313E72661FBDC16";

        public Form1()
        {
            InitializeComponent();
            BootChecks();
        }

        public Form1(string[] args)
        {
            // CLI Mode
            InitializeComponent();

            // certs.sys / key.bin
            if (BootChecks() == false)
                return;

            // DEBUG
            /*
            for (int i = 0; i < args.Length; i++)
            {
                WriteStatus(i + ": " + args[i]);
            }
            */

            // Vars
            bool startnow = false;

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

                // Running Downloads in background so no form freezing
                NUSDownloader.RunWorkerAsync();
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "NUSD - " + version + " - WB3000";
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

            // Check for DSi common key bin file...
            if (File.Exists(currentdir + "dskey.bin") == false)
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
                // Read version of Database.xml
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("database.xml");
                XmlNodeList DatabaseList = xDoc.GetElementsByTagName("database");
                XmlAttributeCollection Attributes = DatabaseList[0].Attributes;
                WriteStatus("Database.xml detected.");
                WriteStatus(" - Version: " + Attributes[0].Value);

                // Load it up...
                ClearDatabaseStrip();
                FillDatabaseStrip();
                LoadRegionCodes();
            }

            return result;
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
                FileStream fs = File.OpenRead(opentmd.FileName);
                byte[] tmd = ReadFully(fs, 20);
                WriteStatus("TMD Loaded (" + tmd.Length + " bytes)");

                // Read ID...
                titleidbox.Text = "";
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
                string sysversion = "";
                for (int i = 0; i < 8; i++)
                {
                    sysversion += MakeProperLength(ConvertToHex(Convert.ToString(tmd[0x184 + i])));
                }
                sysversion = Convert.ToString(int.Parse(sysversion.Substring(14, 2), System.Globalization.NumberStyles.HexNumber));
                if (sysversion != "0")
                    WriteStatus("Requires: IOS" + sysversion);

                // Read Content #...
                string contentstrnum = "";
                for (int x = 478; x < 480; x++)
                {
                    contentstrnum += TrimLeadingZeros(Convert.ToString(tmd[x]));
                }
                WriteStatus("Content Count: " + contentstrnum);

                string[] tmdcontents = GetContentNames(tmd, Convert.ToInt32(contentstrnum));
                string[] tmdsizes = GetContentSizes(tmd, Convert.ToInt32(contentstrnum));
                byte[] tmdhashes = GetContentHashes(tmd, Convert.ToInt32(contentstrnum));
                byte[] tmdindices = GetContentIndices(tmd, Convert.ToInt32(contentstrnum));

                // Loop through each content and display name, hash, index
                for (int i = 0; i < Convert.ToInt32(contentstrnum); i++)
                {
                    WriteStatus("   Content " + (i + 1) + ": " + tmdcontents[i] + " (" + Convert.ToString(int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber)) + " bytes)");
                    byte[] hash = new byte[20];
                    for (int x = 0; x < 20; x++)
                    {
                        hash[x] = tmdhashes[(i*20)+x];
                    }
                    WriteStatus("  - Hash: " + DisplayBytes(hash));
                    WriteStatus("  - Index: " + tmdindices[i]);

                }
            }
        }

        private void WriteStatus(string Update)
        {
            // Small thing for writing text to the statusbox...

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

            string hexval;
            hexval = String.Format("{0:x2}", uiDecimal);
            return hexval;
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
            //WriteStatus(DisplayBytes(contenthashes));
            return contenthashes;
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

            // Running Downloads in background so no form freezing
            NUSDownloader.RunWorkerAsync();
        }

        private void NUSDownloader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Preparations for Downloading
            Control.CheckForIllegalCrossThreadCalls = false;
            WriteStatus("Starting NUS Download. Please be patient!");
            button3.Enabled = false;
            titleidbox.Enabled = false;
            titleversion.Enabled = false;
            button3.Text = "Starting NUS Download!";

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
            generalWC.Headers.Add("User-Agent", "Opera/9.30 (Nintendo Wii; U; ; 2071; Wii Shop Channel/16.0(A); en)");

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                titledirectory = currentdir + titleid + @"\";
            else
                titledirectory = currentdir + titleid + "v" + titleversion.Text + @"\";

            button3.Text = "Prerequisites: (0/2)";

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
                button3.Enabled = true;
                titleidbox.Enabled = true;
                titleversion.Enabled = true;
                button3.Text = "Start NUS Download!";
                dlprogress.Value = 0;
                DeleteTitleDirectory();
                return;
            }
            button3.Text = "Prerequisites: (1/2)";
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
                    titleidbox.Enabled = true;
                    titleversion.Enabled = true;
                    button3.Text = "Start NUS Download!";
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
            button3.Text = "Prerequisites: (2/2)";
            dlprogress.Value = 100;

            // Obtain TitleKey
            byte[] titlekey = new byte[16];
            if (decryptbox.Checked == true)
            { 
                // Create ticket file holder
                FileStream fs1 = File.OpenRead(titledirectory + @"\cetk");
                byte[] cetkbuf = ReadFully(fs1, 20);
                fs1.Close();

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

                // Common Key
                byte[] keyBytes;
                if (wiimode)
                    keyBytes = LoadCommonKey(@"\key.bin");
                else
                    keyBytes = LoadCommonKey(@"\dskey.bin");

                initCrypt(iv, keyBytes);

                WriteStatus("Title Key: " + DisplayBytes(Decrypt(titlekey)));
                titlekey = Decrypt(titlekey);
            }

            // Read the tmd as a stream...
            FileStream fs = File.OpenRead(titledirectory + tmdfull);
            byte[] tmd = ReadFully(fs, 20);

            // Read Title Version...
            string tmdversion = "";
            for (int x = 476; x < 478; x++)
            {
                tmdversion += MakeProperLength(ConvertToHex(Convert.ToString(tmd[x])));
            }
            titleversion.Text = Convert.ToString(int.Parse(tmdversion, System.Globalization.NumberStyles.HexNumber));

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
            button3.Text = "Content: (0/" + contentstrnum + ")";
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
                    button3.Enabled = true;
                    titleidbox.Enabled = true;
                    titleversion.Enabled = true;
                    button3.Text = "Start NUS Download!";
                    dlprogress.Value = 0;
                    DeleteTitleDirectory();
                    return;
                }

                // Progress reporting advances...
                button3.Text = "Content: (" + (i + 1) + @"/" + contentstrnum + ")";
                currentcontentlocation += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);

                // Decrypt stuff...
                if (decryptbox.Checked == true)
                {
                    // Create content file holder
                    FileStream cont = File.OpenRead(titledirectory + @"\" + tmdcontents[i]);
                    byte[] contbuf = ReadFully(cont, 20);
                    cont.Close();

                    // Create ticket file holder
                    FileStream fs1 = File.OpenRead(titledirectory + @"\cetk");
                    byte[] cetkbuf = ReadFully(fs1, 20);
                    fs1.Close();

                    // IV (00+IDX+more000)
                    byte[] iv = new byte[16];
                    for (int x = 0; x < 8; x++)
                    {
                        iv[x] = 0x00;
                    }
                    for (int x = 0; x < 7; x++)
                    {
                        iv[x + 8] = 0x00;
                    }
                    iv[1] = tmdindices[i];
                    
                    initCrypt(iv, titlekey); 

                    // Create decrypted file
                    string zeros = "000000";
                    FileStream decfs = new FileStream(titledirectory + @"\" + zeros + i.ToString("X2") + ".app", FileMode.Create);
                    decfs.Write(Decrypt(contbuf), 0, int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber));
                    decfs.Close();
                    WriteStatus("  - Decrypted: " + zeros + i.ToString("X2") + ".app");

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
                        WriteStatus("    - True Hash: " + DisplayBytes(hash));
                        WriteStatus("    - You Got: " + DisplayBytes(ComputeSHA(Decrypt(contbuf))));
                    }
                }

                dlprogress.Value = Convert.ToInt32(((currentcontentlocation / totalcontentsize) * 100));
            }

            WriteStatus("NUS Download Finished.");

            if ((packbox.Checked == true) && (wiimode == true))
            {
                PackWAD(titleid, tmdfull, tmdcontents.Length, tmdcontents, tmdsizes, titledirectory);
            }

            button3.Enabled = true;
            titleidbox.Enabled = true;
            titleversion.Enabled = true;
            button3.Text = "Start NUS Download!";
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

            /*if (!(currentdir.EndsWith(@"\")) || !(currentdir.EndsWith(@"/")))
                currentdir += @"\"; */

            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar)) == false)
                currentdir += Path.DirectorySeparatorChar;

            // Get placement directory early...
            string titledirectory;
            if (titleversion.Text == "")
                //titledirectory = currentdir + titleidbox.Text + @"\";
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
            FileStream certfs = File.OpenRead(currentdir + @"\cert.sys");
            byte[] certsbuf = ReadFully(certfs, 20);
            certfs.Close();

            // Create ticket file holder
            FileStream fs1 = File.OpenRead(totaldirectory + @"\cetk");
            byte[] cetkbuf = ReadFully(fs1, 20);
            fs1.Close();

            // Create tmd file holder
            FileStream fs2 = File.OpenRead(totaldirectory + @"\" + tmdfilename);
            byte[] tmdbuf = ReadFully(fs2, 20);
            fs2.Close();

            if (wadnamebox.Text.Contains("[v]") == true)
                wadnamebox.Text = wadnamebox.Text.Replace("[v]", "v" + titleversion.Text);

            // Create wad file
            FileStream wadfs = new FileStream(totaldirectory + @"\" + wadnamebox.Text, FileMode.Create);

            // Add wad stuffs
            WADHeader wad = new WADHeader();
            wad.HeaderSize = 0x20;
            wad.WadType = 0x49730000;
            wad.CertChainSize = 0xA00;

            // TMDSize is length of buffer.
            wad.TMDSize = tmdbuf.Length;
            // TicketSize is length of cetkbuf.
            wad.TicketSize = cetkbuf.Length;

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
                FileStream cont = File.OpenRead(totaldirectory + @"\" + contentnames[i]);
                byte[] contbuf = ReadFully(cont, 20);
                cont.Close();

                wadfs.Write(contbuf, 0, contbuf.Length);

                /*if (int.Parse(contentsizes[i], System.Globalization.NumberStyles.HexNumber) != contbuf.Length)
                {
                    // Content size mismatch
                    WriteStatus(contentnames[i] + " wrote (0x" + Convert.ToString((wadfs.Length - contbuf.Length), 16) + ") (Mismatch)");
                }
                else
                {
                    WriteStatus(contentnames[i] + " wrote (0x" + Convert.ToString((wadfs.Length - contbuf.Length), 16) + ")");
                } */
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
            byte[] chainsize = InttoByteArray(wad.CertChainSize);
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
            byte[] tmdsize = InttoByteArray(strippedtmd);
            wadfs.Seek(0x14, SeekOrigin.Begin);
            wadfs.Write(tmdsize, 0, 4);

            // Write data size
            wadfs.Seek(0x18, SeekOrigin.Begin);
            byte[] datasize = InttoByteArray(wad.DataSize);
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

        private byte[] InttoByteArray(int size)
        {
            // Take integer and make into byte array
            byte[] b = new byte[4];
            b = BitConverter.GetBytes(size);

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
            WriteStatus("This program coded by WB3000");
            WriteStatus("");
            string currentdir = Application.StartupPath;
            if (currentdir.EndsWith(Convert.ToString(Path.DirectorySeparatorChar)) == false)
                currentdir += Path.DirectorySeparatorChar;
            if (File.Exists(currentdir + "key.bin") == false)
            {
                WriteStatus("Wii Decryption: Need (key.bin)");
            }
            else
            {
                WriteStatus("Wii Decryption: OK");
            }

            if (File.Exists(currentdir + "dskey.bin") == false)
            {
                WriteStatus("DSi Decryption: Need (dskey.bin)");
            }
            else
            {
                WriteStatus("DSi Decryption: OK");
            }
            
            WriteStatus("");
            WriteStatus("Special thanks to:");
            WriteStatus(" * Crediar for his wadmaker tool + source, and for the advice!");
            WriteStatus(" * SquidMan/Galaxy/comex for advice/sources.");
            WriteStatus(" * #WiiDev for general assistance whenever I had questions.");
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
            FileStream cetk = File.OpenRead(currentdir + "cetk");
            byte[] cetkbuf = ReadFully(cetk, 20);
            cetk.Close();

            // Create tmd file holder
            FileStream tmd = File.OpenRead(currentdir + "tmd.289");
            byte[] tmdbuf = ReadFully(tmd, 20);
            tmd.Close();

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
                if (packbox.Checked == true)
                {
                    if (titleidbox.Enabled == true)
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
                }
            }
            else
            {
                wadnamebox.Enabled = false;
                wadnamebox.Text = "";
            }
        }

        private void titleidbox_TextChanged(object sender, EventArgs e)
        {
            // Change WAD name if applicable
            if (packbox.Checked == true)
            {
                if (titleidbox.Enabled == true)
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
            }
        }

        private void titleversion_TextChanged(object sender, EventArgs e)
        {
            // Change WAD name if applicable
            if (packbox.Checked == true)
            {
                if (titleidbox.Enabled == true)
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
            }
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

        public string DisplayBytes(byte[] bytes)
        {
            string output = "";
            for (int i = 0; i < bytes.Length; ++i)
            {
                output += bytes[i].ToString("X2") + " ";
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
                // Read common key into array
                FileStream ckey = File.OpenRead(currentdir + keyfile);
                byte[] ckeybuf = ReadFully(ckey, 16);
                ckey.Close();
                return ckeybuf;
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
                    string stticket = "";

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
                                XMLToolStripItem.DropDownItems.Add("Latest Version");
                                if (ChildrenOfTheNode[z].InnerText != "")
                                {
                                    for (int y = 0; y < versions.Length; y++)
                                    {
                                        XMLToolStripItem.DropDownItems.Add("v" + versions[y]);
                                    }
                                }
                                break;
                            case "region":
                                /* string[] regions = ChildrenOfTheNode[z].InnerText.Split(',');
                                if (ChildrenOfTheNode[z].InnerText != "")
                                {
                                    for (int y = 0; y < regions.Length; y++)
                                    {
                                        XMLToolStripItem.DropDownItems.Add(regions[y]);
                                    }
                                }
                                break; */
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
                                bool ticket = Convert.ToBoolean(ChildrenOfTheNode[z].InnerText);
                                if (!ticket)
                                    stticket += "(-)";
                                else
                                    stticket += "(+)";
                                break;
                        }
                        XMLToolStripItem.Text = titleID + " " + stticket + " " + descname;
                        XMLToolStripItem.Text = String.Format("{0} {1} {2}", titleID, stticket, descname);
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
                    case "Commodore 64":
                        C64MenuList.DropDownItems.Add(additionitem);
                        break;
                    case "NeoGeo":
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
                    case "TurboGrafx16":
                        TurboGrafx16MenuList.DropDownItems.Add(additionitem);
                        break;
                    case "TurboGrafxCD":
                        TurboGrafxCDMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "MSX":
                        MSXMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "SMS":
                        SegaMSMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "Genesis":
                        GenesisMenuList.DropDownItems.Add(additionitem);
                        break;
                    case "VCArcade":
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

        void wwitem_regionclicked(object sender, ToolStripItemClickedEventArgs e)
        {
            titleidbox.Text = e.ClickedItem.OwnerItem.Text.Substring(0, 16);
            titleversion.Text = "";
            titleidbox.Text = titleidbox.Text.Replace("XX", e.ClickedItem.Text.Substring(0, 2));
            if (e.ClickedItem.OwnerItem.Text.Contains("(-)"))
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
        }

        void sysitem_versionclicked(object sender, ToolStripItemClickedEventArgs e)
        {
            titleidbox.Text = e.ClickedItem.OwnerItem.Text.Substring(0, 16);
            if (e.ClickedItem.Text != "Latest Version")
            {
                if (e.ClickedItem.Text.Contains("v"))
                {
                    if (e.ClickedItem.Text.Contains(" "))
                        titleversion.Text = e.ClickedItem.Text.Substring(1, e.ClickedItem.Text.IndexOf(' '));
                    else
                        titleversion.Text = e.ClickedItem.Text.Substring(1, e.ClickedItem.Text.Length - 1);
                }
                else
                {
                    titleidbox.Text = titleidbox.Text.Replace("XX", e.ClickedItem.Text.Substring(0, 2));
                    titleversion.Text = "";
                }
            }
            else
            {
                titleversion.Text = "";
            }
            if (e.ClickedItem.OwnerItem.Text.Contains("(-)"))
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
        }

        void HandleMismatch(int contentsize, int actualsize)
        {
            if (contentsize != actualsize)
            {
                if ((contentsize - actualsize) > 16)
                {
                    statusbox.Text += " (BAD Mismatch)";
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
		            <region index=0>41 (All/System)</region>
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
    }
}
