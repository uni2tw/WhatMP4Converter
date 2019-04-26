using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            public static int State = 1;
            public static int FullPath = 2;
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
            this.Height = 345;
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

            Console.WriteLine("form start.");

            conf = AppConf.Reload();
            chkRun.Checked = conf.Auto;
            this.lbOutputPath.Text = conf.Output;

            if (string.IsNullOrEmpty(this.lbOutputPath.Text) || Directory.Exists(this.lbOutputPath.Text) == false)
            {
                this.lbOutputPath.Text = "同目錄";
            }
            this.timer1.Enabled = this.chkRun.Checked;

            InitComboBoxs();        

            InitLogBox();

            InitResource();
        }

        private void InitComboBoxs()
        {
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

            if (conf.Shrink.Width == 1920)
            {
                cbShrinkWidth.SelectedIndex = 1;
            }
            else if (conf.Shrink.Width == 1280)
            {
                cbShrinkWidth.SelectedIndex = 2;
            }
            else if (conf.Shrink.Width == 720)
            {
                cbShrinkWidth.SelectedIndex = 3;
            }
            else
            {
                cbShrinkWidth.SelectedIndex = 0;
            }


            cbGainFontSize.DisplayMember = "Text";
            cbGainFontSize.Items.Clear();
            cbGainFontSize.Items.Add(new GainFontSizeDataItem { Text = "-10", Value = -10 });
            cbGainFontSize.Items.Add(new GainFontSizeDataItem { Text = "-5", Value = -5 });
            cbGainFontSize.Items.Add(new GainFontSizeDataItem { Text = "0", Value = 0 });
            cbGainFontSize.Items.Add(new GainFontSizeDataItem { Text = "5", Value = 5 });
            cbGainFontSize.Items.Add(new GainFontSizeDataItem { Text = "10", Value = 10 });
            cbGainFontSize.Items.Add(new GainFontSizeDataItem { Text = "15", Value = 15 });
            cbGainFontSize.Items.Add(new GainFontSizeDataItem { Text = "20", Value = 20 });
            cbGainFontSize.SelectedIndex = 2;
        }

        /// <summary>
        /// 解開嵌入resource的ffmpeg.exe 
        /// </summary>
        private void InitResource()
        {
            string binPath = Helper.GetRelativePath("bin");
            if (Directory.Exists(binPath) == false)
            {
                Directory.CreateDirectory(binPath);
            }
            string ffmpegExePath = Path.Combine(binPath, "ffmpeg.exe");
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
                    
                    FFmpegShrinkWidth shrinkWidth = (FFmpegShrinkWidth)cbShrinkWidth.SelectedIndex;
                    int gainFontSize = ((GainFontSizeDataItem)cbGainFontSize.SelectedItem).Value;
                    var convertTask = new FFmpegConvertTask(this.conf, theTaskId);
                    this.currentTask = convertTask;
                    convertTask.OnProgress += delegate (bool isStart, bool isFinish, bool? isFail, 
                        double progress, string message, string taskId)
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
                                    if (string.IsNullOrEmpty(message) == false)
                                    {
                                        item.SubItems[1].Text += message;
                                    }
                                }
                                else
                                {
                                    item.SubItems[1].Text = string.Format("{0:0.##\\%}", progress * 100);
                                }
                            }
                        });
                    };
                    convertTask.OnLog += delegate (string str, LogLevel level)
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
                    convertTask.SrcFilePath = srcFilePath;
                    convertTask.DestFilePath = destFlePath;
                    convertTask.Quality = crf;
                    convertTask.ShrinkVideoWidth = shrinkWidth;
                    convertTask.GainFontSize = gainFontSize;
                    var asyncTask = Task.Factory.StartNew(delegate ()
                    {
                        convertTask.Execute();
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
                        task.OnProgress += delegate (bool isStart, bool isFinish, bool? isFail, double progress,
                            string message, string taskId)
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
                else if (mode == OperatingMode.Cut)
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
                        item.SubItems[LiveViewColumns.State].Text = "設定";
                        ShowCutPreviewform(theTaskId, filePath);
                    }
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
                    MessageBox.Show(ext +" 格式不支援");
                    return;
                }
                if (this.Mode == OperatingMode.Merge
                    && ext != ".mp4")
                {
                    MessageBox.Show("合併模式只支援MP4");
                    return;
                }
                if (this.Mode == OperatingMode.Cut                    )
                {
                    if (ext != ".mp4")
                    {
                        MessageBox.Show("栽切模式只支援MP4");
                        return;
                    }
                    if (files.Count > 1)
                    {
                        MessageBox.Show("栽切模式一次只處理一個影片檔");
                    }
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

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
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

        private void ShowCutPreviewform(string taskId, string srcFilePath)
        {
            //string temp = Path.GetFullPath("./");
            CutPreviewForm previewForm = new CutPreviewForm();
            previewForm.conf = this.conf;
            previewForm.Text = Path.GetFileName(srcFilePath);
            previewForm.SrcFilePath = srcFilePath;
            previewForm.ShowDialog();
            //MessageBox.Show(previewForm.TotTime.ToString());

            FFmpegCutTask task = new FFmpegCutTask(this.conf, taskId);
            task.StartTime = previewForm.StartTime;
            task.ToTime = previewForm.endTime;
            task.SrcFilePath = srcFilePath;
            string renameToMP4 = Path.GetFileNameWithoutExtension(srcFilePath) + ".mp4";             
            task.DestFilePath = Path.Combine(conf.Output, renameToMP4);
            task.IsPreview = false;
            task.Execute();



            previewForm.Dispose();
        }

        private void ShowExtractAssForm()
        {
            ExtractAssForm form = new ExtractAssForm();
            form.ShowDialog();
        }

        private void CmiOpenFolder_Click(object sender, EventArgs e)
        {
            var item = listView1.SelectedItems[LiveViewColumns.FullPath];
            Process.Start(new ProcessStartInfo
            {
                 FileName = "explorer",
                 Arguments = "/select," + item.Text
            });
        }

        private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                listView1.ContextMenuStrip = null;
            }
            else
            {
                listView1.ContextMenuStrip = contextMenuStripFileTree;
            }
        }

        private void MenuItemExtractAss_Click(object sender, EventArgs e)
        {
            ShowExtractAssForm();
        }
    }
}
