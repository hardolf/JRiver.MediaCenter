using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Media Center play control.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class McPlayControl : UserControl
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
        [Browsable(false)]
        public int CurrentSeconds
        {
            get { return _currentSeconds; }
            private set
            {
                _currentSeconds = value;
                McPositionTrackBar.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the large change.
        /// </summary>
        /// <value>
        /// The large change.
        /// </value>
        [Browsable(true), Description("The number of seconds to jump when making a large jump.")]
        public int LargeChange
        {
            get => McPositionTrackBar.LargeChange;
            private set => McPositionTrackBar.LargeChange = value;
        }


        /// <summary>
        /// Gets or sets the lyrics finder core.
        /// </summary>
        /// <value>
        /// The lyrics finder core.
        /// </value>
        [Browsable(false)]
        private LyricsFinderCore LyricsFinderCore { get; set; }

        /// <summary>
        /// Gets or sets the maximum seconds.
        /// </summary>
        /// <value>
        /// The maximum seconds.
        /// </value>
        [Browsable(true)]
        public int MaxSeconds
        {
            get => McPositionTrackBar.Maximum;
            private set => McPositionTrackBar.Maximum = value;
        }

        /// <summary>
        /// Gets or sets the owner control.
        /// </summary>
        /// <value>
        /// The owner control.
        /// </value>
        [Browsable(false)]
        private Control OwnerControl { get; set; }

        /// <summary>
        /// Gets or sets the small change.
        /// </summary>
        /// <value>
        /// The small change.
        /// </value>
        [Browsable(true), Description("The number of seconds to jump when making a small jump.")]
        public int SmallChange
        {
            get => McPositionTrackBar.SmallChange;
            private set => McPositionTrackBar.SmallChange = value;
        }


        /// <summary>
        /// Prevents a default instance of the <see cref="McPlayControl"/> class from being created.
        /// </summary>
        private McPlayControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McPlayControl"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="lyricsFinderCore">The lyrics finder core.</param>
        public McPlayControl(Control owner, LyricsFinderCore lyricsFinderCore)
            : this()
        {
            OwnerControl = owner;
            LyricsFinderCore = lyricsFinderCore;
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

            var pos = McPositionTrackBar.Value;
            var diff = (isLargeJump) ? LargeChange : SmallChange;

            if (diff == 0)
                return;

            var newPos = (isBackward) ? pos - diff : pos + diff;

            if (newPos < 0)
                newPos = 0;

            if (newPos > McPositionTrackBar.Maximum)
                newPos = McPositionTrackBar.Maximum;

            var rsp = await McRestService.PositionAsync((int) (newPos * 1000));

            if (!rsp.IsOk)
                throw new Exception($"Setting play position to {pos} seconds in Media Center failed.");
        }


        /// <summary>
        /// Handles the MouseDown event of the McCurrentPositionTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private async void McPositionTrackBar_MouseDownAsync(object sender, MouseEventArgs e)
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
        private async void McPositionTrackBar_MouseUpAsync(object sender, MouseEventArgs e)
        {
            try
            {
                // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
                await _semaphoreSlim.WaitAsync();

                _isLocked = false;

                var pos = McPositionTrackBar.Value;
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


        /// <summary>
        /// Handles the Load event of the McPlayControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void McPlayControl_LoadAsync(object sender, EventArgs e)
        {
            try
            {
                await SetMaxSecondsAsync(0); // Set this before the current seconds
                await SetCurrentSecondsAsync(0);

                if (ToolsPlayStartStopButton.GetStartingEventSubscribers().Length == 0)
                    ToolsPlayStartStopButton.Starting += ToolsPlayStartStopButton_StartAsync;

                if (ToolsPlayStartStopButton.GetStoppingEventSubscribers().Length == 0)
                    ToolsPlayStartStopButton.Stopping += ToolsPlayStartStopButton_StopAsync;

                TrackingLabel.Left = McPositionTrackBar.Width / 2 - TrackingLabel.Width / 2;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }




        /// <summary>
        /// Handles the MouseEnter event of the McPlayControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void McPlayControl_MouseEnterAsync(object sender, EventArgs e)
        {
            try
            {
                this.Focus();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the MouseEnter event of the McControlLeftToolStrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void McControlLeftToolStrip_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }


        /// <summary>
        /// Pauses the playback.
        /// </summary>
        public void PauseStat()
        {
            ToolsPlayStartStopButton.SetRunningState(false);
        }


        /// <summary>
        /// Start or resume playback.
        /// </summary>
        public void PlayStat()
        {
            ToolsPlayStartStopButton.SetRunningState(true);
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

            var max = new TimeSpan(McPositionTrackBar.Maximum * 10000000);
            var maxTxt = (max.TotalMinutes > 59) ? $"{max.TotalHours:###0}:{max.Minutes:00}:{max.Seconds:00}" : $"{max.TotalMinutes:#0}:{max.Seconds:00}";

            var pos = new TimeSpan(value * 10000000);
            var posTxt = (max.TotalMinutes > 59) ? $"{pos.TotalHours:###0}:{pos.Minutes:00}:{pos.Seconds:00}" : $"{pos.TotalMinutes:#0}:{pos.Seconds:00}";

            TrackingLabel.Text = $"{posTxt} / {maxTxt}";
            TrackingLabel.Left = McPositionTrackBar.Width / 2 - TrackingLabel.Width / 2;
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
        /// Stops the playback.
        /// </summary>
        public void StopStat()
        {
            ToolsPlayStartStopButton.SetRunningState(false);
        }


        /// <summary>
        /// Handles the Starting event of the ToolsPlayStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void ToolsPlayStartStopButton_StartAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                await LyricsFinderCore.PlayOrPauseAsync();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Starting event of the ToolsPlayStartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        private async void ToolsPlayStartStopButton_StopAsync(object sender, StartStopButtonEventArgs e)
        {
            try
            {
                await LyricsFinderCore.PlayOrPauseAsync();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }

    }

}
