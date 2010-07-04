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
            this.statusbox = new System.Windows.Forms.TextBox();
            this.packbox = new System.Windows.Forms.CheckBox();
            this.localuse = new System.Windows.Forms.CheckBox();
            this.NUSDownloader = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.wadnamebox = new System.Windows.Forms.TextBox();
            this.ignoreticket = new System.Windows.Forms.CheckBox();
            this.decryptbox = new System.Windows.Forms.CheckBox();
            this.databaseButton = new System.Windows.Forms.Button();
            this.databaseStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SystemMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.IOSMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.VCMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.C64MenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.C64MenuListDrop = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.WiiWareMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RegionCodesList = new System.Windows.Forms.ToolStripMenuItem();
            this.MassUpdateList = new System.Windows.Forms.ToolStripMenuItem();
            this.PALMassUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.NTSCMassUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.KoreaMassUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.extrasStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadInfoFromTMDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.emulateUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.uSANTSCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.europePALToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.japanNTSCJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.koreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.proxySettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.loadNUSScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.getCommonKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commonKeykeybinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.koreanKeykkeybinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.updateDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveaswadbox = new System.Windows.Forms.CheckBox();
            this.deletecontentsbox = new System.Windows.Forms.CheckBox();
            this.proxyBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ProxyUser = new System.Windows.Forms.TextBox();
            this.SaveProxyBtn = new System.Windows.Forms.Button();
            this.ProxyAssistBtn = new System.Windows.Forms.Button();
            this.ProxyURL = new System.Windows.Forms.TextBox();
            this.ProxyVerifyBox = new System.Windows.Forms.GroupBox();
            this.SaveProxyPwdBtn = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.ProxyPwdBox = new System.Windows.Forms.TextBox();
            this.consoleCBox = new System.Windows.Forms.ComboBox();
            this.scriptsbutton = new System.Windows.Forms.Button();
            this.scriptsStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.titleversion = new wmgCMS.WaterMarkTextBox();
            this.titleidbox = new wmgCMS.WaterMarkTextBox();
            this.dlprogress = new wyDay.Controls.Windows7ProgressBar();
            this.databaseStrip.SuspendLayout();
            this.extrasStrip.SuspendLayout();
            this.proxyBox.SuspendLayout();
            this.ProxyVerifyBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Extrasbtn
            // 
            this.Extrasbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Extrasbtn.Location = new System.Drawing.Point(194, 5);
            this.Extrasbtn.Name = "Extrasbtn";
            this.Extrasbtn.Size = new System.Drawing.Size(68, 27);
            this.Extrasbtn.TabIndex = 0;
            this.Extrasbtn.Text = "Extras...";
            this.Extrasbtn.UseVisualStyleBackColor = true;
            this.Extrasbtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // downloadstartbtn
            // 
            this.downloadstartbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.downloadstartbtn.Location = new System.Drawing.Point(12, 64);
            this.downloadstartbtn.Name = "downloadstartbtn";
            this.downloadstartbtn.Size = new System.Drawing.Size(250, 25);
            this.downloadstartbtn.TabIndex = 4;
            this.downloadstartbtn.Text = "Start NUS Download!";
            this.downloadstartbtn.UseVisualStyleBackColor = true;
            this.downloadstartbtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // statusbox
            // 
            this.statusbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusbox.Location = new System.Drawing.Point(12, 116);
            this.statusbox.Multiline = true;
            this.statusbox.Name = "statusbox";
            this.statusbox.Size = new System.Drawing.Size(250, 268);
            this.statusbox.TabIndex = 5;
            // 
            // packbox
            // 
            this.packbox.AutoSize = true;
            this.packbox.Location = new System.Drawing.Point(12, 416);
            this.packbox.Name = "packbox";
            this.packbox.Size = new System.Drawing.Size(92, 17);
            this.packbox.TabIndex = 6;
            this.packbox.Text = "Pack -> WAD";
            this.packbox.UseVisualStyleBackColor = true;
            this.packbox.CheckedChanged += new System.EventHandler(this.packbox_CheckedChanged);
            this.packbox.EnabledChanged += new System.EventHandler(this.packbox_EnabledChanged);
            // 
            // localuse
            // 
            this.localuse.AutoSize = true;
            this.localuse.Checked = true;
            this.localuse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localuse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.localuse.Location = new System.Drawing.Point(104, 463);
            this.localuse.Name = "localuse";
            this.localuse.Size = new System.Drawing.Size(76, 17);
            this.localuse.TabIndex = 8;
            this.localuse.Text = "Local Files";
            this.localuse.UseVisualStyleBackColor = true;
            // 
            // NUSDownloader
            // 
            this.NUSDownloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.NUSDownloader_DoWork);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(194, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "v";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(154, 389);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(53, 21);
            this.button2.TabIndex = 14;
            this.button2.Text = "About";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // wadnamebox
            // 
            this.wadnamebox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wadnamebox.Enabled = false;
            this.wadnamebox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wadnamebox.Location = new System.Drawing.Point(102, 416);
            this.wadnamebox.MaxLength = 99999;
            this.wadnamebox.Name = "wadnamebox";
            this.wadnamebox.Size = new System.Drawing.Size(160, 18);
            this.wadnamebox.TabIndex = 17;
            // 
            // ignoreticket
            // 
            this.ignoreticket.AutoSize = true;
            this.ignoreticket.Location = new System.Drawing.Point(104, 440);
            this.ignoreticket.Name = "ignoreticket";
            this.ignoreticket.Size = new System.Drawing.Size(89, 17);
            this.ignoreticket.TabIndex = 18;
            this.ignoreticket.Text = "Ignore Ticket";
            this.ignoreticket.UseVisualStyleBackColor = true;
            // 
            // decryptbox
            // 
            this.decryptbox.AutoSize = true;
            this.decryptbox.Location = new System.Drawing.Point(199, 440);
            this.decryptbox.Name = "decryptbox";
            this.decryptbox.Size = new System.Drawing.Size(63, 17);
            this.decryptbox.TabIndex = 19;
            this.decryptbox.Text = "Decrypt";
            this.decryptbox.UseVisualStyleBackColor = true;
            // 
            // databaseButton
            // 
            this.databaseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.databaseButton.Location = new System.Drawing.Point(12, 5);
            this.databaseButton.Name = "databaseButton";
            this.databaseButton.Size = new System.Drawing.Size(85, 27);
            this.databaseButton.TabIndex = 20;
            this.databaseButton.Text = "Database...";
            this.databaseButton.UseVisualStyleBackColor = true;
            this.databaseButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // databaseStrip
            // 
            this.databaseStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SystemMenuList,
            this.IOSMenuList,
            this.VCMenuList,
            this.WiiWareMenuList,
            this.toolStripSeparator1,
            this.RegionCodesList,
            this.MassUpdateList});
            this.databaseStrip.Name = "databaseStrip";
            this.databaseStrip.ShowItemToolTips = false;
            this.databaseStrip.Size = new System.Drawing.Size(167, 142);
            // 
            // SystemMenuList
            // 
            this.SystemMenuList.AutoSize = false;
            this.SystemMenuList.Name = "SystemMenuList";
            this.SystemMenuList.Size = new System.Drawing.Size(154, 22);
            this.SystemMenuList.Text = "System";
            // 
            // IOSMenuList
            // 
            this.IOSMenuList.Name = "IOSMenuList";
            this.IOSMenuList.Size = new System.Drawing.Size(166, 22);
            this.IOSMenuList.Text = "IOS";
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
            this.VCMenuList.Name = "VCMenuList";
            this.VCMenuList.Size = new System.Drawing.Size(166, 22);
            this.VCMenuList.Text = "Virtual Console";
            // 
            // C64MenuList
            // 
            this.C64MenuList.DropDown = this.C64MenuListDrop;
            this.C64MenuList.Name = "C64MenuList";
            this.C64MenuList.Size = new System.Drawing.Size(194, 22);
            this.C64MenuList.Text = "Commodore 64";
            // 
            // C64MenuListDrop
            // 
            this.C64MenuListDrop.Name = "C64MenuListDrop";
            this.C64MenuListDrop.OwnerItem = this.C64MenuList;
            this.C64MenuListDrop.Size = new System.Drawing.Size(61, 4);
            // 
            // GenesisMenuList
            // 
            this.GenesisMenuList.Name = "GenesisMenuList";
            this.GenesisMenuList.Size = new System.Drawing.Size(194, 22);
            this.GenesisMenuList.Text = "Mega Drive/Genesis";
            // 
            // MSXMenuList
            // 
            this.MSXMenuList.Name = "MSXMenuList";
            this.MSXMenuList.Size = new System.Drawing.Size(194, 22);
            this.MSXMenuList.Text = "MSX";
            // 
            // N64MenuList
            // 
            this.N64MenuList.Name = "N64MenuList";
            this.N64MenuList.Size = new System.Drawing.Size(194, 22);
            this.N64MenuList.Text = "Nintendo 64";
            // 
            // NeoGeoMenuList
            // 
            this.NeoGeoMenuList.Name = "NeoGeoMenuList";
            this.NeoGeoMenuList.Size = new System.Drawing.Size(194, 22);
            this.NeoGeoMenuList.Text = "NeoGeo";
            // 
            // NESMenuList
            // 
            this.NESMenuList.Name = "NESMenuList";
            this.NESMenuList.Size = new System.Drawing.Size(194, 22);
            this.NESMenuList.Text = "NES";
            // 
            // SegaMSMenuList
            // 
            this.SegaMSMenuList.Name = "SegaMSMenuList";
            this.SegaMSMenuList.Size = new System.Drawing.Size(194, 22);
            this.SegaMSMenuList.Text = "Sega Master System";
            // 
            // SNESMenuList
            // 
            this.SNESMenuList.Name = "SNESMenuList";
            this.SNESMenuList.Size = new System.Drawing.Size(194, 22);
            this.SNESMenuList.Text = "SNES";
            // 
            // TurboGrafx16MenuList
            // 
            this.TurboGrafx16MenuList.Name = "TurboGrafx16MenuList";
            this.TurboGrafx16MenuList.Size = new System.Drawing.Size(194, 22);
            this.TurboGrafx16MenuList.Text = "TruboGrafx-16";
            // 
            // TurboGrafxCDMenuList
            // 
            this.TurboGrafxCDMenuList.Name = "TurboGrafxCDMenuList";
            this.TurboGrafxCDMenuList.Size = new System.Drawing.Size(194, 22);
            this.TurboGrafxCDMenuList.Text = "TurboGrafx-CD";
            // 
            // VCArcadeMenuList
            // 
            this.VCArcadeMenuList.Name = "VCArcadeMenuList";
            this.VCArcadeMenuList.Size = new System.Drawing.Size(194, 22);
            this.VCArcadeMenuList.Text = "Virtual Console Arcade";
            // 
            // WiiWareMenuList
            // 
            this.WiiWareMenuList.Name = "WiiWareMenuList";
            this.WiiWareMenuList.Size = new System.Drawing.Size(166, 22);
            this.WiiWareMenuList.Text = "WiiWare";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // RegionCodesList
            // 
            this.RegionCodesList.Name = "RegionCodesList";
            this.RegionCodesList.Size = new System.Drawing.Size(166, 22);
            this.RegionCodesList.Text = "Region Codes";
            this.RegionCodesList.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.RegionCodesList_DropDownItemClicked);
            // 
            // MassUpdateList
            // 
            this.MassUpdateList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PALMassUpdate,
            this.NTSCMassUpdate,
            this.KoreaMassUpdate});
            this.MassUpdateList.Name = "MassUpdateList";
            this.MassUpdateList.Size = new System.Drawing.Size(166, 22);
            this.MassUpdateList.Text = "Download Scripts";
            // 
            // PALMassUpdate
            // 
            this.PALMassUpdate.Name = "PALMassUpdate";
            this.PALMassUpdate.Size = new System.Drawing.Size(104, 22);
            this.PALMassUpdate.Text = "PAL";
            // 
            // NTSCMassUpdate
            // 
            this.NTSCMassUpdate.Name = "NTSCMassUpdate";
            this.NTSCMassUpdate.Size = new System.Drawing.Size(104, 22);
            this.NTSCMassUpdate.Text = "NTSC";
            // 
            // KoreaMassUpdate
            // 
            this.KoreaMassUpdate.Name = "KoreaMassUpdate";
            this.KoreaMassUpdate.Size = new System.Drawing.Size(104, 22);
            this.KoreaMassUpdate.Text = "Korea";
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(213, 383);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(49, 27);
            this.button3.TabIndex = 31;
            this.button3.Text = "Clear";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // extrasStrip
            // 
            this.extrasStrip.AllowMerge = false;
            this.extrasStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadInfoFromTMDToolStripMenuItem,
            this.toolStripSeparator3,
            this.emulateUpdate,
            this.toolStripSeparator4,
            this.proxySettingsToolStripMenuItem,
            this.toolStripSeparator6,
            this.loadNUSScriptToolStripMenuItem,
            this.toolStripSeparator7,
            this.getCommonKeyMenuItem,
            this.toolStripSeparator2,
            this.updateDatabaseToolStripMenuItem});
            this.extrasStrip.Name = "extrasStrip";
            this.extrasStrip.Size = new System.Drawing.Size(220, 166);
            // 
            // loadInfoFromTMDToolStripMenuItem
            // 
            this.loadInfoFromTMDToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.page_white_magnify;
            this.loadInfoFromTMDToolStripMenuItem.Name = "loadInfoFromTMDToolStripMenuItem";
            this.loadInfoFromTMDToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.loadInfoFromTMDToolStripMenuItem.Text = "Load Info from TMD";
            this.loadInfoFromTMDToolStripMenuItem.Click += new System.EventHandler(this.loadInfoFromTMDToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(216, 6);
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
            this.emulateUpdate.Size = new System.Drawing.Size(219, 22);
            this.emulateUpdate.Text = "Emulate Wii System Update";
            this.emulateUpdate.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.emulateUpdate_DropDownItemClicked);
            // 
            // uSANTSCToolStripMenuItem
            // 
            this.uSANTSCToolStripMenuItem.Name = "uSANTSCToolStripMenuItem";
            this.uSANTSCToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.uSANTSCToolStripMenuItem.Text = "USA";
            // 
            // europePALToolStripMenuItem
            // 
            this.europePALToolStripMenuItem.Name = "europePALToolStripMenuItem";
            this.europePALToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.europePALToolStripMenuItem.Text = "EUROPE";
            // 
            // japanNTSCJToolStripMenuItem
            // 
            this.japanNTSCJToolStripMenuItem.Name = "japanNTSCJToolStripMenuItem";
            this.japanNTSCJToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.japanNTSCJToolStripMenuItem.Text = "JAPAN";
            // 
            // koreaToolStripMenuItem
            // 
            this.koreaToolStripMenuItem.Name = "koreaToolStripMenuItem";
            this.koreaToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.koreaToolStripMenuItem.Text = "KOREA";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(216, 6);
            // 
            // proxySettingsToolStripMenuItem
            // 
            this.proxySettingsToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.server_link;
            this.proxySettingsToolStripMenuItem.Name = "proxySettingsToolStripMenuItem";
            this.proxySettingsToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.proxySettingsToolStripMenuItem.Text = "Proxy Settings";
            this.proxySettingsToolStripMenuItem.Click += new System.EventHandler(this.proxySettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(216, 6);
            // 
            // loadNUSScriptToolStripMenuItem
            // 
            this.loadNUSScriptToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.script_go;
            this.loadNUSScriptToolStripMenuItem.Name = "loadNUSScriptToolStripMenuItem";
            this.loadNUSScriptToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.loadNUSScriptToolStripMenuItem.Text = "Load NUS Script";
            this.loadNUSScriptToolStripMenuItem.Click += new System.EventHandler(this.loadNUSScriptToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(216, 6);
            // 
            // getCommonKeyMenuItem
            // 
            this.getCommonKeyMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.commonKeykeybinToolStripMenuItem,
            this.koreanKeykkeybinToolStripMenuItem});
            this.getCommonKeyMenuItem.Image = global::NUS_Downloader.Properties.Resources.key;
            this.getCommonKeyMenuItem.Name = "getCommonKeyMenuItem";
            this.getCommonKeyMenuItem.Size = new System.Drawing.Size(219, 22);
            this.getCommonKeyMenuItem.Text = "Retrieve Key";
            // 
            // commonKeykeybinToolStripMenuItem
            // 
            this.commonKeykeybinToolStripMenuItem.Name = "commonKeykeybinToolStripMenuItem";
            this.commonKeykeybinToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.commonKeykeybinToolStripMenuItem.Text = "Common Key (key.bin)";
            this.commonKeykeybinToolStripMenuItem.Click += new System.EventHandler(this.commonKeykeybinToolStripMenuItem_Click);
            // 
            // koreanKeykkeybinToolStripMenuItem
            // 
            this.koreanKeykkeybinToolStripMenuItem.Name = "koreanKeykkeybinToolStripMenuItem";
            this.koreanKeykkeybinToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.koreanKeykkeybinToolStripMenuItem.Text = "Korean Key (kkey.bin)";
            this.koreanKeykkeybinToolStripMenuItem.Click += new System.EventHandler(this.koreanKeykkeybinToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(216, 6);
            // 
            // updateDatabaseToolStripMenuItem
            // 
            this.updateDatabaseToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.database_save;
            this.updateDatabaseToolStripMenuItem.Name = "updateDatabaseToolStripMenuItem";
            this.updateDatabaseToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.updateDatabaseToolStripMenuItem.Text = "Update Database";
            this.updateDatabaseToolStripMenuItem.Click += new System.EventHandler(this.updateDatabaseToolStripMenuItem_Click);
            // 
            // saveaswadbox
            // 
            this.saveaswadbox.AutoSize = true;
            this.saveaswadbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveaswadbox.Location = new System.Drawing.Point(22, 437);
            this.saveaswadbox.Name = "saveaswadbox";
            this.saveaswadbox.Size = new System.Drawing.Size(58, 16);
            this.saveaswadbox.TabIndex = 43;
            this.saveaswadbox.Text = "SaveAs";
            this.saveaswadbox.UseVisualStyleBackColor = true;
            // 
            // deletecontentsbox
            // 
            this.deletecontentsbox.AutoSize = true;
            this.deletecontentsbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deletecontentsbox.Location = new System.Drawing.Point(22, 454);
            this.deletecontentsbox.Name = "deletecontentsbox";
            this.deletecontentsbox.Size = new System.Drawing.Size(62, 28);
            this.deletecontentsbox.TabIndex = 44;
            this.deletecontentsbox.Text = "Delete\r\nContents";
            this.deletecontentsbox.UseVisualStyleBackColor = true;
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
            this.proxyBox.Location = new System.Drawing.Point(33, 221);
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
            this.ProxyVerifyBox.Controls.Add(this.SaveProxyPwdBtn);
            this.ProxyVerifyBox.Controls.Add(this.label14);
            this.ProxyVerifyBox.Controls.Add(this.ProxyPwdBox);
            this.ProxyVerifyBox.Location = new System.Drawing.Point(33, 202);
            this.ProxyVerifyBox.Name = "ProxyVerifyBox";
            this.ProxyVerifyBox.Size = new System.Drawing.Size(212, 75);
            this.ProxyVerifyBox.TabIndex = 46;
            this.ProxyVerifyBox.TabStop = false;
            this.ProxyVerifyBox.Text = "Verify Credentials";
            this.ProxyVerifyBox.Visible = false;
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
            this.SaveProxyPwdBtn.Click += new System.EventHandler(this.button18_Click);
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
            // consoleCBox
            // 
            this.consoleCBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.consoleCBox.FormattingEnabled = true;
            this.consoleCBox.Items.AddRange(new object[] {
            "Wii",
            "DSi"});
            this.consoleCBox.Location = new System.Drawing.Point(12, 389);
            this.consoleCBox.Name = "consoleCBox";
            this.consoleCBox.Size = new System.Drawing.Size(58, 21);
            this.consoleCBox.TabIndex = 48;
            this.consoleCBox.SelectedIndexChanged += new System.EventHandler(this.consoleCBox_SelectedIndexChanged);
            // 
            // scriptsbutton
            // 
            this.scriptsbutton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.scriptsbutton.Location = new System.Drawing.Point(103, 5);
            this.scriptsbutton.Name = "scriptsbutton";
            this.scriptsbutton.Size = new System.Drawing.Size(85, 27);
            this.scriptsbutton.TabIndex = 51;
            this.scriptsbutton.Text = "Scripts...";
            this.scriptsbutton.UseVisualStyleBackColor = true;
            this.scriptsbutton.Click += new System.EventHandler(this.scriptsbutton_Click);
            // 
            // scriptsStrip
            // 
            this.scriptsStrip.Name = "scriptsStrip";
            this.scriptsStrip.ShowItemToolTips = false;
            this.scriptsStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // titleversion
            // 
            this.titleversion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.titleversion.Location = new System.Drawing.Point(204, 38);
            this.titleversion.MaxLength = 8;
            this.titleversion.Name = "titleversion";
            this.titleversion.Size = new System.Drawing.Size(58, 20);
            this.titleversion.TabIndex = 50;
            this.titleversion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.titleversion.WaterMarkColor = System.Drawing.Color.Silver;
            this.titleversion.WaterMarkText = "Version";
            this.titleversion.TextChanged += new System.EventHandler(this.titleversion_TextChanged);
            // 
            // titleidbox
            // 
            this.titleidbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.titleidbox.Location = new System.Drawing.Point(12, 38);
            this.titleidbox.MaxLength = 16;
            this.titleidbox.Name = "titleidbox";
            this.titleidbox.Size = new System.Drawing.Size(176, 20);
            this.titleidbox.TabIndex = 49;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 492);
            this.Controls.Add(this.scriptsbutton);
            this.Controls.Add(this.titleversion);
            this.Controls.Add(this.titleidbox);
            this.Controls.Add(this.dlprogress);
            this.Controls.Add(this.consoleCBox);
            this.Controls.Add(this.deletecontentsbox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ProxyVerifyBox);
            this.Controls.Add(this.saveaswadbox);
            this.Controls.Add(this.proxyBox);
            this.Controls.Add(this.wadnamebox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.databaseButton);
            this.Controls.Add(this.decryptbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.packbox);
            this.Controls.Add(this.statusbox);
            this.Controls.Add(this.ignoreticket);
            this.Controls.Add(this.downloadstartbtn);
            this.Controls.Add(this.localuse);
            this.Controls.Add(this.Extrasbtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(545, 520);
            this.MinimumSize = new System.Drawing.Size(280, 520);
            this.Name = "Form1";
            this.Text = "NUSD";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.databaseStrip.ResumeLayout(false);
            this.extrasStrip.ResumeLayout(false);
            this.proxyBox.ResumeLayout(false);
            this.proxyBox.PerformLayout();
            this.ProxyVerifyBox.ResumeLayout(false);
            this.ProxyVerifyBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Extrasbtn;
        private System.Windows.Forms.Button downloadstartbtn;
        private System.Windows.Forms.TextBox statusbox;
        private System.Windows.Forms.CheckBox packbox;
        private System.Windows.Forms.CheckBox localuse;
        private System.ComponentModel.BackgroundWorker NUSDownloader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox wadnamebox;
        private System.Windows.Forms.CheckBox ignoreticket;
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
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ContextMenuStrip extrasStrip;
        private System.Windows.Forms.ToolStripMenuItem loadInfoFromTMDToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem emulateUpdate;
        private System.Windows.Forms.ToolStripMenuItem uSANTSCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem europePALToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem japanNTSCJToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem koreaToolStripMenuItem;
        private System.Windows.Forms.CheckBox saveaswadbox;
        private System.Windows.Forms.CheckBox deletecontentsbox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
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
        private System.Windows.Forms.ToolStripMenuItem loadNUSScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem getCommonKeyMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem updateDatabaseToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip C64MenuListDrop;
        private System.Windows.Forms.ToolStripMenuItem MassUpdateList;
        private System.Windows.Forms.ToolStripMenuItem PALMassUpdate;
        private System.Windows.Forms.ToolStripMenuItem NTSCMassUpdate;
        private System.Windows.Forms.ToolStripMenuItem KoreaMassUpdate;
        private System.Windows.Forms.ComboBox consoleCBox;
        private System.Windows.Forms.ToolStripMenuItem commonKeykeybinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem koreanKeykkeybinToolStripMenuItem;
        private wmgCMS.WaterMarkTextBox titleidbox;
        private wmgCMS.WaterMarkTextBox titleversion;
        private System.Windows.Forms.Button scriptsbutton;
        private System.Windows.Forms.ContextMenuStrip scriptsStrip;
    }
}

