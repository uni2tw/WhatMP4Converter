using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhatMP4Converter.Core
{
    public class FFmpegCutTask : FFmpegTaskBase
    {

        public FFmpegQuality Quality { get; set; }

        public TimeSpan? Duration { get; set; }
        public string VideoWidth { get; set; }
        public string VideoHeight { get; set; }
        public string VideoEncodeType { get; set; }
        public string AudioEncodeType { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan ToTime { get; set; }

        private Regex regexDuration = new Regex(@"Duration: ([\d:.]*),",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexTime = new Regex(@"time=([\d:.]*) bitrate=",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
      
        public FFmpegCutTask(AppConf conf, string taskId) : base(conf, taskId)
        {
        }

        public bool IsPreview { get; set; }
        protected override void DoExecute()
        {
            this.Result = DoInfo();
            if (this.Result)
            {
                this.Result = DoPreview();
            }
        }

        private bool DoInfo()
        {
            FFmpegInfoTask infoTask = new FFmpegInfoTask(this.conf, Guid.NewGuid().ToString());
            infoTask.SrcFilePath = this.SrcFilePath;
            infoTask.Execute();
            if (infoTask.Result == false)
            {
                return false;
            }
            this.Duration = infoTask.Duration;
            this.VideoEncodeType = infoTask.VideoEncodeType;
            this.VideoWidth = infoTask.VideoWidth;
            this.VideoHeight = infoTask.VideoHeight;
            this.AudioEncodeType = infoTask.AudioEncodeType;
            return true;
        }

        private bool DoPreview()
        {
            bool result = true;
            DateTime startTime = DateTime.Now;
            proc = new Process();
            string defaultVideoEncodeParam = GetDefaultVideoEncodeParam(Quality);

                string audioParam;
                if (this.AudioEncodeType.Equals("aac", StringComparison.OrdinalIgnoreCase))
                {
                    audioParam = "-c:a copy";
                    //audioParam = "-c:a aac -b:a 128k -ac 2";
                }
                else
                {
                    audioParam = "-c:a aac -b:a 128k -ac 2";
                }
                string videoParam;
                if (this.VideoEncodeType.Equals("h264", StringComparison.OrdinalIgnoreCase))
                {
                    videoParam = "-c:v copy";
                    //videoParam = defaultVideoEncodeParam;
                }
                else
                {
                    videoParam = defaultVideoEncodeParam;
                }

            //https://stackoverflow.com/questions/14005110/how-to-split-a-video-using-ffmpeg-so-that-each-chunk-starts-with-a-key-frame/33188399#33188399
            string startToParam = string.Format("-ss {0:00}:{1:00}:{2:00}.{3:000} -to {4:00}:{5:00}:{6:00}.{7:000}",
                this.StartTime.Hours,
                this.StartTime.Minutes,
                this.StartTime.Seconds,
                this.StartTime.Milliseconds,
                this.ToTime.Hours,
                this.ToTime.Minutes,
                this.ToTime.Seconds,
                this.ToTime.Milliseconds);

            if (this.IsPreview)
            {
                DestFilePath = Helper.AppendFileName(DestFilePath, DateTime.Now.Ticks.ToString());
            }

            List<string> filters = new List<string>();

            int videoWidth;
            if (conf.Shrink != null && conf.Shrink.Auto && conf.Shrink.Width > 0 &&
                int.TryParse(this.VideoWidth, out videoWidth) && videoWidth > conf.Shrink.Width)
            {
                filters.Add(string.Format("scale={0}:-1", conf.Shrink.Width));
            }

            string filtersParam = string.Empty;
            if (filters.Count > 0)
            {
                filtersParam = string.Format("-vf \"{0}\"", string.Join(",", filters));
                videoParam = defaultVideoEncodeParam;
            }

            string threadsParam = string.Empty;
            if (conf.Threads != null)
            {
                if (conf.Threads.Auto)
                {
                    int recommendThreadCount = (int)(Helper.GetCpuCoreNumber() * 0.75d);
                    if (recommendThreadCount > 0)
                    {
                        threadsParam = string.Format("-threads {0}", recommendThreadCount);
                    }
                }
                else
                {
                    threadsParam = string.Format("-threads {0}", conf.Threads.Number);
                }
            }

            string argument = string.Format(" -y -i \"{0}\" {1} -strict -2 {2} {3} {4} {5} \"{6}\"",
                            SrcFilePath,
                            startToParam,
                            videoParam,
                            audioParam,
                            filtersParam,
                            threadsParam,
                            DestFilePath);
            WriteLog("ffmpeg.exe " + argument, LogLevel.Info);
            proc = FFmpegTaskBase.CreateProc();
            proc.StartInfo.Arguments = argument;
            proc.Start();
            proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                string line = e.Data;
                WriteLog(line, LogLevel.Debug);
                Console.WriteLine(line);
            };
            proc.BeginOutputReadLine();
            proc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                string line = e.Data;
                if (string.IsNullOrEmpty(line))
                {
                    return;
                }

                WriteLog(line, LogLevel.Debug);

                {
                    Match match = regexDuration.Match(line);
                    if (match.Success)
                    {
                        this.Duration = TimeSpan.Parse(match.Groups[1].Value);
                    }
                }
                Console.WriteLine(line);
                if (this.Duration != null)
                {
                    Match match = regexTime.Match(line);
                    if (match.Success)
                    {
                        TimeSpan time = TimeSpan.Parse(match.Groups[1].Value);
                        var progress = time.TotalSeconds / Duration.Value.TotalSeconds;
                        BroadCastProgress(false, false, false, progress, null);
                    }
                }
                if (line == "Conversion failed!" || line.Contains("Invalid argument"))
                {
                    result = false;
                }
            };
            proc.BeginErrorReadLine();

            proc.WaitForExit();
            if (result)
            {
                WriteLog(string.Format("完成: {0}, 費時: {1} 分",
                    Path.GetFileName(this.DestFilePath),
                    (DateTime.Now - startTime).TotalMinutes.ToString("0.#")), LogLevel.Info);
            }
            return result;
        }

        public override PreCheckResult PreCheck(out string confirmMessage)
        {
            confirmMessage = string.Empty;
            if (string.IsNullOrEmpty(DestFilePath))
            {
                return PreCheckResult.Fail;
            }
            if (File.Exists(this.SrcFilePath) == false)
            {
                return PreCheckResult.Fail;
            }
            if (ToTime == TimeSpan.Zero || StartTime > ToTime)
            {
                return PreCheckResult.Fail;
            }
            return PreCheckResult.OK;
        }
    }
}
