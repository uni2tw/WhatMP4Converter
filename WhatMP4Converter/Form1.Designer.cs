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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbFFmpegPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbOutputPath = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabConvertPage = new System.Windows.Forms.TabPage();
            this.cbCrf = new System.Windows.Forms.ComboBox();
            this.chkRun = new System.Windows.Forms.CheckBox();
            this.tabMergePage = new System.Windows.Forms.TabPage();
            this.lbMergeModeMessage = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.nenuItemOpenFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSwitchMain = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSwitchLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabConvertPage.SuspendLayout();
            this.tabMergePage.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 214);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 79);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(632, 76);
            this.panel2.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbFFmpegPath);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.lbOutputPath);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 76);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "設定";
            // 
            // lbFFmpegPath
            // 
            this.lbFFmpegPath.AutoSize = true;
            this.lbFFmpegPath.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbFFmpegPath.Location = new System.Drawing.Point(91, 46);
            this.lbFFmpegPath.Name = "lbFFmpegPath";
            this.lbFFmpegPath.Size = new System.Drawing.Size(41, 12);
            this.lbFFmpegPath.TabIndex = 16;
            this.lbFFmpegPath.Text = "未設定";
            this.lbFFmpegPath.Click += new System.EventHandler(this.lbFFmpegPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "FFmpeg路徑: ";
            // 
            // lbOutputPath
            // 
            this.lbOutputPath.AutoSize = true;
            this.lbOutputPath.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbOutputPath.Location = new System.Drawing.Point(79, 24);
            this.lbOutputPath.Name = "lbOutputPath";
            this.lbOutputPath.Size = new System.Drawing.Size(41, 12);
            this.lbOutputPath.TabIndex = 14;
            this.lbOutputPath.Text = "同目錄";
            this.lbOutputPath.Click += new System.EventHandler(this.lbOutputPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 24);
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
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(301, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(331, 76);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 15;
            // 
            // tabConvertPage
            // 
            this.tabConvertPage.Controls.Add(this.cbCrf);
            this.tabConvertPage.Controls.Add(this.chkRun);
            this.tabConvertPage.Location = new System.Drawing.Point(4, 25);
            this.tabConvertPage.Name = "tabConvertPage";
            this.tabConvertPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabConvertPage.Size = new System.Drawing.Size(323, 47);
            this.tabConvertPage.TabIndex = 0;
            this.tabConvertPage.Text = "轉換";
            this.tabConvertPage.UseVisualStyleBackColor = true;
            // 
            // cbCrf
            // 
            this.cbCrf.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbCrf.FormattingEnabled = true;
            this.cbCrf.Items.AddRange(new object[] {
            "壓縮畫質: 好",
            "壓縮畫質: 標準",
            "壓縮畫質: 差"});
            this.cbCrf.Location = new System.Drawing.Point(6, 11);
            this.cbCrf.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cbCrf.Name = "cbCrf";
            this.cbCrf.Size = new System.Drawing.Size(103, 20);
            this.cbCrf.TabIndex = 14;
            // 
            // chkRun
            // 
            this.chkRun.AutoSize = true;
            this.chkRun.Location = new System.Drawing.Point(127, 13);
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
            this.tabMergePage.Size = new System.Drawing.Size(323, 47);
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
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(323, 47);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "裁切";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 293);
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
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabConvertPage.ResumeLayout(false);
            this.tabConvertPage.PerformLayout();
            this.tabMergePage.ResumeLayout(false);
            this.tabMergePage.PerformLayout();
            this.tabPage1.ResumeLayout(false);
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
        private System.Windows.Forms.Label lbFFmpegPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbOutputPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbMergeModeMessage;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button1;
    }
}

