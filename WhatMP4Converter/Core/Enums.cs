using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatMP4Converter.Core
{
    public enum FFmpegQuality
    {
        High = 0,
        Standard = 1,
        Low = 2
    }

    public enum FFmepgPreset
    {
        Undefind = 0,
        veryfast = 1,
        faster = 2,
        fast =  3,
        medium = 4,
        slow = 5,
        slower = 6,
        veryslow = 7
    }

    public enum OperatingMode
    {
        None = -1,
        Convert = 0,
        Merge = 1,
        Cut = 2,
        ExtractAss = 3
    }

    public enum FFmpegShrinkWidth
    {
        None = 0,
        FullHD = 1,
        HD = 2,
        Low = 3,
    }

    public enum PreCheckResult
    {
        Fail,
        OK,
        MissingFontAtAss,        
    }
}
