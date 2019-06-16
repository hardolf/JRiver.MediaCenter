using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Utility class to examine the Running Object Table (ROT).
    /// </summary>
    public static class ExamineRot
    {

        private const int _OK = 0x00000000;

        private static readonly string _header = "".PadRight(80, '-');


        private static void OleCheck(string message, int result)
        {
            if (result != _OK)
                throw new COMException(message, result);
        }


        private static IEnumerable<IMoniker> EnumRunningObjects()
        {
            var result = NativeMethods.GetRunningObjectTable(0, out IRunningObjectTable objTbl);

            OleCheck("GetRunningObjectTable failed.", result);
            IMoniker[] monikers = new IMoniker[1];

            objTbl.EnumRunning(out IEnumMoniker enumMoniker);
            enumMoniker.Reset();

            while (enumMoniker.Next(1, monikers, IntPtr.Zero) == _OK)
            {
                yield return monikers[0];
            }
        }


        private static bool TryGetCLSIDFromDisplayName(string displayName, out string clsid)
        {
            var bBracket = displayName.IndexOf("{", StringComparison.InvariantCultureIgnoreCase);
            var eBracket = displayName.IndexOf("}", StringComparison.InvariantCultureIgnoreCase);

            if ((bBracket > 0) && (eBracket > 0) && (eBracket > bBracket))
            {
                clsid = displayName.Substring(bBracket, eBracket - bBracket + 1);
                return true;
            }
            else
            {
                clsid = string.Empty;
                return false;
            }
        }


        private static string ReadSubKeyValue(string keyName, RegistryKey key)
        {
            using (var subKey = key.OpenSubKey(keyName))
            {
                if (subKey != null)
                {
                    var value = subKey.GetValue("");

                    return value == null ? string.Empty : value.ToString();
                }
            }

            return string.Empty;
        }


        private static string GetMonikerString(IMoniker moniker)
        {
            var sb = new StringBuilder();
            var result = NativeMethods.CreateBindCtx(0, out IBindCtx ctx);

            OleCheck("CreateBindCtx failed.", result);
            moniker.GetDisplayName(ctx, null, out string displayName);

            if (TryGetCLSIDFromDisplayName(displayName, out string clsid))
            {
                sb.AppendLine($"DisplayName            : {displayName}");

                using (var regClass = Registry.ClassesRoot.OpenSubKey("\\CLSID\\" + clsid))
                {
                    if (regClass != null)
                    {
                        var regClassDisplay = regClass.GetValue("");
                        var progId = ReadSubKeyValue("ProgID", regClass);
                        var localServer32 = ReadSubKeyValue("LocalServer32", regClass);

                        sb.AppendLine($"RegClass ClsID         : {regClassDisplay}");
                        sb.AppendLine($"RegClass ProgID        : {progId}");
                        sb.AppendLine($"RegClass LocalServer32 : {localServer32}");
                    }
                }
            }
            else
            {
                sb.AppendLine($"DisplayName            : Unknown application");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Gets the Running Object Table (ROT) as text.
        /// </summary>
        /// <returns>String listing the ROT.</returns>
        public static string GetRotText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Running Object Table (ROT)"); //  "DisplayName\tRegId\tProgId\tServer");
            sb.AppendLine(_header);

            foreach (var moniker in EnumRunningObjects())
            {
                sb.Append(GetMonikerString(moniker));
                sb.AppendLine(_header);
            }

            Console.WriteLine(sb.ToString());

            return sb.ToString();
        }

    }

}
