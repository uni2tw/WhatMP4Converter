using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace WhatMP4Converter.Core
{
    public class ChineseConverter
    {
        static Dictionary<int, Dictionary<string, string>> dictGroups = new Dictionary<int, Dictionary<string, string>>();

        const int WordMinLen = 2;
        const int WordMaxLen = 12;

        static ChineseConverter()
        {
            //只打算加入2~12字的轉換詞句
            for (int i = WordMinLen; i <= WordMaxLen; i++)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dictGroups.Add(i, dict);
            }

            DirectoryInfo diDictionary = new DirectoryInfo(Helper.GetRelativePath("Dictionary"));
            if (diDictionary.Exists)
            {
                var fileInfos = diDictionary.GetFiles("*.dat");
                foreach(var fi in fileInfos)
                {
                    string[] lines = File.ReadAllLines(fi.FullName);
                    for (int i=0;i< lines.Length;i++)
                    {
                        string line = lines[i].Trim();
                        string[] parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 2)
                        {
                            continue;
                        }
                        string s = parts[0].Trim();
                        string t = parts[1].Trim();

                        if (dictGroups.ContainsKey(t.Length))
                        {
                            var dict = dictGroups[t.Length];
                            if (dict.ContainsKey(s) == false)
                            {
                                dict.Add(s, t);
                            }
                        }

                    }
                }
            }

            
        }


        //沒有在意執行速度的版本，整行掃瞄比對
        public static string ToTraditional(string line)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < line.Length;i++)
            {
                bool anyMatch = false;
                for (int len = WordMaxLen; len >= WordMinLen; len--)
                {
                    var dict = dictGroups[len];
                    if (line.Length >= i + len)
                    {
                        string part = line.Substring(i, len);
                        if (dict.ContainsKey(part))
                        {
                            sb.Append(dict[part]);
                            i = i + len - 1;
                            anyMatch = true;
                            break;
                        }
                    }
                }
                if (anyMatch == false)
                {
                    sb.Append(line[i]);
                }
            }



            return sb.ToString(); 
        }

    }
}
