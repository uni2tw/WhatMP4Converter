﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhatMP4Converter.Core
{
    public class FFmpegExtractAssTask : FFmpegTaskBase
    {
        public class StreamInfo
        {
            public string Id { get; set; }
            public string SubType { get; set; }
            public string Type { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", Id, Type, SubType);
            }
        }
        private Regex regexStreamInfo = new Regex(@"[sS]tream #([\d]:[\d]+)[(\w)]*: ([Audio|Video|Subtitle|Attachment]*): ([\w]*)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        public FFmpegExtractAssTask(AppConf conf, string taskId) : base(conf, taskId)
        {

        }

        public override PreCheckResult PreCheck(out string confirmMessage)
        {
            confirmMessage = string.Empty;
            return PreCheckResult.OK;
        }

        protected override void DoExecute()
        {
            List<StreamInfo> infos = new List<StreamInfo>();
            if (DoList(infos))
            {
                this.Result = DoExtract(infos);
            }
        }

        private bool DoExtract(List<StreamInfo> streamInfos)
        {
            streamInfos = streamInfos.Where(t =>
                t.Type.Equals("Subtitle", StringComparison.OrdinalIgnoreCase) &&
                t.SubType.Equals("ass", StringComparison.OrdinalIgnoreCase))
                .ToList();
            foreach (StreamInfo streamInfo in streamInfos)
            {
                string streamIdx = string.Empty;
                if (streamInfos.Count  > 1)
                {
                    streamIdx = "." + streamInfo.Id.Split(new char[] { ':' })[1];
                }
                if (streamInfo.Id.Contains(":") == false)
                {
                    continue;
                }                
                string destAssFile = Path.GetFileNameWithoutExtension(Path.GetFileName(SrcFilePath)) + streamIdx + ".ass";
                string destAssFilePath = Path.Combine(conf.Output, destAssFile);
                if (DoExtractAss(streamInfo, destAssFilePath)==false)
                {
                    return false;
                }

            }
            return true;
        }

        public bool DoExtractAss(StreamInfo info, string destAssFilePath)
        {


            bool result = true;
            DateTime startTime = DateTime.Now;
            proc = new Process();
            string argument = string.Format(" -i \"{0}\" -map {1} \"{2}\"",
                            SrcFilePath, info.Id, destAssFilePath);
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

                Console.WriteLine(line);

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

        public bool DoList(List<StreamInfo> infos)
        {
            bool result = true;
            DateTime startTime = DateTime.Now;
            proc = new Process();
            string argument = string.Format(" -i \"{0}\"",
                            SrcFilePath);
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

                Match match = regexStreamInfo.Match(line);
                if (match.Success) {
                    infos.Add(new StreamInfo
                    {
                        Id = match.Groups[1].Value,
                        Type = match.Groups[2].Value,
                        SubType = match.Groups[3].Value
                    });
                }

                WriteLog(line, LogLevel.Debug);

                Console.WriteLine(line);

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
    }
}
