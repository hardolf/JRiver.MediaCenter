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

        private const int _mcPositionTrackBarMouseBufferZone = 5; // Seconds

        private bool _isLocked = false;

        private int _currentSeconds = 0;

        // Instantiate a Singleton of the Semaphore with a value of 1. 
        // This means that only 1 thread can be granted access at a time. 
        // Source: https://blog.cdemi.io/async-waiting-inside-c-sharp-locks/
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);


        /**********************/
        /***** Properties *****/
        /**********************/

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
        /// Gets or sets the index of the current item.
        /// </summary>
        /// <value>
        /// The index of the current item.
        /// </value>
        public int CurrentItems { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the control has input focus.
        /// </summary>
        public override bool Focused
        {
            get
            {
                var ret = base.Focused || TrackingLabel.Focused || McPositionTrackBar.Focused
                    || McControlLeftToolStrip.Focused || McControlToolStripContainer.Focused;

                return ret;
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
        /// Gets or sets the maximum number of items.
        /// </summary>
        /// <value>
        /// The maximum number of items.
        /// </value>
        public int MaxItems { get; private set; }

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


        /************************/
        /***** Constructors *****/
        /************************/

        /// <summary>
        /// Prevents a default instance of the <see cref="McPlayControl"/> class from being created.
        /// </summary>
        private McPlayControl()
        {
            InitializeComponent();

            ToolsPlayStartStopButton.Starting += ToolsPlayStartStopButton_StartAsync;
            ToolsPlayStartStopButton.Stopping += ToolsPlayStartStopButton_StopAsync;
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


        /********************************/
        /***** Methods and delegates*****/
        /********************************/

        /// <summary>
        /// Jump the playing position asynchronous.
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

            var rsp = await McRestService.PositionAsync(newPos * 1000);

            if (!rsp.IsOk)
                throw new Exception($"Setting play position to {pos} seconds in Media Center failed.");
        }


        /// <summary>
        /// Jumps the playing position to the end asynchronous.
        /// </summary>
        public async Task JumpEndAsync()
        {

            var rsp = await McRestService.PositionAsync(McPositionTrackBar.Maximum * 1000);

            if (!rsp.IsOk)
                throw new Exception($"Setting play position to the track end in Media Center failed.");
        }


        /// <summary>
        /// Jumps the playing position to the beginning asynchronous.
        /// </summary>
        public async Task JumpBeginningAsync()
        {

            var rsp = await McRestService.PositionAsync(McPositionTrackBar.Minimum * 1000);

            if (!rsp.IsOk)
                throw new Exception($"Setting play position to the track beginning in Media Center failed.");
        }


        /// <summary>
        /// Handles the MouseEnter event of the McControlLeftToolStrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void McControlLeftToolStrip_MouseEnterAsync(object sender, EventArgs e)
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
        /// Handles the MouseLeave event of the McControlLeftToolStrip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void McControlLeftToolStrip_MouseLeaveAsync(object sender, EventArgs e)
        {
            try
            {
                if (OwnerControl.CanFocus)
                    OwnerControl.Focus();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
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
                SetMaxState(0, 0); // Set this before the current seconds
                SetCurrentState(0, 0);

                McPositionTrackBar.Top = 0;

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
        /// Handles the KeyDown event of the McPositionTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// We don't handle key operations in the trackbar, we let the parent controls do that.
        /// </remarks>
        private async void McPositionTrackBar_KeyDownAndUpAsync(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Handles the KeyPressAsync event of the McPositionTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// We don't handle key operations in the trackbar, we let the parent controls do that.
        /// </remarks>
        private async void McPositionTrackBar_KeyPressAsync(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
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
        /// Handles the MouseLeave event of the McPlayControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void McPlayControl_MouseLeaveAsync(object sender, EventArgs e)
        {
            try
            {
                if (OwnerControl.CanFocus)
                    OwnerControl.Focus();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
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

                // Current trackbar position
                var currentPosInt = McPositionTrackBar.Value;

                // Jump to the clicked location (in seconds) but keep a buffer zone of +/- _mcPositionTrackBarMouseBufferZone/2 seconds
                var newPosDbl = ((double)e.X / (double)McPositionTrackBar.Width) * (McPositionTrackBar.Maximum - McPositionTrackBar.Minimum);
                var newPosInt = Convert.ToInt32(newPosDbl);
                var newDiff = Math.Abs(newPosInt - CurrentSeconds);

                // Change only if outside the trackbar buffer zone
                if (newDiff > _mcPositionTrackBarMouseBufferZone / 2)
                {
                    var rsp = await McRestService.PositionAsync(newPosInt * 1000); // milliseconds

                    if (!rsp.IsOk)
                        throw new Exception($"Setting play position to {currentPosInt} seconds in Media Center failed.");
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
        /// Handles the PreviewKeyDown event of the McPositionTrackBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PreviewKeyDownEventArgs"/> instance containing the event data.</param>
        private async void McPositionTrackBar_PreviewKeyDownAsync(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Tab:
                        e.IsInputKey = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }


        /// <summary>
        /// Pauses the playback.
        /// </summary>
        public void Pause()
        {
            ToolsPlayStartStopButton.Stop();
        }


        /// <summary>
        /// Sets the status to playback paused without raising any events.
        /// </summary>
        public void PauseStat()
        {
            ToolsPlayStartStopButton.SetRunningState(false);
        }


        /// <summary>
        /// Starts the playback.
        /// </summary>
        public void Play()
        {
            ToolsPlayStartStopButton.Start();
        }


        /// <summary>
        /// Sets the status to playback started without raising any events.
        /// </summary>
        public void PlayStat()
        {
            ToolsPlayStartStopButton.SetRunningState(true);
        }


        /// <summary>
        /// Sets the current seconds.
        /// </summary>
        /// <param name="currentSeconds">The current seconds.</param>
        /// <param name="currentItems">The current items.</param>
        public void SetCurrentState(int currentSeconds, int currentItems)
        {
            const long secTicks = 10000000; // 1 tick is 100 ns

            if (_isLocked) return;

            CurrentSeconds = currentSeconds;
            CurrentItems = currentItems;

            long maxTicks = MaxSeconds * secTicks;
            var max = new TimeSpan(maxTicks);
            var maxTxt = (max.Hours > 0) ? $"{max.Hours:###0}:{max.Minutes:00}:{max.Seconds:00}" : $"{max.Minutes:#0}:{max.Seconds:00}";

            long posTicks = currentSeconds * secTicks;
            var pos = new TimeSpan(posTicks);
            var posTxt = (pos.Hours > 0) ? $"{pos.Hours:###0}:{pos.Minutes:00}:{pos.Seconds:00}" : $"{pos.Minutes:#0}:{pos.Seconds:00}";

            TrackingLabel.Text = $"{posTxt} / {maxTxt} - {CurrentItems} of {MaxItems}";
            TrackingLabel.Left = McPositionTrackBar.Width / 2 - TrackingLabel.Width / 2;
            TrackingLabel.BringToFront();

            Visible = (MaxItems > 0);
        }


        /// <summary>
        /// Sets the maximum seconds.
        /// </summary>
        /// <param name="maxSeconds">The maximum sec onds.</param>
        /// <param name="maxItems">The maximum items.</param>
        public void SetMaxState(int maxSeconds, int maxItems)
        {
            if (_isLocked) return;

            MaxSeconds = maxSeconds;
            MaxItems = maxItems;

            Visible = (MaxItems > 0);
        }


        /// <summary>
        /// Stops the playback.
        /// </summary>
        public void Stop()
        {
            ToolsPlayStartStopButton.Stop();
        }


        /// <summary>
        /// Sets the status to playback stopped without raising any events.
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

                if (OwnerControl.CanFocus)
                    OwnerControl.Focus();
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

                if (OwnerControl.CanFocus)
                    OwnerControl.Focus();
            }
            catch (Exception ex)
            {
                await ErrorHandling.ShowAndLogErrorHandlerAsync($"Error in {SharedComponents.Utility.GetActualAsyncMethodName()} event.", ex);
            }
        }

    }

}
