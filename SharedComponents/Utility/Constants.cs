using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenter.SharedComponents
{
    public static class Constants
    {
        public static readonly string NewLine = Environment.NewLine;
        public static readonly string DoubleNewLine = NewLine + NewLine;
        public static readonly string TripleNewLine = NewLine + NewLine + NewLine;

        public static readonly string DateFormat = "yyyy.MM.dd";
        public static readonly string IntegerFormat = "###,###,##0";
        public static readonly string TimeFormat = "HH:mm:ss";
        public static readonly string TimeSpanFormatLong = @"hh\:mm\:ss";
        public static readonly string TimeSpanFormatShort = @"mm\:ss";
        public static readonly string DateTimeFormat = $"{DateFormat} - {TimeFormat}";

        public static readonly DateTime MediaCenterZeroUtcDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }

}
