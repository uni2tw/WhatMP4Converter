using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhatMP4Converter.Core;

namespace WhatMP4Converter
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
            MyInitForm();
        }
        private QueueConvertWorkCenter workCenter;
        private AppConf conf;
        RichTextBox logBox;
        private void MyInitForm()
        {
            this.Width = 640;
            this.Height = 320;
            //this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = "MP4 轉檔工具";
            this.listView1.View = View.Details;
            this.listView1.Columns.Add(
                new ColumnHeader { Name = "chFileName", Text = "檔案名稱",  Width = GetWidth(this.listView1, 0.8) });
            
            this.listView1.Columns.Add(
                new ColumnHeader { Name = "chStatus", Text = "狀態", Width = GetWidth(this.listView1, 0.2) });

            this.listView1.Columns.Add(
                new ColumnHeader { Name = "chFullPath", Text = "檔案路徑", Width = 0});

            this.chkRun.CheckedChanged += ChkRun_CheckedChanged;
            this.timer1.Interval = 1000;
            this.timer1.Tick += Timer1_Tick;            

            //Config.FFmpegPath = this.lbFFmpegPath.Text = @"C:\tools\ffmpeg2\bin\";
            Console.WriteLine("form start.");

            workCenter = new QueueConvertWorkCenter();
            conf = AppConf.Reload();
            chkRun.Checked = conf.Auto;
            this.lbOutputPath.Text = conf.Output;
            this.lbFFmpegPath.Text = conf.FFmpeg.Path;
            if (string.IsNullOrEmpty(this.lbFFmpegPath.Text) || Directory.Exists(this.lbFFmpegPath.Text) == false)
            {
                this.lbFFmpegPath.Text = conf.Strings.Undefined;
            }
            if (string.IsNullOrEmpty(this.lbOutputPath.Text) || Directory.Exists(this.lbOutputPath.Text) == false)
            {
                this.lbOutputPath.Text = "同目錄";
            }
            this.timer1.Enabled = this.chkRun.Checked;

            if (conf.Quality.Default.Equals("standard", StringComparison.OrdinalIgnoreCase))
            {
                cbCrf.SelectedIndex = 1;
            }
            else if (conf.Quality.Default.Equals("high", StringComparison.OrdinalIgnoreCase))
            {
                cbCrf.SelectedIndex = 0;
            }
            else if (conf.Quality.Default.Equals("low", StringComparison.OrdinalIgnoreCase))
            {
                cbCrf.SelectedIndex = 2;
            }

            InitLogBox();
        }

        private void InitLogBox()
        {
            logBox = new RichTextBox();
            logBox.Location = new Point(listView1.Left, listView1.Top);
            logBox.Size = new Size(listView1.Width, listView1.Height);
            logBox.Dock = DockStyle.Fill;            
            logBox.WordWrap = false;
            logBox.Visible = false;
            logBox.TextChanged += delegate (object sender, EventArgs e)
            {
                //logBox.SelectionStart = logBox.Text.Length;
                //// scroll it automatically
                //logBox.ScrollToCaret();
            };
            groupBox1.Controls.Add(logBox);
        }



        private void ChkRun_CheckedChanged(object sender, EventArgs e)
        {
            this.timer1.Enabled = this.chkRun.Checked;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (workCenter.AnyRun())
            {
                return;
            }

            this.timer1.Enabled = false;

            foreach (ListViewItem li in this.listView1.Items)
            {
                string status = li.SubItems[1].Text;
                string srcFilePath = li.SubItems[2].Text;
                string fileNameNoExt = Path.GetFileNameWithoutExtension(srcFilePath) + ".mp4";
                string destFlePath;
                if (string.IsNullOrEmpty(conf.Output))
                {
                    destFlePath = Path.Combine(Path.GetDirectoryName(srcFilePath), fileNameNoExt);
                }
                else
                {
                    destFlePath = Path.Combine(conf.Output, fileNameNoExt);
                }

                //if (status == "等候" && workCenter.Exist(srcFilePath)==false && workCenter.AnyRun() == false)
                if (status == "等候" && workCenter.AnyRun() == false)
                {
                    FFmpegQuality crf = (FFmpegQuality)cbCrf.SelectedIndex;
                    Task.Factory.StartNew(delegate ()
                    {
                        QueueConvertWork work = workCenter.StartWork(srcFilePath, destFlePath, conf);
                        work.OnProgress += delegate (bool isStart, bool isFinish, bool result, double progress)
                        {
                            if (isStart)
                            {
                                SafeInvoke(this.listView1, delegate ()
                                {
                                    foreach (ListViewItem item in this.listView1.Items)
                                    {
                                        string filePath = item.SubItems[2].Text;
                                        if (filePath == work.SrcFilePath)
                                        {
                                            item.SubItems[1].Text = "轉換";
                                        }
                                    }
                                });
                                Console.Write("開始 {0}.{1}",
                                    work.SrcFilePath, Environment.NewLine);
                            }
                            else if (isFinish)
                            {
                                SafeInvoke(this.listView1, delegate ()
                                {
                                    foreach (ListViewItem item in this.listView1.Items)
                                    {
                                        string filePath = item.SubItems[2].Text;
                                        if (filePath == work.SrcFilePath)
                                        {
                                            if (result)
                                            {
                                                item.SubItems[1].Text = "完成";                                                
                                            }
                                            else
                                            {
                                                item.SubItems[1].Text = "失敗";
                                            }
                                        }
                                    }
                                });
                                Console.Write("完成 {0}.{1}",
                                    work.SrcFilePath, Environment.NewLine);
                            }
                            else
                            {
                                SafeInvoke(this.listView1, delegate ()
                                {
                                    foreach (ListViewItem item in this.listView1.Items)
                                    {
                                        string filePath = item.SubItems[2].Text;
                                        if (filePath == work.SrcFilePath)
                                        {
                                            item.SubItems[1].Text = string.Format("{0:0.##\\%}", progress * 100);
                                        }
                                    }
                                });
                                Console.Write("轉換 {0} ==> {1}.{2}",
                                    work.SrcFilePath,
                                    string.Format("{0:0.##\\%}", progress * 100),
                                    Environment.NewLine);
                            }
                        };
                        work.OnLog += delegate (string str, LogLevel level)
                        {
                            SafeInvoke(logBox, delegate ()
                            {
                                if (level == LogLevel.Debug)
                                {
                                    logBox.AppendText(str + Environment.NewLine);
                                } else if (level == LogLevel.Info)
                                {
                                    logBox.AppendText(str + Environment.NewLine, Color.Blue);
                                }
                            });
                            
                        };
                        work.Execute(crf);
                    });
                    break;
                }
            }
            this.timer1.Enabled = true;
        }

        public static void SafeInvoke(Control control, Action handler)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(handler);
            }
            else
            {
                handler();
            }
        }

        private int GetWidth(Control form1, double v)
        {
            return (int)(form1.Width * v) - 2;
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    bool allowDrop = true;
            //    if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            //    {
            //        string filename = string.Empty;
            //        Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
            //        if (data != null)
            //        {
            //            if ((data.Length == 1) && (data.GetValue(0) is String))
            //            {
            //                filename = ((string[])data)[0];
            //                Console.WriteLine(filename);
            //                string ext = Path.GetExtension(filename).ToLower();
            //                if (ext != ".mkv" 
            //                    && ext!= ".rmvb"
            //                    && ext != ".wmv"
            //                    && ext != ".mp4"
            //                    && ext != ".avi")
            //                {                                
            //                    allowDrop = false;
            //                }
            //            }
            //        }
            //    }
            //    if (allowDrop)
            //    {
            //        e.Effect = DragDropEffects.All;
            //    }
            //}
            //else
            //{
            //    e.Effect = DragDropEffects.None;
            //}
            e.Effect = DragDropEffects.Copy;
            
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            for (int i = 0; i < s.Length; i++)
            {
                string filePath = s[i];
                if (File.Exists(filePath) == false)
                {
                    continue;
                }
                string fileName= Path.GetFileName(filePath);
                string ext = Path.GetExtension(filePath);
                if (ext != ".mkv"
                    && ext != ".rm"
                    && ext != ".rmvb"
                    && ext != ".wmv"
                    && ext != ".mp4"
                    && ext != ".avi")
                {
                    MessageBox.Show("此格式不支援");
                    return;
                }
                this.listView1.Items.Add(
                    new ListViewItem(new string[] { fileName , "等候", filePath }));
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lbFFmpegPath_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (Directory.Exists(this.lbFFmpegPath.Text))
                {
                    //not working
                    fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                    fbd.SelectedPath = this.lbFFmpegPath.Text;
                }
                DialogResult result = fbd.ShowDialog();
                
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    lbFFmpegPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            conf.FFmpeg.Path = lbFFmpegPath.Text;
            conf.Output = lbOutputPath.Text;
            conf.Auto = chkRun.Checked;
            AppConf.Update(conf);
            workCenter.Shutdown();
        }

        private void lbOutputPath_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (Directory.Exists(this.lbOutputPath.Text))
                {
                    //not working
                    fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                    fbd.SelectedPath = this.lbOutputPath.Text;
                }
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    lbOutputPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (listView1.Visible)
                {
                    listView1.Visible = false;
                    logBox.Visible = true;
                }
                else
                {
                    listView1.Visible = true;
                    logBox.Visible = false;
                }
            }
        }

        private void menuItemQuit_Click(object sender, EventArgs e)
        {
            if (workCenter.AnyRun())
            {
                if (MessageBox.Show("檔案轉換中，確定要中止嗎?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Close();
                }
            } else
            {
                this.Close();
            }            
        }

        private void menuItemSwitchLog_Click(object sender, EventArgs e)
        {
            if (listView1.Visible)
            {
                listView1.Visible = false;
                logBox.Visible = true;
            }
        }

        private void menuItemSwitchMain_Click(object sender, EventArgs e)
        {
            if (listView1.Visible == false)
            {
                listView1.Visible = true;
                logBox.Visible = false;
            }
        }

        private void nenuItemOpenFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "影像檔 (*.*)|*.avi;*.mp4;*.mkv;*.rm;*.rmvb;*.wmv";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(dialog.FileName);
            }
        }
    }
}
