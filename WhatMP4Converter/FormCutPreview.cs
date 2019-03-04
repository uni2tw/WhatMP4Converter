using System;
using System.IO;
using System.Windows.Forms;
using WhatMP4Converter.Core;

namespace WhatMP4Converter
{
    //https://stackoverflow.com/questions/17461670/play-video-without-using-media-player-winform
    public partial class FormCutPreview : Form
    {
        int pointerPos = 1;
        public string SrcFilePath;
        public TimeSpan StartTime { get; set; }
        public TimeSpan TotTime { get; set; }

        public bool Result { get; set; }

        public AppConf conf;
        public FormCutPreview()
        {
            InitializeComponent();

        }

        private void FormCutPreview_Load(object sender, EventArgs e)
        {
            if (File.Exists(this.SrcFilePath) == false || Path.GetExtension(this.SrcFilePath).ToLower() != ".mp4")
            {
                Result = false;
                Close();
            }
        }

        public void Play()
        {




        }

        private void FormCutPreview_Shown(object sender, EventArgs e)
        {
            InitForm();
            RenderPointer();
        }

        private void InitForm()
        {

            FFmpegInfoTask task = new FFmpegInfoTask(
                this.conf,
                Guid.NewGuid().ToString());
            task.SrcFilePath = this.SrcFilePath;
            task.Execute();

            tbToMinute.Text = (task.Duration.Value.Hours * 60 + 
                task.Duration.Value.Minutes).ToString("00");
            tbToSecond.Text = task.Duration.Value.Seconds.ToString("00");
            tbToMillSecond.Text = task.Duration.Value.Milliseconds.ToString().PadLeft(3, '0');
        }

