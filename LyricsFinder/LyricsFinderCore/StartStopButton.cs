using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Start / stop toolstrip type.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ToolStripButton" />
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    internal partial class StartStopToolStripButton : ToolStripButton
    {

        /// <summary>
        /// Occurs when starting.
        /// </summary>
        internal event StartEventHandler Starting;

        /// <summary>
        /// Occurs when stopping.
        /// </summary>
        internal event StopEventHandler Stopping;


        private bool _clicked = false;
        private bool _isRunning = false;

        private string _textStart = "&Start";
        private string _textStop = "&Stop";

        private Bitmap _imageStart = null;
        private Bitmap _imageStop = null;


        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StartStopToolStripButton"/> class.
        /// </summary>
        public StartStopToolStripButton()
        {
            InitializeComponent();

            this.IsRunning = false;
        } // StartStopToolStripButton constructor

        #endregion Constructors and Destructors


        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="StartStopToolStripButton"/> is clicked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clicked; otherwise, <c>false</c>.
        /// </value>
        [Browsable(true)]
        public bool Clicked
        {
            get
            {
                return _clicked;
            }
            set
            {
                _clicked = value;
            }
        }


        /// <summary>
        /// Gets or sets the image start.
        /// </summary>
        /// <value>
        /// The image start.
        /// </value>
        [Browsable(true)]
        [EditorAttribute(typeof(ImageEditor), typeof(UITypeEditor))]
        public Bitmap ImageStart
        {
            get
            {
                return _imageStart;
            }
            set
            {
                _imageStart = value;
            }
        }


        /// <summary>
        /// Gets or sets the image stop.
        /// </summary>
        /// <value>
        /// The image stop.
        /// </value>
        [Browsable(true)]
        [EditorAttribute(typeof(ImageEditor), typeof(UITypeEditor))]
        public Bitmap ImageStop
        {
            get
            {
                return _imageStop;
            }
            set
            {
                _imageStop = value;
            }
        }


        /// <summary>
        /// Gets or sets the start text.
        /// </summary>
        /// <value>
        /// The text start.
        /// </value>
        [Browsable(true)]
        public string TextStart
        {
            get
            {
                return _textStart;
            }
            set
            {
                _textStart = value;
            }
        }


        /// <summary>
        /// Gets or sets the stop text.
        /// </summary>
        /// <value>
        /// The text stop.
        /// </value>
        [Browsable(true)]
        public string TextStop
        {
            get
            {
                return _textStop;
            }
            set
            {
                _textStop = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (value == _isRunning) return;

                SetRunningState(value);

                if (value)
                    this.OnStart(this, new StartStopButtonEventArgs(this._clicked));
                else
                    this.OnStop(this, new StartStopButtonEventArgs(this._clicked));
            }
        }

        #endregion Properties


        #region Public Methods

        /// <summary>
        /// Gets the starting event subscribers.
        /// </summary>
        /// <returns></returns>
        public Delegate[] GetStartingEventSubscribers()
        {
            return Starting?.GetInvocationList() ?? Array.Empty<Delegate>();
        }


        /// <summary>
        /// Gets the stopping event subscribers.
        /// </summary>
        /// <returns></returns>
        public Delegate[] GetStoppingEventSubscribers()
        {
            return Stopping?.GetInvocationList() ?? Array.Empty<Delegate>();
        }


        /// <summary>
        /// Sets the state of the running without trigger any events.
        /// </summary>
        /// <param name="isRunning">if set to <c>true</c> [is running].</param>
        public void SetRunningState(bool isRunning)
        {
            _isRunning = isRunning;

            if (isRunning)
            {
                this.Image = this.ImageStop;
                this.Text = this.TextStop;
            }
            else
            {
                this.Image = this.ImageStart;
                this.Text = this.TextStart;
            }
        }


        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
        } // Start


        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this.IsRunning = false;
        } // Stop

        #endregion Public Methods


        #region Private routines

        /// <summary>
        /// Called when [start].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
        internal virtual void OnStart(object sender, StartStopButtonEventArgs e)
        {
            StartEventHandler handler = Starting;

            // Invokes the delegates.
            handler?.Invoke(this, e);
        } // OnStart


        /// <summary>
        /// Called when [stop].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MediaCenter.LyricsFinder.StartStopButtonEventArgs" /> instance containing the event data.</param>
        internal virtual void OnStop(object sender, StartStopButtonEventArgs e)
        {
            StopEventHandler handler = Stopping;

            // Invokes the delegates.
            handler?.Invoke(this, e);
        } // OnStop


        /// <summary>
        /// Handles the Click event of the StartStopButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StartStopButton_Click(object sender, EventArgs e)
        {
            this._clicked = true;

            this.IsRunning = !this.IsRunning;

            this._clicked = false;
        } // StartStopButton_Click

        #endregion Private routines

    } // class StartStopToolStripButton



    #region Events

    /// <summary>
    /// Start / stop button event arguments.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    [Serializable]
    internal class StartStopButtonEventArgs : EventArgs
    {

        private readonly bool _clicked = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="StartStopButtonEventArgs"/> class.
        /// </summary>
        public StartStopButtonEventArgs()
            :
            base()
        {
        } // StartStopButtonEventArgs constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="StartStopButtonEventArgs"/> class.
        /// </summary>
        /// <param name="isClicked">if set to <c>true</c> [is clicked].</param>
        public StartStopButtonEventArgs(
            bool isClicked)
            :
            this()
        {
            _clicked = isClicked;
        } // StartStopButtonEventArgs constructor


        /// <summary>
        /// Gets a value indicating whether this <see cref="StartStopButtonEventArgs"/> is clicked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clicked; otherwise, <c>false</c>.
        /// </value>
        public bool Clicked
        {
            get
            {
                return _clicked;
            }
            //set
            //{
            //  _clicked = value;
            //}
        }

    } // class StartStopButtonEventArgs

    /// <summary>
    /// Start event handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
    internal delegate void StartEventHandler(object sender, StartStopButtonEventArgs e);

    /// <summary>
    /// Stop event handler.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="StartStopButtonEventArgs"/> instance containing the event data.</param>
    internal delegate void StopEventHandler(object sender, StartStopButtonEventArgs e);

    #endregion Events

}
