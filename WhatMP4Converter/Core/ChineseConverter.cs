using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace WhatMP4Converter.Core
{
    public class ChineseConverter
    {
        static Dictionary<int, Dictionary<string, string>> dictGroups = new Dictionary<int, Dictionary<string, string>>();

        const int WordMinLen = 1;
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
                var fileInfos = new List<FileInfo>(diDictionary.GetFiles("*.dat"));
                //把 Other.dat 放到最後處理，後面的設定可以蓋掉前面的
                var fiOther = fileInfos.FirstOrDefault(t => t.Name.Equals("Other.dat"));
                fileInfos.Remove(fiOther);
                fileInfos.Add(fiOther);
                foreach (var fi in fileInfos)
                {
                    int minWord = WordMinLen;
                    if (fi.Name.Equals("Other.Dat", StringComparison.OrdinalIgnoreCase) == false)
                    {
                        minWord = 2;
                    }
                    string[] lines = File.ReadAllLines(fi.FullName);
                    for (int i=0;i< lines.Length;i++)
                    {
                        string line = lines[i].Trim();
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        if (line[0] == ':' || line[0] == '-')
                        {
                            continue;
                        }
                        string[] parts = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 2)
                        {
                            continue;
                        }
                        string s = parts[0].Trim();
                        string t = parts[1].Trim();

                        if (s.Length < minWord || s.Length >WordMaxLen)
                        {
                            continue;
                        }

                        if (dictGroups.ContainsKey(s.Length))
                        {
                            var dict = dictGroups[s.Length];
                            if (dict.ContainsKey(s) == false)
                            {
                                if (t.Contains(" "))
                                {
                                    dict.Add(s, t.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                                }
                                else
                                {
                                    dict.Add(s, t);
                                }
                            }
                            else
                            {
                                if (t.Contains(" "))
                                {
                                    dict[s] = t.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                                }
                                else
                                {
                                    dict[s] = t;
                                }
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
