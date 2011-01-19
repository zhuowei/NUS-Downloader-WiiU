///////////////////////////////////////////
// NUS Downloader: Form1.Designer.cs     //
// $Rev::                              $ //
// $Author::                           $ //
// $Date::                             $ //
///////////////////////////////////////////

namespace NUS_Downloader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Extrasbtn = new System.Windows.Forms.Button();
            this.downloadstartbtn = new System.Windows.Forms.Button();
            this.statusbox = new System.Windows.Forms.RichTextBox();
            this.NUSDownloader = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.wadnamebox = new System.Windows.Forms.TextBox();
            this.databaseStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SystemMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.systemFakeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IOSMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.iosFakeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VCMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.C64MenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.GenesisMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.MSXMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.N64MenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.NeoGeoMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.NESMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.SegaMSMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.SNESMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.TurboGrafx16MenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.TurboGrafxCDMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.VCArcadeMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.vcFakeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WiiWareMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.wwFakeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.dsiSystemToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.dsiFakeSystemToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.dSiWareToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.dSiWareFakeToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RegionCodesList = new System.Windows.Forms.ToolStripMenuItem();
            this.wiiRegionCodesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.dsiRegionCodesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.updateDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extrasStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadInfoFromTMDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.proxySettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.openNUSDDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moreExtrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runFolderFixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wiiBrewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databasePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeNUSDFilesFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localTicketInventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutNUSDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proxyBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ProxyUser = new System.Windows.Forms.TextBox();
            this.SaveProxyBtn = new System.Windows.Forms.Button();
            this.ProxyAssistBtn = new System.Windows.Forms.Button();
            this.ProxyURL = new System.Windows.Forms.TextBox();
            this.ProxyVerifyBox = new System.Windows.Forms.GroupBox();
            this.SaveProxyPwdPermanentBtn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SaveProxyPwdBtn = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.ProxyPwdBox = new System.Windows.Forms.TextBox();
            this.scriptsbutton = new System.Windows.Forms.Button();
            this.scriptsStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.scriptsLocalMenuEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptsDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadNUSScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.emulateUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.uSANTSCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.europePALToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.japanNTSCJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.koreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iosPatchGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.iosPatchesListBox = new System.Windows.Forms.CheckedListBox();
            this.iosPatchGroupBoxOKbtn = new System.Windows.Forms.Button();
            this.richPanel = new System.Windows.Forms.Panel();
            this.databaseButton = new System.Windows.Forms.Button();
            this.saveaswadbtn = new System.Windows.Forms.Button();
            this.iosPatchCheckbox = new System.Windows.Forms.CheckBox();
            this.keepenccontents = new System.Windows.Forms.CheckBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.packbox = new System.Windows.Forms.CheckBox();
            this.decryptbox = new System.Windows.Forms.CheckBox();
            this.localuse = new System.Windows.Forms.CheckBox();
            this.serverLbl = new System.Windows.Forms.Label();
            this.titleidbox = new wmgCMS.WaterMarkTextBox();
            this.dlprogress = new wyDay.Controls.Windows7ProgressBar();
            this.titleversion = new wmgCMS.WaterMarkTextBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.databaseStrip.SuspendLayout();
            this.extrasStrip.SuspendLayout();
            this.proxyBox.SuspendLayout();
            this.ProxyVerifyBox.SuspendLayout();
            this.scriptsStrip.SuspendLayout();
            this.iosPatchGroupBox.SuspendLayout();
            this.richPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Extrasbtn
            // 
            this.Extrasbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Extrasbtn.Location = new System.Drawing.Point(194, 5);
            this.Extrasbtn.Name = "Extrasbtn";
            this.Extrasbtn.Size = new System.Drawing.Size(68, 27);
            this.Extrasbtn.TabIndex = 2;
            this.Extrasbtn.Text = "Extras...";
            this.Extrasbtn.UseVisualStyleBackColor = true;
            this.Extrasbtn.Click += new System.EventHandler(this.extrasMenuButton_Click);
            // 
            // downloadstartbtn
            // 
            this.downloadstartbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.downloadstartbtn.Location = new System.Drawing.Point(12, 64);
            this.downloadstartbtn.Name = "downloadstartbtn";
            this.downloadstartbtn.Size = new System.Drawing.Size(250, 25);
            this.downloadstartbtn.TabIndex = 5;
            this.downloadstartbtn.Text = "Start NUS Download!";
            this.downloadstartbtn.UseVisualStyleBackColor = true;
            this.downloadstartbtn.Click += new System.EventHandler(this.DownloadBtn_Click);
            // 
            // statusbox
            // 
            this.statusbox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusbox.Location = new System.Drawing.Point(-2, -2);
            this.statusbox.Name = "statusbox";
            this.statusbox.ReadOnly = true;
            this.statusbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.statusbox.Size = new System.Drawing.Size(252, 269);
            this.statusbox.TabIndex = 0;
            this.statusbox.Text = "";
            // 
            // NUSDownloader
            // 
            this.NUSDownloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.NUSDownloader_DoWork);
            this.NUSDownloader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.NUSDownloader_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(159, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "v";
            // 
            // wadnamebox
            // 
            this.wadnamebox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wadnamebox.Enabled = false;
            this.wadnamebox.Location = new System.Drawing.Point(102, 390);
            this.wadnamebox.MaxLength = 99999;
            this.wadnamebox.Name = "wadnamebox";
            this.wadnamebox.Size = new System.Drawing.Size(161, 20);
            this.wadnamebox.TabIndex = 17;
            // 
            // databaseStrip
            // 
            this.databaseStrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databaseStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SystemMenuList,
            this.systemFakeMenuItem,
            this.IOSMenuList,
            this.iosFakeMenuItem,
            this.VCMenuList,
            this.vcFakeMenuItem,
            this.WiiWareMenuList,
            this.wwFakeMenuItem,
            this.toolStripSeparator5,
            this.dsiSystemToolStripMenu,
            this.dsiFakeSystemToolStripMenu,
            this.dSiWareToolStripMenu,
            this.dSiWareFakeToolStripMenu,
            this.toolStripSeparator1,
            this.RegionCodesList,
            this.toolStripSeparator4,
            this.updateDatabaseToolStripMenuItem});
            this.databaseStrip.Name = "databaseStrip";
            this.databaseStrip.ShowItemToolTips = false;
            this.databaseStrip.Size = new System.Drawing.Size(164, 330);
            this.databaseStrip.Text = "Hidden";
            this.databaseStrip.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.anyStrip_Closed);
            // 
            // SystemMenuList
            // 
            this.SystemMenuList.AutoSize = false;
            this.SystemMenuList.Image = ((System.Drawing.Image)(resources.GetObject("SystemMenuList.Image")));
            this.SystemMenuList.Name = "SystemMenuList";
            this.SystemMenuList.Size = new System.Drawing.Size(158, 22);
            this.SystemMenuList.Text = "System";
            // 
            // systemFakeMenuItem
            // 
            this.systemFakeMenuItem.Image = global::NUS_Downloader.Properties.Resources.arrow_ticker;
            this.systemFakeMenuItem.Name = "systemFakeMenuItem";
            this.systemFakeMenuItem.Size = new System.Drawing.Size(163, 22);
            this.systemFakeMenuItem.Text = "System";
            this.systemFakeMenuItem.Visible = false;
            // 
            // IOSMenuList
            // 
            this.IOSMenuList.Image = ((System.Drawing.Image)(resources.GetObject("IOSMenuList.Image")));
            this.IOSMenuList.Name = "IOSMenuList";
            this.IOSMenuList.Size = new System.Drawing.Size(163, 22);
            this.IOSMenuList.Text = "IOS";
            // 
            // iosFakeMenuItem
            // 
            this.iosFakeMenuItem.Image = global::NUS_Downloader.Properties.Resources.arrow_ticker;
            this.iosFakeMenuItem.Name = "iosFakeMenuItem";
            this.iosFakeMenuItem.Size = new System.Drawing.Size(163, 22);
            this.iosFakeMenuItem.Text = "IOS";
            this.iosFakeMenuItem.Visible = false;
            // 
            // VCMenuList
            // 
            this.VCMenuList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.C64MenuList,
            this.GenesisMenuList,
            this.MSXMenuList,
            this.N64MenuList,
            this.NeoGeoMenuList,
            this.NESMenuList,
            this.SegaMSMenuList,
            this.SNESMenuList,
            this.TurboGrafx16MenuList,
            this.TurboGrafxCDMenuList,
            this.VCArcadeMenuList});
            this.VCMenuList.Image = ((System.Drawing.Image)(resources.GetObject("VCMenuList.Image")));
            this.VCMenuList.Name = "VCMenuList";
            this.VCMenuList.Size = new System.Drawing.Size(163, 22);
            this.VCMenuList.Text = "Virtual Console";
            // 
            // C64MenuList
            // 
            this.C64MenuList.Name = "C64MenuList";
            this.C64MenuList.Size = new System.Drawing.Size(182, 22);
            this.C64MenuList.Text = "Commodore 64";
            // 
            // GenesisMenuList
            // 
            this.GenesisMenuList.Name = "GenesisMenuList";
            this.GenesisMenuList.Size = new System.Drawing.Size(182, 22);
            this.GenesisMenuList.Text = "Mega Drive/Genesis";
            // 
            // MSXMenuList
            // 
            this.MSXMenuList.Name = "MSXMenuList";
            this.MSXMenuList.Size = new System.Drawing.Size(182, 22);
            this.MSXMenuList.Text = "MSX";
            // 
            // N64MenuList
            // 
            this.N64MenuList.Name = "N64MenuList";
            this.N64MenuList.Size = new System.Drawing.Size(182, 22);
            this.N64MenuList.Text = "Nintendo 64";
            // 
            // NeoGeoMenuList
            // 
            this.NeoGeoMenuList.Name = "NeoGeoMenuList";
            this.NeoGeoMenuList.Size = new System.Drawing.Size(182, 22);
            this.NeoGeoMenuList.Text = "NeoGeo";
            // 
            // NESMenuList
            // 
            this.NESMenuList.Name = "NESMenuList";
            this.NESMenuList.Size = new System.Drawing.Size(182, 22);
            this.NESMenuList.Text = "NES";
            // 
            // SegaMSMenuList
            // 
            this.SegaMSMenuList.Name = "SegaMSMenuList";
            this.SegaMSMenuList.Size = new System.Drawing.Size(182, 22);
            this.SegaMSMenuList.Text = "Sega Master System";
            // 
            // SNESMenuList
            // 
            this.SNESMenuList.Name = "SNESMenuList";
            this.SNESMenuList.Size = new System.Drawing.Size(182, 22);
            this.SNESMenuList.Text = "SNES";
            // 
            // TurboGrafx16MenuList
            // 
            this.TurboGrafx16MenuList.Name = "TurboGrafx16MenuList";
            this.TurboGrafx16MenuList.Size = new System.Drawing.Size(182, 22);
            this.TurboGrafx16MenuList.Text = "TruboGrafx-16";
            // 
            // TurboGrafxCDMenuList
            // 
            this.TurboGrafxCDMenuList.Name = "TurboGrafxCDMenuList";
            this.TurboGrafxCDMenuList.Size = new System.Drawing.Size(182, 22);
            this.TurboGrafxCDMenuList.Text = "TurboGrafx-CD";
            // 
            // VCArcadeMenuList
            // 
            this.VCArcadeMenuList.Name = "VCArcadeMenuList";
            this.VCArcadeMenuList.Size = new System.Drawing.Size(182, 22);
            this.VCArcadeMenuList.Text = "Virtual Console Arcade";
            // 
            // vcFakeMenuItem
            // 
            this.vcFakeMenuItem.Image = global::NUS_Downloader.Properties.Resources.arrow_ticker;
            this.vcFakeMenuItem.Name = "vcFakeMenuItem";
            this.vcFakeMenuItem.Size = new System.Drawing.Size(163, 22);
            this.vcFakeMenuItem.Text = "Virtual Console";
            this.vcFakeMenuItem.Visible = false;
            // 
            // WiiWareMenuList
            // 
            this.WiiWareMenuList.Image = ((System.Drawing.Image)(resources.GetObject("WiiWareMenuList.Image")));
            this.WiiWareMenuList.Name = "WiiWareMenuList";
            this.WiiWareMenuList.Size = new System.Drawing.Size(163, 22);
            this.WiiWareMenuList.Text = "WiiWare";
            // 
            // wwFakeMenuItem
            // 
            this.wwFakeMenuItem.Image = global::NUS_Downloader.Properties.Resources.arrow_ticker;
            this.wwFakeMenuItem.Name = "wwFakeMenuItem";
            this.wwFakeMenuItem.Size = new System.Drawing.Size(163, 22);
            this.wwFakeMenuItem.Text = "WiiWare";
            this.wwFakeMenuItem.Visible = false;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(160, 6);
            // 
            // dsiSystemToolStripMenu
            // 
            this.dsiSystemToolStripMenu.Image = global::NUS_Downloader.Properties.Resources.dsi16x16;
            this.dsiSystemToolStripMenu.Name = "dsiSystemToolStripMenu";
            this.dsiSystemToolStripMenu.Size = new System.Drawing.Size(163, 22);
            this.dsiSystemToolStripMenu.Text = "System";
            // 
            // dsiFakeSystemToolStripMenu
            // 
            this.dsiFakeSystemToolStripMenu.Image = global::NUS_Downloader.Properties.Resources.arrow_ticker;
            this.dsiFakeSystemToolStripMenu.Name = "dsiFakeSystemToolStripMenu";
            this.dsiFakeSystemToolStripMenu.Size = new System.Drawing.Size(163, 22);
            this.dsiFakeSystemToolStripMenu.Text = "System";
            this.dsiFakeSystemToolStripMenu.Visible = false;
            // 
            // dSiWareToolStripMenu
            // 
            this.dSiWareToolStripMenu.Image = global::NUS_Downloader.Properties.Resources.dsi16x16;
            this.dSiWareToolStripMenu.Name = "dSiWareToolStripMenu";
            this.dSiWareToolStripMenu.Size = new System.Drawing.Size(163, 22);
            this.dSiWareToolStripMenu.Text = "DSiWare";
            // 
            // dSiWareFakeToolStripMenu
            // 
            this.dSiWareFakeToolStripMenu.Image = global::NUS_Downloader.Properties.Resources.arrow_ticker;
            this.dSiWareFakeToolStripMenu.Name = "dSiWareFakeToolStripMenu";
            this.dSiWareFakeToolStripMenu.Size = new System.Drawing.Size(163, 22);
            this.dSiWareFakeToolStripMenu.Text = "DSiWare";
            this.dSiWareFakeToolStripMenu.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // RegionCodesList
            // 
            this.RegionCodesList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wiiRegionCodesMenu,
            this.dsiRegionCodesMenu});
            this.RegionCodesList.Name = "RegionCodesList";
            this.RegionCodesList.Size = new System.Drawing.Size(163, 22);
            this.RegionCodesList.Text = "Region Codes";
            // 
            // wiiRegionCodesMenu
            // 
            this.wiiRegionCodesMenu.Name = "wiiRegionCodesMenu";
            this.wiiRegionCodesMenu.Size = new System.Drawing.Size(89, 22);
            this.wiiRegionCodesMenu.Text = "Wii";
            this.wiiRegionCodesMenu.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.wiiRegionCodesMenu_DropDownItemClicked);
            // 
            // dsiRegionCodesMenu
            // 
            this.dsiRegionCodesMenu.Name = "dsiRegionCodesMenu";
            this.dsiRegionCodesMenu.Size = new System.Drawing.Size(89, 22);
            this.dsiRegionCodesMenu.Text = "DSi";
            this.dsiRegionCodesMenu.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.dsiRegionCodesMenu_DropDownItemClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(160, 6);
            // 
            // updateDatabaseToolStripMenuItem
            // 
            this.updateDatabaseToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.database_save;
            this.updateDatabaseToolStripMenuItem.Name = "updateDatabaseToolStripMenuItem";
            this.updateDatabaseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.updateDatabaseToolStripMenuItem.Text = "Update Databases";
            this.updateDatabaseToolStripMenuItem.Click += new System.EventHandler(this.updateDatabaseToolStripMenuItem_Click);
            // 
            // extrasStrip
            // 
            this.extrasStrip.AllowMerge = false;
            this.extrasStrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extrasStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadInfoFromTMDToolStripMenuItem,
            this.toolStripSeparator3,
            this.proxySettingsToolStripMenuItem,
            this.toolStripSeparator6,
            this.openNUSDDirectoryToolStripMenuItem,
            this.moreExtrasToolStripMenuItem,
            this.donateToolStripMenuItem,
            this.toolStripSeparator7,
            this.aboutNUSDToolStripMenuItem});
            this.extrasStrip.Name = "extrasStrip";
            this.extrasStrip.Size = new System.Drawing.Size(178, 154);
            this.extrasStrip.Text = "Hidden";
            this.extrasStrip.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.anyStrip_Closed);
            this.extrasStrip.Opening += new System.ComponentModel.CancelEventHandler(this.extrasStrip_Opening);
            // 
            // loadInfoFromTMDToolStripMenuItem
            // 
            this.loadInfoFromTMDToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.page_white_magnify;
            this.loadInfoFromTMDToolStripMenuItem.Name = "loadInfoFromTMDToolStripMenuItem";
            this.loadInfoFromTMDToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.loadInfoFromTMDToolStripMenuItem.Text = "Load Info from TMD";
            this.loadInfoFromTMDToolStripMenuItem.Click += new System.EventHandler(this.loadInfoFromTMDToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(174, 6);
            // 
            // proxySettingsToolStripMenuItem
            // 
            this.proxySettingsToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.server_link;
            this.proxySettingsToolStripMenuItem.Name = "proxySettingsToolStripMenuItem";
            this.proxySettingsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.proxySettingsToolStripMenuItem.Text = "Proxy Settings";
            this.proxySettingsToolStripMenuItem.Click += new System.EventHandler(this.proxySettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(174, 6);
            // 
            // openNUSDDirectoryToolStripMenuItem
            // 
            this.openNUSDDirectoryToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.folder;
            this.openNUSDDirectoryToolStripMenuItem.Name = "openNUSDDirectoryToolStripMenuItem";
            this.openNUSDDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openNUSDDirectoryToolStripMenuItem.Text = "Open NUSD Directory";
            this.openNUSDDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openNUSDDirectoryToolStripMenuItem_Click);
            // 
            // moreExtrasToolStripMenuItem
            // 
            this.moreExtrasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runFolderFixToolStripMenuItem,
            this.wiiBrewToolStripMenuItem,
            this.removeNUSDFilesFoldersToolStripMenuItem,
            this.databaseToolStripMenuItem});
            this.moreExtrasToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.wrench;
            this.moreExtrasToolStripMenuItem.Name = "moreExtrasToolStripMenuItem";
            this.moreExtrasToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.moreExtrasToolStripMenuItem.Text = "More Extras...";
            this.moreExtrasToolStripMenuItem.Visible = false;
            // 
            // runFolderFixToolStripMenuItem
            // 
            this.runFolderFixToolStripMenuItem.Name = "runFolderFixToolStripMenuItem";
            this.runFolderFixToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.runFolderFixToolStripMenuItem.Text = "Run \'FolderFix\'";
            this.runFolderFixToolStripMenuItem.Click += new System.EventHandler(this.runFolderFixToolStripMenuItem_Click);
            // 
            // wiiBrewToolStripMenuItem
            // 
            this.wiiBrewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainPageToolStripMenuItem,
            this.databasePageToolStripMenuItem});
            this.wiiBrewToolStripMenuItem.Name = "wiiBrewToolStripMenuItem";
            this.wiiBrewToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.wiiBrewToolStripMenuItem.Text = "WiiBrew";
            // 
            // mainPageToolStripMenuItem
            // 
            this.mainPageToolStripMenuItem.Name = "mainPageToolStripMenuItem";
            this.mainPageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.mainPageToolStripMenuItem.Text = "Main Page";
            this.mainPageToolStripMenuItem.Click += new System.EventHandler(this.mainPageToolStripMenuItem_Click);
            // 
            // databasePageToolStripMenuItem
            // 
            this.databasePageToolStripMenuItem.Name = "databasePageToolStripMenuItem";
            this.databasePageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.databasePageToolStripMenuItem.Text = "Database Page";
            this.databasePageToolStripMenuItem.Click += new System.EventHandler(this.databasePageToolStripMenuItem_Click);
            // 
            // removeNUSDFilesFoldersToolStripMenuItem
            // 
            this.removeNUSDFilesFoldersToolStripMenuItem.Name = "removeNUSDFilesFoldersToolStripMenuItem";
            this.removeNUSDFilesFoldersToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.removeNUSDFilesFoldersToolStripMenuItem.Text = "Remove NUSD Files\\Folders";
            this.removeNUSDFilesFoldersToolStripMenuItem.Click += new System.EventHandler(this.removeNUSDFilesFoldersToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localTicketInventoryToolStripMenuItem});
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.databaseToolStripMenuItem.Text = "Database";
            // 
            // localTicketInventoryToolStripMenuItem
            // 
            this.localTicketInventoryToolStripMenuItem.Name = "localTicketInventoryToolStripMenuItem";
            this.localTicketInventoryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.localTicketInventoryToolStripMenuItem.Text = "Local Ticket Inventory";
            this.localTicketInventoryToolStripMenuItem.Click += new System.EventHandler(this.localTicketInventoryToolStripMenuItem_Click);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.money;
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.donateToolStripMenuItem.Text = "Donate!";
            this.donateToolStripMenuItem.Visible = false;
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
            // 
            // aboutNUSDToolStripMenuItem
            // 
            this.aboutNUSDToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.information;
            this.aboutNUSDToolStripMenuItem.Name = "aboutNUSDToolStripMenuItem";
            this.aboutNUSDToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.aboutNUSDToolStripMenuItem.Text = "About NUSD";
            this.aboutNUSDToolStripMenuItem.Click += new System.EventHandler(this.aboutNUSDToolStripMenuItem_Click);
            // 
            // proxyBox
            // 
            this.proxyBox.BackColor = System.Drawing.Color.White;
            this.proxyBox.Controls.Add(this.label13);
            this.proxyBox.Controls.Add(this.label12);
            this.proxyBox.Controls.Add(this.ProxyUser);
            this.proxyBox.Controls.Add(this.SaveProxyBtn);
            this.proxyBox.Controls.Add(this.ProxyAssistBtn);
            this.proxyBox.Controls.Add(this.ProxyURL);
            this.proxyBox.Location = new System.Drawing.Point(31, 250);
            this.proxyBox.Name = "proxyBox";
            this.proxyBox.Size = new System.Drawing.Size(212, 114);
            this.proxyBox.TabIndex = 45;
            this.proxyBox.TabStop = false;
            this.proxyBox.Text = "Proxy Settings";
            this.proxyBox.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 55);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "User:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 29);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Proxy:";
            // 
            // ProxyUser
            // 
            this.ProxyUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProxyUser.Location = new System.Drawing.Point(55, 53);
            this.ProxyUser.Name = "ProxyUser";
            this.ProxyUser.Size = new System.Drawing.Size(151, 20);
            this.ProxyUser.TabIndex = 30;
            // 
            // SaveProxyBtn
            // 
            this.SaveProxyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveProxyBtn.Location = new System.Drawing.Point(6, 79);
            this.SaveProxyBtn.Name = "SaveProxyBtn";
            this.SaveProxyBtn.Size = new System.Drawing.Size(161, 26);
            this.SaveProxyBtn.TabIndex = 29;
            this.SaveProxyBtn.Text = "Save Proxy Settings";
            this.SaveProxyBtn.UseVisualStyleBackColor = true;
            this.SaveProxyBtn.Click += new System.EventHandler(this.SaveProxyBtn_Click);
            // 
            // ProxyAssistBtn
            // 
            this.ProxyAssistBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ProxyAssistBtn.Image = global::NUS_Downloader.Properties.Resources.help;
            this.ProxyAssistBtn.Location = new System.Drawing.Point(177, 79);
            this.ProxyAssistBtn.Name = "ProxyAssistBtn";
            this.ProxyAssistBtn.Size = new System.Drawing.Size(29, 26);
            this.ProxyAssistBtn.TabIndex = 28;
            this.ProxyAssistBtn.UseVisualStyleBackColor = true;
            this.ProxyAssistBtn.Click += new System.EventHandler(this.ProxyAssistBtn_Click);
            // 
            // ProxyURL
            // 
            this.ProxyURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProxyURL.Location = new System.Drawing.Point(55, 27);
            this.ProxyURL.Name = "ProxyURL";
            this.ProxyURL.Size = new System.Drawing.Size(151, 20);
            this.ProxyURL.TabIndex = 0;
            // 
            // ProxyVerifyBox
            // 
            this.ProxyVerifyBox.BackColor = System.Drawing.SystemColors.Control;
            this.ProxyVerifyBox.Controls.Add(this.SaveProxyPwdPermanentBtn);
            this.ProxyVerifyBox.Controls.Add(this.checkBox1);
            this.ProxyVerifyBox.Controls.Add(this.SaveProxyPwdBtn);
            this.ProxyVerifyBox.Controls.Add(this.label14);
            this.ProxyVerifyBox.Controls.Add(this.ProxyPwdBox);
            this.ProxyVerifyBox.Location = new System.Drawing.Point(31, 222);
            this.ProxyVerifyBox.Name = "ProxyVerifyBox";
            this.ProxyVerifyBox.Size = new System.Drawing.Size(212, 133);
            this.ProxyVerifyBox.TabIndex = 46;
            this.ProxyVerifyBox.TabStop = false;
            this.ProxyVerifyBox.Text = "Verify Credentials";
            this.ProxyVerifyBox.Visible = false;
            // 
            // SaveProxyPwdPermanentBtn
            // 
            this.SaveProxyPwdPermanentBtn.Enabled = false;
            this.SaveProxyPwdPermanentBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveProxyPwdPermanentBtn.Location = new System.Drawing.Point(9, 104);
            this.SaveProxyPwdPermanentBtn.Name = "SaveProxyPwdPermanentBtn";
            this.SaveProxyPwdPermanentBtn.Size = new System.Drawing.Size(197, 23);
            this.SaveProxyPwdPermanentBtn.TabIndex = 36;
            this.SaveProxyPwdPermanentBtn.Text = "Save (To File)";
            this.SaveProxyPwdPermanentBtn.UseVisualStyleBackColor = true;
            this.SaveProxyPwdPermanentBtn.Click += new System.EventHandler(this.SaveProxyPwdPermanentBtn_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 72);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(199, 30);
            this.checkBox1.TabIndex = 35;
            this.checkBox1.Text = "I understand that NUSD stores proxy\r\npasswords in plain text.";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // SaveProxyPwdBtn
            // 
            this.SaveProxyPwdBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveProxyPwdBtn.Location = new System.Drawing.Point(9, 43);
            this.SaveProxyPwdBtn.Name = "SaveProxyPwdBtn";
            this.SaveProxyPwdBtn.Size = new System.Drawing.Size(197, 23);
            this.SaveProxyPwdBtn.TabIndex = 34;
            this.SaveProxyPwdBtn.Text = "Save (This Session Only)";
            this.SaveProxyPwdBtn.UseVisualStyleBackColor = true;
            this.SaveProxyPwdBtn.Click += new System.EventHandler(this.SaveProxyPwdButton_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 21);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "Proxy Pass:";
            // 
            // ProxyPwdBox
            // 
            this.ProxyPwdBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProxyPwdBox.Location = new System.Drawing.Point(71, 19);
            this.ProxyPwdBox.Name = "ProxyPwdBox";
            this.ProxyPwdBox.Size = new System.Drawing.Size(135, 20);
            this.ProxyPwdBox.TabIndex = 32;
            this.ProxyPwdBox.UseSystemPasswordChar = true;
            this.ProxyPwdBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ProxyPwdBox_KeyPress);
            // 
            // scriptsbutton
            // 
            this.scriptsbutton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.scriptsbutton.Location = new System.Drawing.Point(103, 5);
            this.scriptsbutton.Name = "scriptsbutton";
            this.scriptsbutton.Size = new System.Drawing.Size(85, 27);
            this.scriptsbutton.TabIndex = 1;
            this.scriptsbutton.Text = "Scripts...";
            this.scriptsbutton.UseVisualStyleBackColor = true;
            this.scriptsbutton.Click += new System.EventHandler(this.scriptsbutton_Click);
            // 
            // scriptsStrip
            // 
            this.scriptsStrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scriptsStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptsLocalMenuEntry,
            this.scriptsDatabaseToolStripMenuItem,
            this.loadNUSScriptToolStripMenuItem,
            this.toolStripSeparator2,
            this.emulateUpdate});
            this.scriptsStrip.Name = "scriptsStrip";
            this.scriptsStrip.ShowItemToolTips = false;
            this.scriptsStrip.Size = new System.Drawing.Size(206, 98);
            this.scriptsStrip.Text = "Hidden";
            this.scriptsStrip.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.anyStrip_Closed);
            // 
            // scriptsLocalMenuEntry
            // 
            this.scriptsLocalMenuEntry.Enabled = false;
            this.scriptsLocalMenuEntry.Image = global::NUS_Downloader.Properties.Resources.script_code;
            this.scriptsLocalMenuEntry.Name = "scriptsLocalMenuEntry";
            this.scriptsLocalMenuEntry.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.scriptsLocalMenuEntry.Size = new System.Drawing.Size(205, 22);
            this.scriptsLocalMenuEntry.Text = "Scripts (Local)";
            // 
            // scriptsDatabaseToolStripMenuItem
            // 
            this.scriptsDatabaseToolStripMenuItem.Enabled = false;
            this.scriptsDatabaseToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.script_code_red;
            this.scriptsDatabaseToolStripMenuItem.Name = "scriptsDatabaseToolStripMenuItem";
            this.scriptsDatabaseToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.scriptsDatabaseToolStripMenuItem.Text = "Scripts (Database)";
            // 
            // loadNUSScriptToolStripMenuItem
            // 
            this.loadNUSScriptToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.script_go;
            this.loadNUSScriptToolStripMenuItem.Name = "loadNUSScriptToolStripMenuItem";
            this.loadNUSScriptToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.loadNUSScriptToolStripMenuItem.Text = "Load NUS Script";
            this.loadNUSScriptToolStripMenuItem.Click += new System.EventHandler(this.loadNUSScriptToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(202, 6);
            // 
            // emulateUpdate
            // 
            this.emulateUpdate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uSANTSCToolStripMenuItem,
            this.europePALToolStripMenuItem,
            this.japanNTSCJToolStripMenuItem,
            this.koreaToolStripMenuItem});
            this.emulateUpdate.Image = global::NUS_Downloader.Properties.Resources.server_connect;
            this.emulateUpdate.Name = "emulateUpdate";
            this.emulateUpdate.Size = new System.Drawing.Size(205, 22);
            this.emulateUpdate.Text = "Emulate Wii System Update";
            this.emulateUpdate.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.emulateUpdate_DropDownItemClicked);
            // 
            // uSANTSCToolStripMenuItem
            // 
            this.uSANTSCToolStripMenuItem.Name = "uSANTSCToolStripMenuItem";
            this.uSANTSCToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.uSANTSCToolStripMenuItem.Text = "USA";
            // 
            // europePALToolStripMenuItem
            // 
            this.europePALToolStripMenuItem.Name = "europePALToolStripMenuItem";
            this.europePALToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.europePALToolStripMenuItem.Text = "EUROPE";
            // 
            // japanNTSCJToolStripMenuItem
            // 
            this.japanNTSCJToolStripMenuItem.Name = "japanNTSCJToolStripMenuItem";
            this.japanNTSCJToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.japanNTSCJToolStripMenuItem.Text = "JAPAN";
            // 
            // koreaToolStripMenuItem
            // 
            this.koreaToolStripMenuItem.Name = "koreaToolStripMenuItem";
            this.koreaToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.koreaToolStripMenuItem.Text = "KOREA";
            // 
            // iosPatchGroupBox
            // 
            this.iosPatchGroupBox.Controls.Add(this.label2);
            this.iosPatchGroupBox.Controls.Add(this.iosPatchesListBox);
            this.iosPatchGroupBox.Controls.Add(this.iosPatchGroupBoxOKbtn);
            this.iosPatchGroupBox.Location = new System.Drawing.Point(31, 187);
            this.iosPatchGroupBox.Name = "iosPatchGroupBox";
            this.iosPatchGroupBox.Size = new System.Drawing.Size(212, 115);
            this.iosPatchGroupBox.TabIndex = 55;
            this.iosPatchGroupBox.TabStop = false;
            this.iosPatchGroupBox.Text = "IOS Patches";
            this.iosPatchGroupBox.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Patch the following bugs into any IOS\r\nI download:";
            // 
            // iosPatchesListBox
            // 
            this.iosPatchesListBox.BackColor = System.Drawing.SystemColors.Menu;
            this.iosPatchesListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.iosPatchesListBox.CheckOnClick = true;
            this.iosPatchesListBox.FormattingEnabled = true;
            this.iosPatchesListBox.Items.AddRange(new object[] {
            "Trucha bug",
            "ES_Identify",
            "NAND permissions"});
            this.iosPatchesListBox.Location = new System.Drawing.Point(6, 62);
            this.iosPatchesListBox.Name = "iosPatchesListBox";
            this.iosPatchesListBox.Size = new System.Drawing.Size(115, 45);
            this.iosPatchesListBox.TabIndex = 1;
            // 
            // iosPatchGroupBoxOKbtn
            // 
            this.iosPatchGroupBoxOKbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.iosPatchGroupBoxOKbtn.Location = new System.Drawing.Point(127, 84);
            this.iosPatchGroupBoxOKbtn.Name = "iosPatchGroupBoxOKbtn";
            this.iosPatchGroupBoxOKbtn.Size = new System.Drawing.Size(75, 23);
            this.iosPatchGroupBoxOKbtn.TabIndex = 0;
            this.iosPatchGroupBoxOKbtn.Text = "OK";
            this.iosPatchGroupBoxOKbtn.UseVisualStyleBackColor = true;
            this.iosPatchGroupBoxOKbtn.Click += new System.EventHandler(this.iosPatchGroupBoxOKbtn_Click);
            // 
            // richPanel
            // 
            this.richPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richPanel.Controls.Add(this.statusbox);
            this.richPanel.Location = new System.Drawing.Point(12, 116);
            this.richPanel.Name = "richPanel";
            this.richPanel.Size = new System.Drawing.Size(250, 268);
            this.richPanel.TabIndex = 56;
            // 
            // databaseButton
            // 
            this.databaseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.databaseButton.Location = new System.Drawing.Point(12, 5);
            this.databaseButton.Name = "databaseButton";
            this.databaseButton.Size = new System.Drawing.Size(85, 27);
            this.databaseButton.TabIndex = 0;
            this.databaseButton.Text = "Database...";
            this.databaseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.databaseButton.UseVisualStyleBackColor = true;
            this.databaseButton.Click += new System.EventHandler(this.DatabaseButton_Click);
            // 
            // saveaswadbtn
            // 
            this.saveaswadbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveaswadbtn.AutoSize = true;
            this.saveaswadbtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.saveaswadbtn.BackColor = System.Drawing.Color.Transparent;
            this.saveaswadbtn.Enabled = false;
            this.saveaswadbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveaswadbtn.Image = global::NUS_Downloader.Properties.Resources.disk;
            this.saveaswadbtn.Location = new System.Drawing.Point(239, 409);
            this.saveaswadbtn.MaximumSize = new System.Drawing.Size(0, 24);
            this.saveaswadbtn.MinimumSize = new System.Drawing.Size(0, 24);
            this.saveaswadbtn.Name = "saveaswadbtn";
            this.saveaswadbtn.Size = new System.Drawing.Size(24, 24);
            this.saveaswadbtn.TabIndex = 11;
            this.saveaswadbtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveaswadbtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.saveaswadbtn.UseVisualStyleBackColor = false;
            this.saveaswadbtn.Click += new System.EventHandler(this.saveaswadbtn_Click);
            this.saveaswadbtn.MouseEnter += new System.EventHandler(this.saveaswadbtn_MouseEnter);
            this.saveaswadbtn.MouseLeave += new System.EventHandler(this.saveaswadbtn_MouseLeave);
            // 
            // iosPatchCheckbox
            // 
            this.iosPatchCheckbox.Enabled = false;
            this.iosPatchCheckbox.Image = global::NUS_Downloader.Properties.Resources.bug_add;
            this.iosPatchCheckbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iosPatchCheckbox.Location = new System.Drawing.Point(11, 487);
            this.iosPatchCheckbox.Name = "iosPatchCheckbox";
            this.iosPatchCheckbox.Size = new System.Drawing.Size(104, 24);
            this.iosPatchCheckbox.TabIndex = 10;
            this.iosPatchCheckbox.Text = "Patch IOS...";
            this.iosPatchCheckbox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iosPatchCheckbox.UseVisualStyleBackColor = true;
            this.iosPatchCheckbox.CheckedChanged += new System.EventHandler(this.iosPatchCheckbox_CheckedChanged);
            // 
            // keepenccontents
            // 
            this.keepenccontents.Checked = true;
            this.keepenccontents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.keepenccontents.Image = global::NUS_Downloader.Properties.Resources.package;
            this.keepenccontents.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.keepenccontents.Location = new System.Drawing.Point(11, 410);
            this.keepenccontents.Name = "keepenccontents";
            this.keepenccontents.Size = new System.Drawing.Size(177, 26);
            this.keepenccontents.TabIndex = 8;
            this.keepenccontents.Text = "Keep Encrypted Contents";
            this.keepenccontents.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.keepenccontents.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.AutoSize = true;
            this.clearButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clearButton.BackColor = System.Drawing.Color.Transparent;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Image = global::NUS_Downloader.Properties.Resources.bin_closed;
            this.clearButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.clearButton.Location = new System.Drawing.Point(238, 360);
            this.clearButton.MaximumSize = new System.Drawing.Size(0, 24);
            this.clearButton.MinimumSize = new System.Drawing.Size(0, 24);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(24, 24);
            this.clearButton.TabIndex = 12;
            this.clearButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.clearButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.ClearStatusbox);
            this.clearButton.MouseEnter += new System.EventHandler(this.clearButton_MouseEnter);
            this.clearButton.MouseLeave += new System.EventHandler(this.clearButton_MouseLeave);
            // 
            // packbox
            // 
            this.packbox.Image = global::NUS_Downloader.Properties.Resources.box;
            this.packbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.packbox.Location = new System.Drawing.Point(11, 388);
            this.packbox.Name = "packbox";
            this.packbox.Size = new System.Drawing.Size(98, 22);
            this.packbox.TabIndex = 7;
            this.packbox.Text = "Pack WAD";
            this.packbox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.packbox.UseVisualStyleBackColor = true;
            this.packbox.CheckedChanged += new System.EventHandler(this.packbox_CheckedChanged);
            this.packbox.EnabledChanged += new System.EventHandler(this.packbox_EnabledChanged);
            // 
            // decryptbox
            // 
            this.decryptbox.Image = global::NUS_Downloader.Properties.Resources.package_green;
            this.decryptbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.decryptbox.Location = new System.Drawing.Point(11, 436);
            this.decryptbox.Name = "decryptbox";
            this.decryptbox.Size = new System.Drawing.Size(231, 26);
            this.decryptbox.TabIndex = 9;
            this.decryptbox.Text = "Create Decrypted Contents (*.app)";
            this.decryptbox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.decryptbox.UseVisualStyleBackColor = true;
            // 
            // localuse
            // 
            this.localuse.AutoSize = true;
            this.localuse.Checked = true;
            this.localuse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localuse.Image = global::NUS_Downloader.Properties.Resources.drive_disk;
            this.localuse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.localuse.Location = new System.Drawing.Point(11, 463);
            this.localuse.MinimumSize = new System.Drawing.Size(0, 22);
            this.localuse.Name = "localuse";
            this.localuse.Size = new System.Drawing.Size(162, 22);
            this.localuse.TabIndex = 12;
            this.localuse.Text = "Use Local Files If Present";
            this.localuse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.localuse.UseVisualStyleBackColor = true;
            // 
            // serverLbl
            // 
            this.serverLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverLbl.Location = new System.Drawing.Point(233, 38);
            this.serverLbl.Name = "serverLbl";
            this.serverLbl.Size = new System.Drawing.Size(29, 20);
            this.serverLbl.TabIndex = 57;
            this.serverLbl.Text = "Wii";
            this.serverLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.serverLbl.TextChanged += new System.EventHandler(this.serverLbl_TextChanged);
            this.serverLbl.Click += new System.EventHandler(this.serverLbl_Click);
            this.serverLbl.MouseEnter += new System.EventHandler(this.serverLbl_MouseEnter);
            this.serverLbl.MouseLeave += new System.EventHandler(this.serverLbl_MouseLeave);
            // 
            // titleidbox
            // 
            this.titleidbox.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.titleidbox.Location = new System.Drawing.Point(12, 38);
            this.titleidbox.MaxLength = 16;
            this.titleidbox.Name = "titleidbox";
            this.titleidbox.Size = new System.Drawing.Size(141, 21);
            this.titleidbox.TabIndex = 3;
            this.titleidbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.titleidbox.WaterMarkColor = System.Drawing.Color.Silver;
            this.titleidbox.WaterMarkText = "Title ID";
            this.titleidbox.TextChanged += new System.EventHandler(this.titleidbox_TextChanged);
            // 
            // dlprogress
            // 
            this.dlprogress.ContainerControl = this;
            this.dlprogress.Location = new System.Drawing.Point(12, 95);
            this.dlprogress.Name = "dlprogress";
            this.dlprogress.Size = new System.Drawing.Size(250, 15);
            this.dlprogress.TabIndex = 47;
            // 
            // titleversion
            // 
            this.titleversion.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.titleversion.Location = new System.Drawing.Point(169, 38);
            this.titleversion.MaxLength = 8;
            this.titleversion.Name = "titleversion";
            this.titleversion.Size = new System.Drawing.Size(58, 21);
            this.titleversion.TabIndex = 4;
            this.titleversion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.titleversion.WaterMarkColor = System.Drawing.Color.Silver;
            this.titleversion.WaterMarkText = "Version";
            this.titleversion.TextChanged += new System.EventHandler(this.titleversion_TextChanged);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(174, 6);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(274, 516);
            this.Controls.Add(this.serverLbl);
            this.Controls.Add(this.iosPatchGroupBox);
            this.Controls.Add(this.ProxyVerifyBox);
            this.Controls.Add(this.proxyBox);
            this.Controls.Add(this.scriptsbutton);
            this.Controls.Add(this.titleidbox);
            this.Controls.Add(this.dlprogress);
            this.Controls.Add(this.titleversion);
            this.Controls.Add(this.databaseButton);
            this.Controls.Add(this.iosPatchCheckbox);
            this.Controls.Add(this.downloadstartbtn);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.keepenccontents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Extrasbtn);
            this.Controls.Add(this.wadnamebox);
            this.Controls.Add(this.richPanel);
            this.Controls.Add(this.decryptbox);
            this.Controls.Add(this.packbox);
            this.Controls.Add(this.saveaswadbtn);
            this.Controls.Add(this.localuse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseWheel);
            this.databaseStrip.ResumeLayout(false);
            this.extrasStrip.ResumeLayout(false);
            this.proxyBox.ResumeLayout(false);
            this.proxyBox.PerformLayout();
            this.ProxyVerifyBox.ResumeLayout(false);
            this.ProxyVerifyBox.PerformLayout();
            this.scriptsStrip.ResumeLayout(false);
            this.iosPatchGroupBox.ResumeLayout(false);
            this.iosPatchGroupBox.PerformLayout();
            this.richPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Extrasbtn;
        private System.Windows.Forms.Button downloadstartbtn;
        private System.Windows.Forms.RichTextBox statusbox;
        private System.Windows.Forms.CheckBox packbox;
        private System.Windows.Forms.CheckBox localuse;
        private System.ComponentModel.BackgroundWorker NUSDownloader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox wadnamebox;
        private System.Windows.Forms.CheckBox decryptbox;
        private System.Windows.Forms.Button databaseButton;
        private System.Windows.Forms.ContextMenuStrip databaseStrip;
        private System.Windows.Forms.ToolStripMenuItem SystemMenuList;
        private System.Windows.Forms.ToolStripMenuItem IOSMenuList;
        private System.Windows.Forms.ToolStripMenuItem VCMenuList;
        private System.Windows.Forms.ToolStripMenuItem WiiWareMenuList;
        private System.Windows.Forms.ToolStripMenuItem C64MenuList;
        private System.Windows.Forms.ToolStripMenuItem NeoGeoMenuList;
        private System.Windows.Forms.ToolStripMenuItem NESMenuList;
        private System.Windows.Forms.ToolStripMenuItem SNESMenuList;
        private System.Windows.Forms.ToolStripMenuItem N64MenuList;
        private System.Windows.Forms.ToolStripMenuItem MSXMenuList;
        private System.Windows.Forms.ToolStripMenuItem TurboGrafx16MenuList;
        private System.Windows.Forms.ToolStripMenuItem SegaMSMenuList;
        private System.Windows.Forms.ToolStripMenuItem GenesisMenuList;
        private System.Windows.Forms.ToolStripMenuItem VCArcadeMenuList;
        private System.Windows.Forms.ToolStripMenuItem TurboGrafxCDMenuList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem RegionCodesList;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.ContextMenuStrip extrasStrip;
        private System.Windows.Forms.ToolStripMenuItem loadInfoFromTMDToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem proxySettingsToolStripMenuItem;
        private System.Windows.Forms.GroupBox proxyBox;
        private System.Windows.Forms.TextBox ProxyUser;
        private System.Windows.Forms.Button SaveProxyBtn;
        private System.Windows.Forms.Button ProxyAssistBtn;
        private System.Windows.Forms.TextBox ProxyURL;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox ProxyVerifyBox;
        private System.Windows.Forms.Button SaveProxyPwdBtn;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox ProxyPwdBox;
        private wyDay.Controls.Windows7ProgressBar dlprogress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private wmgCMS.WaterMarkTextBox titleidbox;
        private wmgCMS.WaterMarkTextBox titleversion;
        private System.Windows.Forms.Button scriptsbutton;
        private System.Windows.Forms.ContextMenuStrip scriptsStrip;
        private System.Windows.Forms.ToolStripMenuItem loadNUSScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emulateUpdate;
        private System.Windows.Forms.ToolStripMenuItem uSANTSCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem europePALToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem japanNTSCJToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem koreaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem updateDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptsLocalMenuEntry;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem scriptsDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutNUSDToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button SaveProxyPwdPermanentBtn;
        private System.Windows.Forms.CheckBox keepenccontents;
        private System.Windows.Forms.Button saveaswadbtn;
        private System.Windows.Forms.CheckBox iosPatchCheckbox;
        private System.Windows.Forms.GroupBox iosPatchGroupBox;
        private System.Windows.Forms.CheckedListBox iosPatchesListBox;
        private System.Windows.Forms.Button iosPatchGroupBoxOKbtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel richPanel;
        private System.Windows.Forms.ToolStripMenuItem systemFakeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iosFakeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vcFakeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wwFakeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openNUSDDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moreExtrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runFolderFixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wiiBrewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databasePageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeNUSDFilesFoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localTicketInventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem dsiSystemToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem dsiFakeSystemToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem dSiWareToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem dSiWareFakeToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem wiiRegionCodesMenu;
        private System.Windows.Forms.ToolStripMenuItem dsiRegionCodesMenu;
        private System.Windows.Forms.Label serverLbl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    }
}

