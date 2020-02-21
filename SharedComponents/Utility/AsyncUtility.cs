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

        /// <summary>
        /// Appends the control text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="text">The text.</param>
        /// <exception cref="ArgumentNullException">
        /// control
        /// or
        /// text
        /// </exception>
        public static void AppendControlText(this Control control, string text)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (text == null) throw new ArgumentNullException(nameof(text));

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


        /// <summary>
        /// Gets the control enabled.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">control</exception>
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


        /// <summary>
        /// Gets the control text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">control</exception>
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


        /// <summary>
        /// Gets the progress bar value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">control</exception>
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


        /// <summary>
        /// Gets the tool strip item text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">control</exception>
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


        /// <summary>
        /// Gets the tool strip progress bar value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">control</exception>
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


        /// <summary>
        /// Sets the control enabled.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <exception cref="ArgumentNullException">control</exception>
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
                if (enabled && control.CanFocus)
                    control.Focus();
            }
        }


        /// <summary>
        /// Sets the control text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="text">The text.</param>
        /// <exception cref="ArgumentNullException">
        /// control
        /// or
        /// text
        /// </exception>
        public static void SetControlText(this Control control, string text)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (text == null) throw new ArgumentNullException(nameof(text));

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


        /// <summary>
        /// Sets the progress bar value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">control</exception>
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


        /// <summary>
        /// Sets the tool strip item text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="text">The text.</param>
        /// <exception cref="ArgumentNullException">
        /// control
        /// or
        /// text
        /// </exception>
        public static void SetToolStripItemText(this ToolStripItem control, string text)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (text == null) throw new ArgumentNullException(nameof(text));

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


        /// <summary>
        /// Sets the tool strip item tool tip text.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="text">The text.</param>
        /// <exception cref="ArgumentNullException">
        /// control
        /// or
        /// text
        /// </exception>
        public static void SetToolStripItemToolTipText(this ToolStripItem control, string text)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (text == null) throw new ArgumentNullException(nameof(text));

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


        /// <summary>
        /// Sets the tool strip progress bar value.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">control</exception>
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


        /// <summary>
        /// Wait for any task and return the task if the condition is met.
        /// Otherwise wait again for the other tasks until there is no more task to wait for.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks">The tasks.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>The first task that finishes with a result that fulfills the condition.</returns>
        /// <remarks>
        /// Inspired by Sir Rufo's answer in https://stackoverflow.com/questions/38289158/how-to-implement-task-whenany-with-a-predicate.
        /// </remarks>
        public static async Task<T> WhenAny<T>(this IEnumerable<Task<T>> tasks, Predicate<Task<T>> condition)
        {
            var taskList = tasks.ToList();

            while (taskList.Count > 0)
            {
                var task = await Task.WhenAny(taskList).ConfigureAwait(false);

                if (condition(task))
                    return task.Result;

                taskList.Remove(task);
            }

            return default;
        }


    }

}