        private void Panel1_Click(object sender, EventArgs e)
        {






        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox1_Click(object sender, EventArgs e)
        {
        }

        private void BtnUltrafastCutToPreview_Click(object sender, EventArgs e)
        {

        }

        private TimeSpan GetToTime()
        {
            int totalMinutes = int.Parse(tbToMinute.Text);
            int totalSeconds = int.Parse(tbToSecond.Text);
            int totalMillSeconds = int.Parse(tbToMillSecond.Text);
            return new TimeSpan(0, totalMinutes / 60, totalMinutes % 60, totalSeconds, totalMillSeconds);            
        }

        private TimeSpan GetStartTime()
        {
            int totalMinutes = int.Parse(tbStartMinute.Text);
            int totalSeconds = int.Parse(tbStartSecond.Text);
            int totalMillSeconds = int.Parse(tbStartMillSecond.Text);
            return new TimeSpan(0, totalMinutes / 60, totalMinutes % 60, totalSeconds, totalMillSeconds);
        }

        private void RenderPointer()
        {
            if (pointerPos == 0)
            {
                lbPointer.Left = tbStartMinute.Left - 1;
                tbStartMinute.Focus();
                tbStartMinute.SelectAll();
            }
            else if (pointerPos == 1)
            {
                lbPointer.Left = tbStartSecond.Left - 1;
                tbStartSecond.Focus();
                tbStartSecond.SelectAll();
            }
            else if (pointerPos == 2)
            {
                lbPointer.Left = tbStartMillSecond.Left - 1;
                tbStartMillSecond.Focus();
                tbStartMillSecond.SelectAll();
            }
            if (pointerPos == 3)
            {
                lbPointer.Left = tbToMinute.Left - 1;
                tbToMinute.Focus();
                tbToMinute.SelectAll();
            }
            else if (pointerPos == 4)
            {
                lbPointer.Left = tbToSecond.Left - 1;
                tbToSecond.Focus();
                tbToSecond.SelectAll();
            }
            else if (pointerPos == 5)
            {
                lbPointer.Left = tbToMillSecond.Left - 1;
                tbToMillSecond.Focus();
                tbToMillSecond.SelectAll();
            }
        }

        private void FormCutPreview_KeyDown(object sender, KeyEventArgs e)
        {
            bool handled = false;
            bool pressCtrl = e.Modifiers.HasFlag(Keys.Control);
            if (e.KeyCode == Keys.Left && this.pointerPos > 0)
            {
                handled = true;
                this.pointerPos--;
            }
            if (e.KeyCode == Keys.Right && this.pointerPos < 5)
            {
                handled = true;
                this.pointerPos++;
            }

            if (e.KeyCode == Keys.Space)
            {
                handled = true;
                if (pointerPos >= 0 && pointerPos <=3)
                {
                    btnPreviewStart.PerformClick();
                } else
                {
                    btnPreviewEnd.PerformClick();
                }
            }

            if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.Up)
            {
                handled = true;
                if (pointerPos == 0)
                {
                    int val;
                    if (int.TryParse(tbStartMinute.Text, out val))
                    {
                        if (pressCtrl)
                        {
                            val -= 10;
                        }
                        else
                        {
                            val--;
                        }
                        if (val < 0)
                        {
                            val = 0;
                        }
                        tbStartMinute.Text = val.ToString("00");
                    }
                }
                if (pointerPos == 1)
                {
                    int val;
                    if (int.TryParse(tbStartSecond.Text, out val))
                    {
                        if (pressCtrl)
                        {
                            val -= 10;
                        }
                        else
                        {
                            val--;
                        }
                        if (val < 0)
                        {
                            val = 0;
                        }
                        tbStartSecond.Text = val.ToString("00");
                    }
                }
                if (pointerPos == 2)
                {
                    int val;
                    if (int.TryParse(tbStartMillSecond.Text, out val))
                    {
                        if (pressCtrl)
                        {
                            val -= 10;
                        }
                        else
                        {
                            val--;
                        }
                        if (val < 0)
                        {
                            val = 0;
                        }
                        tbStartMillSecond.Text = val.ToString("000");
                    }
                }
                if (pointerPos == 3)
                {
                    int val;
                    if (int.TryParse(tbToMinute.Text, out val))
                    {
                        if (pressCtrl)
                        {
                            val -= 10;
                        }
                        else
                        {
                            val--;
                        }
                        if (val < 0)
                        {
                            val = 0;
                        }
                        tbToMinute.Text = val.ToString("00");
                    }
                }
                if (pointerPos == 4)
                {
                    int val;
                    if (int.TryParse(tbToSecond.Text, out val))
                    {
                        if (pressCtrl)
                        {
                            val -= 10;
                        }
                        else
                        {
                            val--;
                        }
                        if (val < 0)
                        {
                            val = 0;
                        }
                        tbToSecond.Text = val.ToString("00");
                    }
                }
                if (pointerPos == 5)
                {
                    int val;
                    if (int.TryParse(tbToMillSecond.Text, out val))
                    {
                        if (pressCtrl)
                        {
                            val -= 10;
                        }
                        else
                        {
                            val--;
                        }
                        if (val < 0)
                        {
                            val = 0;
                        }
                        tbToMillSecond.Text = val.ToString("000");
                    }
                }
            }


            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Down)
            {
                handled = true;
                if (pointerPos == 0)
                {
                    int val;
                    if (int.TryParse(tbStartMinute.Text, out val) == false)
                    {
                        val = 0;
                    }
                    if (pressCtrl)
                    {
                        val += 10;
                    }
                    else
                    {
                        val++;
                    }
                    if (val > 199)
                    {
                        val = 199;
                    }
                    tbStartMinute.Text = val.ToString("00");
                }
                if (pointerPos == 1)
                {
                    int val;
                    if (int.TryParse(tbStartSecond.Text, out val) == false)
                    {
                        val = 0;
                    }
                    if (pressCtrl)
                    {
                        val += 10;
                    }
                    else
                    {
                        val++;
                    }
                    if (val > 59)
                    {
                        val = 59;
                    }
                    tbStartSecond.Text = val.ToString("00");
                }
                if (pointerPos == 2)
                {
                    int val;
                    if (int.TryParse(tbStartMillSecond.Text, out val) == false)
                    {
                        val = 0;
                    }
                    if (pressCtrl)
                    {
                        val += 10;
                    }
                    else
                    {
                        val++;
                    }
                    if (val > 999)
                    {
                        val = 999;
                    }
                    tbStartMillSecond.Text = val.ToString("000");
                }
                if (pointerPos == 3)
                {
                    int val;
                    if (int.TryParse(tbToMinute.Text, out val))
                    {
                        val = 0;
                    }
                    if (pressCtrl)
                    {
                        val += 10;
                    }
                    else
                    {
                        val++;
                    }
                    if (val > 199)
                    {
                        val = 199;
                    }
                    tbToMinute.Text = val.ToString("00");
                }
                if (pointerPos == 4)
                {
                    int val;
                    if (int.TryParse(tbToSecond.Text, out val) == false)
                    {
                        val = 0;
                    }
                    if (pressCtrl)
                    {
                        val += 10;
                    }
                    else
                    {
                        val++;
                    }
                    if (val > 59)
                    {
                        val = 59;
                    }
                    tbToSecond.Text = val.ToString("00");

                }
                if (pointerPos == 5)
                {
                    int val;
                    if (int.TryParse(tbToMillSecond.Text, out val) == false)
                    {
                        val = 0;
                    }
                    if (pressCtrl)
                    {
                        val += 10;
                    }
                    else
                    {
                        val++;
                    }
                    if (val > 999)
                    {
                        val = 999;
                    }
                    tbToMillSecond.Text = val.ToString("000");
                }
            }
   
