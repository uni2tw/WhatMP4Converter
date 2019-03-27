using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WhatMP4Converter.Core
{
    public class Helper
    {
        public static string GetRootPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static string GetRelativePath(params string[] paths)
        {
            if (paths.Length == 1)
            {
                return Path.Combine(GetRootPath(), paths[0]);
            }
            else if (paths.Length == 2)
            {
                return Path.Combine(GetRootPath(), paths[0], paths[1]);
            }
            else if (paths.Length == 3)
            {
                return Path.Combine(GetRootPath(), paths[0], paths[1], paths[2]);
            }
            else if (paths.Length > 3)
            {
                throw new Exception("GetRelativePath 目前不支援超過3個目錄參數陣列");
            }
            return GetRootPath();
        }

        public static int GetCpuCoreNumber()
        {
            return Environment.ProcessorCount;
        }

        public static string AppendFileName(string fileName, string appendStr)
        {
            string dirName = Path.GetDirectoryName(fileName);
            string mainName = Path.GetFileNameWithoutExtension(fileName);
            string extName = Path.GetExtension(fileName);
            
            string newFileName = string.Format("{0}{1}{2}",
                mainName,
                appendStr,
                extName);
            if (string.IsNullOrEmpty(dirName) == false)
            {
                return Path.Combine(dirName, newFileName);
            }
            return newFileName;
        }

        public static bool TryFindAssFile(string srcFilePath, out string assFilePath, out List<string> assFilePaths)
        {
            assFilePath = null;
            assFilePaths = new List<string>();

            FileInfo fiInfo = new FileInfo(srcFilePath);
            if (fiInfo.Exists == false)
            {
                return false;
            }

            string srcMajorName = Path.GetFileNameWithoutExtension(srcFilePath);            

            foreach (var fi in fiInfo.Directory.GetFiles())
            {
                string targetMarjorName = Path.GetFileNameWithoutExtension(fi.Name);
                string targetExtName = Path.GetExtension(fi.Name);

                if (targetExtName.Equals(".ass", StringComparison.OrdinalIgnoreCase) == false)
                {
                    continue;
                }

                if (IsPartialFileName(targetMarjorName, srcMajorName))
                {
                    assFilePaths.Add(fi.FullName);
                }
            }

            assFilePath = assFilePaths.FirstOrDefault();

            return assFilePath != null;
        }

        private static bool IsPartialFileName(string targetMarjorName, string srcMajorName)
        {
            if (string.IsNullOrEmpty(targetMarjorName) || string.IsNullOrEmpty(srcMajorName))
            {
                return false;
            }
            targetMarjorName = targetMarjorName.ToLower();
            srcMajorName = srcMajorName.ToLower();

            if (srcMajorName.Equals(targetMarjorName))
            {
                return true;
            }
            if (targetMarjorName.StartsWith(srcMajorName + "."))
            {
                return true;
            }

            return false;
        }

        static Regex regexFontStyle = new Regex(@"Style: [^,]+,([^,]+),([^,]+),[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[^,]+,[\d]+");

        public static string ChangeAssFontSize(string text, int incrSize = 10)
        {            
            MatchCollection matches = regexFontStyle.Matches(text);
            for (int i = matches.Count - 1; i >= 0; i--)
            {
                Match match = matches[i];
                Group group = match.Groups[2];
                string replacement = (int.Parse(group.Value) + incrSize).ToString();
                text = ReplaceStrByPos(text, group.Index, group.Length, replacement);
            }
            return text;
        }
        
        static Regex regexAssDialog = new Regex(@"Dialogue:[ ]*[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,([\w\W]*)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //適用於中英字幕，分別使用2種字型時
        //Dialogue: 0,0:01:53.47,0:01:54.35,*Default,NTP,0000,0000,0000,,有什么問題嗎\N{\fn微软雅黑\fs14}There a problem?
        static Regex regexAssDialog2 = new Regex(@"Dialogue:[ ]*[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,[^,]*,([\w\W]+){[\w\W]*}([\w\W]+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static string ConvertAssToTraditionalChinese(string assText)
        {
            StringBuilder sb = new StringBuilder();
            string[] lines = assText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                {
                    string newLine = line;
                    bool anyMatch = false;
                    for (int i = 1; i <= 2; i++)
                    {
                        Match match = regexAssDialog2.Match(newLine);
                        if (match.Success)
                        {
                            var group = match.Groups[i];
                            string replacement = ChineseConverter.ToTraditional(group.Value);                            
                            newLine = ReplaceStrByPos(newLine, group.Index, group.Length, replacement);
                            anyMatch = true;
                        }
                    }
                    if (anyMatch) {
                        sb.AppendLine(newLine);
                        continue;
                    }
                }
                {
                    Match match = regexAssDialog.Match(line);
                    if (match.Success)
                    {
                        string replacement = ChineseConverter.ToTraditional(match.Groups[1].Value);
                        string newLine = ReplaceStrByPos(match.Groups[1].Value, match.Groups[1].Index, match.Groups[1].Length, replacement);
                        sb.AppendLine(newLine);
                        continue;
                    }
                }
                sb.AppendLine(line);                             
            }
            return sb.ToString();
        }

        private static string ReplaceStrByPos(string text, int index, int length, string replacement)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(text.Substring(0, index));
            builder.Append(replacement);
            builder.Append(text.Substring(index + length));
            return builder.ToString();
        }
        /// <summary>
        /// 取得 有 ass 字幕檔目錄下，所有字幕檔使用的字型
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static List<string> GetAssFontStyles(DirectoryInfo dirInfo, string searchPattern = "*.ass")
        {
            var result = new List<string>();
            foreach (var fi in dirInfo.GetFiles(searchPattern))
            {
                result.AddRange(GetAssFontStyles(fi));
            }
            return result.Distinct().ToList();
        }

        public static List<string> GetAssFontStyles(FileInfo fi)
        {
            var result = new List<string>();
            string text = File.ReadAllText(fi.FullName);
            MatchCollection matches = regexFontStyle.Matches(text);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    result.Add(match.Groups[1].Value.TrimStart('@'));
                }
            }
            return result.Distinct().ToList();
        }

        /// <summary>
        /// 取得系統安裝的中文或日文字型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetChineseFonts()
        {
            Dictionary<string, string> installedChineseFonts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            InstalledFontCollection installedFontCollection = new InstalledFontCollection();
            CultureInfo[] cultureInfoArray = new CultureInfo[5]
               {
                  new CultureInfo("en-US"),
                  new CultureInfo("ja-JP"),
                  new CultureInfo("zh-TW"),
                  new CultureInfo("zh-CN"),
                  new CultureInfo("zh-HK")
               };
            foreach (System.Drawing.FontFamily family in installedFontCollection.Families)
            {
                if (family.Name == "")
                {
                    continue;
                }

                if (family.Name.StartsWith("EPSON "))
                {
                    string name = family.GetName(cultureInfoArray[1].LCID);

                    installedChineseFonts[name] = name;
                }
                else
                {
                    string fontDescription = (string)null;

                    for (int index = 0; index < cultureInfoArray.Length; ++index)
                    {
                        string cultureFontName = family.GetName(cultureInfoArray[index].LCID);
                        //if (cultureFontName.Any(ch => Char.GetUnicodeCategory(ch) == UnicodeCategory.OtherLetter) == false)
                        //{
                        //    continue;
                        //}
                        if (family.Name == cultureFontName && installedChineseFonts.ContainsKey(family.Name) == false)
                        {
                            installedChineseFonts.Add(family.Name, family.Name);
                        }
                        else if (family.Name != cultureFontName)
                        {
                            if (family.Name.Length == 0)
                            {
                                fontDescription = cultureFontName;
                            }
                            else
                            {
                                fontDescription = family.Name + " <" + cultureFontName + ">";
                            }
                            installedChineseFonts[cultureFontName] = fontDescription;
                        }
                    }

                }
            }
            return installedChineseFonts;
        }
    }
}
