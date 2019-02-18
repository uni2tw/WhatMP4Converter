using System;
using System.IO;
using System.Reflection;

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
    }
}
