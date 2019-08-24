using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WhatMP4Converter.Core
{
    public abstract class FFmpegTaskBase
    {
        protected Process proc;

        protected AppConf conf;

        public string TaskId { get; private set; }

        public delegate void ProgressHandler(bool isStart, bool isFinish, bool? isFail, double progress, string message, string taskid);
        public event ProgressHandler OnProgress;

        public delegate void LogHandler(string str, LogLevel level);

        public event LogHandler OnLog;

        public PreCheckResult CheckResult { get; set; }

        public string SrcFilePath { get; set; }

        public string DestFilePath { get; set; }

        public bool IsClosed { get; set; }

        protected abstract void DoExecute();

        public abstract PreCheckResult PreCheck(out string confirmMessage);

        public bool Result { get; set; }

        public FFmpegTaskBase(AppConf conf, string taskId)
        {
            this.conf = conf;
            this.TaskId = taskId;
        }

        public void Execute()
        {
            BroadCastProgress(true, false, null, 0, null);
            string confirmMessage;
            PreCheckResult checkResult = PreCheck(out confirmMessage);
            if (checkResult == PreCheckResult.OK)
            {
                DoExecute();
                Result = true;
            }

            BroadCastProgress(false, true, Result, 100, confirmMessage);
            IsClosed = true;
        }

        public void Stop()
        {
            if (proc == null || this.IsClosed)
            {
                return;
            }
            proc.Kill();
        }

        protected void WriteLog(string str, LogLevel level)
        {
            if (OnLog != null)
            {
                OnLog(str, level);
            }
            Console.WriteLine(str);
        }

        protected void BroadCastProgress(bool isStart, bool isFinish, bool? isFail, double progress, string message)
        {
            if (OnProgress != null)
            {
                OnProgress(isStart, isFinish, isFail, progress,  message, this.TaskId);
            }
        }

        protected string GetDefaultVideoEncodeParam(FFmpegQuality quality)
        {
            //版本2
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("-c:v libx264 -crf ");
                if (quality == FFmpegQuality.High)
                {
                    sb.Append(conf.Quality.High.Crf);
                    sb.Append(" -preset ");
                    sb.Append(conf.Quality.High.Preset.ToString());
                }
                else if (quality == FFmpegQuality.Standard)
                {
                    sb.Append(conf.Quality.Standard.Crf);
                    sb.Append(" -preset ");
                    sb.Append(conf.Quality.Standard.Preset.ToString());
                }
                else if (quality == FFmpegQuality.Low)
                {
                    sb.Append(conf.Quality.Low.Crf);
                    sb.Append(" -preset ");
                    sb.Append(conf.Quality.Low.Preset.ToString());
                }

                //部份舊的avi檔無法轉換，需要加上此設定
                sb.Append(" -max_muxing_queue_size 1024");

                return sb.ToString();
            }

            ////版本 1
            //{
            //    return "-c:v libx264 -crf 18 -preset veryslow";
            //}
        }

        public static Process CreateProc()
        {
            Process proc = new Process();            
            proc.StartInfo.FileName =  Path.GetFullPath("./bin/ffmpeg.exe");
            //proc.StartInfo.FileName = Path.Combine(ffmpegPath, "ffmpeg.exe");            
            if (File.Exists(proc.StartInfo.FileName) == false)
            {
                return null;
            }
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            return proc;
        }

        protected int GetShrinkWidth(FFmpegShrinkWidth shrinkWidth)
        {
            if (shrinkWidth == FFmpegShrinkWidth.FullHD)
            {
                return 1920;
            }
            if (shrinkWidth == FFmpegShrinkWidth.HD)
            {
                return 1280;
            }
            if (shrinkWidth == FFmpegShrinkWidth.Low)
            {
                return 720;
            }
            return -1;
        }

    }
}
