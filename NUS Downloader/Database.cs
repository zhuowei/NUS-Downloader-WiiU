///////////////////////////////////////////
// NUS Downloader: Database.cs           //
// $Rev::                              $ //
// $Author::                           $ //
// $Date::                             $ //
///////////////////////////////////////////

using System;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Drawing;

namespace NUS_Downloader
{
    class Database
    {
        private string SystemTag = "SYS";
        private string IosTag = "IOS";
        private string VcTag = "VC";
        private string WwTag = "WW";
        private string UpdateTag = "UPD";

        private string DSiSystemTag = "DSISYSTEM";
        private string DSiWareTag = "DSIWARE";

        private string[] VcConsoles = new string[11] { "C64", "GEN", "MSX", "N64", "NEO", "NES", 
            "SMS", "SNES", "TG16", "TGCD", "ARC" };

        private string databaseString;

        public static Image green = Properties.Resources.bullet_green;
        public static Image orange = Properties.Resources.bullet_orange;
        public static Image redgreen = Properties.Resources.bullet_redgreen;
        public static Image redorange = Properties.Resources.bullet_redorange;

        public static Image green_blue = Properties.Resources.bullet_green_blue;
        public static Image orange_blue = Properties.Resources.bullet_orange_blue;
        public static Image redgreen_blue = Properties.Resources.bullet_redgreen_blue;
        public static Image redorange_blue = Properties.Resources.bullet_redorange_blue;

        public void LoadDatabaseToStream(string databaseFile)
        {
            // Does it exist?
            if (!File.Exists(databaseFile))
                throw new FileNotFoundException("I couldn't find the database file!", "database.xml");
            
            databaseString = File.ReadAllText(databaseFile);
            
        }

        public string GetDatabaseVersion()
        {
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);
            XmlNodeList DatabaseList = xDoc.GetElementsByTagName("database");
            XmlAttributeCollection Attributes = DatabaseList[0].Attributes;
            return Attributes[0].Value;
        }

