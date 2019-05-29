using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediaCenter.LyricsFinder.Model.McRestService;


namespace MediaCenter.LyricsFinder.Model
{

    /// <summary>
    /// BackgroundWorker communcation type.
    /// </summary>
    [Serializable]
    internal class WorkerUserState
    {

        /// <summary>
        /// Gets or sets the index of the current item in <see cref="Items"/>.
        /// </summary>
        /// <value>
        /// The index of the current item.
        /// </value>
        public int CurrentItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the current item.
        /// </summary>
        /// <value>
        /// The current item.
        /// </value>
        public McMplItem CurrentItem { get; set; }

        /// <summary>
        /// Gets or sets the current playlist items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public Dictionary<int, McMplItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the lyrics result.
        /// </summary>
        /// <value>
        /// The lyrics result.
        /// </value>
        public LyricResultEnum LyricsStatus { get; set; }

        /// <summary>
        /// Gets or sets the lyrics text.
        /// </summary>
        /// <value>
        /// The lyrics text.
        /// </value>
        public List<string> LyricsTextList { get; set; }

        /// <summary>
        /// Gets or sets the message to report back to the GUI thread.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets whether existing lyrics should be overwritten or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if existing lyrics should be overwritten; else <c>false</c>.
        /// </value>
        public bool OverwriteLyrics { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerUserState"/> class.
        /// </summary>
        public WorkerUserState()
        {
            Items = new Dictionary<int, McMplItem>();
            LyricsStatus = LyricResultEnum.NotProcessedYet;
            LyricsTextList = new List<string>();
        }

    }

}