            if (handled == false)
            {
                e.Handled = false;                
                return;
            }
            RenderPointer();
            //e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void btnPreviewStart_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            FFmpegCutTask task = new FFmpegCutTask(this.conf, "test");
            task.SrcFilePath = this.SrcFilePath;
            string temp = Helper.GetRelativePath("temp");
            if (Directory.Exists(temp) == false)
            {
                Directory.CreateDirectory(temp);
            }
            task.DestFilePath = Path.Combine(temp, Path.GetFileName(this.SrcFilePath));
            task.IsPreview = true;
            task.StartTime = GetStartTime();
            task.ToTime = GetToTime();
            if ((task.ToTime - task.StartTime).TotalSeconds > 5)
            {
                task.ToTime = task.StartTime.Add(TimeSpan.FromSeconds(5));
            }
            task.Execute();
            //axWindowsMediaPlayer1.URL = task.DestFilePath;
            axWindowsMediaPlayer1.URL = task.DestFilePath;            
            //@"t:\01.mp4";
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void BtnPreviewEnd_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            FFmpegCutTask task = new FFmpegCutTask(this.conf, "test");
            task.SrcFilePath = this.SrcFilePath;
            string temp = Helper.GetRelativePath("temp");
            if (Directory.Exists(temp) == false)
            {
                Directory.CreateDirectory(temp);
            }
            task.DestFilePath = Path.Combine(temp, Path.GetFileName(this.SrcFilePath));
            task.IsPreview = true;
            task.StartTime = GetStartTime();
            task.ToTime = GetToTime();
            if ((task.ToTime - task.StartTime).TotalSeconds > 5)
            {
                task.StartTime = task.ToTime.Subtract(TimeSpan.FromSeconds(5));
            }
            task.Execute();
            //axWindowsMediaPlayer1.URL = task.DestFilePath;
            axWindowsMediaPlayer1.URL = task.DestFilePath;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 3;
            //@"t:\01.mp4";
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void TbToSecond_Click(object sender, EventArgs e)
        {
            pointerPos = 4;
            RenderPointer();
        }
        private void TbToMillSecond_Click(object sender, EventArgs e)
        {
            pointerPos = 5;
            RenderPointer();
        }

        private void TbStartMinute_Click(object sender, EventArgs e)
        {
            pointerPos = 0;
            RenderPointer();
        }

        private void TbStartSecond_Click(object sender, EventArgs e)
        {
            pointerPos = 1;
            RenderPointer();
        }

        private void TbStartMillSecond_Click(object sender, EventArgs e)
        {
            pointerPos = 2;
            RenderPointer();
        }

        private void TbToMinute_Click(object sender, EventArgs e)
        {
            pointerPos = 3;
            RenderPointer();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            this.StartTime = GetStartTime();
            this.TotTime = GetToTime();
            this.Result = true;
            this.Close();
        }
    }
}
