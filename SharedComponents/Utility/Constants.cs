using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenter.SharedComponents
{
    public static class Constants
    {
        public static readonly string DateFormat = "yyyy.MM.dd";
        public static readonly string IntegerFormat = "###,###,##0";
        public static readonly string TimeFormat = "HH:mm.ss";

        public static readonly string DateTimeFormat = $"{DateFormat} - {TimeFormat}";
    }

}
