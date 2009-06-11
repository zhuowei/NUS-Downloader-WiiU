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
            this.TMDButton = new System.Windows.Forms.Button();
            this.titleidbox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.statusbox = new System.Windows.Forms.TextBox();
            this.packbox = new System.Windows.Forms.CheckBox();
            this.dlprogress = new System.Windows.Forms.ProgressBar();
            this.localuse = new System.Windows.Forms.CheckBox();
            this.NUSDownloader = new System.ComponentModel.BackgroundWorker();
            this.titleversion = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.getcerts = new System.Windows.Forms.Button();
            this.wadnamebox = new System.Windows.Forms.TextBox();
            this.ignoreticket = new System.Windows.Forms.CheckBox();
            this.decryptbox = new System.Windows.Forms.CheckBox();
            this.databaseButton = new System.Windows.Forms.Button();
            this.databaseStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SystemMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.IOSMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.VCMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.C64MenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.NeoGeoMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.NESMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.SNESMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.N64MenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.MSXMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.TurboGrafx16MenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.SegaMSMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.GenesisMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.VCArcadeMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.TurboGrafxCDMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.WiiWareMenuList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RegionCodesList = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TMDButton
            // 
            this.TMDButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.TMDButton.Location = new System.Drawing.Point(128, 12);
            this.TMDButton.Name = "TMDButton";
            this.TMDButton.Size = new System.Drawing.Size(50, 20);
            this.TMDButton.TabIndex = 0;
            this.TMDButton.Text = "TMD...";
            this.TMDButton.UseVisualStyleBackColor = true;
            this.TMDButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // titleidbox
            // 
            this.titleidbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.titleidbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.titleidbox.Location = new System.Drawing.Point(12, 38);
            this.titleidbox.MaxLength = 16;
            this.titleidbox.Name = "titleidbox";
            this.titleidbox.Size = new System.Drawing.Size(176, 20);
            this.titleidbox.TabIndex = 2;
            this.titleidbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.titleidbox.TextChanged += new System.EventHandler(this.titleidbox_TextChanged);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(12, 64);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(250, 25);
            this.button3.TabIndex = 4;
            this.button3.Text = "Start NUS Download!";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // statusbox
            // 
            this.statusbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.packbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.packbox.AutoSize = true;
            this.packbox.Location = new System.Drawing.Point(12, 422);
            this.packbox.Name = "packbox";
            this.packbox.Size = new System.Drawing.Size(92, 17);
            this.packbox.TabIndex = 6;
            this.packbox.Text = "Pack -> WAD";
            this.packbox.UseVisualStyleBackColor = true;
            this.packbox.CheckedChanged += new System.EventHandler(this.packbox_CheckedChanged);
            // 
            // dlprogress
            // 
            this.dlprogress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dlprogress.Location = new System.Drawing.Point(12, 95);
            this.dlprogress.Name = "dlprogress";
            this.dlprogress.Size = new System.Drawing.Size(250, 15);
            this.dlprogress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.dlprogress.TabIndex = 7;
            // 
            // localuse
            // 
            this.localuse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.localuse.AutoSize = true;
            this.localuse.Checked = true;
            this.localuse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localuse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.localuse.Location = new System.Drawing.Point(12, 468);
            this.localuse.Name = "localuse";
            this.localuse.Size = new System.Drawing.Size(175, 17);
            this.localuse.TabIndex = 8;
            this.localuse.Text = "Use/Keep Local Files if Present";
            this.localuse.UseVisualStyleBackColor = true;
            // 
            // NUSDownloader
            // 
            this.NUSDownloader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.NUSDownloader_DoWork);
            // 
            // titleversion
            // 
            this.titleversion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleversion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.titleversion.Location = new System.Drawing.Point(204, 38);
            this.titleversion.MaxLength = 8;
            this.titleversion.Name = "titleversion";
            this.titleversion.Size = new System.Drawing.Size(58, 20);
            this.titleversion.TabIndex = 3;
            this.titleversion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.titleversion.TextChanged += new System.EventHandler(this.titleversion_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(194, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "v";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Enter a Title ID Below:";
            // 
            // radioButton2
            // 
            this.radioButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButton2.Image = global::NUS_Downloader.Properties.Resources.dsi;
            this.radioButton2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.radioButton2.Location = new System.Drawing.Point(78, 385);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(74, 31);
            this.radioButton2.TabIndex = 13;
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.Checked = true;
            this.radioButton1.Image = global::NUS_Downloader.Properties.Resources.wilolgoi;
            this.radioButton1.Location = new System.Drawing.Point(12, 385);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(60, 31);
            this.radioButton1.TabIndex = 12;
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = false;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(193, 465);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 20);
            this.button2.TabIndex = 14;
            this.button2.Text = "About";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // getcerts
            // 
            this.getcerts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.getcerts.BackColor = System.Drawing.Color.Bisque;
            this.getcerts.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.getcerts.Location = new System.Drawing.Point(48, 246);
            this.getcerts.Name = "getcerts";
            this.getcerts.Size = new System.Drawing.Size(176, 46);
            this.getcerts.TabIndex = 16;
            this.getcerts.Text = "Generate Certs";
            this.getcerts.UseVisualStyleBackColor = false;
            this.getcerts.Click += new System.EventHandler(this.getcerts_Click);
            // 
            // wadnamebox
            // 
            this.wadnamebox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.wadnamebox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wadnamebox.Enabled = false;
            this.wadnamebox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wadnamebox.Location = new System.Drawing.Point(102, 421);
            this.wadnamebox.MaxLength = 99999;
            this.wadnamebox.Name = "wadnamebox";
            this.wadnamebox.Size = new System.Drawing.Size(160, 18);
            this.wadnamebox.TabIndex = 17;
            // 
            // ignoreticket
            // 
            this.ignoreticket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ignoreticket.AutoSize = true;
            this.ignoreticket.Location = new System.Drawing.Point(12, 445);
            this.ignoreticket.Name = "ignoreticket";
            this.ignoreticket.Size = new System.Drawing.Size(135, 17);
            this.ignoreticket.TabIndex = 18;
            this.ignoreticket.Text = "Ignore Ticket if Missing";
            this.ignoreticket.UseVisualStyleBackColor = true;
            // 
            // decryptbox
            // 
            this.decryptbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.decryptbox.AutoSize = true;
            this.decryptbox.Location = new System.Drawing.Point(154, 445);
            this.decryptbox.Name = "decryptbox";
            this.decryptbox.Size = new System.Drawing.Size(108, 17);
            this.decryptbox.TabIndex = 19;
            this.decryptbox.Text = "Decrypt Contents";
            this.decryptbox.UseVisualStyleBackColor = true;
            // 
            // databaseButton
            // 
            this.databaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.databaseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.databaseButton.Location = new System.Drawing.Point(184, 12);
            this.databaseButton.Name = "databaseButton";
            this.databaseButton.Size = new System.Drawing.Size(78, 20);
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
            this.RegionCodesList});
            this.databaseStrip.Name = "databaseStrip";
            this.databaseStrip.Size = new System.Drawing.Size(155, 120);
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
            this.IOSMenuList.Size = new System.Drawing.Size(154, 22);
            this.IOSMenuList.Text = "IOS";
            // 
            // VCMenuList
            // 
            this.VCMenuList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.C64MenuList,
            this.NeoGeoMenuList,
            this.NESMenuList,
            this.SNESMenuList,
            this.N64MenuList,
            this.MSXMenuList,
            this.TurboGrafx16MenuList,
            this.SegaMSMenuList,
            this.GenesisMenuList,
            this.VCArcadeMenuList,
            this.TurboGrafxCDMenuList});
            this.VCMenuList.Name = "VCMenuList";
            this.VCMenuList.Size = new System.Drawing.Size(154, 22);
            this.VCMenuList.Text = "Virtual Console";
            // 
            // C64MenuList
            // 
            this.C64MenuList.Name = "C64MenuList";
            this.C64MenuList.Size = new System.Drawing.Size(194, 22);
            this.C64MenuList.Text = "Commodore 64";
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
            // SNESMenuList
            // 
            this.SNESMenuList.Name = "SNESMenuList";
            this.SNESMenuList.Size = new System.Drawing.Size(194, 22);
            this.SNESMenuList.Text = "SNES";
            // 
            // N64MenuList
            // 
            this.N64MenuList.Name = "N64MenuList";
            this.N64MenuList.Size = new System.Drawing.Size(194, 22);
            this.N64MenuList.Text = "Nintendo 64";
            // 
            // MSXMenuList
            // 
            this.MSXMenuList.Name = "MSXMenuList";
            this.MSXMenuList.Size = new System.Drawing.Size(194, 22);
            this.MSXMenuList.Text = "MSX";
            // 
            // TurboGrafx16MenuList
            // 
            this.TurboGrafx16MenuList.Name = "TurboGrafx16MenuList";
            this.TurboGrafx16MenuList.Size = new System.Drawing.Size(194, 22);
            this.TurboGrafx16MenuList.Text = "TruboGrafx-16";
            // 
            // SegaMSMenuList
            // 
            this.SegaMSMenuList.Name = "SegaMSMenuList";
            this.SegaMSMenuList.Size = new System.Drawing.Size(194, 22);
            this.SegaMSMenuList.Text = "Sega Master System";
            // 
            // GenesisMenuList
            // 
            this.GenesisMenuList.Name = "GenesisMenuList";
            this.GenesisMenuList.Size = new System.Drawing.Size(194, 22);
            this.GenesisMenuList.Text = "Mega Drive/Genesis";
            // 
            // VCArcadeMenuList
            // 
            this.VCArcadeMenuList.Name = "VCArcadeMenuList";
            this.VCArcadeMenuList.Size = new System.Drawing.Size(194, 22);
            this.VCArcadeMenuList.Text = "Virtual Console Arcade";
            // 
            // TurboGrafxCDMenuList
            // 
            this.TurboGrafxCDMenuList.Name = "TurboGrafxCDMenuList";
            this.TurboGrafxCDMenuList.Size = new System.Drawing.Size(194, 22);
            this.TurboGrafxCDMenuList.Text = "TurboGrafx-CD";
            // 
            // WiiWareMenuList
            // 
            this.WiiWareMenuList.Name = "WiiWareMenuList";
            this.WiiWareMenuList.Size = new System.Drawing.Size(154, 22);
            this.WiiWareMenuList.Text = "WiiWare";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // RegionCodesList
            // 
            this.RegionCodesList.Name = "RegionCodesList";
            this.RegionCodesList.Size = new System.Drawing.Size(154, 22);
            this.RegionCodesList.Text = "Region Codes";
            this.RegionCodesList.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.RegionCodesList_DropDownItemClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 496);
            this.Controls.Add(this.databaseButton);
            this.Controls.Add(this.decryptbox);
            this.Controls.Add(this.ignoreticket);
            this.Controls.Add(this.wadnamebox);
            this.Controls.Add(this.getcerts);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.titleversion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.localuse);
            this.Controls.Add(this.dlprogress);
            this.Controls.Add(this.packbox);
            this.Controls.Add(this.statusbox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.titleidbox);
            this.Controls.Add(this.TMDButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(290, 534);
            this.Name = "Form1";
            this.Text = "NUSD";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.databaseStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TMDButton;
        private System.Windows.Forms.TextBox titleidbox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox statusbox;
        private System.Windows.Forms.CheckBox packbox;
        private System.Windows.Forms.ProgressBar dlprogress;
        private System.Windows.Forms.CheckBox localuse;
        private System.ComponentModel.BackgroundWorker NUSDownloader;
        private System.Windows.Forms.TextBox titleversion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button getcerts;
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
    }
}

