using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.McWs;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Media Center control form.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class McControlForm : Form
    {

        private bool _isLocked = false;

        private int _currentSeconds = 0;

        // Instantiate a Singleton of the Semaphore with a value of 1. 
        // This means that only 1 thread can be granted access at a time. 
        // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);


        /// <summary>
        /// Gets or sets the current seconds.
        /// </summary>
        /// <value>
        /// The current seconds.
        /// </value>
        public int CurrentSeconds
        {
            get { return _currentSeconds; }
            private set
            {
                _currentSeconds = value;
                McCurrentPositionTrackBar.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum seconds.
        /// </summary>
        /// <value>
        /// The maximum seconds.
        /// </value>
        public int MaxSeconds
        {
            get { return McCurrentPositionTrackBar.Maximum; }
            private set { McCurrentPositionTrackBar.Maximum = value; }
        }


        /// <summary>
        /// Gets or sets the owner control.
        /// </summary>
        /// <value>
        /// The owner control.
        /// </value>
        private Control OwnerControl { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="McControlForm"/> class.
        /// </summary>
        private McControlForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McControlForm" /> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public McControlForm(Control owner)
            : this()
        {
            OwnerControl = owner;
        }


        /// <summary>
        /// Jump the playing position.
        /// </summary>
        /// <param name="isBackward">if set to <c>true</c> jump backward; else jump forward.</param>
        /// <param name="isLargeJump">if set to <c>true</c> the jump is large; else it is small.</param>
        /// <exception cref="Exception">Setting play position to {pos} seconds in Media Center failed.</exception>
        public async Task JumpAsync(bool isBackward = false, bool isLargeJump = false)
        {
            if (_isLocked)
                return;

            var pos = McCurrentPositionTrackBar.Value;
            var diff = (isLargeJump) ? McCurrentPositionTrackBar.LargeChange : McCurrentPositionTrackBar.SmallChange;

            if (diff == 0)
                return;

            var newPos = (isBackward) ? pos - diff : pos + diff;

            if (newPos < 0)
                newPos = 0;

            if (newPos > McCurrentPositionTrackBar.Maximum)
                newPos = McCurrentPositionTrackBar.Maximum;

            var rsp = await McRestService.PositionAsync(newPos * 1000);

            if (!rsp.IsOk)
                throw new Exception($"Setting play position to {pos} seconds in Media Center failed.");
        }


        /// <summary>
        /// Sets the current seconds.
        /// </summary>
        /// <param name="value">The value.</param>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task SetCurrentSecondsAsync(int value)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (_isLocked) return;

            CurrentSeconds = value;

            var max = new TimeSpan(McCurrentPositionTrackBar.Maximum * 10000000);
            var maxTxt = (max.TotalMinutes > 59) ? $"{max.TotalHours:###0}:{max.Minutes:00}:{max.Seconds:00}" : $"{max.TotalMinutes:#0}:{max.Seconds:00}";

            var pos = new TimeSpan(value * 10000000);
            var posTxt = (max.TotalMinutes > 59) ? $"{pos.TotalHours:###0}:{pos.Minutes:00}:{pos.Seconds:00}" : $"{pos.TotalMinutes:#0}:{pos.Seconds:00}";

            TrackingLabel.Text = $"{posTxt} / {maxTxt}";
            TrackingLabel.Left = Width / 2 - 10;
            TrackingLabel.BringToFront();
        }


        /// <summary>
        /// Sets the maximum seconds.
        /// </summary>
        /// <param name="value">The value.</param>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task SetMaxSecondsAsync(int value)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (_isLocked) return;

            MaxSeconds = value;
        }


        /// <summary>
        /// Handles the MouseDown event of the McCurrentPositionTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private async void McCurrentPositionTrackBar_MouseDownAsync(object sender, MouseEventArgs e)
        {
            try
            {
                // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
                await _semaphoreSlim.WaitAsync();

                _isLocked = true;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
                _isLocked = false;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }


        /// <summary>
        /// Handles the MouseUp event of the McCurrentPositionTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private async void McCurrentPositionTrackBar_MouseUpAsync(object sender, MouseEventArgs e)
        {
            try
            {
                // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
                await _semaphoreSlim.WaitAsync();

                _isLocked = false;

                var pos = McCurrentPositionTrackBar.Value;
                var diff = Math.Abs(pos - CurrentSeconds);

                if (diff != 0)
                {
                    var rsp = await McRestService.PositionAsync(pos * 1000);

                    if (!rsp.IsOk)
                        throw new Exception($"Setting play position to {pos} seconds in Media Center failed.");
                }

                if (OwnerControl.CanFocus)
                    OwnerControl.Focus();
                else
                    throw new Exception($"The owner control {OwnerControl.ToString()} cannot get focus from the McControlForm.");
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
                _isLocked = false;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

    }

}
