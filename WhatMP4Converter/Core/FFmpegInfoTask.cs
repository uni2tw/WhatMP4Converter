using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace WhatMP4Converter.Core
{
    public class FFmpegInfoTask : FFmpegTaskBase
    {

        public TimeSpan? Duration { get; set; }
        public string VideoWidth { get; set; }
        public string VideoHeight { get; set; }
        public string VideoEncodeType { get; set; }
        public string AudioEncodeType { get; set; }

        private Regex regexDuration = new Regex(@"Duration: ([\d:.]*),",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexTime = new Regex(@"time=([\d:.]*) bitrate=",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexAudio = new Regex(@"Audio: ([\w]+)[ ,]",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexVideo = new Regex(@"Video: ([\w]+)[\w \/,()]+, ([\d]{2,4})x([\d]{2,4})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public FFmpegInfoTask(AppConf conf, string taskId) : base(conf, taskId)
        {
        }

        protected override void DoExecute()
        {
            this.Result = DoInfo();
        }

        protected override bool PreCheck()
        {
            if (File.Exists(this.SrcFilePath)== false)
            {
                return false;
            }
            return true;
        }

        private bool DoInfo()
        {
            bool result = true;
            string argument = string.Format(" -i \"{0}\" -hide_banner",
                            SrcFilePath);
            WriteLog("ffmpeg.exe " + argument, LogLevel.Info);
            proc = FFmpegTaskBase.CreateProc(conf.FFmpeg.Path);
            proc.StartInfo.Arguments = argument;
            proc.Start();
            proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                string line = e.Data;
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
                {
                    Match match = regexDuration.Match(line);
                    if (match.Success)
                    {
                        this.Duration = TimeSpan.Parse(match.Groups[1].Value);
                    }
                }
                if (string.IsNullOrEmpty(this.AudioEncodeType))
                {
                    Match match = regexAudio.Match(line);
                    if (match.Success)
                    {
                        this.AudioEncodeType = match.Groups[1].Value.ToLower();
                        WriteLog(string.Format("Audio: {0}", this.AudioEncodeType), LogLevel.Info);
                    }
                }
                if (string.IsNullOrEmpty(this.VideoEncodeType))
                {
                    Match match = regexVideo.Match(line);
                    if (match.Success)
                    {
                        this.VideoEncodeType = match.Groups[1].Value.ToLower();
                        this.VideoWidth = match.Groups[2].Value;
                        this.VideoHeight = match.Groups[3].Value;

                        WriteLog(string.Format("Video: {0} {1}x{2}",
                            this.VideoEncodeType, this.VideoWidth, this.VideoHeight), LogLevel.Info);
                    }
                }
                Console.WriteLine(line);
            };
            proc.BeginErrorReadLine();

            proc.WaitForExit();

            if (string.IsNullOrEmpty(this.VideoEncodeType) ||
                string.IsNullOrEmpty(this.AudioEncodeType))
            {
                result = false;
            }

            return result;
        }
    }
}
