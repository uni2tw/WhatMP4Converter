namespace WhatMP4Converter
{
    partial class formMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStripFileTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbOutputPath = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabConvertPage = new System.Windows.Forms.TabPage();
            this.cbShrinkWidth = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbGainFontSize = new System.Windows.Forms.ComboBox();
            this.cbCrf = new System.Windows.Forms.ComboBox();
            this.chkRun = new System.Windows.Forms.CheckBox();
            this.tabMergePage = new System.Windows.Forms.TabPage();
            this.lbMergeModeMessage = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabExtractAssPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.nenuItemOpenFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSwitchMain = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSwitchLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.contextMenuStripFileTree.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabConvertPage.SuspendLayout();
            this.tabMergePage.SuspendLayout();
            this.tabExtractAssPage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 24);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.groupBox1.Size = new System.Drawing.Size(632, 190);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(5, 15);
            this.listView1.Margin = new System.Windows.Forms.Padding(0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(622, 175);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListView1_ItemSelectionChanged);
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            // 
            // contextMenuStripFileTree
            // 
            this.contextMenuStripFileTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiOpenFolder});
            this.contextMenuStripFileTree.Name = "CMenuFileTree";
            this.contextMenuStripFileTree.Size = new System.Drawing.Size(119, 26);
            // 
            // cmiOpenFolder
            // 
            this.cmiOpenFolder.Name = "cmiOpenFolder";
            this.cmiOpenFolder.Size = new System.Drawing.Size(118, 22);
            this.cmiOpenFolder.Text = "開啟目錄";
            this.cmiOpenFolder.Click += new System.EventHandler(this.CmiOpenFolder_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 214);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 104);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(632, 101);
            this.panel2.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbOutputPath);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 101);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "設定";
            // 
            // lbOutputPath
            // 
            this.lbOutputPath.AutoSize = true;
            this.lbOutputPath.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbOutputPath.Location = new System.Drawing.Point(79, 31);
            this.lbOutputPath.Name = "lbOutputPath";
            this.lbOutputPath.Size = new System.Drawing.Size(41, 12);
            this.lbOutputPath.TabIndex = 14;
            this.lbOutputPath.Text = "同目錄";
            this.lbOutputPath.Click += new System.EventHandler(this.lbOutputPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "輸出路徑: ";
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabConvertPage);
            this.tabControl1.Controls.Add(this.tabMergePage);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabExtractAssPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(215, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(417, 101);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 15;
            // 
            // tabConvertPage
            // 
            this.tabConvertPage.Controls.Add(this.cbShrinkWidth);
            this.tabConvertPage.Controls.Add(this.label3);
            this.tabConvertPage.Controls.Add(this.cbGainFontSize);
            this.tabConvertPage.Controls.Add(this.cbCrf);
            this.tabConvertPage.Controls.Add(this.chkRun);
            this.tabConvertPage.Location = new System.Drawing.Point(4, 25);
            this.tabConvertPage.Name = "tabConvertPage";
            this.tabConvertPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabConvertPage.Size = new System.Drawing.Size(409, 72);
            this.tabConvertPage.TabIndex = 0;
            this.tabConvertPage.Text = "轉換";
            this.tabConvertPage.UseVisualStyleBackColor = true;
            // 
            // cbShrinkWidth
            // 
            this.cbShrinkWidth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbShrinkWidth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbShrinkWidth.FormattingEnabled = true;
            this.cbShrinkWidth.Items.AddRange(new object[] {
            "最大寬度: 不指定",
            "最大寬度: 1920",
            "最大寬度: 1280",
            "最大寬度: 720"});
            this.cbShrinkWidth.Location = new System.Drawing.Point(15, 39);
            this.cbShrinkWidth.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cbShrinkWidth.Name = "cbShrinkWidth";
            this.cbShrinkWidth.Size = new System.Drawing.Size(103, 20);
            this.cbShrinkWidth.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "字型";
            // 
            // cbGainFontSize
            // 
            this.cbGainFontSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbGainFontSize.FormattingEnabled = true;
            this.cbGainFontSize.Items.AddRange(new object[] {
            "-10",
            "-5",
            "+0",
            "+5",
            "+10",
            "+15",
            "+20"});
            this.cbGainFontSize.Location = new System.Drawing.Point(168, 11);
            this.cbGainFontSize.Name = "cbGainFontSize";
            this.cbGainFontSize.Size = new System.Drawing.Size(48, 20);
            this.cbGainFontSize.TabIndex = 17;
            // 
            // cbCrf
            // 
            this.cbCrf.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbCrf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCrf.FormattingEnabled = true;
            this.cbCrf.Items.AddRange(new object[] {
            "壓縮畫質: 好",
            "壓縮畫質: 標準",
            "壓縮畫質: 差"});
            this.cbCrf.Location = new System.Drawing.Point(15, 8);
            this.cbCrf.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cbCrf.Name = "cbCrf";
            this.cbCrf.Size = new System.Drawing.Size(103, 20);
            this.cbCrf.TabIndex = 14;
            // 
            // chkRun
            // 
            this.chkRun.AutoSize = true;
            this.chkRun.Location = new System.Drawing.Point(236, 13);
            this.chkRun.Name = "chkRun";
            this.chkRun.Size = new System.Drawing.Size(72, 16);
            this.chkRun.TabIndex = 13;
            this.chkRun.Text = "進行轉換";
            this.chkRun.UseVisualStyleBackColor = true;
            // 
            // tabMergePage
            // 
            this.tabMergePage.Controls.Add(this.lbMergeModeMessage);
            this.tabMergePage.Location = new System.Drawing.Point(4, 25);
            this.tabMergePage.Name = "tabMergePage";
            this.tabMergePage.Padding = new System.Windows.Forms.Padding(3);
            this.tabMergePage.Size = new System.Drawing.Size(409, 72);
            this.tabMergePage.TabIndex = 1;
            this.tabMergePage.Text = "合併";
            this.tabMergePage.UseVisualStyleBackColor = true;
            // 
            // lbMergeModeMessage
            // 
            this.lbMergeModeMessage.AutoSize = true;
            this.lbMergeModeMessage.Location = new System.Drawing.Point(24, 19);
            this.lbMergeModeMessage.Name = "lbMergeModeMessage";
            this.lbMergeModeMessage.Size = new System.Drawing.Size(195, 12);
            this.lbMergeModeMessage.TabIndex = 0;
            this.lbMergeModeMessage.Text = "只支援MP4，且使用無損畫質的合併";
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(409, 72);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "裁切";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabExtractAssPage
            // 
            this.tabExtractAssPage.Controls.Add(this.label1);
            this.tabExtractAssPage.Location = new System.Drawing.Point(4, 25);
            this.tabExtractAssPage.Name = "tabExtractAssPage";
            this.tabExtractAssPage.Size = new System.Drawing.Size(409, 72);
            this.tabExtractAssPage.TabIndex = 3;
            this.tabExtractAssPage.Text = "匯出字幕";
            this.tabExtractAssPage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "匯出MKV的內嵌ASS字幕";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nenuItemOpenFiles,
            this.menuItemSwitchMain,
            this.menuItemSwitchLog,
            this.menuItemQuit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(632, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // nenuItemOpenFiles
            // 
            this.nenuItemOpenFiles.Name = "nenuItemOpenFiles";
            this.nenuItemOpenFiles.Size = new System.Drawing.Size(65, 20);
            this.nenuItemOpenFiles.Text = "選取檔案";
            this.nenuItemOpenFiles.Click += new System.EventHandler(this.nenuItemOpenFiles_Click);
            // 
            // menuItemSwitchMain
            // 
            this.menuItemSwitchMain.Name = "menuItemSwitchMain";
            this.menuItemSwitchMain.Size = new System.Drawing.Size(53, 20);
            this.menuItemSwitchMain.Text = "主畫面";
            this.menuItemSwitchMain.Click += new System.EventHandler(this.menuItemSwitchMain_Click);
            // 
            // menuItemSwitchLog
            // 
            this.menuItemSwitchLog.Name = "menuItemSwitchLog";
            this.menuItemSwitchLog.Size = new System.Drawing.Size(65, 20);
            this.menuItemSwitchLog.Text = "日誌畫面";
            this.menuItemSwitchLog.Click += new System.EventHandler(this.menuItemSwitchLog_Click);
            // 
            // menuItemQuit
            // 
            this.menuItemQuit.Name = "menuItemQuit";
            this.menuItemQuit.ShortcutKeyDisplayString = "";
            this.menuItemQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemQuit.Size = new System.Drawing.Size(41, 20);
            this.menuItemQuit.Text = "結束";
            this.menuItemQuit.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 318);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStripFileTree.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabConvertPage.ResumeLayout(false);
            this.tabConvertPage.PerformLayout();
            this.tabMergePage.ResumeLayout(false);
            this.tabMergePage.PerformLayout();
            this.tabExtractAssPage.ResumeLayout(false);
            this.tabExtractAssPage.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem nenuItemOpenFiles;
        private System.Windows.Forms.ToolStripMenuItem menuItemSwitchMain;
        private System.Windows.Forms.ToolStripMenuItem menuItemSwitchLog;
        private System.Windows.Forms.ToolStripMenuItem menuItemQuit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabConvertPage;
        private System.Windows.Forms.ComboBox cbCrf;
        private System.Windows.Forms.CheckBox chkRun;
        private System.Windows.Forms.TabPage tabMergePage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbOutputPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbMergeModeMessage;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox cbGainFontSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbShrinkWidth;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFileTree;
        private System.Windows.Forms.ToolStripMenuItem cmiOpenFolder;
        private System.Windows.Forms.TabPage tabExtractAssPage;
        private System.Windows.Forms.Label label1;
    }
}

