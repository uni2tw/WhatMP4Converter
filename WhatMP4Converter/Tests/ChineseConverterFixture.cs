using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatMP4Converter.Core;

namespace WhatMP4Converter.Tests
{
    public class ChineseConverterFixture
    {
        public void ToTraditionalTest()
        {
            Debug.Assert(ChineseConverter.ToTraditional("后面") ==  "後面");
            Debug.Assert(ChineseConverter.ToTraditional("有什么問題嗎") == "有什麼問題嗎");
            Debug.Assert(ChineseConverter.ToTraditional("我们与恶的距离") == "我們與惡的距離");
        }
    }
}
