using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using MediaCenter.SharedComponents;

/*
 * Documentation for JRiver MediaCenter REST Web service can be found here:
 * https://wiki.jriver.com/index.php/Web_Service_Interface
 * and on the installed Media Center:
 * http://localhost:52199/MCWS/v1/
 * 
 */


namespace MediaCenter.McWs
{

    /// <summary>
    /// JRiver MediaCenter REST Web service type.
    /// </summary>
    public static class McRestService
    {

        private const string _missingTokenMessage = "Token must be specified.";


        /// <summary>
        /// Gets the MediaCenter MCWS access key.
        /// </summary>
        /// <value>
        /// The MediaCenter MCWS access key.
        /// </value>
        public static string McWsAccessKey { get; private set; }

        /// <summary>
        /// Gets the MediaCenter MCWS password.
        /// </summary>
        /// <value>
        /// The MediaCenter MCWS password.
        /// </value>
        public static string McWsPassword { get; private set; }

        /// <summary>
        /// Gets the MediaCenter MCWS URL.
        /// </summary>
        /// <value>
        /// The MediaCenter MCWS URL.
        /// </value>
        public static string McWsUrl { get; private set; }

        /// <summary>
        /// Gets the MediaCenter MCWS user name.
        /// </summary>
        /// <value>
        /// The MediaCenter MCWS user name.
        /// </value>
        public static string McWsUserName { get; private set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public static string McWsToken { get; private set; }

        /*
        /// <summary>
        /// Adds to playing now.
        /// </summary>
        /// <returns>
        ///   <see cref="McResponse" /> object.
        /// </returns>
        public static async Task<McResponse> AddToPlayingNow(int key)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.AddToPlayingNow, key);
            var rsp = await Utility.DoTextRequestAsync(requestUrl).ConfigureAwait(false);
            var ret = new McResponse(rsp);

            return ret;
        }
        */


        /// <summary>
        /// Creates the request URL for MC server.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="key">The key.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// MediaCenter REST service request URL string.
        /// </returns>
        /// <exception cref="Exception">Token must be specified.
        /// or
        /// Token must be specified.</exception>
        /// <exception cref="NotImplementedException">CreateRequestUrl for command \"{command}</exception>
        /// <exception cref="ArgumentException">id must be specified.</exception>
        private static Uri CreateRequestUrl(McCommandEnum command, int key = -1, string field = null, string value = null)
        {
            var sb = new StringBuilder(McWsUrl);

            if ((sb[sb.Length - 1] == '\\')
                || (sb[sb.Length - 1] == '/'))
                sb.Length--;

            if (!(new[] { McCommandEnum.Alive, McCommandEnum.Authenticate }).Contains(command)
                && McWsToken.IsNullOrEmptyTrimmed())
                    throw new ArgumentException(_missingTokenMessage);

            switch (command)
            {
                case McCommandEnum.AddToPlayingNow:
                    sb.Append($"/Playlist/AddFile?Token={McWsToken}&PlaylistType=ID&FileType=Key&File={key}");
                    break;

                case McCommandEnum.Alive:
                case McCommandEnum.Authenticate:
                    sb.Append($"/{command}");
                    break;

                case McCommandEnum.GetImage:
                    sb.Append($"/File/{command}?Token={McWsToken}&File={key}&FileType=Key&Type=Thumbnail&ThumbnailSize=Large&Format=jpg");
                    break;

                case McCommandEnum.GetInfo:
                    sb.Append($"/File/{command}?Token={McWsToken}&Action=MPL&File={value}");
                    break;

                case McCommandEnum.GetInfoFull:
                    sb.Append($"/File/GetInfo?Token={McWsToken}&Action=MPL&Fields=Calculated&File={value}");
                    break;

                case McCommandEnum.Info:
                case McCommandEnum.PlayPause:
                case McCommandEnum.Stop:
                    sb.Append($"/Playback/{command}?Token={McWsToken}");
                    break;

                case McCommandEnum.PlayByIndex:
                    sb.Append($"/Playback/{command}?Token={McWsToken}&Index={key}");
                    break;

                case McCommandEnum.PlayByKey:
                    sb.Append($"/Playback/{command}?Token={McWsToken}&Key={key}");
                    break;

                case McCommandEnum.Playlist:
                    sb.Append($"/Playback/{command}?Token={McWsToken}&Action=MPL");
                    break;

                case McCommandEnum.PlaylistFiles:
                    sb.Append($"/Playlist/Files?Token={McWsToken}&PlaylistType=ID&Action=MPL&Playlist={key}");
                    break;

                case McCommandEnum.PlaylistList:
                    sb.Append($"/Playlists/List?Token={McWsToken}&Action=MPL");
                    break;

                case McCommandEnum.PlayPlaylist:
                    sb.Append($"/Playback/{command}?Token={McWsToken}&Playlist={key}");
                    break;

                case McCommandEnum.Position:
                    if (key < -1)
                        sb.Append($"/Playback/{command}?Token={McWsToken}");
                    else
                        sb.Append($"/Playback/{command}?Token={McWsToken}&Position={key}");

                    if (!field.IsNullOrEmptyTrimmed())
                    {
                        var rel = int.Parse(field, CultureInfo.InvariantCulture);

                        if (rel != 0)
                            sb.Append($"&Relative={rel}");
                    }
                    break;

                case McCommandEnum.SetInfo:
                    sb.Append($"/File/{command}?Token={McWsToken}&File={key}&FileType=Key&Formatted=1&Field={field}&Value={value}");
                    break;

                default:
                    throw new NotImplementedException($"CreateRequestUrl for command \"{command}\" is not implemented.");
            }

            var ret = new UriBuilder(sb.ToString());

            return ret.Uri;
        }


        /// <summary>
        /// Gets authentication from MC server.
        /// </summary>
        /// <returns>
        ///   <see cref="McAliveResponse" /> object.
        /// </returns>
        /// <remarks>
        /// This must be the first command to the MediaCenter Web Service.
        /// </remarks>
        public static async Task<McAliveResponse> GetAliveAsync()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Alive);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = new McAliveResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Gets authentication from MC server.
        /// </summary>
        /// <returns>
        ///   <see cref="McAuthenticationResponse" /> object.
        /// </returns>
        /// <remarks>
        /// <para>This must be the second command to the MediaCenter Web Service.</para>
        /// <para>For subsequent requests, the returned authentication token is taken from the static <see cref="McWsToken" /> property.</para>
        /// </remarks>
        public static async Task<McAuthenticationResponse> GetAuthenticationAsync()
        {
            var user = McWsUserName;
            var psw = McWsPassword;
            var requestUrl = CreateRequestUrl(McCommandEnum.Authenticate);
            var rsp = await Utility.HttpGetStringAsync(requestUrl, user, psw).ConfigureAwait(false);
            var ret = new McAuthenticationResponse(rsp);

            McWsToken = ret.Token;

            return ret;
        }


