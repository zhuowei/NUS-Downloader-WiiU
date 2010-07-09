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

        private string[] VcConsoles = new string[11] { "C64", "GEN", "MSX", "N64", "NEO", "NES", 
            "SMS", "SNES", "TG16", "TGCD", "ARC" };

        MemoryStream databaseStream;

        private Image green = Properties.Resources.bullet_green;
        private Image orange = Properties.Resources.bullet_orange;
        private Image redorb = Properties.Resources.bullet_red;
        private Image redgreen = Properties.Resources.bullet_redgreen;
        private Image redorange = Properties.Resources.bullet_redorange;

        public void LoadDatabaseToStream(string databaseFile)
        {
            // Load database.xml into MemoryStream
            string databasestr = File.ReadAllText(databaseFile);
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] databasebytes = encoding.GetBytes(databasestr);

            // Load the memory stream
            databaseStream = new MemoryStream(databasebytes);
            databaseStream.Seek(0, SeekOrigin.Begin);
        }

        public string GetDatabaseVersion()
        {
            if (databaseStream.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(databaseStream);
            XmlNodeList DatabaseList = xDoc.GetElementsByTagName("database");
            XmlAttributeCollection Attributes = DatabaseList[0].Attributes;
            return Attributes[0].Value;
        }
                        
        public ToolStripItemCollection LoadSystemTitles()
        {
            if (databaseStream.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(databaseStream);

            ToolStripItemCollection systemTitleCollection = new ToolStripItemCollection(null, null);

            XmlNodeList SystemTitlesXMLNodes = xDoc.GetElementsByTagName(SystemTag);

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
                //AddToolStripItemToStrip(i, XMLToolStripItem, XMLAttributes);
                systemTitleCollection.Add(XMLToolStripItem);
            }

            return systemTitleCollection;
        }

        public ToolStripItemCollection LoadIosTitles()
        {
            if (databaseStream.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(databaseStream);

            ToolStripItemCollection iosTitleCollection = new ToolStripItemCollection(null, null);

            XmlNodeList IosTitlesXMLNodes = xDoc.GetElementsByTagName(IosTag);

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
                
                iosTitleCollection.Add(XMLToolStripItem);
            }

            return iosTitleCollection;
        }

        public ToolStripItemCollection[] LoadVirtualConsoleTitles()
        {
            if (databaseStream.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(databaseStream);

            ToolStripItemCollection[] vcTitleCollection = new ToolStripItemCollection[VcConsoles.Length];

            XmlNodeList VirtualConsoleXMLNodes = xDoc.GetElementsByTagName(VcTag);

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
                        vcTitleCollection[a].Add(XMLToolStripItem);
			    }
            }

            return vcTitleCollection;
        }

        public ToolStripItemCollection LoadWiiWareTitles()
        {
            if (databaseStream.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(databaseStream);

            ToolStripItemCollection wwTitleCollection = new ToolStripItemCollection(null, null);

            XmlNodeList WiiWareTitlesXMLNodes = xDoc.GetElementsByTagName(WwTag);

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
                
                wwTitleCollection.Add(XMLToolStripItem);
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
            if (databaseStream.Length < 1)
            {
                throw new Exception("Load the database into a memory stream first!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(databaseStream);

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
    }
}
