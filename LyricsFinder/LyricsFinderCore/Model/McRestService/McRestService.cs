using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.SharedComponents;

/*
 * Documentation for JRiver MediaCenter REST Web service can be found here:
 * https://wiki.jriver.com/index.php/Web_Service_Interface
 */


namespace MediaCenter.LyricsFinder.Model.McRestService
{

    /// <summary>
    /// JRiver MediaCenter REST Web service type.
    /// </summary>
    internal static class McRestService
    {

        private const string MissingTokenMessage = "Token must be specified.";


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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

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
            var rsp = await DoTextRequest(requestUrl);
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
            var url = McWsUrl;
            var sb = new StringBuilder(url);

            if (sb[sb.Length - 1] == '\\')
                sb.Length--;

            if (!(new[] { McCommandEnum.Alive, McCommandEnum.Authenticate }).Contains(command))
                if (McWsToken.IsNullOrEmptyTrimmed())
                    throw new ArgumentException(MissingTokenMessage);

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

                case McCommandEnum.Info:
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

                case McCommandEnum.PlayPause:
                case McCommandEnum.Stop:
                    sb.Append($"/Playback/{command}?Token={McWsToken}");
                    break;

                case McCommandEnum.SetInfo:
                    sb.Append($"/File/{command}?Token={McWsToken}&File={key}&FileType=Key&Formatted=1&Field={field}&Value={value}");
                    break;

                default:
                    throw new NotImplementedException($"CreateRequestUrl for command \"{command}\" is not implemented.");
            }

            var ret = new Uri(SerializableUri.EscapeUriString(sb.ToString()));

            return ret;
        }


        /// <summary>
        /// Sends the request to the MC server and reads the response.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// Complete REST service Web request image.
        /// </returns>
        /// <exception cref="NullReferenceException">Response is null</exception>
        /// <exception cref="Exception">Server error (HTTP {rsp.StatusCode}: {rsp.StatusDescription}</exception>
        /// <exception cref="System.NullReferenceException">Response is null</exception>
        /// <exception cref="System.Exception"></exception>
        private static async Task<Bitmap> DoImageRequest(Uri requestUrl, string userName = "", string password = "")
        {
            Bitmap ret = null;

            var req = WebRequest.Create(requestUrl) as HttpWebRequest;

            if (userName.Length > 0)
                req.Credentials = new NetworkCredential(userName, password);

            try
            {
                using (var rsp = req.GetResponse() as HttpWebResponse)
                {
                    if (rsp == null)
                        throw new NullReferenceException(Helpers.Utility.NullResponseMessage);
                    if (rsp.StatusCode != HttpStatusCode.OK)
                        throw new Exception(Helpers.Utility.HttpWebServerErrorMessage(rsp));

                    using (var rspStream = rsp.GetResponseStream())
                    {
                        ret = new Bitmap(rspStream);
                    }
                }
            }
            catch (WebException ex)
            {
                throw new Exception($"\"The call to the Media Center MCWS failed: \"{ex.Message}\". Request: \"{req.RequestUri.ToString()}\".", ex);
            }

            return ret;
        }


        /// <summary>
        /// Sends the request to the MC server and reads the response.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// Complete REST service Web request string.
        /// </returns>
        public static async Task<string> DoTextRequest(Uri requestUrl, string userName = "", string password = "")
        {
            string ret = null;

            try
            {
                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(userName, password) })
                {
                    using (var client = new HttpClient(handler))
                    {
                        ret = await client.GetStringAsync(requestUrl);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"\"The call to the Media Center MCWS failed: \"{ex.Message}\". Request: \"{requestUrl.ToString()}\".", ex);
            }

            return ret;
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
        public static async Task<McAliveResponse> GetAlive()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Alive);
            var rsp = await DoTextRequest(requestUrl);
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
        public static async Task<McAuthenticationResponse> GetAuthentication()
        {
            var user = McWsUserName;
            var psw = McWsPassword;
            var requestUrl = CreateRequestUrl(McCommandEnum.Authenticate);
            var rsp = await DoTextRequest(requestUrl, user, psw);
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
        public static async Task<McGetImageResponse> GetImage(int key)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.GetImage, key);
            var rsp = await DoImageRequest(requestUrl);
            var ret = new McGetImageResponse(rsp);

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
        public static async Task<McMplResponse> GetPlaylistFiles(int id, string name = "")
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlaylistFiles, id);
            var rsp = await DoTextRequest(requestUrl);
            var ret = new McMplResponse(rsp, id, name);

            return ret;
        }


        /// <summary>
        /// Gets the play list from MC server.
        /// </summary>
        /// <returns>
        ///   <see cref="McMplResponse" /> object.
        /// </returns>
        public static async Task<McPlayListsResponse> GetPlayLists()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlaylistList);
            var rsp = await DoTextRequest(requestUrl);
            var ret = new McPlayListsResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Gets the current "Playing Now" list from MC server.
        /// </summary>
        /// <returns>
        ///   <see cref="McMplResponse" /> object.
        /// </returns>
        public static async Task<McMplResponse> GetPlayNowList()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Playlist);
            var rsp = await DoTextRequest(requestUrl);
            var ret = new McMplResponse(rsp, 0, "Playing Now");

            return ret;
        }


        /// <summary>
        /// Get the playback info.
        /// </summary>
        /// <returns>
        ///   <see cref="McInfoResponse" /> object.
        /// </returns>
        public static async Task<McInfoResponse> Info()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Info);
            var rsp = await DoTextRequest(requestUrl);
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
        public static async Task<McResponse> PlayByIndex(int index)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlayByIndex, index);
            var rsp = await DoTextRequest(requestUrl);
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
        public static async Task<McResponse> PlayByKey(int key)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlayByKey, key);
            var rsp = await DoTextRequest(requestUrl);
            var ret = new McResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Pauses the playing.
        /// </summary>
        /// <returns>
        ///   <see cref="McResponse" /> object.
        /// </returns>
        public static async Task<McResponse> PlayPause()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlayPause);
            var rsp = await DoTextRequest(requestUrl);
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
        public static async Task<McResponse> PlayPlaylist(int id)
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.PlayPlaylist, id);
            var rsp = await DoTextRequest(requestUrl);
            var ret = new McResponse(rsp);

            return ret;
        }


        /// <summary>
        /// Stops the playing.
        /// </summary>
        /// <returns>
        ///   <see cref="McResponse" /> object.
        /// </returns>
        public static async Task<McResponse> PlayStop()
        {
            var requestUrl = CreateRequestUrl(McCommandEnum.Stop);
            var rsp = await DoTextRequest(requestUrl);
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
        public static async Task<McSetInfoResponse> SetInfo(int key, string field, string value)
        {
            // For som reason, the MediaCenter is not satisfied with the token alone...
            var user = McWsUserName;
            var psw = McWsPassword;
            var requestUrl = CreateRequestUrl(McCommandEnum.SetInfo, key, field, value);
            var rsp = await DoTextRequest(requestUrl, user, psw);
            var ret = new McSetInfoResponse(rsp);

            return ret;
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }

}