        public static string GetDatabaseVersion(string databaseString)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);
            XmlNodeList DatabaseList = xDoc.GetElementsByTagName("database");
            XmlAttributeCollection Attributes = DatabaseList[0].Attributes;
            return Attributes[0].Value;
        }
                        
        public ToolStripMenuItem[] LoadSystemTitles()
        {
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);

            XmlNodeList SystemTitlesXMLNodes = xDoc.GetElementsByTagName(SystemTag);

            ToolStripMenuItem[] systemTitleCollection = new ToolStripMenuItem[SystemTitlesXMLNodes.Count];

            for (int x = 0; x < SystemTitlesXMLNodes.Count; x++)
            {
                ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                XmlAttributeCollection XMLAttributes = SystemTitlesXMLNodes[x].Attributes;

                string titleID = "";
                string descname = "";
                bool dangerous = false;
                bool ticket = true;

                XmlNodeList ChildrenOfTheNode = SystemTitlesXMLNodes[x].ChildNodes;

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
                                        ToolStripMenuItem regitem =
                                            (ToolStripMenuItem)XMLToolStripItem.DropDownItems[b];
                                        regitem.DropDownItems.Add("Latest Version");
                                        for (int y = 0; y < versions.Length; y++)
                                        {
                                            regitem.DropDownItems.Add("v" + versions[y]);
                                        }
                                        //regitem.DropDownItemClicked += new ToolStripItemClickedEventHandler(Application.);
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
                                    XMLToolStripItem.DropDownItems.Add(RegionFromIndex(Convert.ToInt32(regions[y])));
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
                    {   // Wait what?
                        XMLToolStripItem.Text = descname;
                    }
                }
                //AddToolStripItemToStrip(i, XMLToolStripItem, XMLAttributes);
                systemTitleCollection[x] = XMLToolStripItem;
            }

            return systemTitleCollection;
        }

        public ToolStripMenuItem[] LoadIosTitles()
        {
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);
            XmlNodeList IosTitlesXMLNodes = xDoc.GetElementsByTagName(IosTag);
            ToolStripMenuItem[] iosTitleCollection = new ToolStripMenuItem[IosTitlesXMLNodes.Count];

            for (int x = 0; x < IosTitlesXMLNodes.Count; x++)
            {
                ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                XmlAttributeCollection XMLAttributes = IosTitlesXMLNodes[x].Attributes;

                string titleID = "";
                string descname = "";
                bool dangerous = false;
                bool ticket = true;

                XmlNodeList ChildrenOfTheNode = IosTitlesXMLNodes[x].ChildNodes;

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
                        case "ticket":
                            ticket = Convert.ToBoolean(ChildrenOfTheNode[z].InnerText);
                            break;
                        case "danger":
                            dangerous = true;
                            XMLToolStripItem.ToolTipText = ChildrenOfTheNode[z].InnerText;
                            break;
                        default:
                            break;
                    }
                    XMLToolStripItem.Image = SelectItemImage(ticket, dangerous);

                    if (titleID != "")
                        XMLToolStripItem.Text = String.Format("{0} - {1}", titleID, descname);
                    else
                        XMLToolStripItem.Text = descname; //Huh
                }
                
                iosTitleCollection[x] = XMLToolStripItem;
            }

            return iosTitleCollection;
        }

        public ToolStripMenuItem[][] LoadVirtualConsoleTitles()
        {
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);
            XmlNodeList VirtualConsoleXMLNodes = xDoc.GetElementsByTagName(VcTag);
            ToolStripMenuItem[][] vcTitleCollection = new ToolStripMenuItem[VcConsoles.Length][];

            for (int j = 0; j < vcTitleCollection.Length; j++)
            {
                vcTitleCollection[j] = new ToolStripMenuItem[0];
            }

            for (int x = 0; x < VirtualConsoleXMLNodes.Count; x++)
            {
                ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                XmlAttributeCollection XMLAttributes = VirtualConsoleXMLNodes[x].Attributes;

                string titleID = "";
                string descname = "";
                bool dangerous = false;
                bool ticket = true;

                XmlNodeList ChildrenOfTheNode = VirtualConsoleXMLNodes[x].ChildNodes;

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
                                        ToolStripMenuItem regitem =
                                            (ToolStripMenuItem)XMLToolStripItem.DropDownItems[b];
                                        regitem.DropDownItems.Add("Latest Version");
                                        for (int y = 0; y < versions.Length; y++)
                                        {
                                            regitem.DropDownItems.Add("v" + versions[y]);
                                        }
                                        //regitem.DropDownItemClicked += new ToolStripItemClickedEventHandler(deepitem_clicked);
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
                                    XMLToolStripItem.DropDownItems.Add(RegionFromIndex(Convert.ToInt32(regions[y])));
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
                    {   // Wait what?
                        XMLToolStripItem.Text = descname;
                    }
                }

                for (int a = 0; a < VcConsoles.Length; a++)
			    {
                    if (XMLAttributes[0].Value == VcConsoles[a])
                    {
                        Array.Resize(ref vcTitleCollection[a], vcTitleCollection[a].Length + 1);
                        vcTitleCollection[a][vcTitleCollection[a].Length - 1] = XMLToolStripItem;
                    }
			    }
            }

            return vcTitleCollection;
        }

        public ToolStripMenuItem[] LoadWiiWareTitles()
        {
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);
            XmlNodeList WiiWareTitlesXMLNodes = xDoc.GetElementsByTagName(WwTag);
            ToolStripMenuItem[] wwTitleCollection = new ToolStripMenuItem[WiiWareTitlesXMLNodes.Count];

            for (int x = 0; x < WiiWareTitlesXMLNodes.Count; x++)
            {
                ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                XmlAttributeCollection XMLAttributes = WiiWareTitlesXMLNodes[x].Attributes;

                string titleID = "";
                string descname = "";
                bool dangerous = false;
                bool ticket = true;

                XmlNodeList ChildrenOfTheNode = WiiWareTitlesXMLNodes[x].ChildNodes;

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
                                        ToolStripMenuItem regitem =
                                            (ToolStripMenuItem)XMLToolStripItem.DropDownItems[b];
                                        regitem.DropDownItems.Add("Latest Version");
                                        for (int y = 0; y < versions.Length; y++)
                                        {
                                            regitem.DropDownItems.Add("v" + versions[y]);
                                        }
                                        //regitem.DropDownItemClicked += new ToolStripItemClickedEventHandler(deepitem_clicked);
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
                                    XMLToolStripItem.DropDownItems.Add(RegionFromIndex(Convert.ToInt32(regions[y])));
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
                    {   // Wait what?
                        XMLToolStripItem.Text = descname;
                    }
                }
                
                wwTitleCollection[x] = XMLToolStripItem;
            }

            return wwTitleCollection;
        }

        /// <summary>
        /// Gathers the region based on index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="databasexml">XmlDocument with database inside</param>
        /// <returns>Region desc</returns>
        private string RegionFromIndex(int index)
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
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);

            XmlNodeList XMLRegionList = xDoc.GetElementsByTagName("REGIONS");
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
        /// Loads the region codes.
        /// </summary>
        public ToolStripMenuItem[] LoadRegionCodes()
        {
            /* TODO: make this check InvokeRequired...
            if (this.InvokeRequired)
            {
                Debug.Write("TOLDYOUSO!");
                BootChecksCallback bcc = new BootChecksCallback(LoadRegionCodes);
                this.Invoke(bcc);
                return;
            }*/

            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);

            XmlNodeList XMLRegionList = xDoc.GetElementsByTagName("REGIONS");
            XmlNodeList ChildrenOfTheNode = XMLRegionList[0].ChildNodes;

            ToolStripMenuItem[] regionItems = new ToolStripMenuItem[ChildrenOfTheNode.Count];

            // For each child node (region node)
            for (int z = 0; z < ChildrenOfTheNode.Count; z++)
            {
                regionItems[z] = new ToolStripMenuItem();
                regionItems[z].Text = ChildrenOfTheNode[z].InnerText;
            }

            return regionItems;
        }

        public ToolStripMenuItem[] LoadScripts()
        {
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);

            XmlNodeList ScriptXMLNodes = xDoc.GetElementsByTagName(UpdateTag);

            ToolStripMenuItem[] scriptCollection = new ToolStripMenuItem[ScriptXMLNodes.Count];

            for (int x = 0; x < ScriptXMLNodes.Count; x++)
            {
                ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                XmlAttributeCollection XMLAttributes = ScriptXMLNodes[x].Attributes;
                XmlNodeList ChildrenOfTheNode = ScriptXMLNodes[x].ChildNodes;

                for (int z = 0; z < ChildrenOfTheNode.Count; z++)
                {
                    switch (ChildrenOfTheNode[z].Name)
                    {
                        case "name":
                            XMLToolStripItem.Text = ChildrenOfTheNode[z].InnerText;
                            break;
                        case "script":
                            XMLToolStripItem.ToolTipText = ChildrenOfTheNode[z].InnerText;
                            break;
                        default:
                            break;
                    }
                    XMLToolStripItem.Image = Properties.Resources.script_start;
                    
                }
                scriptCollection[x] = XMLToolStripItem;
            }

            return scriptCollection;
        }

        public ToolStripMenuItem[] LoadDSiSystemTitles()
        {
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);
            XmlNodeList DSiSystemTitlesXMLNodes = xDoc.GetElementsByTagName(DSiSystemTag);
            ToolStripMenuItem[] dsiSystemTitleCollection = new ToolStripMenuItem[DSiSystemTitlesXMLNodes.Count];

            for (int x = 0; x < DSiSystemTitlesXMLNodes.Count; x++)
            {
                ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                XmlAttributeCollection XMLAttributes = DSiSystemTitlesXMLNodes[x].Attributes;

                string titleID = "";
                string descname = "";
                bool dangerous = false;
                bool ticket = true;

                XmlNodeList ChildrenOfTheNode = DSiSystemTitlesXMLNodes[x].ChildNodes;

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
                                        ToolStripMenuItem regitem =
                                            (ToolStripMenuItem)XMLToolStripItem.DropDownItems[b];
                                        regitem.DropDownItems.Add("Latest Version");
                                        for (int y = 0; y < versions.Length; y++)
                                        {
                                            regitem.DropDownItems.Add("v" + versions[y]);
                                        }
                                        //regitem.DropDownItemClicked += new ToolStripItemClickedEventHandler(deepitem_clicked);
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
                                    XMLToolStripItem.DropDownItems.Add(RegionFromIndex(Convert.ToInt32(regions[y])));
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
                    {   // Wait what?
                        XMLToolStripItem.Text = descname;
                    }
                }

                dsiSystemTitleCollection[x] = XMLToolStripItem;
            }

            return dsiSystemTitleCollection;
        }

        public ToolStripMenuItem[] LoadDsiWareTitles()
        {
            if (databaseString.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(databaseString);
            XmlNodeList DSiWareTitlesXMLNodes = xDoc.GetElementsByTagName(DSiWareTag);
            ToolStripMenuItem[] DSiWareTitleCollection = new ToolStripMenuItem[DSiWareTitlesXMLNodes.Count];

            for (int x = 0; x < DSiWareTitlesXMLNodes.Count; x++)
            {
                ToolStripMenuItem XMLToolStripItem = new ToolStripMenuItem();
                XmlAttributeCollection XMLAttributes = DSiWareTitlesXMLNodes[x].Attributes;

                string titleID = "";
                string descname = "";
                bool dangerous = false;
                bool ticket = true;

                XmlNodeList ChildrenOfTheNode = DSiWareTitlesXMLNodes[x].ChildNodes;

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
                                        ToolStripMenuItem regitem =
                                            (ToolStripMenuItem)XMLToolStripItem.DropDownItems[b];
                                        regitem.DropDownItems.Add("Latest Version");
                                        for (int y = 0; y < versions.Length; y++)
                                        {
                                            regitem.DropDownItems.Add("v" + versions[y]);
                                        }
                                        //regitem.DropDownItemClicked += new ToolStripItemClickedEventHandler(deepitem_clicked);
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
                                    XMLToolStripItem.DropDownItems.Add(RegionFromIndex(Convert.ToInt32(regions[y])));
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
                    {   // Wait what?
                        XMLToolStripItem.Text = descname;
                    }
                }

                DSiWareTitleCollection[x] = XMLToolStripItem;
            }

            return DSiWareTitleCollection;
        }
    }
}
