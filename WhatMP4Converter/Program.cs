using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhatMP4Converter.Core;
using WhatMP4Converter.Tests;

namespace WhatMP4Converter
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            new ChineseConverterFixture().ToTraditionalTest();
            //System.IO.File.WriteAllText(@"c:\temp\1.txt",
            //    ChineseConverter.ToTraditional(
            //        System.IO.File.ReadAllText(@"H:\[VCB-Studio] Bungo Stray Dogs [Ma10p_1080p]\Bungo Stray Dogs [13].ass",
            //        System.Text.Encoding.UTF8)));

            //Helper.ConvertAssToTraditionalChinese(
            //    @"Dialogue: 0,0:01:53.47,0:01:54.35,*Default,NTP,0000,0000,0000,,有什么問題嗎\N{\fn微软雅黑\fs14}There a problem?");

            Application.Run(new formMain());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
            
        }
    }
}
