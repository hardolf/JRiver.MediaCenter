using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenter.LyricsFinder
{
    internal static class NativeMethods
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern Int32 MessageBeep(int uType);

        [DllImport("ole32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern Int32 GetRunningObjectTable(uint reserved, out IRunningObjectTable pprot);

        [DllImport("ole32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern Int32 CreateBindCtx(uint reserved, out IBindCtx ppbc);

    }

}
