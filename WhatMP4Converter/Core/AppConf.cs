using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WhatMP4Converter.Core
{
    public class AppConfConverter
    {
        const string _CLOSE_MARK = "}";
        public string Serialize(object target)
        {
            StringBuilder sb = new StringBuilder();
            SerializeObject(sb, target, string.Empty);
            return sb.ToString();
        }
        private void SerializeObject(StringBuilder sb , object target, string ident)
        {
            List<PropertyInfo> propInfos;
            Dictionary<string, PropertyInfo> propKeys = GetObjectProps(target, out propInfos);

            foreach (PropertyInfo pi in propInfos)
            {
                string key = pi.Name.ToLower();
                if (pi.PropertyType.IsClass && pi.PropertyType != typeof(string))
                {
                    sb.AppendLine();
                    sb.AppendLine(ident + key + " {");
                    object propObj = pi.GetValue(target);
                    SerializeObject(sb, propObj, ident + "\t");
                    sb.AppendLine(ident + "}");
                }
                if (propKeys[key].PropertyType.IsEnum)
                {
                    object val = pi.GetValue(target);
                    sb.AppendLine(ident + key + " " + val);
                }
                if (pi.PropertyType == typeof(bool))
                {
                    bool val = (bool)pi.GetValue(target);
                    sb.AppendLine(ident + key + " " + RenderBool(val));
                }
                if (pi.PropertyType == typeof(string))
                {
                    string val = (string)pi.GetValue(target);
                    sb.AppendLine(ident+key + " " + RenderString(val));
                }
                if (pi.PropertyType == typeof(int))
                {
                    int val = (int)pi.GetValue(target);
                    sb.AppendLine(ident + key + " " + val);
                }
                if (pi.PropertyType == typeof(int?))
                {
                    int? val = (int?)pi.GetValue(target);
                    RenderNullableInt(sb, key, ident , val);
                }
            }
        }

        public T Deserialize<T>(string str) where T : class, new()
        {
            string[] lines = str.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            T target = new T();
            DeserializeObject(lines, 0, target);
            return target;
        }

        private int DeserializeObject(string[] lines, int lineStart, object target)
        {
            int i;
            for (i = lineStart; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                //跳過空字串與注解
                if (line == string.Empty || line[0] == '#')
                {
                    continue;
                }
                var parts = SplitString(line);
                if (parts.Length == 0)
                {
                    continue;
                }
                string key = parts[0];
                string val = parts.Length > 1 ? parts[1] : string.Empty;

                if (key == _CLOSE_MARK)
                {
                    break;
                }

                List<PropertyInfo> temp;
                Dictionary<string, PropertyInfo> propKeys = GetObjectProps(target, out temp);

                if (propKeys.ContainsKey(key))
                {
                    PropertyInfo prop = propKeys[key];
                    if (propKeys[key].PropertyType.IsClass && propKeys[key].PropertyType != typeof(string))
                    {
                        object propObj = prop.GetValue(target);

                        propObj = Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(target, propObj);

                        i = DeserializeObject(lines, i + 1, propObj);
                    }
                    if (propKeys[key].PropertyType.IsEnum)
                    {
                        object enumVal = Enum.Parse(propKeys[key].PropertyType, val, true);
                        prop.SetValue(target, enumVal);
                    }
                    if (propKeys[key].PropertyType == typeof(bool) || propKeys[key].PropertyType == typeof(bool?))
                    {
                        prop.SetValue(target, ParseBool(val));
                    }
                    if (propKeys[key].PropertyType == typeof(string))
                    {
                        prop.SetValue(target, val);
                    }
                    if (propKeys[key].PropertyType == typeof(int) || propKeys[key].PropertyType == typeof(int?))
                    {
                        prop.SetValue(target, ParseInt32(val));
                    }
                }
            }
            return i;
        }

        private Dictionary<string, PropertyInfo> GetObjectProps(object target, out List<PropertyInfo> sortedList)
        {
            Type t = target.GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, int> ranks = new Dictionary<string, int>();
            int baseRank = 1000;
            foreach (PropertyInfo prop in props)
            {
                result.Add(prop.Name, prop);
                ConfPropAttribute attr = prop.GetCustomAttribute<ConfPropAttribute>();
                if (attr == null)
                {
                    ranks.Add(prop.Name, baseRank);
                    baseRank++;
                }
                else
                {
                    ranks.Add(prop.Name, attr.Rank);
                }
            }
            var grp = from r in result
                      from rank in ranks
                      where r.Key == rank.Key
                      orderby rank.Value
                      select r.Value;
            sortedList = grp.ToList();
            return result;
        }

        #region utils

        private int ParseInt32(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }

        private void RenderNullableInt(StringBuilder sb, string keyName, string tab, int? number)
        {
            sb.Append(tab);
            sb.Append(keyName);
            sb.Append(" ");
            if (number != null)
            {
                sb.Append(number.Value);
            }
            sb.AppendLine();
        }

        private string RenderString(string outputPath)
        {
            if (outputPath.Contains(" "))
            {
                return string.Format("\"{0}\"", outputPath);
            }
            return outputPath;
        }

        private string[] SplitString(string line)
        {
            List<string> result = new List<string>();

            //line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            bool mining = false;
            bool inQuote = false;
            StringBuilder sbWord = new StringBuilder();
            foreach (char ch in line)
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

        private string RenderBool(bool auto)
        {
            return auto ? "on" : "off";
        }

        private bool ParseBool(string val)
        {
            if (val == "on" || val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        #endregion
    }

    public class ConfPropAttribute : Attribute
    {
        public int Rank { get; set; }
        public ConfPropAttribute(int rank)
        {
            this.Rank = rank;
        }
    }

    public class AppConf
    {
        [ConfProp(0)]
        public bool Auto { get; set; }
        [ConfProp(1)]
        public string Output { get; set; }
        [ConfProp(2)]
        public ThreadsConfig Threads { get; set; }
        [ConfProp(5)]
        public StringsConfig Strings { get; set; }
        [ConfProp(4)]
        public ShrinkConfig Shrink { get; set; }
        [ConfProp(3)]
        public QualitySection Quality { get; set; }

        public static AppConf Reload()
        {
            AppConfConverter conv = new AppConfConverter();           
            string str = File.ReadAllText(Helper.GetRelativePath("config.conf"));
            AppConf config = conv.Deserialize<AppConf>(str);
            return config;
        }

        public static void Update(AppConf conf)
        {
            AppConfConverter conv = new AppConfConverter();
            string confPath = Helper.GetRelativePath("config.conf");
            string str = conv.Serialize(conf);
            File.WriteAllText(confPath, str);
        }

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

        public override string ToString()
        {
            return string.Format("{0} {1}", Crf, Preset);
        }
    }
}
