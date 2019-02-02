﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WhatMP4Converter.Core
{
    public class AppConf
    {
        public bool Auto { get; set; }

        public string OutputPath { get; set; }

        public ThreadsConfig Threads { get; set; }

        public FFmpegConfig FFmpeg { get; set; }

        public StringsConfig Strings { get; set; }

        public ShrinkConfig Shrink { get; set; }

        public QualitySection Quality { get; set; }

        public static AppConf Reload()
        {
            AppConf config = new AppConf();
            string[] lines = File.ReadAllLines(Helper.GetRelativePath("config.conf"));
            object target = config;
            foreach(string temp in lines)
            {
                string line = temp.Trim();
                //跳過注解
                if (line.Length >0 && line[0] == '#')
                {
                    continue;
                }
                var parts = SplitString(line);
                if (parts.Length == 0) {
                    continue;
                }
                string key = parts[0];
                string val = parts.Length > 1 ? parts[1] : string.Empty;

                if (target == config)
                {
                    if (key == "auto")
                    {
                        config.Auto = ParseBool(val);
                    }
                    if (key == "output")
                    {
                        config.OutputPath =val;
                    }
                    if (key == "threads")
                    {
                        config.Threads = new ThreadsConfig();
                        target = config.Threads;
                    }
                    if (key == "ffmpeg")
                    {
                        config.FFmpeg = new FFmpegConfig();
                        target = config.FFmpeg;
                    }
                    if (key == "strings")
                    {
                        config.Strings = new StringsConfig();
                        target = config.Strings;
                    }
                    if (key == "shrink")
                    {
                        config.Shrink = new ShrinkConfig();
                        target = config.Shrink;
                    }
                    if (key == "quality")
                    {
                        config.Quality = new QualitySection();
                        target = config.Quality;
                    }
                }
                else if (target == config.Threads)
                {
                    if (key == "auto")
                    {
                        config.Threads.Auto = ParseBool(val);
                    }
                    if (key == "number")
                    {
                        config.Threads.Number = ParseInt(val);
                    }
                    if (key == "}")
                    {
                        target = config;
                    }
                }
                else if (target == config.FFmpeg)
                {
                    if (key == "path")
                    {
                        config.FFmpeg.Path = val;
                    }
                    if (key == "}")
                    {
                        target = config;
                    }
                }
                else if (target == config.Strings)
                {
                    if (key == "done")
                    {
                        config.Strings.Done = val;
                    }
                    if (key == "undefined")
                    {
                        config.Strings.Undefined = val;
                    }
                    if (key == "}")
                    {
                        target = config;
                    }
                }
                else if (target == config.Shrink)
                {
                    if (key == "auto")
                    {
                        config.Shrink.Auto = ParseBool(val);
                    }
                    if (key == "width")
                    {
                        config.Shrink.Width = ParseInt(val);
                    }
                    if (key == "}")
                    {
                        target = config;
                    }
                }
                else if (target == config.Quality)
                {
                    if (key == "default")
                    {
                        config.Quality.Default = val;
                    }
                    if (key == "high")
                    {
                        config.Quality.High = new QualityOptionSection();
                        target = config.Quality.High;
                    }
                    if (key == "standard")
                    {
                        config.Quality.Standard = new QualityOptionSection();
                        target = config.Quality.Standard;
                    }
                    if (key == "low")
                    {
                        config.Quality.Low = new QualityOptionSection();
                        target = config.Quality.Low;
                    }
                    if (key == "}")
                    {
                        target = config;
                    }
                }
                else if (config.Quality != null && target == config.Quality.High)
                {
                    if (key == "crf")
                    {
                        config.Quality.High.Crf = ParseInt(val);
                    }
                    if (key == "preset")
                    {
                        FFmepgPreset preset;
                        if (Enum.TryParse<FFmepgPreset>(val, out preset) == false)
                        {
                            throw new Exception("Preset " + val + " is invalid.");
                        }
                        config.Quality.High.Preset = preset;
                    }
                    if (key == "}")
                    {
                        target = config.Quality;
                    }
                }
                else if (config.Quality != null && target == config.Quality.Standard)
                {
                    if (key == "crf")
                    {
                        config.Quality.Standard.Crf = ParseInt(val);
                    }
                    if (key == "preset")
                    {
                        FFmepgPreset preset;
                        if (Enum.TryParse<FFmepgPreset>(val, out preset) == false)
                        {
                            throw new Exception("Preset " + val + " is invalid.");
                        }
                        config.Quality.Standard.Preset = preset;
                    }
                    if (key == "}")
                    {
                        target = config.Quality;
                    }
                }
                else if (config.Quality != null && target == config.Quality.Low)
                {
                    if (key == "crf")
                    {
                        config.Quality.Low.Crf = ParseInt(val);
                    }
                    if (key == "preset")
                    {
                        FFmepgPreset preset;
                        if (Enum.TryParse<FFmepgPreset>(val, out preset) == false)
                        {
                            throw new Exception("Preset " + val + " is invalid.");
                        }
                        config.Quality.Low.Preset = preset;
                    }
                    if (key == "}")
                    {
                        target = config.Quality;
                    }
                }
            }
            return config;
        }

        private static int ParseInt(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }

        public static void Update(AppConf conf)
        {
            string confPath = Helper.GetRelativePath("config.conf");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("auto " + RenderBool(conf.Auto));
            sb.AppendLine("output " + RenderString(conf.OutputPath));
            sb.AppendLine();


            sb.AppendLine("threads {");
            sb.AppendLine("\tauto " + RenderBool(conf.Threads.Auto));
            RenderNullableInt(sb, "number","\t", conf.Threads.Number);
            sb.AppendLine("}");

            sb.AppendLine("ffmpeg {");
            sb.AppendLine("\tpath " + RenderString(conf.FFmpeg.Path));
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("quality {");
            sb.AppendLine("\tdefault " + conf.Quality.Default);
            sb.AppendLine("\thigh {");
            sb.AppendLine("\t\tcrf " + conf.Quality.High.Crf);
            sb.AppendLine("\t\tpreset " + conf.Quality.High.Preset);
            sb.AppendLine("\t}");
            sb.AppendLine("\tstandard {");
            sb.AppendLine("\t\tcrf " + conf.Quality.Standard.Crf);
            sb.AppendLine("\t\tpreset " + conf.Quality.Standard.Preset);
            sb.AppendLine("\t}");
            sb.AppendLine("\tlow {");
            sb.AppendLine("\t\tcrf " + conf.Quality.Low.Crf);
            sb.AppendLine("\t\tpreset " + conf.Quality.Low.Preset);
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendLine("shrink {");            
            sb.AppendLine("\tauto " + RenderBool(conf.Shrink.Auto));
            sb.AppendLine("\twidth " + conf.Shrink.Width);
            sb.AppendLine("}");

            sb.AppendLine("strings {");
            sb.AppendLine("\tdone " + conf.Strings.Done);
            sb.AppendLine("\tundefined " + conf.Strings.Undefined);
            sb.AppendLine("}");

            File.WriteAllText(confPath, sb.ToString());
        }

        private static void RenderNullableInt(StringBuilder sb, string keyName, string tab, int? number)
        {
            if (number == null)
            {
                sb.Append("#");
            }
            sb.Append(tab);
            sb.Append(keyName);
            sb.Append(number.GetValueOrDefault());
            sb.AppendLine();
        }

        private static string RenderString(string outputPath)
        {
            if (outputPath.Contains(" "))
            {
                return string.Format("\"{0}\"", outputPath);
            }
            return outputPath;
        }

        private static string[] SplitString(string line)
        {
            List<string> result = new List<string>();

            //line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            bool mining = false;
            bool inQuote = false;
            StringBuilder sbWord = new StringBuilder();
            foreach(char ch in line)
            {
                if (mining == false && ch != ' ' && ch != '\t')
                {
                    mining = true;                    
                }
                if (mining)
                {
                    if (sbWord.Length == 0 && ch == '\"')
                    {
                        inQuote = true;
                        continue;
                    }
                    if (inQuote && ch == '\"')
                    {
                        mining = false;
                        result.Add(sbWord.ToString());
                        sbWord.Clear();
                        continue;
                    }
                    if (ch != ' ' && ch != '\t')
                    {
                        sbWord.Append(ch);
                    }
                    if (ch == ' ' || ch == '\t')
                    {
                        if (inQuote)
                        {
                            sbWord.Append(ch);
                        }
                        else
                        {
                            mining = false;
                            result.Add(sbWord.ToString());
                            sbWord.Clear();
                            continue;
                        }
                    }
                }                
            }
            if (sbWord.Length > 0)
            {
                result.Add(sbWord.ToString());
            }

            return result.ToArray();
        }

        private static string RenderBool(bool auto)
        {
            return auto ? "on" : "off";
        }

        private static bool ParseBool(string val)
        {
            if (val == "on" || val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
    public class FFmpegConfig
    {
        public string Path { get; set; }
    }

    public class StringsConfig {
        public string Done { get; set; }
        public string Undefined { get; set; }
    }

    public class ShrinkConfig
    {
        public bool Auto { get; set; }
        public int Width { get; set; }
    }

    public class ThreadsConfig
    {
        public bool Auto { get; set; }
        public int? Number { get; set; }
    }

    public class QualitySection
    {
        public string Default { get; set; }
        public QualityOptionSection High { get; set; }
        public QualityOptionSection Standard { get; set; }
        public QualityOptionSection Low { get; set; }
    }
    public class QualityOptionSection
    {
        public int Crf { get; set; }
        public FFmepgPreset Preset { get; set; }
    }
}