        /// <summary>
        /// Gets the image from MC server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <see cref="McGetImageResponse" /> object.
        /// </returns>
        public static async Task<McGetImageResponse> GetImageAsync(int key)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.GetImage, key);
            var rsp = await Utility.HttpGetImageAsync(requestUrl).ConfigureAwait(false);
            var ret = new McGetImageResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Get the playback info.
        /// </summary>
        /// <param name="fileKey">The file key.</param>
        /// <param name="includeCalculated">if set to <c>true</c> include calculated fields; else do not.</param>
        /// <returns>
        ///   <see cref="McInfoResponse" /> object.
        /// </returns>
        public static async Task<McMplResponse> GetInfoAsync(string fileKey, bool includeCalculated = false)
        {
            var cmd = (includeCalculated) ? McCommandEnum.GetInfoFull : McCommandEnum.GetInfo;
            var requestUrl = CreateRequestUrl(cmd, value: fileKey);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = await McMplResponse.CreateMcMplResponseAsync(rsp).ConfigureAwait(false);

            return ret;
        }


        /// <summary>
        /// Gets the a playlist from MC server.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name, optional.</param>
        /// <returns>
        ///   <see cref="McMplResponse" /> object.
        /// </returns>
        public static async Task<McMplResponse> GetPlaylistFilesAsync(int id, string name = "")
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlaylistFiles, id);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = await McMplResponse.CreateMcMplResponseAsync(rsp, id, name).ConfigureAwait(false);

            return ret;
        }


        /// <summary>
        /// Gets the play list from MC server.
        /// </summary>
        /// <returns>
        ///   <see cref="McMplResponse" /> object.
        /// </returns>
        public static async Task<McPlayListsResponse> GetPlayListsAsync()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlaylistList);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = await McPlayListsResponse.CreateMcPlayListsResponseAsync(rsp).ConfigureAwait(false);

            return ret;
        }


        /// <summary>
        /// Gets the current "Playing Now" list from MC server.
        /// </summary>
        /// <returns>
        ///   <see cref="McMplResponse" /> object.
        /// </returns>
        public static async Task<McMplResponse> GetPlayNowListAsync()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Playlist);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = await McMplResponse.CreateMcMplResponseAsync(rsp, 0, "Playing Now").ConfigureAwait(false);

            return ret;
        }


        /// <summary>
        /// Get the playback info.
        /// </summary>
        /// <returns>
        ///   <see cref="McInfoResponse" /> object.
        /// </returns>
        public static async Task<McInfoResponse> InfoAsync()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Info);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = new McInfoResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Initializes this instance with the specified MediaCenter MCWS parameters.
        /// </summary>
        /// <param name="mcWebServiceAccessKey">The MediaCenter MCWS access key.</param>
        /// <param name="mcWebServiceUrl">The MediaCenter MCWS URL.</param>
        /// <param name="mcWsUserName">Name of the MediaCenter MCWS user.</param>
        /// <param name="mcWsPassword">The MediaCenter MCWS password.</param>
        /// <exception cref="ArgumentNullException">Parameter must be specified: {nameof(mcWebServiceAccessKey)}.
        /// or
        /// Parameter must be specified: {nameof(mcWebServiceUrl)}.
        /// or
        /// Parameter must be specified: {nameof(mcWsUserName)}.
        /// or
        /// Parameter must be specified: {nameof(mcWsPassword)}.</exception>
        public static void Init(string mcWebServiceAccessKey, string mcWebServiceUrl, string mcWsUserName, string mcWsPassword)
        {
            if (mcWebServiceAccessKey.IsNullOrEmptyTrimmed()) throw new ArgumentNullException($"Parameter must be specified: {nameof(mcWebServiceAccessKey)}.");
            if (mcWebServiceUrl.IsNullOrEmptyTrimmed()) throw new ArgumentNullException($"Parameter must be specified: {nameof(mcWebServiceUrl)}.");
            if (mcWsUserName.IsNullOrEmptyTrimmed()) throw new ArgumentNullException($"Parameter must be specified: {nameof(mcWsUserName)}.");
            if (mcWsPassword.IsNullOrEmptyTrimmed()) throw new ArgumentNullException($"Parameter must be specified: {nameof(mcWsPassword)}.");

            McWsAccessKey = mcWebServiceAccessKey;
            McWsUrl = mcWebServiceUrl;
            McWsUserName = mcWsUserName;
            McWsPassword = mcWsPassword;
        }


        /// <summary>
        /// Plays the item in the Playing Now list by the index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <see cref="McResponse" /> object.
        /// </returns>
        public static async Task<McResponse> PlayByIndexAsync(int index)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlayByIndex, index);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = new McResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Plays the item by the file key.
        /// </summary>
        /// <param name="key">The file key.</param>
        /// <returns>
        ///   <see cref="McResponse" /> object.
        /// </returns>
        public static async Task<McResponse> PlayByKeyAsync(int key)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlayByKey, key);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = new McResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Pauses the playing.
        /// </summary>
        /// <returns>
        ///   <see cref="McResponse" /> object.
        /// </returns>
        public static async Task<McResponse> PlayPauseAsync()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlayPause);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = new McResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Plays the playlist with the specified ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <see cref="McResponse" /> object.
        /// </returns>
        public static async Task<McResponse> PlayPlaylistAsync(int id)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlayPlaylist, id);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = new McResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Stops the playing.
        /// </summary>
        /// <returns>
        ///   <see cref="McResponse" /> object.
        /// </returns>
        public static async Task<McResponse> PlayStopAsync()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Stop);
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = new McResponse(rsp);

            return ret;
        }


        public static async Task<McResponse> PositionAsync(int? position = null, int? relative = null)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Position, position ?? int.MinValue, relative?.ToString(CultureInfo.InvariantCulture));
            var rsp = await Utility.HttpGetStringAsync(requestUrl).ConfigureAwait(false);
            var ret = new McResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Sets the information of an item to the MC server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <see cref="McSetInfoResponse" /> object.
        /// </returns>
        public static async Task<McSetInfoResponse> SetInfoAsync(int key, string field, string value)
        {
            // For som reason, the MediaCenter is not satisfied with the token alone...
            var user = McWsUserName;
            var psw = McWsPassword;
            var requestUrl = CreateRequestUrl(McCommandEnum.SetInfo, key, field, value);
            var rsp = await Utility.HttpGetStringAsync(requestUrl, user, psw).ConfigureAwait(false);
            var ret = new McSetInfoResponse(rsp);

            return ret;
        }

    }

}
