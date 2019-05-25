﻿using System;
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

using MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference;
using MediaCenter.LyricsFinder.Model.LyricServices.Properties;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Chart Lyrics service type.
    /// </summary>
    [Serializable]
    public class ChartLyricsService : AbstractLyricService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLyricService"/> class.
        /// </summary>
        public ChartLyricsService()
            : base()
        {
            IsImplemented = true;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="getAll">If set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns></returns>
        public override AbstractLyricService Process(McMplItem item, bool getAll = false)
        {
            base.Process(item); // Result: not found

            apiv1Soap client = null;

            try
            {
                client = CreateServiceClient<apiv1Soap>("apiv1Soap");

                var rsp1 = client.SearchLyric(item.Artist, item.Name);

                if ((rsp1 != null) && (rsp1.Length > 0))
                {
                    foreach (var rspLyricResult in rsp1)
                    {
                        if (rspLyricResult == null) continue;
                        if (rspLyricResult.LyricId == 0) continue;

                        var rsp2 = client.GetLyric(rspLyricResult.LyricId, rspLyricResult.LyricChecksum);

                        AddFoundLyric(rsp2.Lyric, new SerializableUri(rsp2.LyricUrl));
                    }
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