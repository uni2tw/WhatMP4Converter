using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhatMP4Converter.Core;

namespace WhatMP4Converter
{
    public partial class formMain : Form
    {
        public class LiveViewColumns
        {
            public static int TaskId = 3;
            public static int Mode = 4;
            public static int InTask = 5;
        }
        public formMain()
        {
            InitializeComponent();
            MyInitForm();
        }

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

            this.listView1.Columns.Add(
                new ColumnHeader { Name = "chTaskd", Text = "工作ID", Width = 0 });

            this.listView1.Columns.Add(
                new ColumnHeader { Name = "chMode", Text = "工作類型", Width = 0 });

            this.listView1.Columns.Add(
                new ColumnHeader { Name = "chInTask", Text = "是曾在工作佇列", Width = 0 });

            this.chkRun.CheckedChanged += ChkRun_CheckedChanged;
            this.timer1.Interval = 1000;
            this.timer1.Tick += Timer1_Tick;            

            //Config.FFmpegPath = this.lbFFmpegPath.Text = @"C:\tools\ffmpeg2\bin\";
            Console.WriteLine("form start.");

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

        /// <summary>
        /// 解開嵌入resource的ffmpeg.exe 
        /// </summary>
        private void InitResource()
        {
            string ffmpegExePath = Helper.GetRelativePath("ffmpeg.exe");
            if (File.Exists(ffmpegExePath) == false)
            {
                using (FileStream fs = new FileStream(ffmpegExePath, FileMode.CreateNew, FileAccess.Write))
                {
                    Stream resource = Assembly.GetEntryAssembly().GetManifestResourceStream("WhatMP4Converter.assets.ffmpeg.exe");
                    resource.CopyTo(fs);
                }
            }
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


        public OperatingMode Mode
        {
            get
            {
                return (OperatingMode)tabControl1.SelectedIndex;
            }
        }

        private FFmpegTaskBase currentTask;

        private void ChkRun_CheckedChanged(object sender, EventArgs e)
        {
            this.timer1.Enabled = this.chkRun.Checked;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;

            try
            {
                if (this.currentTask != null)
                {
                    return;
                }
                string theTaskId = null;
                OperatingMode mode = OperatingMode.None;
                foreach (ListViewItem item in this.listView1.Items)
                {
                    string inTask = item.SubItems[LiveViewColumns.InTask].Text;
                    if (inTask == "0")
                    {
                        theTaskId = item.SubItems[3].Text;
                        mode = (OperatingMode)Enum.Parse(typeof(OperatingMode), item.SubItems[LiveViewColumns.Mode].Text);
                        break;
                    }
                }
                if (theTaskId == null)
                {
                    //MessageBox.Show("沒有需要合併的MP4");                
                    return;
                }

                if (mode == OperatingMode.Convert)
                {
                    string srcFilePath = null;
                    string destFlePath = null;
                    foreach (ListViewItem item in this.listView1.Items)
                    {
                        string itemTaskId = item.SubItems[LiveViewColumns.TaskId].Text;
                        if (itemTaskId != theTaskId)
                        {
                            continue;
                        }
                        srcFilePath = item.SubItems[2].Text;
                        string renameToMP4 = Path.GetFileNameWithoutExtension(srcFilePath) + ".mp4";
                        if (string.IsNullOrEmpty(conf.Output))
                        {
                            destFlePath = Path.Combine(Path.GetDirectoryName(srcFilePath), renameToMP4);
                        }
                        else
                        {
                            destFlePath = Path.Combine(conf.Output, renameToMP4);
                        }
                        item.SubItems[LiveViewColumns.InTask].Text = "1";
                        break;
                    }
                    FFmpegQuality crf = (FFmpegQuality)cbCrf.SelectedIndex;
                    var asyncTask = Task.Factory.StartNew(delegate ()
                    {                        
                        var task = new FFmpegConvertTask(this.conf, theTaskId);
                        this.currentTask = task;
                        task.OnProgress += delegate (bool isStart, bool isFinish, bool? isFail, double progress, string taskId)
                        {
                            SafeInvoke(listView1, delegate ()
                            {
                                foreach (ListViewItem item in this.listView1.Items)
                                {
                                    string itemTsakId = item.SubItems[3].Text;
                                    if (itemTsakId != taskId)
                                    {
                                        continue;
                                    }
                                    if (isStart)
                                    {
                                        item.SubItems[1].Text = "轉換";
                                    }
                                    else if (isFinish && isFail == true)
                                    {
                                        item.SubItems[1].Text = "完成";
                                    }
                                    else if (isFinish && isFail == false)
                                    {
                                        item.SubItems[1].Text = "失敗";
                                    }
                                    else
                                    {
                                        item.SubItems[1].Text = string.Format("{0:0.##\\%}", progress * 100);
                                    }
                                }
                            });
                        };
                        task.OnLog += delegate (string str, LogLevel level)
                        {
                            SafeInvoke(logBox, delegate ()
                            {
                                if (level == LogLevel.Debug)
                                {
                                    logBox.AppendText(str + Environment.NewLine);
                                }
                                else if (level == LogLevel.Info)
                                {
                                    logBox.AppendText(str + Environment.NewLine, Color.Blue);
                                }
                            });

                        };
                        task.SrcFilePath = srcFilePath;
                        task.DestFilePath = destFlePath;
                        task.Quality = crf;
                        task.Execute();
                    });
                    asyncTask.ContinueWith(delegate (Task t)
                    {
                        this.currentTask = null;
                    });
                }
                else if (mode == OperatingMode.Merge)
                {
                    List<string> inputFiles = new List<string>();
                    foreach (ListViewItem item in this.listView1.Items)
                    {
                        string itemTaskId = item.SubItems[LiveViewColumns.TaskId].Text;
                        if (itemTaskId != theTaskId)
                        {
                            continue;
                        }
                        string filePath = item.SubItems[2].Text;
                        inputFiles.Add(filePath);
                        item.SubItems[LiveViewColumns.InTask].Text = "1";
                    }

                    var asyncTask = Task.Factory.StartNew(delegate ()
                    {
                        var task = new FFmpegMergeTask(this.conf, theTaskId);
                        this.currentTask = task;
                        task.OnProgress += delegate (bool isStart, bool isFinish, bool? isFail, double progress, string taskId)
                        {
                            SafeInvoke(listView1, delegate ()
                            {
                                foreach (ListViewItem item in this.listView1.Items)
                                {
                                    string itemTsakId = item.SubItems[3].Text;
                                    if (itemTsakId != taskId)
                                    {
                                        continue;
                                    }
                                    if (isStart)
                                    {
                                        item.SubItems[1].Text = "開始合併";
                                    }
                                    else if (isFinish && isFail == true)
                                    {
                                        item.SubItems[1].Text = "合併完成";
                                    }
                                    else if (isFinish && isFail == false)
                                    {
                                        item.SubItems[1].Text = "合併失敗";
                                    }
                                    else
                                    {
                                        item.SubItems[1].Text = string.Format("{0:0.##\\%}", progress * 100);
                                    }
                                }
                            });
                        };

                        task.OnLog += delegate (string str, LogLevel level)
                        {
                            SafeInvoke(logBox, delegate ()
                            {
                                if (level == LogLevel.Debug)
                                {
                                    logBox.Invoke((MethodInvoker)(
                                        () => logBox.AppendText(str + Environment.NewLine)));
                                }
                                else if (level == LogLevel.Info)
                                {
                                    logBox.Invoke((MethodInvoker)(
                                        () => logBox.AppendText(str + Environment.NewLine, Color.Blue)));
                                }
                            });
                        };

                        task.InputFiles = inputFiles;

                        task.Execute();
                    });
                    asyncTask.ContinueWith(delegate (Task t)
                    {
                        this.currentTask = null;
                    });
                }
            }
            finally
            {
                this.timer1.Enabled = true;
            }

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
            if (e.Data.GetDataPresent(DataFormats.FileDrop) == false)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            
            string[] filePaths = (string[])((IDataObject)e.Data).GetData(DataFormats.FileDrop);
            bool allowDrop = true;
            foreach (string filePath in filePaths)
            {
                string ext = Path.GetExtension(filePath).ToLower();
                if (ext != ".mkv"
                                && ext != ".rm"
                                && ext != ".rmvb"
                                && ext != ".wmv"
                                && ext != ".mp4"
                                && ext != ".mpg"
                                && ext != ".mpeg"
                                && ext != ".avi")
                {
                    allowDrop = false;
                    break;
                }
            }

            if (allowDrop)
            {
                e.Effect = DragDropEffects.Copy;
            } else
            {
                e.Effect = DragDropEffects.None;
            }            
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string taskGroupId = Guid.NewGuid().ToString();
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            List<string> files = new List<string>(s);
            files.Sort();
            for (int i = 0; i < files.Count; i++)
            {
                string filePath = files[i];
                if (File.Exists(filePath) == false)
                {
                    continue;
                }
                string fileName= Path.GetFileName(filePath);
                string ext = Path.GetExtension(filePath);
                if (this.Mode== OperatingMode.Convert 
                    && ext != ".mkv"
                    && ext != ".rm"
                    && ext != ".rmvb"
                    && ext != ".wmv"
                    && ext != ".mp4"
                    && ext != ".mpg"
                    && ext != ".mpeg"
                    && ext != ".avi")
                {
                    MessageBox.Show("此格式不支援");
                    return;
                }
                if (this.Mode == OperatingMode.Merge
                    && ext != ".mp4")
                {
                    MessageBox.Show("合併模式只支援MP4");
                    return;
                }
                this.listView1.Items.Add(
                    new ListViewItem(new string[] {
                        fileName, "等候", filePath, taskGroupId, this.Mode.ToString(), "0" }));
                if (this.Mode == OperatingMode.Convert)
                {
                    taskGroupId = Guid.NewGuid().ToString();
                }
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
            this.Close();       
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

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.currentTask != null)
            {
                if (MessageBox.Show("檔案轉換中，確定要中止嗎?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.currentTask.Stop();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FormCutPreview previewForm = new FormCutPreview();
            previewForm.conf = this.conf;
            previewForm.ShowDialog();
            previewForm.Dispose();
        }
    }
}
