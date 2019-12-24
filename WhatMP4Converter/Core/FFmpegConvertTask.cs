using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WhatMP4Converter.Core
{
    public class FFmpegConvertTask : FFmpegTaskBase
    {
        public FFmpegQuality Quality { get; set; }

        public TimeSpan? Duration { get; set; }
        public string VideoWidth { get; set; }
        public string VideoHeight { get; set; }
        public string VideoEncodeType { get; set; }
        public string AudioEncodeType { get; set; }

        public string AssFilePath { get; set; }

        public FFmpegShrinkWidth ShrinkVideoWidth { get; set; }

        public int GainFontSize { get; set; }

        private Regex regexDuration = new Regex(@"Duration: ([\d:.]*),",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexTime = new Regex(@"time=([\d:.]*) bitrate=",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Regex regexAudio = new Regex(@"Audio: ([\w]+)[ ,]",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        //v2
        private Regex regexVideo = new Regex(@"Video: ([\w]+)[\w \/,()[\]]+,[ ]*([\d]{2,4})x([\d]{2,4})",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public FFmpegConvertTask(AppConf conf, string taskId) : base(conf, taskId)
        {
            this.ShrinkVideoWidth = FFmpegShrinkWidth.HD;
        }

        protected override void DoExecute()
        {
            this.Result = DoInfo();
            if (this.Result)
            {
                this.Result = DoConvert();
            }
        }

        private bool DoConvert()
        {
            bool result = true;
            DateTime startTime = DateTime.Now;
            proc = new Process();
            string defaultVideoEncodeParam = GetDefaultVideoEncodeParam(Quality);

            string audioParam = string.Empty;
            if (this.AudioEncodeType != null)
            {
                if (this.AudioEncodeType.Equals("aac", StringComparison.OrdinalIgnoreCase))
                {
                    audioParam = "-c:a copy";
                }
                else
                {
                    audioParam = "-c:a aac -b:a 128k -ac 2";
                }
            }
            string videoParam;
            if (conf.Quality.alwasy_encode == false
                && this.VideoEncodeType.Equals("h264", StringComparison.OrdinalIgnoreCase))
            {
                videoParam = "-c:v copy";
            }
            else
            {
                videoParam = defaultVideoEncodeParam;
            }

            List<string> filters = new List<string>();


            string ext = Path.GetExtension(this.SrcFilePath);
            string assFilePath = null;
            List<string> assFilePaths;
            if (Helper.TryFindAssFile(this.SrcFilePath, out assFilePath, out assFilePaths))
            {
                this.AssFilePath = assFilePath;
                WriteLog("合併字幕: " + Path.GetFileName(this.AssFilePath), LogLevel.Info);
            }
            if (string.IsNullOrEmpty(this.AssFilePath) == false && File.Exists(this.AssFilePath))
            {
                string assFileName = Path.GetFileName(this.AssFilePath);
                string assText = File.ReadAllText(this.AssFilePath, Encoding.UTF8);
                assText = Helper.ChangeAssFontSize(assText, this.GainFontSize);



                //assText = ChineseConverter.ToTraditional(assText);
                //List<string> assLines = AssHelper.ToTraditional(assText);
                //assText = string.Join(Environment.NewLine, assLines);
                assText = Helper.ConvertAssToTraditionalChinese(assText);
                File.WriteAllText(Helper.GetRelativePath(assFileName), assText);
                //File.Copy(this.AssFilePath, Helper.GetRelativePath(assFileName), true);
                filters.Add(string.Format("subtitles='{0}'", assFileName));
            }

            int shrinkWidth = GetShrinkWidth(ShrinkVideoWidth);
            int oriWidth;
            if (shrinkWidth > 0 && int.TryParse(this.VideoWidth, out oriWidth) && 
                oriWidth > shrinkWidth)
            {
                videoParam = defaultVideoEncodeParam;
                //filters.Add(string.Format("scale={0}:-1", shrinkWidth));
                filters.Add(string.Format("scale={0}:trunc(ow/a/2)*2", shrinkWidth));
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

            string argument = string.Format(" -y -i \"{0}\" -strict -2 {1} {2} {3} {4} \"{5}\"",
                            SrcFilePath,
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

        private bool DoInfo()
        {
            bool result = true;
            string argument = string.Format(" -i \"{0}\" -hide_banner",
                            SrcFilePath);
            WriteLog("ffmpeg.exe " + argument, LogLevel.Info);
            proc = FFmpegTaskBase.CreateProc();
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
                if (string.IsNullOrEmpty(this.AudioEncodeType))
                {
                    Match match = regexAudio.Match(line);
                    if (match.Success)
                    {
                        this.AudioEncodeType = match.Groups[1].Value;
                        WriteLog(string.Format("Audio: {0}", this.AudioEncodeType), LogLevel.Info);
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

                        WriteLog(string.Format("Video: {0} {1}x{2}",
                            this.VideoEncodeType, this.VideoWidth, this.VideoHeight), LogLevel.Info);
                    }
                }
                Console.WriteLine(line);
            };
            proc.BeginErrorReadLine();

            proc.WaitForExit();

            //if (string.IsNullOrEmpty(this.VideoEncodeType) ||
            //    string.IsNullOrEmpty(this.AudioEncodeType))
            if (string.IsNullOrEmpty(this.VideoEncodeType))
            {
                result = false;
            }

            return result;
        }

        public override PreCheckResult PreCheck(out string confirmMessage)
        {
            confirmMessage = string.Empty;
            if (File.Exists(this.SrcFilePath) == false)
            {
                return PreCheckResult.Fail;
            }

            string ext = Path.GetExtension(this.SrcFilePath);
            string assFilePath = null;
            List<string> assFilePaths;
            if (Helper.TryFindAssFile(this.SrcFilePath, out assFilePath, out assFilePaths))
            {
                List<string> assFonts = Helper.GetAssFontStyles(new FileInfo(assFilePath));
                Dictionary<string, string> systemFonts = Helper.GetChineseFonts();

                var missingFonds = new HashSet<String>(assFonts).Except(systemFonts.Keys).ToList();
                
                if (missingFonds.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("缺失 ");
                    for (int i = 0; i < missingFonds.Count; i++) {
                        if (i > 0 )
                        {
                            sb.Append(", ");
                        }
                        string missingFont = missingFonds[i];
                        sb.Append(missingFont);
                    }
                    confirmMessage = sb.ToString();
                    WriteLog(confirmMessage, LogLevel.Info);
                    return PreCheckResult.MissingFontAtAss;
                }

            }


            return PreCheckResult.OK;
        }

    }
}
