using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// Sends event to 
    /// </summary>
    /// <seealso cref="IMessageFilter" />
    /// <remarks>
    /// <para>
    /// Usage example: Application.AddMessageFilter(new MessageFilter(listBox1.Handle, listBox2.Handle));
    /// </para>
    /// <para>
    /// Source inspired by: https://stackoverflow.com/questions/24106649/how-to-pass-an-event-to-another-control-in-c-sharp-winforms
    /// </para>
    /// </remarks>
    public class MessageFilter : IMessageFilter
    {

        private IntPtr _sourceWindowHandle;
        private IntPtr _destWindowHandle;


        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        public static int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam)
        {
            return NativeMethods.SendMessage(hWnd, msg, wParam, lParam);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFilter"/> class.
        /// </summary>
        /// <param name="sourceHandle">The source handle.</param>
        /// <param name="destHandle">The dest handle.</param>
        public MessageFilter(IntPtr sourceHandle, IntPtr destHandle)
        {
            _sourceWindowHandle = sourceHandle;
            _destWindowHandle = destHandle;
        }


        /// <summary>
        /// Filters out a message before it is dispatched.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
        /// <returns>
        ///   <see langword="true" /> to filter the message and stop it from being dispatched; <see langword="false" /> to allow the message to continue to the next filter or control.
        /// </returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.HWnd == _sourceWindowHandle) // && m.Msg == 0x020A) // mousewheel
                SendMessage(_destWindowHandle, m.Msg, (int)m.WParam, (int)m.LParam);

            return false;
        }

    }



    /// <summary>
    /// Contains the external P/Invokes
    /// </summary>
    internal static class NativeMethods
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    }

}
