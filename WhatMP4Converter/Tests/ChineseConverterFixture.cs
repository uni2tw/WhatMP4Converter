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
            Debug.Assert(ChineseConverter.ToTraditional("不干") == "不幹");
            Debug.Assert(ChineseConverter.ToTraditional("擦不干") == "擦不乾");
            Debug.Assert(ChineseConverter.ToTraditional("有什么問題嗎") == "有什麼問題嗎");
            Debug.Assert(ChineseConverter.ToTraditional("我们与恶的距离") == "我們與惡的距離");
            Debug.Assert(ChineseConverter.ToTraditional("就是只要我開口") == "就是只要我開口");
            Debug.Assert(ChineseConverter.ToTraditional("好懷念啊") == "好懷念啊");
            Debug.Assert(ChineseConverter.ToTraditional("我和一帮开着机关枪车的年轻气盛的团伙") == "我和一幫開著機關槍車的年輕氣盛的集團");
            Debug.Assert(ChineseConverter.ToTraditional("世上就没个简单又能让人安心的自杀办法吗") == "世上就沒個簡單又能讓人安心的自殺辦法嗎");
            Debug.Assert(ChineseConverter.ToTraditional("觉得今晚会更有平时的风味") == "覺得今晚會更有平時的風味");
            Debug.Assert(ChineseConverter.ToTraditional("忙活到八点 才弄来这么一个老式怀表") == "忙碌到八點 才弄來這麼一個老式懷錶");
            Debug.Assert(ChineseConverter.ToTraditional("问我這種最底層员工的工作 也不會有意思的") == "問我這種最底層員工的工作 也不會有意思的");


        }
    }
}
