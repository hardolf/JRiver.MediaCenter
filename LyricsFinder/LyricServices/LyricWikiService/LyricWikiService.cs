using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.LyricServices.LyricWikiServiceReference;
using MediaCenter.LyricsFinder.Model.LyricServices.Properties;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// LyricWiki lyrics service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class LyricWikiService : AbstractLyricService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LyricWikiService" /> class.
        /// </summary>
        public LyricWikiService()
        {
            IsImplemented = false;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="getAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns></returns>
        /// <exception cref="CommunicationException"></exception>
        public override AbstractLyricService Process(McMplItem item, bool getAll = false)
        {
            base.Process(item); // Result: not found

            LyricWikiPortType client = null;

            try
            {
                client = CreateServiceClient<LyricWikiPortType>("LyricWikiPort");

                var rsp = client.getSong(item.Artist, item.Name);

                if ((rsp != null) && (!rsp.lyrics.IsNullOrEmptyTrimmed()))
                {
                    AddFoundLyric(rsp.lyrics, new SerializableUri(rsp.url));
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (client != null)
                        ((ICommunicationObject)client).Abort();
                }
                catch { /* I don't care */ }

                throw new CommunicationException($"Failed to get info from \"{Credit.ServiceName}\".", ex);
            }

            return this;
        }

    }

}
