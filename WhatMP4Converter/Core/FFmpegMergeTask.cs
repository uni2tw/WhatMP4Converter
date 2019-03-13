using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WhatMP4Converter.Core
{
    public class FFmpegMergeTask : FFmpegTaskBase
    {
        Regex regexSize = new Regex(@"size=[ ]+([\d]+)[kbKB]+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public List<string> InputFiles { get; set; }

        public FFmpegMergeTask(AppConf conf, string taskId) : base(conf, taskId)
        {
            
        }

        protected override void DoExecute()
        {
            CreateListFileByInputFiles();
            DoMerge();
            DeleteListFile();
        }

        private void CreateListFileByInputFiles()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in this.InputFiles)
            {
                sb.AppendFormat("file '{0}'", line.Replace("\\", "\\\\"));
                sb.AppendLine();
            }

            this.SrcFilePath = Helper.GetRelativePath(Guid.NewGuid() + ".txt");
            string mergedFilePath = Path.GetFileNameWithoutExtension(this.InputFiles[0])
                +".merged"
                + Path.GetExtension(this.InputFiles[0]);
            this.DestFilePath = Path.Combine(conf.Output, mergedFilePath);

            File.WriteAllText(this.SrcFilePath, sb.ToString());
            
        }

        private void DeleteListFile()
        {
            if (File.Exists(this.SrcFilePath))
            {
                File.Delete(this.SrcFilePath);
            }
        }

        private void DoMerge()
        {
            //create merged file list
            proc = FFmpegMergeTask.CreateProc();
            string argument = string.Format("-y -f concat -safe 0 -i {0} -c copy {1}",
                            this.SrcFilePath, this.DestFilePath);
            WriteLog("ffmpeg.exe " + argument, LogLevel.Info);
            proc.StartInfo.Arguments = argument;
            proc.Start();
            /*
                -y -f concat -safe 0 -i c:\temp\1.txt -c copy c:\temp\output.mp4
             */
            proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                string line = e.Data;
                if (string.IsNullOrEmpty(line))
                {
                    return;
                }
                WriteLog(line, LogLevel.Debug);
            };
            proc.BeginOutputReadLine();
            proc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                string line = e.Data;
                if (string.IsNullOrEmpty(line))
                {
                    return;
                }
                WriteLog(line, LogLevel.Debug);

                Match match = regexSize.Match(line);
                if (match.Success)
                {
                    int grandTotalFileSize = int.Parse(match.Groups[1].Value);
                    long totalFileSize = 0;
                    foreach(var inputFile in this.InputFiles)
                    {
                        totalFileSize += new FileInfo(inputFile).Length / 1024;
                    }
                    double progress = grandTotalFileSize / (double)totalFileSize;
                    BroadCastProgress(false, false, null, progress);
                }

                if (line == "Conversion failed!" || line.Contains("Invalid argument"))
                {
                    Result = false;
                }
            };
            proc.BeginErrorReadLine();

            proc.WaitForExit();
        }

        public override PreCheckResult PreCheck(out string confirmMessage)
        {
            confirmMessage = string.Empty;
            if (InputFiles == null || InputFiles.Count == 0)
            {
                return PreCheckResult.Fail;
            }

            return PreCheckResult.OK;
        }
    }
}
