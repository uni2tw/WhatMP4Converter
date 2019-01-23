using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace WhatMP4Converter.Core
{
    public class QueueConvertWork
    {
        private Process proc;

        public AppConf conf;

        public string SrcFilePath { get; set; }
        public string DestFilePath { get; set; }
        //private delegate void MyDelegate();
        public delegate void ProgressHandler(bool IsStart, bool IsFinish, bool fail, double Progress);
        public event ProgressHandler OnProgress;
        public delegate void LogHandler(string str, LogLevel level);
        public event LogHandler OnLog;
        public bool IsClosed { get; set; }
        public bool IsFail { get; set; }
        public TimeSpan? Duration { get; set; }
        public string VideoWidth { get; set; }
        public string VideoHeight { get; set; }
        public string VideoEncodeType { get; set; }
        public string AudioEncodeType { get; set; }

        public string AssFilePath { get; set; }

        private Regex regexDuration = new Regex(@"Duration: ([\d:.]*),", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexTime = new Regex(@"time=([\d:.]*) bitrate=",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexAudio = new Regex(@"Audio: ([\w]+)[ ,]",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexVideo = new Regex(@"Video: ([\w]+) [\w /,()]+, ([\d]{2,4})x([\d]{2,4})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public QueueConvertWork(string srcFilePath, string destFilePath, AppConf conf)
        {            
            this.SrcFilePath = srcFilePath;
            this.DestFilePath = destFilePath;
            this.conf = conf;
        }

        public void Execute()
        {
            if (OnProgress != null)
            {
                OnProgress(true, false, false, 0);
            }
            bool result = false;
            result = Info();
            if (result)
            {
                result = Convert();
            }
            if (OnProgress != null)
            {
                OnProgress(false, true, result, 100);
            }
            IsClosed = true;
        }

        private bool Info()
        {
            bool result = true;
            proc = new Process();
            string argument = string.Format(" -i \"{0}\" -hide_banner",
                            SrcFilePath);
            OnLog("ffmpeg.exe " + argument, LogLevel.Info);
            proc.StartInfo.FileName = Path.Combine(conf.FFmpeg.Path, "ffmpeg.exe");
            proc.StartInfo.Arguments = argument;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                string line = e.Data;
                if (OnLog != null)
                {
                    OnLog(line, LogLevel.Debug);
                }
                Console.WriteLine(line);
            };
            proc.BeginOutputReadLine();
            proc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                string line = e.Data;
                if (string.IsNullOrEmpty(line))
                {
                    return;
                }
                if (OnLog != null)
                {
                    OnLog(line, LogLevel.Debug);
                }
                if (string.IsNullOrEmpty(this.AudioEncodeType))
                {
                    Match match = regexAudio.Match(line);
                    if (match.Success)
                    {
                        this.AudioEncodeType = match.Groups[1].Value;
                        OnLog(string.Format("Audio: {0}", this.AudioEncodeType), LogLevel.Info);
                    }
                }
                if (string.IsNullOrEmpty(this.VideoEncodeType))
                {
                    Match match = regexVideo.Match(line);
                    if (match.Success)
                    {
                        this.VideoEncodeType = match.Groups[1].Value;
                        this.VideoWidth = match.Groups[2].Value;
                        this.VideoHeight = match.Groups[3].Value;
                        if (OnLog != null)
                        {
                            OnLog(string.Format("Video: {0} {1}x{2}",
                                this.VideoEncodeType, this.VideoWidth, this.VideoHeight), LogLevel.Info);
                        }
                    }
                }
                Console.WriteLine(line);
            };
            proc.BeginErrorReadLine();

            proc.WaitForExit();


            string ext = Path.GetExtension(this.SrcFilePath);
            this.AssFilePath = this.SrcFilePath.Replace(ext, string.Empty) + ".ass";
            if (File.Exists(this.AssFilePath) == false)
            {
                this.AssFilePath = null;
            }
            else
            {
                OnLog("合併字幕: " + Path.GetFileName(this.AssFilePath), LogLevel.Info);
            }

            if (string.IsNullOrEmpty(this.VideoEncodeType) ||
                string.IsNullOrEmpty(this.AudioEncodeType))
            {
                result = false;
            }
            
            return result;
        }

        public bool Convert()
        {
            bool result = true;
            proc = new Process();

            string audioParam;
            if (this.AudioEncodeType.Equals("aac", StringComparison.OrdinalIgnoreCase)) {
                audioParam = "-c:a copy";
            } else
            {
                audioParam = "-c:a aac -b:a 128k -ac 2";
            }
            string videoParam;
            if (this.VideoEncodeType.Equals("h264", StringComparison.OrdinalIgnoreCase))
            {
                videoParam = "-c:v copy";
            }
            else
            {
                videoParam = "-c:v libx264 -crf 18";
            }

            List<string> filters = new List<string>();
            
            if (string.IsNullOrEmpty(this.AssFilePath)==false && File.Exists(this.AssFilePath))
            {
                string assFileName = Path.GetFileName(this.AssFilePath);
                File.Copy(this.AssFilePath, Helper.GetRelativePath(assFileName), true);
                filters.Add(string.Format("subtitles='{0}'", assFileName));
            }
            
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
                videoParam = "-c:v libx264 -crf 18";
            }

            string threadsParam = string.Empty;
            if (conf.Threads != null)
            {
                if (conf.Threads.Auto)
                {
                    threadsParam = string.Format("-threads {0}", Helper.GetCpuCoreNumber() - 1);
                }
                else
                {
                    threadsParam = string.Format("-threads {0}", conf.Threads.Number);
                }
            }

            string argument = string.Format(" -y -i \"{0}\" -strict -2 {1} {2} {3} {4} \"{5}\"",
                            SrcFilePath,
                            videoParam,
                            audioParam,
                            filtersParam,
                            threadsParam,
                            DestFilePath);
            OnLog("ffmpeg.exe " + argument, LogLevel.Info);
            proc.StartInfo.FileName = Path.Combine(conf.FFmpeg.Path, "ffmpeg.exe");
            proc.StartInfo.Arguments = argument;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;      
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                string line = e.Data;
                if (OnLog != null)
                {
                    OnLog(line, LogLevel.Debug);
                }
                Console.WriteLine(line);
            };
            proc.BeginOutputReadLine();
            proc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) {
                string line = e.Data;
                if (string.IsNullOrEmpty(line))
                {
                    return;
                }
                if (OnLog != null)
                {
                    OnLog(line, LogLevel.Debug);
                }
                {
                    Match match = regexDuration.Match(line);
                    if (match.Success)
                    {
                        this.Duration = TimeSpan.Parse(match.Groups[1].Value);
                    }
                }             
                Console.WriteLine(line);
                if (this.Duration != null && OnProgress != null)
                {                    
                    Match match = regexTime.Match(line);
                    if (match.Success)
                    {                        
                        TimeSpan time = TimeSpan.Parse(match.Groups[1].Value);
                        var progress = time.TotalSeconds / Duration.Value.TotalSeconds;
                        OnProgress(false, false, false, progress);
                    }
                }
                if (line ==  "Conversion failed!")
                {
                    result = false;
                }
            };
            proc.BeginErrorReadLine();

            proc.WaitForExit();
            return result;            
        }

        public void Stop()
        {
            if (proc == null || this.IsClosed)
            {
                return;
            }
            proc.Kill();            
        }
    }
}
