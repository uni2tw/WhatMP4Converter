namespace WhatMP4Converter
{
    partial class FormCutPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCutPreview));
            this.panel1 = new System.Windows.Forms.Panel();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.tbStartMinute = new System.Windows.Forms.TextBox();
            this.tbStartSecond = new System.Windows.Forms.TextBox();
            this.tbStartMillSecond = new System.Windows.Forms.TextBox();
            this.btnPreviewStart = new System.Windows.Forms.Button();
            this.tbToMillSecond = new System.Windows.Forms.TextBox();
            this.tbToSecond = new System.Windows.Forms.TextBox();
            this.tbToMinute = new System.Windows.Forms.TextBox();
            this.lbPointer = new System.Windows.Forms.Label();
            this.btnPreviewEnd = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.axWindowsMediaPlayer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 360);
            this.panel1.TabIndex = 0;
            this.panel1.Click += new System.EventHandler(this.Panel1_Click);
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(0, 0);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(720, 360);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            this.axWindowsMediaPlayer1.TabStop = false;
            // 
            // tbStartMinute
            // 
            this.tbStartMinute.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbStartMinute.Location = new System.Drawing.Point(12, 390);
            this.tbStartMinute.Name = "tbStartMinute";
            this.tbStartMinute.Size = new System.Drawing.Size(32, 22);
            this.tbStartMinute.TabIndex = 2;
            this.tbStartMinute.Text = "00";
            this.tbStartMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbStartMinute.Click += new System.EventHandler(this.TbStartMinute_Click);
            // 
            // tbStartSecond
            // 
            this.tbStartSecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbStartSecond.Location = new System.Drawing.Point(50, 390);
            this.tbStartSecond.Name = "tbStartSecond";
            this.tbStartSecond.Size = new System.Drawing.Size(32, 22);
            this.tbStartSecond.TabIndex = 3;
            this.tbStartSecond.Text = "00";
            this.tbStartSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbStartSecond.Click += new System.EventHandler(this.TbStartSecond_Click);
            // 
            // tbStartMillSecond
            // 
            this.tbStartMillSecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbStartMillSecond.Location = new System.Drawing.Point(88, 390);
            this.tbStartMillSecond.Name = "tbStartMillSecond";
            this.tbStartMillSecond.Size = new System.Drawing.Size(32, 22);
            this.tbStartMillSecond.TabIndex = 4;
            this.tbStartMillSecond.Text = "000";
            this.tbStartMillSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbStartMillSecond.Click += new System.EventHandler(this.TbStartMillSecond_Click);
            // 
            // btnPreviewStart
            // 
            this.btnPreviewStart.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnPreviewStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviewStart.Location = new System.Drawing.Point(272, 390);
            this.btnPreviewStart.Name = "btnPreviewStart";
            this.btnPreviewStart.Size = new System.Drawing.Size(75, 23);
            this.btnPreviewStart.TabIndex = 5;
            this.btnPreviewStart.Text = "起始預覽";
            this.btnPreviewStart.UseVisualStyleBackColor = false;
            this.btnPreviewStart.Click += new System.EventHandler(this.btnPreviewStart_Click);
            // 
            // tbToMillSecond
            // 
            this.tbToMillSecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbToMillSecond.Location = new System.Drawing.Point(217, 390);
            this.tbToMillSecond.Name = "tbToMillSecond";
            this.tbToMillSecond.Size = new System.Drawing.Size(32, 22);
            this.tbToMillSecond.TabIndex = 8;
            this.tbToMillSecond.Text = "000";
            this.tbToMillSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbToMillSecond.Click += new System.EventHandler(this.TbToMillSecond_Click);
            // 
            // tbToSecond
            // 
            this.tbToSecond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbToSecond.Location = new System.Drawing.Point(179, 390);
            this.tbToSecond.Name = "tbToSecond";
            this.tbToSecond.Size = new System.Drawing.Size(32, 22);
            this.tbToSecond.TabIndex = 7;
            this.tbToSecond.Text = "00";
            this.tbToSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbToSecond.Click += new System.EventHandler(this.TbToSecond_Click);
            // 
            // tbToMinute
            // 
            this.tbToMinute.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbToMinute.Location = new System.Drawing.Point(141, 390);
            this.tbToMinute.Name = "tbToMinute";
            this.tbToMinute.Size = new System.Drawing.Size(32, 22);
            this.tbToMinute.TabIndex = 6;
            this.tbToMinute.Text = "00";
            this.tbToMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbToMinute.Click += new System.EventHandler(this.TbToMinute_Click);
            // 
            // lbPointer
            // 
            this.lbPointer.ForeColor = System.Drawing.Color.Crimson;
            this.lbPointer.Location = new System.Drawing.Point(11, 415);
            this.lbPointer.Name = "lbPointer";
            this.lbPointer.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lbPointer.Size = new System.Drawing.Size(32, 12);
            this.lbPointer.TabIndex = 9;
            this.lbPointer.Text = "+ -";
            // 
            // btnPreviewEnd
            // 
            this.btnPreviewEnd.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnPreviewEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviewEnd.Location = new System.Drawing.Point(364, 390);
            this.btnPreviewEnd.Name = "btnPreviewEnd";
            this.btnPreviewEnd.Size = new System.Drawing.Size(75, 23);
            this.btnPreviewEnd.TabIndex = 10;
            this.btnPreviewEnd.Text = "終點預覽";
            this.btnPreviewEnd.UseVisualStyleBackColor = false;
            this.btnPreviewEnd.Click += new System.EventHandler(this.BtnPreviewEnd_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Location = new System.Drawing.Point(459, 390);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 11;
            this.btnConfirm.Text = "確定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // FormCutPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 453);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnPreviewEnd);
            this.Controls.Add(this.lbPointer);
            this.Controls.Add(this.tbToMillSecond);
            this.Controls.Add(this.tbToSecond);
            this.Controls.Add(this.tbToMinute);
            this.Controls.Add(this.btnPreviewStart);
            this.Controls.Add(this.tbStartMillSecond);
            this.Controls.Add(this.tbStartSecond);
            this.Controls.Add(this.tbStartMinute);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCutPreview";
            this.Text = "FormCutPreview";
            this.Load += new System.EventHandler(this.FormCutPreview_Load);
            this.Shown += new System.EventHandler(this.FormCutPreview_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCutPreview_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.TextBox tbStartMinute;
        private System.Windows.Forms.TextBox tbStartSecond;
        private System.Windows.Forms.TextBox tbStartMillSecond;
        private System.Windows.Forms.Button btnPreviewStart;
        private System.Windows.Forms.TextBox tbToMillSecond;
        private System.Windows.Forms.TextBox tbToSecond;
        private System.Windows.Forms.TextBox tbToMinute;
        private System.Windows.Forms.Label lbPointer;
        private System.Windows.Forms.Button btnPreviewEnd;
        private System.Windows.Forms.Button btnConfirm;
    }
}