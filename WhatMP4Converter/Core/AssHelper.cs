using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhatMP4Converter.Core
{
    public class AssHelper
    {
        static Regex regexFonts = new Regex(@"; Font Subset: ([\w]+) - [\W\w]+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        public static void ChangeAllFontName(string destAssFilePath, string unityFontName)
        {
            List<string> fontNames = new List<string>();
            string[] lines = File.ReadAllLines(destAssFilePath);
            foreach (string line in lines)
            {
                foreach (Match match in regexFonts.Matches(line))
                {
                    fontNames.Add(match.Groups[1].Value);
                }
            }
            string str = File.ReadAllText(destAssFilePath);
            foreach (string fontName in fontNames)
            {
                str = str.Replace(fontName, unityFontName);
            }
            File.WriteAllText(destAssFilePath, str);
        }

        //public static string ToTraditional(string assText)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string[] lines = assText.Split( new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach(string line in lines)
        //    {
        //        sb.AppendLine(Helper.ConvertAssToTraditionalChinese()
        //    }
        //    return sb.ToString();
        //}
    }
}
