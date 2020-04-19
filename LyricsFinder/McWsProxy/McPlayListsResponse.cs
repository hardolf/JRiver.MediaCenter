using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MediaCenter.McWs
{

    /// <summary>
    /// Playlist list.
    /// </summary>
    [Serializable, XmlType("Response")]
    public class McPlayListsResponse
    {

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ok.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ok; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public virtual bool IsOk { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [XmlIgnore]
        public virtual Dictionary<string, McPlayListType> PlayLists { get; } = new Dictionary<string, McPlayListType>();

        [XmlAttribute("Status")]
        public string Status
        {
            get
            {
                return (IsOk) ? "OK" : "";
            }
            set
            {
                IsOk = (value != null) && (value.Equals("OK", StringComparison.InvariantCultureIgnoreCase) ? true : false);
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="McPlayListsResponse"/> class.
        /// </summary>
        protected McPlayListsResponse()
        {
        }


        /// <summary>
        /// Creates the mc play lists response.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns><see cref="McPlayListsResponse"/> object.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async Task<McPlayListsResponse> CreateMcPlayListsResponseAsync(string xml)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var ret = new McPlayListsResponse();
            var xDoc = new XmlDocument() { XmlResolver = null };

            using (var sReader = new StringReader(xml))
            using (var xReader = XmlReader.Create(sReader, new XmlReaderSettings() { XmlResolver = null }))
                xDoc.Load(xReader);

            var xmlRoot = xDoc.DocumentElement;

            ret.Status = xmlRoot.GetAttribute("Status");

            var xItems = xmlRoot.GetElementsByTagName("Item");

            foreach (XmlElement xItem in xItems)
            {
                var id = string.Empty;
                var name = string.Empty;
                var path = string.Empty;
                var type = string.Empty;
                var fields = xItem.GetElementsByTagName("Field");

                foreach (XmlElement field in fields)
                {
                    var fName = field.GetAttribute("Name");
                    var fValue = field.InnerText;

                    switch (fName.ToUpperInvariant())
                    {
                        case "ID":
                            id = fValue;
                            break;

                        case "NAME":
                            name = fValue;
                            break;

                        case "PATH":
                            path = fValue;
                            break;

                        case "TYPE":
                            type = fValue;
                            break;

                        default:
                            break;
                    }
                }

                var item = new McPlayListType
                {
                    Id = id,
                    Name = name,
                    Path = path,
                    Type = type
                };

                if (!type.Equals("GROUP", StringComparison.InvariantCultureIgnoreCase))
                    ret.PlayLists.Add(id, item);
            }

            return ret;
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;

            var rsp = obj as McPlayListsResponse;
            var ret = (rsp != null) &&
                (IsOk == rsp.IsOk) &&
                (PlayLists.Count == rsp.PlayLists.Count) &&
                (Status == rsp.Status);

            if (ret && (rsp != null))
            {
                for (int i = 0; i < rsp.PlayLists.Count; i++)
                {
                    if (PlayLists.ElementAt(i).Key.Equals(rsp.PlayLists.ElementAt(i).Key, StringComparison.InvariantCultureIgnoreCase)
                        && PlayLists.ElementAt(i).Value.Equals(rsp.PlayLists.ElementAt(i).Value))
                        continue;
                }
            }

            return ret;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 1337987077;

            hashCode = hashCode * -1521134295 + IsOk.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, McPlayListType>>.Default.GetHashCode(PlayLists);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Status);

            return hashCode;
        }


        /// <summary>
        /// Gets the items playlists.
        /// </summary>
        /// <param name="currentItemIds">The Media Center items currently in LyricsFinder.</param>
        /// <param name="playListTypes">Playlist types to include.</param>
        /// <param name="playListPathFilters">Any playlist paths to exclude. Only part of the path needs to be specified.</param>
        /// <returns>
        /// Sorted dictionary with item keys as keys and sorted dictionaries of the items' playlists as values.
        /// </returns>
        public async Task<Dictionary<int, List<int>>> GetItemsPlaylistsAsync(List<int> currentItemIds, IEnumerable<string> playListTypes = null, IEnumerable<string> playListPathFilters = null)
        {
            var ret = new Dictionary<int, List<int>>();
            var playLists = PlayLists.Where(p => true); // All playlists, subject to later filtering
            var tasks = new List<Task>();

            // Filter by allowed playlist types
            if (playListTypes != null)
                playLists = playLists.Where(p => playListTypes.Any(f => p.Value.Type.Equals(f, StringComparison.InvariantCultureIgnoreCase)));

            // Filter by disallowed playlist path segments
            if (playListPathFilters != null)
                playLists = playLists.Where(p => !playListPathFilters.Any(f => p.Value.Path.ToUpperInvariant().Contains(f.ToUpperInvariant())));

            // Create all the tasks
            foreach (var kvp in playLists)
            {
                tasks.Add(kvp.Value.GetItemPlaylistsAsync(ret, currentItemIds));
            }

            // Run the tasks in parallel
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return ret;
        }

    }



    /// <summary>
    /// Playlist information type.
    /// </summary>
    [Serializable]
    public class McPlayListType
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [XmlElement]
        public string Id { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Path { get; set; }

        [XmlElement]
        public string Type { get; set; }

        [XmlIgnore]
        public List<int> ItemKeys { get; } = new List<int>();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


        /// <summary>
        /// Gets the unsort key.
        /// </summary>
        /// <value>
        /// The unsort key.
        /// </value>
        public string Key
        {
            get
            {
                return Id;
            }
        }


        /// <summary>
        /// Gets the sort key.
        /// </summary>
        /// <value>
        /// The sort key.
        /// </value>
        public string SortKey
        {
            get
            {
                return string.Join("|", Path, Id);
            }
        }


        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is McPlayListType type &&
                   Id == type.Id &&
                   Name == type.Name &&
                   Path == type.Path &&
                   Type == type.Type;
        }


        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 265471897;

            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);

            return hashCode;
        }


        /// <summary>
        /// Gets the items' playlist IDs.
        /// </summary>
        /// <param name="itemsPlaylists">The items' playlist IDs.</param>
        /// <param name="currentItemIds">The current items in LyricsFinder.</param>
        /// <exception cref="ArgumentNullException">itemsPlaylists</exception>
        public async Task GetItemPlaylistsAsync(Dictionary<int, List<int>> itemsPlaylists, List<int> currentItemIds)
        {
            if (itemsPlaylists is null) throw new ArgumentNullException(nameof(itemsPlaylists));
            if (currentItemIds is null) throw new ArgumentNullException(nameof(currentItemIds));

            var playlistId = int.Parse(Id, CultureInfo.InvariantCulture);
            var rsp = await McRestService.GetPlaylistFilesAsync(int.Parse(Id, CultureInfo.InvariantCulture)).ConfigureAwait(false);
            var mcPlayListFileItems = rsp.Items.Where(i => currentItemIds.Any(c => c == i.Key));

            foreach (var mcFileItem in mcPlayListFileItems)
            {
                if (itemsPlaylists.TryGetValue(mcFileItem.Key, out var itemList))
                    itemList.Add(playlistId);
                else
                {
                    itemList = new List<int>
                    {
                        { playlistId }
                    };

                    itemsPlaylists.Add(mcFileItem.Key, itemList);
                }
            }
        }

    }

}
