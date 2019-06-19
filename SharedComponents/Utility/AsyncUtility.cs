using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaCenter.SharedComponents
{

    public static class AsyncUtility
    {

        // These delegates enables asynchronous calls for setting
        // the control properties.

        public delegate bool GetControlEnabledCallback(Control control);
        public delegate string GetControlTextCallback(Control control);
        public delegate int GetProgressBarValueCallback(ProgressBar control);
        public delegate string GetToolStripItemTextCallback(ToolStripItem control);
        public delegate int GetToolStripProgressBarValueCallback(ToolStripProgressBar control);

        public delegate void SetControlEnabledCallback(Control control, bool enable);
        public delegate void SetControlTextCallback(Control control, string text);
        public delegate void SetProgressBarValueCallback(ProgressBar control, int value);
        public delegate void SetToolStripItemTextCallback(ToolStripItem control, string text);
        public delegate void SetToolStripProgressBarValueCallback(ToolStripProgressBar control, int value);

        // InvokeRequired required compares the thread ID of the
        // calling thread to the thread ID of the creating thread.
        // If these threads are different, it returns true.

        public static void AppendControlText(this Control control, string text)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.InvokeRequired)
            {
                SetControlTextCallback d = new SetControlTextCallback(AppendControlText);
                control.Invoke(d, new object[] { control, text });
            }
            else
            {
                // TextBox method AppendText is limited in number of characters - hence this method...
                StringBuilder sb = new StringBuilder();

                control.SuspendLayout();

                sb.Append(control.Text);
                sb.Append(text);

                control.Text = sb.ToString();

                if (control is TextBoxBase)
                {
                    ((TextBoxBase)control).SelectionStart = sb.Length;
                    ((TextBoxBase)control).ScrollToCaret();
                }

                control.ResumeLayout();
            }
        }


        public static bool GetControlEnabled(this Control control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.InvokeRequired)
            {
                GetControlEnabledCallback d = new GetControlEnabledCallback(GetControlEnabled);
                return (bool)control.Invoke(d, new object[] { control });
            }
            else
            {
                return control.Enabled;
            }
        }


        public static string GetControlText(this Control control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.InvokeRequired)
            {
                GetControlTextCallback d = new GetControlTextCallback(GetControlText);
                return (string)control.Invoke(d, new object[] { control });
            }
            else
            {
                return control.Text;
            }
        }


        public static int GetProgressBarValue(this ProgressBar control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.InvokeRequired)
            {
                GetProgressBarValueCallback d = new GetProgressBarValueCallback(GetProgressBarValue);
                return (int)control.Invoke(d, new object[] { control });
            }
            else
            {
                return control.Value;
            }
        }


        public static string GetToolStripItemText(this ToolStripItem control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.Owner.InvokeRequired)
            {
                GetToolStripItemTextCallback d = new GetToolStripItemTextCallback(GetToolStripItemText);
                return (string)control.Owner.Invoke(d, new object[] { control });
            }
            else
            {
                return control.Text;
            }
        }


        public static int GetToolStripProgressBarValue(this ToolStripProgressBar control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.Owner.InvokeRequired)
            {
                GetToolStripProgressBarValueCallback d = new GetToolStripProgressBarValueCallback(GetToolStripProgressBarValue);
                return (int)control.Owner.Invoke(d, new object[] { control });
            }
            else
            {
                return control.Value;
            }
        }


        public static void SetControlEnabled(this Control control, bool enabled)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.InvokeRequired)
            {
                SetControlEnabledCallback d = new SetControlEnabledCallback(SetControlEnabled);
                control.Invoke(d, new object[] { control, enabled });
            }
            else
            {
                control.Enabled = enabled;
                if (enabled)
                    control.Focus();
            }
        }


        public static void SetControlText(this Control control, string text)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.InvokeRequired)
            {
                SetControlTextCallback d = new SetControlTextCallback(SetControlText);
                control.Invoke(d, new object[] { control, text });
            }
            else
            {
                control.SuspendLayout();

                control.Text = text;

                if (control is TextBoxBase)
                {
                    ((TextBoxBase)control).SelectionStart = text.Length;
                    ((TextBoxBase)control).ScrollToCaret();
                }

                control.ResumeLayout();
            }
        }


        public static void SetProgressBarValue(this ProgressBar control, int value)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.InvokeRequired)
            {
                SetProgressBarValueCallback d = new SetProgressBarValueCallback(SetProgressBarValue);
                control.Invoke(d, new object[] { control, value });
            }
            else
            {
                if (value < control.Minimum)
                    control.Value = control.Minimum;
                else if (value > control.Maximum)
                    control.Value = control.Maximum;
                else
                    control.Value = value;
            }
        }


        public static void SetToolStripItemText(this ToolStripItem control, string text)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.Owner.InvokeRequired)
            {
                SetToolStripItemTextCallback d = new SetToolStripItemTextCallback(SetToolStripItemText);
                control.Owner.Invoke(d, new object[] { control, text });
            }
            else
            {
                control.Text = text;
            }
        }


        public static void SetToolStripItemToolTipText(this ToolStripItem control, string text)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.Owner.InvokeRequired)
            {
                SetToolStripItemTextCallback d = new SetToolStripItemTextCallback(SetToolStripItemToolTipText);
                control.Owner.Invoke(d, new object[] { control, text });
            }
            else
            {
                control.Text = text;
            }
        }


        public static void SetToolStripProgressBarValue(this ToolStripProgressBar control, int value)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (control.Owner.InvokeRequired)
            {
                SetToolStripProgressBarValueCallback d = new SetToolStripProgressBarValueCallback(SetToolStripProgressBarValue);
                control.Owner.Invoke(d, new object[] { control, value });
            }
            else
            {
                control.Value = value;
            }
        }

    }

}
